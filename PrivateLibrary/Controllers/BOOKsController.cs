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
    public class BOOKsController : Controller
    {
        private BookRepository bookRepo = new BookRepository();
        private AuthorRepository authorRepo = new AuthorRepository();
        private GenreRepository genreRepo = new GenreRepository();
        private LanguageRepository languageRepo = new LanguageRepository();
        // GET: BOOKs
        public ActionResult TableOfBooks() {
            List<BOOK> listOfBooks = bookRepo.GetAllBooks();
            return View("TableOfBooks", listOfBooks);
        }

        public ActionResult FindBy()
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            string typedText = Request.Form["Typed"].ToString();
            int n;
            bool isNumeric = int.TryParse(typedText, out n);
            if (isNumeric) { 
                listOfBooks = bookRepo.FindByISBN(typedText);
            }
            else
            {
                listOfBooks = bookRepo.FindByTitle(typedText);
            }
            

            return View("TableOfBooks", listOfBooks);
        }

        public ActionResult FilterBy()
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            string typeOfFiltering = Request.Form["FilterBy"].ToString();
            string name = Request.Form["Typed"].ToString();
            if(typeOfFiltering == "Author")
            {
                listOfBooks = bookRepo.GetAllBooksFromAuthor(name);
            }
            else if (typeOfFiltering == "Genre")
            {
                listOfBooks = bookRepo.GetAllBooksFromGenre(name);
            }
            else
            {
                listOfBooks = bookRepo.GetAllBooksFromLanguage(name);
            }
            return View("TableOfBooks", listOfBooks);
        }

        //Details of Book
        public ActionResult Details(int id)
        {
            
            BOOK book = bookRepo.findBookById(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View("Details", book);
        }

        private void FillAuthors()
        {
            List<SelectListItem> authorsSelection = new List<SelectListItem>();
            List<AUTHOR> authors = authorRepo.GetAllAuthors();
            foreach (AUTHOR author in authors)
            {
                authorsSelection.Add(new SelectListItem { Text = author.NAMEOFATUTHOR });
            }
            ViewBag.Authors = authorsSelection;
        }

        private void FillGenres()
        {
            List<SelectListItem> genresSelection = new List<SelectListItem>();
            List<GENRE> genres = genreRepo.GetAllGenres();
            foreach (GENRE genre in genres)
            {
                genresSelection.Add(new SelectListItem { Text = genre.NAMEOFGENRE });
            }
            ViewBag.Genres = genresSelection;
        }

        private void FillLanguages()
        {
            List<SelectListItem> languagesSelection = new List<SelectListItem>();
            List<LANGUAGE> languages = languageRepo.GetAllLanguages();
            foreach (LANGUAGE language in languages)
            {
                languagesSelection.Add(new SelectListItem { Text = language.NAMEOFTHELANGUAGE });
            }
            ViewBag.Languages = languagesSelection;
        }


        // GET: BOOKs/Create
        public ActionResult Create()
        {
            FillAuthors();
            FillGenres();
            FillLanguages();
            return View();
        }

        // POST: BOOKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ISBNNUMBER,TITLE,DESCRIPTION")] BOOK bOOK)
        {
            bOOK.IDOFBOOK = bookRepo.addIdOfBook();
            bOOK.ISBNNUMBER = bOOK.IDOFBOOK;
            if (ModelState.IsValid)
            {
                string choosenAuthors = Request.Form["ChoosenAuthors"].ToString();
                string[] authorsNames = choosenAuthors.Split(',');
                string nameOfGenre = Request.Form["Genres"].ToString();
                string nameOfLanguage = Request.Form["Languages"].ToString();
                bookRepo.CreateBook(bOOK, authorsNames, nameOfGenre, nameOfLanguage);
            }
            List<BOOK> listOfBooks = bookRepo.GetAllBooks();
            return View("TableOfBooks", listOfBooks);
        }


        //i've tried to fill choosen authors, but for some reason it wont fill dropdown list

        private void FillChoosenAuthors(BOOK book)
        {
            List<SelectListItem> choosen = new List<SelectListItem>();
            List<SelectListItem> authors = new List<SelectListItem>();
            List<AUTHOR> allAuthors = authorRepo.GetAllAuthors();
            List<AUTHOR> authorsFromBook = book.AUTHORs.ToList();
            if(authorsFromBook != null)
            {
                foreach (AUTHOR choosenAuthor in authorsFromBook)
                {
                    choosen.Add(new SelectListItem { Text = choosenAuthor.NAMEOFATUTHOR, Value = choosenAuthor.NAMEOFATUTHOR });
                    foreach (AUTHOR author in allAuthors)
                    {
                        if (author.IDOFAUTHOR != choosenAuthor.IDOFAUTHOR)
                        {
                            authors.Add(new SelectListItem { Text = author.NAMEOFATUTHOR, Value = author.NAMEOFATUTHOR });
                        }
                    }
                }
            }
            
            ViewBag.Authors = authors;
            ViewBag.ChoosenAuthors = choosen;
        }

        
        public ActionResult Update(int id)
        {
            BOOK book = bookRepo.findBookById(id);
            FillChoosenAuthors(book);
            FillGenres();
            FillLanguages();
            if (book == null)
            {
                return HttpNotFound();
            }
            return View("Update", book);
        }
        
        public ActionResult UpdateInBase(int id)
        {
            BOOK book = new BOOK();
            book.IDOFBOOK = id;
            book.TITLE = Request["TITLE"].ToString();
            book.DESCRIPTION = Request["DESCRIPTION"].ToString();
            book.ISBNNUMBER =id;
            if (ModelState.IsValid)
            {
                string choosenAuthors = Request["ChoosenAuthors"].ToString();
                string[] authorsNames = choosenAuthors.Split(',');
                string nameOfGenre = Request["Genres"].ToString();
                string nameOfLanguage = Request["Languages"].ToString();
                bookRepo.UpdateBook(book, authorsNames, nameOfGenre, nameOfLanguage);
            }
            List<BOOK> listOfBooks = bookRepo.GetAllBooks();
            for (int i = 0; i < listOfBooks.Count; i++)
            {
                if(listOfBooks[i].IDOFBOOK == id)
                {
                    listOfBooks[i] = book;
                }
            }
            return View("TableOfBooks", listOfBooks);
        }

        public ActionResult Delete(int id)
        {
            BOOK book = bookRepo.findBookById(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View("Delete", book);
        }

        
        public ActionResult DeleteInBase(int id)
        {
            BOOK book = bookRepo.findBookById(id);
            if (ModelState.IsValid)
            {
                bookRepo.DeleteBook(book);
            }
            List<BOOK> listOfBooks = bookRepo.GetAllBooks();
            return View("TableOfBooks", listOfBooks);
        }

        
    }
}
