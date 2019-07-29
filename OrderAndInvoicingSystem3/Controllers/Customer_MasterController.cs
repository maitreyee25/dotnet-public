using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrderAndInvoicingSystem3.Models;

namespace OrderAndInvoicingSystem3.Controllers
{
    public class Customer_MasterController : Controller
    {
        private OrderInvoiceSystemEntities db = new OrderInvoiceSystemEntities();

        // GET: Customer_Master
        public ActionResult Index()
        {
            try
            {
            return View(db.Customer_Master.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Customer_Master/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_Master customer_Master = db.Customer_Master.Find(id);
            if (customer_Master == null)
            {
                return HttpNotFound();
            }
            return View(customer_Master);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Customer_Master/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Address,City,Pincode,ContactNumber,EmailId")] Customer_Master customer_Master)
        {
            try
            {
            if (ModelState.IsValid)
            {
                customer_Master.Name = customer_Master.Name.Trim();
                customer_Master.EmailId = customer_Master.EmailId.Trim();
                customer_Master.Address = customer_Master.Address.Trim();
                customer_Master.City = customer_Master.City.Trim();
               // to trim end spaces
                db.Customer_Master.Add(customer_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer_Master);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Customer_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            try { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer_Master customer_Master = db.Customer_Master.Find(id);
            if (customer_Master == null)
            {
                return HttpNotFound();
            }
            return View(customer_Master);
        } catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
        }
}

        // POST: Customer_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,City,Pincode,ContactNumber,EmailId")] Customer_Master customer_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(customer_Master).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(customer_Master);
           //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                 return View("Error");
                // throw;
            }

        }

        // GET: Customer_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Customer_Master customer_Master = db.Customer_Master.Find(id);
                if (customer_Master == null)
                {
                    return HttpNotFound();
                }
                return View(customer_Master);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // POST: Customer_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
        try
        {
            Customer_Master customer_Master = db.Customer_Master.Find(id);
            db.Customer_Master.Remove(customer_Master);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
         
            catch (System.Data.Entity.Infrastructure.DbUpdateException ue)
            {
                Console.WriteLine(ue.InnerException);

                //  return View("~/Views/Shared/Error.cshtml");
                return View("ActiveRecordException");
                // throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
               // ViewBag.Message = "Foreign key violation";
                return View("Error");
                // throw;
            }

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
