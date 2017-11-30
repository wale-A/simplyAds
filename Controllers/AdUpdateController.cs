using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SimplyAds.Models;

namespace SimplyAds.Controllers
{
    public class AdUpdateController : TempController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdUpdate
        public async Task<ActionResult> Index()
        {
            var adUpdates = db.AdUpdates.Include(a => a.Advert);
            return View(await adUpdates.ToListAsync());
        }

        // GET: AdUpdate/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdUpdate adUpdate = await db.AdUpdates.FindAsync(id);
            if (adUpdate == null)
            {
                return HttpNotFound();
            }
            return View(adUpdate);
        }

        // GET: AdUpdate/Create
        public ActionResult Create(int? advertId)
        {
            if (advertId == null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var adUpdate = new AdUpdate() { AdvertID = advertId.Value };
            if (Request.IsAjaxRequest())
                return PartialView(adUpdate);
            //ViewBag.AdvertID = new SelectList(db.Adverts, "ID", "ReferenceNo");
            return PartialView(adUpdate);
        }

        // POST: AdUpdate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AdvertID,Text")] AdUpdate adUpdate)
        {
            var ad = await db.Adverts.FindAsync(adUpdate.AdvertID);
            if (ModelState.IsValid)
            {
                adUpdate.DateCreated = DateTime.UtcNow.AddHours(1);
                adUpdate.CreatedBy = User.Identity.Name;
                db.AdUpdates.Add(adUpdate);
                await db.SaveChangesAsync();                
                TempData["UpdateMsg"] = "ad update added";
            }
            else
                TempData["UpdateError"] = "incomplete details for ad update";
            //ViewBag.AdvertID = new SelectList(db.Adverts, "ID", "ReferenceNo", adUpdate.AdvertID);
            return RedirectToAction("SelectedAd", "Admin", new { referenceNo = ad.ReferenceNo});
        }

        // GET: AdUpdate/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           
            AdUpdate adUpdate = await db.AdUpdates.FindAsync(id);
            if (adUpdate == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView(adUpdate);
            return View(adUpdate);
        }

        // POST: AdUpdate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AdUpdate adUpdate)
        {
            var ad = await db.Adverts.FindAsync(adUpdate.AdvertID);
            if (ModelState.IsValid)
            {
                adUpdate.EditedBy = User.Identity.Name;
                adUpdate.DateEdited = DateTime.UtcNow.AddHours(1);
                db.Entry(adUpdate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("SelectedAd", "Admin", new { referenceNo = ad.ReferenceNo });
            }
            return View(adUpdate);
        }

        // GET: AdUpdate/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdUpdate adUpdate = await db.AdUpdates.FindAsync(id);
            if (adUpdate == null)
            {
                return HttpNotFound();
            }
            return View(adUpdate);
        }

        // POST: AdUpdate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AdUpdate adUpdate = await db.AdUpdates.FindAsync(id);
            adUpdate.Deleted = true;
            adUpdate.DeletedBy = User.Identity.Name;
            adUpdate.DateDeleted = DateTime.UtcNow.AddHours(1);
            await db.SaveChangesAsync();
            var ad = await db.Adverts.FindAsync(adUpdate.AdvertID);
            return RedirectToAction("SelectedAd", "Admin", new { referenceNo = ad.ReferenceNo });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
