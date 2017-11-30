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
    public class DurationController : TempController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Duration
        public async Task<ActionResult> Index(int? selectedPageNumber)
        {
            int pageSize = 25;
            int pageNumber = (selectedPageNumber ?? 1);
            var durations = await db.DurationPricing.Where(x => !x.Deleted).ToListAsync();
            return View(durations.ToPagedList(pageNumber, pageSize));
        }

        // GET: Duration/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DurationPricing durationPricing = await db.DurationPricing.FindAsync(id);
            if (durationPricing == null)
            {
                return HttpNotFound();
            }
            return View(durationPricing);
        }

        // GET: Duration/Create
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
                return PartialView();
            return View();
        }

        // POST: Duration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Time,Amount")] DurationPricing durationPricing)
        {
            if (ModelState.IsValid)
            {
                durationPricing.CreatedBy = User.Identity.Name;
                durationPricing.DateCreated = DateTime.UtcNow.AddHours(1);
                db.DurationPricing.Add(durationPricing);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(durationPricing);
        }

        // GET: Duration/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            DurationPricing durationPricing = await db.DurationPricing.FindAsync(id);
            if (durationPricing == null)            
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView(durationPricing);

            return View(durationPricing);
        }

        // POST: Duration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Time,Amount,DateCreated,CreatedBy")] DurationPricing durationPricing)
        {
            if (ModelState.IsValid)
            {
                durationPricing.EditedBy = User.Identity.Name;
                durationPricing.DateEdited = DateTime.UtcNow.AddHours(1);
                db.Entry(durationPricing).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(durationPricing);
        }

        // GET: Duration/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            DurationPricing durationPricing = await db.DurationPricing.FindAsync(id);
            if (durationPricing == null)            
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView();

            return View(durationPricing);
        }

        // POST: Duration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DurationPricing durationPricing = await db.DurationPricing.FindAsync(id);
            durationPricing.DeletedBy = User.Identity.Name;
            durationPricing.DateDeleted = DateTime.UtcNow.AddHours(1);
            durationPricing.Deleted = true;
            db.Entry(durationPricing).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            
            base.Dispose(disposing);
        }
    }
}
