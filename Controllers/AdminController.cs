using SimplyAds.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using System;

namespace SimplyAds.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : TempController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AllAdverts(int ? selectedPageNumber)
        {
            int pageSize = 25;
            int pageNumber = (selectedPageNumber ?? 1);
            var advertsPlaced = await getListOfAdverts();
            //return View(vendor.DeliveryLocations.ToPagedList(pageNumber, pageSize));
            return View(advertsPlaced.ToPagedList(pageNumber, pageSize));
        }

        private async Task<IEnumerable<Advert>> getListOfAdverts()
        {
            var adverts = (await db.Adverts.Include(x => x.AdContent).Where(x => x.Paid).ToListAsync()).OrderByDescending(x => x.DatePlaced);
            return adverts;
        }

        public async Task<ActionResult> SelectedAd(string referenceNo)
        {
            Advert advert;
            if (Request.IsAjaxRequest())
            {
                advert = await getAdvert(referenceNo);
                return PartialView(advert);
            }
            ViewBag.Layout = "~/Views/Shared/_LayoutPage.cshtml";
            advert = await getAdvertWithUpdates(referenceNo);
            return View(advert);
        }

        public async Task<ActionResult> TreatAdPlacement(string referenceNo)
        {
            Advert advert = await getAdvert(referenceNo);
            advert.DateTreated = DateTime.UtcNow.AddHours(1);
            advert.TreatedBy = User.Identity.Name;
            advert.Treated = true;
            await saveExistingAdvert(advert);
            return RedirectToAction("AllAdverts");
        }

        private async Task<Advert> getAdvert(string referenceNo)
        {
            return await db.Adverts.Include(x => x.AdContent).SingleOrDefaultAsync(x => x.ReferenceNo == referenceNo);
        }

        private async Task<Advert> getAdvertWithUpdates(string referenceNo)
        {
            return await db.Adverts.Include(x => x.AdUpdates).Include(x => x.AdContent).SingleOrDefaultAsync(x => x.ReferenceNo == referenceNo);
        }


        private async Task saveExistingAdvert(Advert advert)
        {
            db.Entry(advert).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            } 
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
