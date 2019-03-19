using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrivateLibrary.Models;

namespace PrivateLibrary.Controllers
{
    public class NationalitiesController : Controller
    {
        private PrivateLibraryEntities db = new PrivateLibraryEntities();

        // GET: Nationalities
        public ActionResult Index()
        {
            return View(db.NATIONALITies.ToList());
        }

        // GET: Nationalities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NATIONALITY nATIONALITY = db.NATIONALITies.Find(id);
            if (nATIONALITY == null)
            {
                return HttpNotFound();
            }
            return View(nATIONALITY);
        }

        // GET: Nationalities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nationalities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOFNATIONALITY,NAMEOFNATIONALITY")] NATIONALITY nATIONALITY)
        {
            if (ModelState.IsValid)
            {
                db.NATIONALITies.Add(nATIONALITY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nATIONALITY);
        }

        // GET: Nationalities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NATIONALITY nATIONALITY = db.NATIONALITies.Find(id);
            if (nATIONALITY == null)
            {
                return HttpNotFound();
            }
            return View(nATIONALITY);
        }

        // POST: Nationalities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOFNATIONALITY,NAMEOFNATIONALITY")] NATIONALITY nATIONALITY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nATIONALITY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nATIONALITY);
        }

        // GET: Nationalities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NATIONALITY nATIONALITY = db.NATIONALITies.Find(id);
            if (nATIONALITY == null)
            {
                return HttpNotFound();
            }
            return View(nATIONALITY);
        }

        // POST: Nationalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NATIONALITY nATIONALITY = db.NATIONALITies.Find(id);
            db.NATIONALITies.Remove(nATIONALITY);
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
