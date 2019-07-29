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
    public class OrdersController : Controller
    {
        private OrderInvoiceSystemEntities db = new OrderInvoiceSystemEntities();

        // GET: Orders
        public ActionResult Index()
        {
		 try
            {
            var orders = db.Orders.Include(o => o.Customer_Master);
            return View(orders.ToList());
			 }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Order order = db.Orders.Find(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                return View(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

            // GET: Orders/Details/5
            public ActionResult Invoice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var resultSet = from o in db.Order_line_item
                                join p in db.Product_Master
                                 on o.ProductID equals p.Id
                                where o.OrderId == id
                                select new InvoiceOrder { Product_name = p.Product_name, Rate = p.Rate, GST_rate = p.GST_rate, Quantity = o.Quantity, Price = o.Price, GST = o.GST, Total_price = o.Total_price };
                ////use Ienumerable

                Order orderrow = db.Orders.Single(x => x.ID == id);
                if (orderrow == null)
                {
                    return HttpNotFound();
                }
                ViewBag.OrderDate = orderrow.Order_date;
                ViewBag.DeliveryDate = orderrow.Scheduled_del_date;
                ViewBag.billDate = DateTime.Now;
                ViewBag.TotalGST = orderrow.Total_GST;
                ViewBag.OrderTotal = orderrow.Total_order_price;

                Customer_Master customer_MasterRow = db.Customer_Master.Single(z => z.ID == orderrow.CustomerId);
                if (customer_MasterRow == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CustomerName = customer_MasterRow.Name;
                ViewBag.CustomerAddress = customer_MasterRow.City + "," + customer_MasterRow.Address + "," + customer_MasterRow.Pincode;


                if (resultSet == null)
                {
                    return HttpNotFound();
                }



                return View(resultSet.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }
   // GET: Orders/Create
   public ActionResult Create()
   {
            try
            {
                ViewBag.CustomerId = new SelectList(db.Customer_Master, "ID", "Name");
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CustomerId,Order_date,Scheduled_del_date,Total_order_price,Total_GST")] Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Orders.Add(order);
                    db.SaveChanges();

                    //  ViewBag.OrderId = order.ID;
                    //  return RedirectToAction("Create", "Order_line_item"); // change syntax
                    return RedirectToAction("Index");
                }

                ViewBag.CustomerId = new SelectList(db.Customer_Master, "ID", "ID", order.CustomerId);
                // return View("~/Views/Order_line_item/Create.aspx"); // goes to orderline view
                return View(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order order = db.Orders.Find(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CustomerId = new SelectList(db.Customer_Master, "ID", "Name", order.CustomerId);
                return View(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerId,Order_date,Scheduled_del_date,Total_order_price,Total_GST")] Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CustomerId = new SelectList(db.Customer_Master, "ID", "Name", order.CustomerId);
                return View(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order order = db.Orders.Find(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                return View(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return View("Error");
                // throw;
            }
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Order order = db.Orders.Find(id);
                db.Orders.Remove(order);
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
