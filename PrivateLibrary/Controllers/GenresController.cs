using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrivateLibrary.Models;
using PrivateLibrary.Repositories;

namespace PrivateLibrary.Controllers
{
    public class GenresController : Controller
    {

        private GenreRepository genreRepo = new GenreRepository();

        // GET: Genres
        public ActionResult Index()
        {
            
            return View(genreRepo.GetAllGenres());
        }

        // GET: Genres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NAMEOFGENRE")] GENRE gENRE)
        {
            gENRE.IDOFGENRE = genreRepo.addIdOfGenre();
            if (ModelState.IsValid)
            {
                genreRepo.CreateNewGenre(gENRE);
                return RedirectToAction("Index");
            }

            return View(gENRE);
        }

        // GET: Genres/Edit/5
        public ActionResult Edit(int id)
        {
            GENRE gENRE = genreRepo.findById(id);
            if (gENRE == null)
            {
                return HttpNotFound();
            }
            return View(gENRE);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOFGENRE,NAMEOFGENRE")] GENRE gENRE)
        {
            if (ModelState.IsValid)
            {
                genreRepo.UpdateGenre(gENRE);
                return RedirectToAction("Index");
            }
            return View(gENRE);
        }

        // GET: Genres/Delete/5
        public ActionResult Delete(int id)
        {
            GENRE gENRE = genreRepo.findById(id);
            if (gENRE == null)
            {
                return HttpNotFound();
            }
            return View(gENRE);
        }

        public ActionResult DeleteInBase(int id)
        {
            GENRE genre = genreRepo.findById(id);
            if (ModelState.IsValid)
            {
                genreRepo.DeleteGenre(genre);
            }
            List<GENRE> listOfGenres = genreRepo.GetAllGenres();
            return View("Index", listOfGenres);
        }
    }
}
