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
using PagedList;

namespace SimplyAds.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContentPricingController : TempController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContentPricing
        public async Task<ActionResult> Index(int? selectedPageNumber)
        {
            int pageSize = 25;
            int pageNumber = (selectedPageNumber ?? 1);
            var contentPricing = await db.ContentPricing.Where(x => !x.Deleted).ToListAsync();
            return View(contentPricing.ToPagedList(pageNumber, pageSize));
        }

        // GET: ContentPricing/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentPricing contentPricing = await db.ContentPricing.FindAsync(id);
            if (contentPricing == null)
            {
                return HttpNotFound();
            }
            return View(contentPricing);
        }

        // GET: ContentPricing/Create
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
                return PartialView();
            return View();
        }

        // POST: ContentPricing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Type,Amount")] ContentPricing contentPricing)
        {
            if (ModelState.IsValid)
            {
                contentPricing.CreatedBy = User.Identity.Name;
                contentPricing.DateCreated = DateTime.UtcNow.AddHours(1);

                db.ContentPricing.Add(contentPricing);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contentPricing);
        }

        // GET: ContentPricing/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentPricing contentPricing = await db.ContentPricing.FindAsync(id);
            if (contentPricing == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
                return PartialView(contentPricing);
            return View(contentPricing);
        }

        // POST: ContentPricing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Type,Amount,DateCreated,CreatedBy")] ContentPricing contentPricing)
        {
            if (ModelState.IsValid)
            {
                contentPricing.EditedBy = User.Identity.Name;
                contentPricing.DateEdited = DateTime.UtcNow.AddHours(1);
                db.Entry(contentPricing).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contentPricing);
        }

        // GET: ContentPricing/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentPricing contentPricing = await db.ContentPricing.FindAsync(id);
            if (contentPricing == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
                return PartialView(contentPricing);
            return View(contentPricing);
        }

        // POST: ContentPricing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ContentPricing contentPricing = await db.ContentPricing.FindAsync(id);
            contentPricing.Deleted = true;
            contentPricing.DeletedBy = User.Identity.Name;
            contentPricing.DateDeleted = DateTime.UtcNow.AddHours(1);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
