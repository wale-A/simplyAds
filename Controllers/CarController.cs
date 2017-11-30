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
    public class CarController : TempController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Car
        public async Task<ActionResult> Index(int? selectedPageNumber)
        {
            int pageSize = 25;
            int pageNumber = (selectedPageNumber ?? 1);
            var Car = await db.Car.Where(x => !x.Deleted).ToListAsync();
            return View(Car.ToPagedList(pageNumber, pageSize));
        }

        // GET: Car/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car Car = await db.Car.FindAsync(id);
            if (Car == null)
            {
                return HttpNotFound();
            }
            return View(Car);
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
                return PartialView();
            return View();
        }

        // POST: Car/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Car Car)
        {
            if (ModelState.IsValid)
            {
                Car.DateCreated = DateTime.UtcNow.AddHours(1);
                Car.CreatedBy = User.Identity.Name;

                db.Car.Add(Car);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(Car);
        }

        // GET: Car/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Car Car = await db.Car.FindAsync(id);
            if (Car == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView(Car);

            return View(Car);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Amount,NumberOfCars,DateCreated,CreatedBy")] Car Car)
        {
            if (ModelState.IsValid)
            {
                Car.DateEdited = DateTime.UtcNow.AddHours(1);
                Car.EditedBy = User.Identity.Name;

                db.Entry(Car).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Car);
        }

        // GET: Car/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Car Car = await db.Car.FindAsync(id);
            if (Car == null)
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView();

            return View(Car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Car Car = await db.Car.FindAsync(id);
            Car.Deleted = true;
            Car.DateDeleted = DateTime.UtcNow.AddHours(1);
            Car.DeletedBy = User.Identity.Name;
            db.Entry(Car).State = EntityState.Modified;
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
