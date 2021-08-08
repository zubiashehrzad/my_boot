using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using my_boot.Models;

namespace my_boot.Controllers
{
    public class ProductsController : Controller
    {
        private MyCartEntities db = new MyCartEntities();

        // GET: Products
        public ActionResult Index()
        {
            if (TempData["cart"]!=null)
            {
                float x=0;
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                    foreach (var item in li2)
                {
                    x += item.bill;
                }

                TempData["total"] = x;
            }

            TempData.Keep();
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }
        public ActionResult AddToCart(int ?Id)
        {
            Product p = db.Products.Where(x => x.P_id == Id).SingleOrDefault();
            return View(p);

        }
        List<Cart> li = new List<Cart>();

        [HttpPost]
        public ActionResult AddToCart(Product Pi, string qty, int Id)
        {
            Product p = db.Products.Where(x => x.P_id == Id).SingleOrDefault();
            Cart c = new Cart
            {
                productid = p.P_id,
                price = (float)p.P_price,
                qty = Convert.ToInt32(qty)
            };
            c.bill = c.price * c.qty;
            c.prductname = p.P_name;
           
            if(TempData["cart"]==null)
            {
                li.Add(c);
                TempData["cart"] = li;

            }
            else
            {
                List<Cart>li2 = TempData["cart"] as List<Cart>;
                li2.Add(c);
                TempData["cart"] = li2;

            }

            TempData.Keep();
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            TempData.Keep();
            return View();

        }
        public ActionResult ShowProducts()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.cat_id = new SelectList(db.Categories, "Cat_id", "Cat_name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try 
            { 
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                filename = DateTime.Now.ToString("yymmssff") + filename;
                string extension = Path.GetExtension(product.ImageFile.FileName);
                filename += extension;
                product.P_image = "~/Image/" + filename;
                filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                    product.ImageFile.SaveAs(filename);

                db.Products.Add(product);
                //file.SaveAs(filename);
               
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                { 
                  ViewBag.cat_id = new SelectList(db.Categories, "Cat_id", "Cat_name", product.cat_id);
                   return View(product);
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.cat_id = new SelectList(db.Categories, "Cat_id", "Cat_name", product.cat_id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    string filename = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + filename;
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    filename += extension;
                    product.P_image = "~/Image/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                    product.ImageFile.SaveAs(filename);

                    db.Products.Add(product);
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.cat_id = new SelectList(db.Categories, "Cat_id", "Cat_name", product.cat_id);
                    return View(product);
                }
            }
            catch (Exception)
            {
                return View();
            }

        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
