using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CarRentalSystem.Helper;
using CarRentalSystem.Models;
using CarRentalSystem.ViewModel;
using PagedList;

namespace CarRentalSystem.Controllers
{
    public class CarsController : ApplicationBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cars
        public ActionResult Index(string sortDir, string searchString, string CurrentFilter, int? page, string sortOrder = "")
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            ViewBag.currentFilter = searchString;
            ViewBag.sortDir = sortDir;
            ViewBag.sortOrder = sortOrder;
            var cars = db.Cars.AsQueryable();
            

            if (!string.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(c => (c.Make+ c.Model+c.CarYear+c.CarCategory+c.IsAvailable).Contains(searchString));
            }
            switch (sortOrder.ToLower())
            {
                case "Make":
                    if (sortDir.ToLower() == "desc")
                        cars = cars.OrderByDescending(s => s.Make);
                    else
                        cars = cars.OrderBy(s => s.Make);
                    break;
                case "CarYear":
                    if (sortDir.ToLower() == "desc")
                        cars = cars.OrderByDescending(s => s.CarYear);
                    else
                        cars = cars.OrderBy(s => s.CarYear);

                    break;

                default:
                    cars = cars.OrderBy(s => s.CarYear);
                    break;
            }

            int pageSize = 2;
            int pageNumber = page ?? 1;

            var data = cars.ToPagedList(pageNumber, pageSize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_searchList", data);
            }
            return View(data);
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarId,RegNum,Make,Model,CarYear,CarCategory,PassengerCapacity,IsAvailable,RentalFee,Photo")] CarViewModel carVM)
        {
            if (ModelState.IsValid)
            {
                var car = new Car();
                car.RegNum = carVM.RegNum;
                car.Make = carVM.Make;
                car.Model = carVM.Model;
                car.CarYear = carVM.CarYear;
                car.CarCategory = carVM.CarCategory;
                car.PassengerCapacity = carVM.PassengerCapacity;
                car.IsAvailable = true;
                car.RentalFee = carVM.RentalFee;
                if (carVM.Photo != null)
                {
                    car.Photo = ImageConverter.ByteArrayFromPostedFile(carVM.Photo);
                }
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carVM);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            //var carModel = new CarViewModel();
            Mapper.CreateMap<Car, CarViewModel>().ForMember
                                                    (x => x.Photo, opt => opt.Ignore());
            //Automapper
            var carVM = Mapper.Map<CarViewModel>(car);
            carVM.PhotoDB = car.Photo;
            if (car.IsAvailable)
            {
                carVM.IsAvailable = true;
            }
            else
            {
                carVM.IsAvailable = false;
            }
            
            return View(carVM);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarViewModel carVM)
        {
            if (ModelState.IsValid)
            {
                Car car = db.Cars.Find(carVM.CarId);
                if (carVM != null && carVM.Photo != null)
                {
                    car.Photo = ImageConverter.ByteArrayFromPostedFile(carVM.Photo);
                }
                car.RegNum = carVM.RegNum;
                car.Make = carVM.Make;
                car.Model = carVM.Model;
                car.CarYear = carVM.CarYear;
                car.CarCategory = carVM.CarCategory;
                car.PassengerCapacity = carVM.PassengerCapacity;
                if (carVM.IsAvailable)
                {
                    car.IsAvailable = true;
                }
                else
                {
                    car.IsAvailable = false;
                }
                
                car.RentalFee = carVM.RentalFee;

                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carVM);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
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

        
    }
}
