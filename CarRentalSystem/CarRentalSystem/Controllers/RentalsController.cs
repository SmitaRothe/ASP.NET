using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarRentalSystem.Models;

namespace CarRentalSystem.Controllers
{
    public class RentalsController : ApplicationBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rentals
        public ActionResult Index()
        {
            var rentals = db.Rentals.Include(r => r.Car).Include(r => r.Customer);
            return View(rentals.ToList());
        }

        // GET: Rentals/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            
            return View(rental);
        }

        // GET: Rentals/Create
        public ActionResult Create()
        {
            ViewBag.CarId = new SelectList(db.Cars, "CarId", "CarId");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name");
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RentalId,CarId,CustomerId,RentalDate,ReturnDate,TotalFee,Comments")] Rental rental)
        {
            //Car car = db.Cars.Where(c => c.CarId = rental.CarId)
            if (ModelState.IsValid)
            {
                db.Rentals.Add(rental);

                var car = db.Cars.SingleOrDefault(c => c.CarId == rental.CarId);
                if (car == null)
                {
                    return HttpNotFound("Car is null");
                }
                car.IsAvailable = false;
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarId = new SelectList(db.Cars, "CarId", "RegNum", rental.CarId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", rental.CustomerId);
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarId = new SelectList(db.Cars, "CarId", "RegNum", rental.CarId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", rental.CustomerId);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RentalId,CarId,CustomerId,RentalDate,ReturnDate,TotalFee,Comments")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rental).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarId = new SelectList(db.Cars, "CarId", "RegNum", rental.CarId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", rental.CustomerId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rental rental = db.Rentals.Find(id);
            db.Rentals.Remove(rental);
            db.SaveChanges();
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

        

        [HttpPost]
        public ActionResult GetAvailability(int carId)
        {
            var carAvailable = (from s in db.Cars where s.CarId == carId select s.IsAvailable).FirstOrDefault();
            if (Request.IsAjaxRequest())
            {
                return Json(carAvailable, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult CalcPrice(int carID)
        {
            var carPrice = (from s in db.Cars where s.CarId == carID select s.RentalFee).FirstOrDefault();

            return Json(carPrice, JsonRequestBehavior.AllowGet);
        }
       
    }
}