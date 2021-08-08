using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using my_boot.Models;

namespace my_boot.Controllers
{
    public class CategoriesController : Controller
    {
        private MyCartEntities db = new MyCartEntities();
        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {


            try
            {
                if (ModelState.IsValid)
                {
                    string filename = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + filename;
                    string extension = Path.GetExtension(category.ImageFile.FileName);
                    filename += extension;
                    category.Cat_image = "~/Image/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                    category.ImageFile.SaveAs(filename);

                    db.Categories.Add(category);
                    //file.SaveAs(filename);
                    db.SaveChanges();


                    return RedirectToAction("Index");
                }
                else
                {
                    return View();

                }
            }
            catch (Exception)
            {
                return View();
            }


        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
             {
                string filename = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                filename = DateTime.Now.ToString("yymmssff") + filename;
                string extension = Path.GetExtension(category.ImageFile.FileName);
                filename += extension;
                category.Cat_image = "~/Image/" + filename;
                filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                category.ImageFile.SaveAs(filename);

                db.Categories.Add(category);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

                else
                {
                    return View(category);

                }
            }
            catch (Exception)
            {
                return View();
            }
           
        }

    }
}