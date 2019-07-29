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
    public class Order_line_itemController : Controller
    {
        private OrderInvoiceSystemEntities db = new OrderInvoiceSystemEntities();

        // GET: Order_line_item
        public ActionResult Index()
        {
		            try
            {

            var order_line_item = db.Order_line_item.Include(o => o.Product_Master);
            return View(order_line_item.ToList());
			 }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Order_line_item/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_line_item order_line_item = db.Order_line_item.Find(id);
            if (order_line_item == null)
            {
                return HttpNotFound();
            }
            return View(order_line_item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Order_line_item/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "ID", "ID");
            ViewBag.ProductID = new SelectList(db.Product_Master, "Id", "Id");
            return View();
        }

        // POST: Order_line_item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderId,ProductID,Quantity,Price,GST,Total_price")] Order_line_item order_line_item)
        {
            try
            {
            if (ModelState.IsValid)
            {
                db.Order_line_item.Add(order_line_item);
                db.SaveChanges();
                //Business logic for total order price and total gst
                Order order = db.Orders 
                    .Where(a => a.ID == order_line_item.OrderId)
                    .SingleOrDefault();
                if (order.Total_GST == null)
                    order.Total_GST = 0;

                order.Total_GST = order_line_item.GST + order.Total_GST;
                if (order.Total_order_price == null)
                    order.Total_order_price = 0;

                order.Total_order_price = (order_line_item.Total_price ) + order.Total_order_price; // total price is nullable
                db.Entry(order_line_item).State = EntityState.Modified;
                db.SaveChanges();
                //Business logic end
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Product_Master, "Id", "Id", order_line_item.ProductID);
            return View(order_line_item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Order_line_item/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order_line_item order_line_item = db.Order_line_item.Find(id);
                if (order_line_item == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ProductID = new SelectList(db.Product_Master, "Id", "Id", order_line_item.ProductID);
                return View(order_line_item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // POST: Order_line_item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderId,ProductID,Quantity,Price,GST,Total_price")] Order_line_item order_line_item)
        {
            try
            {
            if (ModelState.IsValid)
            {
                // business logic for adding order total
                Order order = db.Orders
                .Where(a => a.ID == order_line_item.OrderId)
                .SingleOrDefault(); // prachi

                decimal GSTDifference = order_line_item.GSTPrev - order_line_item.GST;
                if (order.Total_GST == null)
                    order.Total_GST = 0;
                order.Total_GST = order.Total_GST - GSTDifference;

                if (order.Total_order_price == null)
                    order.Total_order_price = 0;
                decimal TotalPriceDifference = order_line_item.Total_pricePrev - order_line_item.Total_price;

                order.Total_order_price = order.Total_order_price - TotalPriceDifference;
                // business logic for adding order total end

                db.Entry(order_line_item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Product_Master, "Id", "Id", order_line_item.ProductID);
            

            return View(order_line_item);
			            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                 return View("Error");
                // throw;
            }

        }

        // GET: Order_line_item/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_line_item order_line_item = db.Order_line_item.Find(id);
            if (order_line_item == null)
            {
                return HttpNotFound();
            }
            return View(order_line_item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // POST: Order_line_item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
	 try
            {
            Order_line_item order_line_item = db.Order_line_item.Find(id);
            Order order = db.Orders
                    .Where(a => a.ID == order_line_item.OrderId)
                    .SingleOrDefault();
        

            order.Total_GST =  order.Total_GST - order_line_item.GST;
            order.Total_order_price = order.Total_order_price - order_line_item.Total_price;
            db.Order_line_item.Remove(order_line_item);
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

        public JsonResult PriceGST(int ProductID)
        {
            decimal GSTRate = 0;
            decimal Rate = 0;
            decimal[] GSTRateArray = new decimal[2]; 

            Product_Master productRow = db.Product_Master.Single(x => x.Id == ProductID);
            GSTRate = productRow.GST_rate;
            Rate = productRow.Rate;           
            GSTRateArray[0] = Rate;
            GSTRateArray[1] = GSTRate;
            return Json(GSTRateArray);
        }

    }
}
