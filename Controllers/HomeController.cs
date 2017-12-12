using SimplyAds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using SimplyAds.Misc;
//using SimplyAds

namespace SimplyAds.Controllers
{
    public class HomeController : TempController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var subdomain = getSubdomain(Request);
            if (subdomain.ToLower() == "admin" && !Request.IsAuthenticated)
            { 
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ShowAlert = ViewBag.ShowAlert ?? false;
            await setViewBagDetailsForCarAndDuration();
            return View();
        }

        public static string getSubdomain(HttpRequestBase Request)
        {
            string host = Request.Url.Host.Replace("www.", "");


            var subdomains = host.Split('.');
            string subdomain = "";

            if (subdomains.Length > 2)
            {
                subdomain = subdomains[0];
            }

            return subdomain;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetPrice(HttpPostedFileBase content, AdvertViewModel ad)
        {
            ViewBag.ShowAlert = false;
            await setViewBagDetailsForCarAndDuration();
            if (ModelState.IsValid)
            {
                if (!(!ad.Urgent && (ad.StartDate < DateTime.UtcNow.AddDays(14).AddHours(1))))
                {
                    //try
                    //{
                    CostOfAdvert costOfNewAdvert = await setIndividualChargeForAdvertWithModelAndContentType(ad, content.ContentType);
                    string filePathForSavedContent = TrySavingAdvertContent(content);

                    var adContent = new AdContent()
                    {
                        FileName = content.FileName,
                        FilePath = filePathForSavedContent,
                        ContentType = content.ContentType,
                        DateAdded = DateTime.UtcNow.AddHours(1)
                    };
                    db.AdContent.Add(adContent);
                    Advert newAdvert = new Advert()
                    {
                        ReferenceNo = Guid.NewGuid().ToString("N"),
                        CustomerName = ad.CustomerName,
                        CustomerPhone = ad.CustomerPhone,
                        Amount = costOfNewAdvert.GetTotalCharge(),
                        StartDate = ad.StartDate,
                        DatePlaced = DateTime.UtcNow.AddHours(1),
                        Urgent = ad.Urgent,
                        EndDate = getDateEnding(ad.Duration, ad.StartDate.Value),
                        Duration = ad.Duration,
                        NoOfCars = getNumberOfCars(ad.NoOfCars),
                        AdContentID = adContent.ID,
                        CustomerEmail = ad.CustomerEmail
                    };
                    var trxResult = await PayStackAPI.InitializeTransaction(Convert.ToInt32(newAdvert.Amount),
                        newAdvert.CustomerEmail, Url.Action("Advert", "Home",
                        new { referenceNo = newAdvert.ReferenceNo }, Request.Url.Scheme));

                    if (String.IsNullOrWhiteSpace(trxResult))
                    {
                        ViewBag.ShowAlert = true;
                        TempData["ErrorMessage"] = "Error...an error occurred while trying to place your advert, Please retry.";
                        return RedirectToAction("Index", ad);
                    }
                    var tempString = trxResult.Split('%');
                    string authorizeurl = tempString[0];
                    string trxref = tempString[1];

                    if (authorizeurl != null)
                    {
                        newAdvert.TransactionReference = trxref;
                        db.Adverts.Add(newAdvert);
                        await db.SaveChangesAsync();
                        //TempData["AdSuceessMsg"] = "your add has been placed.";
                        return Redirect(authorizeurl);
                    }
                    //}
                    //catch (Exception exception)
                    //{
                    //    TempData["AdErrorMsg"] = exception.Message;
                    //}
                }
                ViewBag.ShowAlert = true;
                TempData["ErrorMessage"] = "invalid date.";
            }
            ViewBag.ShowAlert = true;
            TempData["ErrorMessage"] = "invalid values entered.";
            //return RedirectToAction("Index", ad);
            return View("Index", null);
        }

        public async Task<ActionResult> Advert(string referenceNo)
        {
            Advert advert = await db.Adverts.Include(x => x.AdUpdates).SingleOrDefaultAsync(x => x.ReferenceNo == referenceNo);
            bool advertPaid = await ConfirmUpdatePaymentForAd(advert);
            setMessageForAdvertView(advertPaid);

            if (Request.IsAjaxRequest())
                return PartialView(advert);
            return View(advert);
        }

        public async Task<string> SearchForAdvert(string referenceNo)
        {
            Advert advert = await db.Adverts.SingleOrDefaultAsync(x => x.ReferenceNo == referenceNo);
            //bool advertPaid = await ConfirmUpdatePaymentForAd(advert);
            var ad = new SearchedAdViewModel()
            {
                Name = advert.CustomerName,
                RefNo = advert.ReferenceNo,
                Email = advert.CustomerEmail,
                Phone = advert.CustomerPhone,
                Duration = advert.Duration,
                Amount = advert.Amount.ToString(),
                Urgent = advert.Urgent,
                Treated = advert.Treated,
                Start = advert.StartDate.Value.ToShortDateString(),
                End = advert.EndDate.Value.ToShortDateString(),
                Cars = advert.NoOfCars.ToString()
            };
            var adAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(ad);
            return adAsJson;
        }

        [HttpPost]
        public async Task<ActionResult> PayForAdvert(string referenceNo)
        {
            Advert advert = await db.Adverts.SingleOrDefaultAsync(x => x.ReferenceNo == referenceNo);
            var url_ref = await PayStackAPI.InitializeTransaction(Convert.ToInt32(advert.Amount),
                            advert.CustomerEmail,
                            Url.Action("Advert", "Home", new { referenceNo = advert.ReferenceNo },
                            Request.Url.Scheme), advert.TransactionReference);
            var tempString = url_ref.Split('%');
            string paystackUrl = tempString[0];
            return Redirect(paystackUrl);
        }

        private void setMessageForAdvertView(bool advertPaid)
        {
            if (!advertPaid)
                TempData["AdNotPaid"] = "advert has not been paid for, please make payment";

            TempData["AdPaid"] = "success, your request for an advert has been placed.";
        }

        private async Task<bool> ConfirmUpdatePaymentForAd(Advert advert)
        {
            bool paid = await PayStackAPI.VerifyTransaction(advert.TransactionReference);
            if (paid)
                await setAdStatusToPaid(advert);
            return paid;
        }

        private async Task setAdStatusToPaid(Advert advert)
        {
            advert.Paid = true;
            db.Entry(advert).State = EntityState.Modified;
            EmailService email = new EmailService();
            await email.SendAsync(new Microsoft.AspNet.Identity.IdentityMessage
            {
                Body = "Advert has been set up",
                Destination = advert.CustomerEmail,
                Subject = "Advert created"
            });
            await db.SaveChangesAsync();
        }

        private string TrySavingAdvertContent(HttpPostedFileBase content)
        {
            if (content != null && content.ContentLength > 0 && (content.ContentType.Contains("image") || content.ContentType.Contains("video")))
                return saveContentReturnFilePath(content);

            else
                throw new Exception("invalid file");
        }

        private string saveContentReturnFilePath(HttpPostedFileBase content)
        {
            var filePath = Guid.NewGuid().ToString("N") + "_" + Path.GetFileName(content.FileName);
            string path = Path.Combine(HttpContext.Server.MapPath("~/AdContent"), filePath);
            if (!Directory.Exists(Path.Combine(HttpContext.Server.MapPath("~/AdContent"))))
                Directory.CreateDirectory(Path.Combine(HttpContext.Server.MapPath("~/AdContent")));
            content.SaveAs(path);
            return filePath;
        }

        private async Task<CostOfAdvert> setIndividualChargeForAdvertWithModelAndContentType(AdvertViewModel ad, string contentType)
        {
            CostOfAdvert costOfAdvert = new CostOfAdvert()
            {
                CarCharge = await GetCarPrice(ad.NoOfCars),
                ContentChage = await GetContentPrice(contentType),
                DurationCharge = await GetDurationPrice(ad.Duration),
                UrgentAdCharge = (ad.Urgent ? await GetUrgentAdPrice() : 0)
            };
            return costOfAdvert;
        }


        private async Task setViewBagDetailsForCarAndDuration()
        {
            var listOfCars = await db.Car.ToListAsync();
            ViewBag.Car = new SelectList(listOfCars, "NumberOfCars", "NumberOfCars");
            var listOfDurations = await db.DurationPricing.ToListAsync();
            ViewBag.Duration = new SelectList(listOfDurations, "Time", "Time");
        }

        private string getNumberOfCars(string noOfCars)
        {
            var match = System.Text.RegularExpressions.Regex.Match(noOfCars, @"\d+");
            return match.Groups[0].Value;
        }

        private DateTime getDateEnding(string duration, DateTime startDate)
        {
            var match = System.Text.RegularExpressions.Regex.Match(duration, @"\d+");
            int noOfDays = Int32.Parse(match.Groups[0].Value);

            if (duration.ToLower().Contains("day"))
                return startDate.AddDays(noOfDays);
            else if (duration.ToLower().Contains("week"))
                return startDate.AddDays(noOfDays * 7);
            else if (duration.ToLower().Contains("month"))
                return startDate.AddDays(noOfDays * 30);
            else if (duration.ToLower().Contains("year"))
                return startDate.AddDays(noOfDays * 365);
            else
                return DateTime.UtcNow.AddHours(1);
        }

        public async Task<decimal> GetCarPrice(string carText)
        {
            int noOfCars;
            int.TryParse(carText, out noOfCars);
            if (noOfCars == 0)
                return 0;
            var car = await db.Car.SingleOrDefaultAsync(x => x.NumberOfCars == noOfCars);
            if (car == null)
                return 0;
            return car.Amount;
        }

        public async Task<decimal> GetDurationPrice(string durationText)
        {
            if (String.IsNullOrWhiteSpace(durationText))
                return 0;
            var duration = await db.DurationPricing.SingleOrDefaultAsync(x => x.Time == durationText);
            if (duration == null)
                return 0;
            return duration.Amount;
        }

        public async Task<decimal> GetContentPrice(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
                return 0;
            var contents = await db.ContentPricing.ToListAsync();
            foreach (var item in contents)
                if (contentType.ToLower().Contains(item.Type.ToLower()))
                    return item.Amount;

            return 0;
        }

        public async Task<decimal> GetUrgentAdPrice()
        {
            var urgentCharge = await db.MiscChargePricing.SingleOrDefaultAsync(x => x.Property == "urgent");
            if (urgentCharge == null)
                return 0;

            return urgentCharge.Amount;
        }

        public async Task<decimal> CalculateAdvertPrice(AjaxAdModel ajaxAdModel)
        {
            decimal urgentPrice = (ajaxAdModel.Urgent ? await GetUrgentAdPrice() : 0);
            decimal carPrice = await GetCarPrice(ajaxAdModel.NoOfCars);
            decimal durationPrice = await GetDurationPrice(ajaxAdModel.Duration);
            decimal contentTypePrice = await GetContentPrice(getContentTypeFromExt(ajaxAdModel.ContentExtension));

            return (urgentPrice + carPrice + durationPrice + contentTypePrice);
        }

        private string getContentTypeFromExt(string extension)
        {
            string contentType;
            switch (extension)
            {
                case "mp4":
                case "3gp":
                case "3gpp":
                case "avi":
                case "f4v":
                case "flv":
                case "mpeg":
                case "mpg":
                case "m4v":
                case "wmv":
                    contentType = "video";
                    break;
                case "jpeg":
                case "jpg":
                case "png":
                case "bmp":
                case "gif":
                    contentType = "image";
                    break;
                default:
                    contentType = "null";
                    break;
            }
            return contentType;
        }


        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            //ViewBag.ReturnUrl = returnUrl;
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}