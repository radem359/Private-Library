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
    public class AuthorsController : Controller
    {
        private AuthorRepository authorRepo = new AuthorRepository();
        private NationalityRepository nationalityRepo = new NationalityRepository();

        // GET: Authors
        public ActionResult TableOfAuthors()
        {
            List<AUTHOR> listOfAuthors = authorRepo.GetAllAuthors();
            return View("TableOfAuthors", listOfAuthors);
        }

        private void FillNationalities()
        {
            List<SelectListItem> nationalitiesSelection = new List<SelectListItem>();
            List<NATIONALITY> nationalities = nationalityRepo.GetAllNationalities();
            foreach (NATIONALITY nationality in nationalities)
            {
                nationalitiesSelection.Add(new SelectListItem { Text = nationality.NAMEOFNATIONALITY });
            }
            ViewBag.NATIONALITY = nationalitiesSelection;
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            FillNationalities();
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInBase([Bind(Include = "IDOFAUTHOR,IDOFNATIONALITY,NAMEOFATUTHOR")] AUTHOR aUTHOR)
        {
            aUTHOR.IDOFAUTHOR = authorRepo.addIdOfAuthor();
            if (ModelState.IsValid)
            {
                string nameOfNationality = Request["NATIONALITY"].ToString();
                authorRepo.CreateAuthor(aUTHOR, nameOfNationality);
            }
            List<AUTHOR> listOfAuthors = authorRepo.GetAllAuthors();
            return View("TableOfAuthors", listOfAuthors);
        }

        // GET: Authors/Edit/5
        public ActionResult Update(int id)
        {
            FillNationalities();
            AUTHOR author = authorRepo.findAuthorById(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View("Update", author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInBase([Bind(Include = "IDOFAUTHOR,NAMEOFATUTHOR,IDOFNATIONALITY")]AUTHOR author)
        {
            if (ModelState.IsValid)
            {

                string nameOfNationality = Request["NATIONALITY"].ToString();
                authorRepo.UpdateAuthor(author, nameOfNationality);
            }
            List<AUTHOR> listOfAuthors = authorRepo.GetAllAuthors();
            return View("TableOfAuthors", listOfAuthors);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int id)
        {
            AUTHOR author = authorRepo.findAuthorById(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View("Delete", author);
        }

        public ActionResult DeleteInBase(int id)
        {
            AUTHOR author = authorRepo.findAuthorById(id);
            if (ModelState.IsValid)
            {
                authorRepo.DeleteAuthor(author);
            }
            List<AUTHOR> listOfAuthors = authorRepo.GetAllAuthors();
            return View("TableOfAuthors", listOfAuthors);
        }
    }
}
