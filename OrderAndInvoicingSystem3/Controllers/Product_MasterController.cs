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
    public class Product_MasterController : Controller
    {
        private OrderInvoiceSystemEntities db = new OrderInvoiceSystemEntities();

        // GET: Product_Master
        public ActionResult Index()
        {
            try
            {
                return View(db.Product_Master.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Product_Master/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product_Master product_Master = db.Product_Master.Find(id);
                if (product_Master == null)
                {
                    return HttpNotFound();
                }
                return View(product_Master);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Product_Master/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Product_name,Category,Rate,GST_rate")] Product_Master product_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product_Master.Product_name = product_Master.Product_name.Trim();
                    product_Master.Category = product_Master.Category.Trim();

                    db.Product_Master.Add(product_Master);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(product_Master);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Product_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            try { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Master product_Master = db.Product_Master.Find(id);
            if (product_Master == null)
            {
                return HttpNotFound();
            }
            return View(product_Master);
        } catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
        // throw;
    }
}

        // POST: Product_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Product_name,Category,Rate,GST_rate")] Product_Master product_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(product_Master).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(product_Master);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                 return View("Error");
                // throw;
            }

        }

        // GET: Product_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product_Master product_Master = db.Product_Master.Find(id);
                if (product_Master == null)
                {
                    return HttpNotFound();
                }
                return View(product_Master);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // POST: Product_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Product_Master product_Master = db.Product_Master.Find(id);
                db.Product_Master.Remove(product_Master);
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
