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
    public class LanguagesController : Controller
    {
        private PrivateLibraryEntities db = new PrivateLibraryEntities();

        // GET: Languages
        public ActionResult Index()
        {
            return View(db.LANGUAGEs.ToList());
        }

        // GET: Languages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LANGUAGE lANGUAGE = db.LANGUAGEs.Find(id);
            if (lANGUAGE == null)
            {
                return HttpNotFound();
            }
            return View(lANGUAGE);
        }

        // GET: Languages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOFLANGUAGE,NAMEOFTHELANGUAGE")] LANGUAGE lANGUAGE)
        {
            if (ModelState.IsValid)
            {
                db.LANGUAGEs.Add(lANGUAGE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lANGUAGE);
        }

        // GET: Languages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LANGUAGE lANGUAGE = db.LANGUAGEs.Find(id);
            if (lANGUAGE == null)
            {
                return HttpNotFound();
            }
            return View(lANGUAGE);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOFLANGUAGE,NAMEOFTHELANGUAGE")] LANGUAGE lANGUAGE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lANGUAGE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lANGUAGE);
        }

        // GET: Languages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LANGUAGE lANGUAGE = db.LANGUAGEs.Find(id);
            if (lANGUAGE == null)
            {
                return HttpNotFound();
            }
            return View(lANGUAGE);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LANGUAGE lANGUAGE = db.LANGUAGEs.Find(id);
            db.LANGUAGEs.Remove(lANGUAGE);
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
