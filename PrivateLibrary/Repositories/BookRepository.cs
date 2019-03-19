using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PrivateLibrary.Models;

namespace PrivateLibrary.Repositories
{
    public class BookRepository
    {
        static string ConnectionString = "data source=.; database=PrivateLibrary; integrated security = SSPI";
        private NationalityRepository nationalityRepo = new NationalityRepository();

        /**return all books from database*/

        public List<BOOK> GetAllBooks()
        {
            List<BOOK> books = new List<BOOK>();
            

            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("select * from Book", connectionToBase);
                connectionToBase.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                 {
                    BOOK book = new BOOK();
                    book.IDOFBOOK = (int)reader["IDOFBOOK"];
                    book.TITLE = (String)reader["TITLE"]; 
                    book.ISBNNUMBER = (int)reader["ISBNNUMBER"];
                    book.DESCRIPTION = (String)reader["DESCRIPTION"];
                    book.GENREs = SetGenres(book);
                    book.AUTHORs = SetAuthors(book);
                    book.LANGUAGEs = SetLanguages(book);
                    books.Add(book);
                }
            }
            return books;
        }

        /**methods for searching */

        //method for searching book by isbn
        public List<BOOK> FindByISBN(string isbn)
        {
            int isbNumber = int.Parse(isbn);
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from Book where ISBNNUMBER = @ISBN", connectionToBase);
                command.Parameters.AddWithValue("@ISBN", isbNumber);
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    BOOK book = new BOOK();
                    book.IDOFBOOK = (int)reader["idofbook"];
                    book.ISBNNUMBER = (int)reader["ISBNNUMBER"];
                    book.TITLE = (string)reader["title"];
                    book.DESCRIPTION = (string)reader["description"];
                    book.GENREs = SetGenres(book);
                    book.AUTHORs = SetAuthors(book);
                    book.LANGUAGEs = SetLanguages(book);
                    listOfBooks.Add(book);
                }
            }
            return listOfBooks;
        }

        

        //method for searching books by title
        public List<BOOK> FindByTitle(string title)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from Book where TITLE = '"+title+"'", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BOOK book = new BOOK();
                    book.IDOFBOOK = (int)reader["idofbook"];
                    book.ISBNNUMBER = (int)reader["isbnnumber"];
                    book.TITLE = (string)reader["title"];
                    book.DESCRIPTION = (string)reader["description"];
                    book.GENREs = SetGenres(book);
                    book.AUTHORs = SetAuthors(book);
                    book.LANGUAGEs = SetLanguages(book);
                    listOfBooks.Add(book);
                }
            }
            return listOfBooks;
        }

        //method for creating whole new book
        internal void CreateBook(BOOK book, string[] namesOfAuthors, string nameOfGenre, string nameOfLanguage)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("insert into BOOK (IDOFBOOK, ISBNNUMBER, TITLE, DESCRIPTION)  VALUES (@Id, @Isbn, '" + book.TITLE + "',  '" + book.DESCRIPTION + "')", connectionToBase);
                command.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                command.Parameters.AddWithValue("@Isbn", book.ISBNNUMBER);
                book.AUTHORs = addAuthors(namesOfAuthors);
                book.GENREs = addGenre(nameOfGenre);
                book.LANGUAGEs = addLanguage(nameOfLanguage);
                SqlDataReader reader = command.ExecuteReader();
                insertIntoWrither(book.AUTHORs.ToList(), book);
                insertIntoBelonging(book.GENREs.ToList().First(), book);
                insertIntoBelonging2(book.LANGUAGEs.ToList().First(), book);
            }
        }

        

        private List<AUTHOR> addAuthors(string[] namesOfAuthors)
        {
            List<AUTHOR> authors = new List<AUTHOR>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                for (int index = 0; index < namesOfAuthors.Length; index++)
                {
                    SqlCommand command = new SqlCommand("select * from AUTHOR where NAMEOFATUTHOR like '" + namesOfAuthors[index] + "'", connectionToBase);
                    SqlDataReader reader = command.ExecuteReader();
                    AUTHOR author = new AUTHOR();
                    while (reader.Read())
                    {
                        author.NAMEOFATUTHOR = (String)reader["NAMEOFATUTHOR"];
                        author.IDOFAUTHOR = (int)reader["IDOFAUTHOR"];
                        author.IDOFNATIONALITY = (int)reader["IDOFNATIONALITY"];
                    }
                    reader.Close();
                    authors.Add(author);
                }
            }
            return authors;
        }


        //getting author by Name. This method is hellping in book repository
        internal AUTHOR getAuthor(string nameOfAUthor)
        {
            AUTHOR author = new AUTHOR();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from AUTHOR where NAMEOFATUTHOR like '" + nameOfAUthor + "'", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    author.IDOFAUTHOR = (int)reader["IDOFAUTHOR"];
                    author.NAMEOFATUTHOR = (string)reader["NAMEOFATUTHOR"];
                    author.IDOFNATIONALITY = (int)reader["IDOFNATIONALITY"];
                }
            }
            return author;
        }

        private List<GENRE> addGenre(string nameOfGenre)
        {
            List<GENRE> genres = new List<GENRE>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from GENRE where NAMEOFGENRE like '" + nameOfGenre + "'", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                GENRE genre = new GENRE();
                while (reader.Read())
                {
                    genre.NAMEOFGENRE = (String)reader["NAMEOFGENRE"];
                    genre.IDOFGENRE = (int)reader["IDOFGENRE"];
                }

                genres.Add(genre);
            }
            return genres;
        }

        private List<LANGUAGE> addLanguage(string nameOfLanguage)
        {
            List<LANGUAGE> languages = new List<LANGUAGE>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from LANGUAGE where NAMEOFTHELANGUAGE like '" + nameOfLanguage + "'", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                LANGUAGE language = new LANGUAGE();
                while (reader.Read())
                {
                    language.NAMEOFTHELANGUAGE = (String)reader["NAMEOFTHELANGUAGE"];
                    language.IDOFLANGUAGE = (int)reader["IDOFLANGUAGE"];
                }

                languages.Add(language);
            }
            return languages;
        }

        public void insertIntoWrither(List<AUTHOR> authors, BOOK book)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                foreach (AUTHOR author in authors)
                {
                    SqlCommand command = new SqlCommand("insert into WRITHER(IDOFAUTHOR, IDOFBOOK) values(@IdA, @IdB)", connectionToBase);
                    command.Parameters.AddWithValue("@IdA", author.IDOFAUTHOR);
                    command.Parameters.AddWithValue("@IdB", book.IDOFBOOK);
                
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Close();
                }
            }
        }

        public void insertIntoWrither(AUTHOR author, BOOK book)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                    SqlCommand command = new SqlCommand("insert into WRITHER(IDOFAUTHOR, IDOFBOOK) values(@IdA, @IdB)", connectionToBase);
                    command.Parameters.AddWithValue("@IdA", author.IDOFAUTHOR);
                    command.Parameters.AddWithValue("@IdB", book.IDOFBOOK);

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Close();
            }
        }

        public void insertIntoBelonging(GENRE genre, BOOK book)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("insert into  BELONGING(IDOFGENRE, IDOFBOOK)  VALUES (@IdA, @IdB)", connectionToBase);
                command.Parameters.AddWithValue("@IdA", genre.IDOFGENRE);
                command.Parameters.AddWithValue("@IdB", book.IDOFBOOK);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }
        }

        public void insertIntoBelonging2(LANGUAGE language, BOOK book)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("insert into  BELONGING2(IDOFLANGUAGE, IDOFBOOK)  VALUES (@IdA, @IdB)", connectionToBase);
                command.Parameters.AddWithValue("@IdA", language.IDOFLANGUAGE);
                command.Parameters.AddWithValue("@IdB", book.IDOFBOOK);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }
        }

        //adding idOfBooks because for some reason I cant send it from Update view to BOOKsController method update
        internal int addIdOfBook(BOOK bOOK)
        {
            int id = 0;
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from BOOK where TITLE like '" + bOOK.TITLE + "'", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = (int)reader["IDOFBOOK"];
                }
            }
            return id;
        }


        //add ifOfBook for new book
        internal int addIdOfBook()
        {
            int id = 0;
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from BOOK", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int idOfBook = (int)reader["IDOFBOOK"];
                    if (id < idOfBook)
                    {
                        id = idOfBook;
                    }
                }
            }
            return id+1;
        }

        //return all book from database using just authors name
        public List<BOOK> GetAllBooksFromAuthor(String name)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from AUTHOR where NAMEOFATUTHOR like '"+name+"'", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                AUTHOR author = new AUTHOR();
                while (reader.Read())
                {
                   author.NAMEOFATUTHOR = (String)reader["NAMEOFATUTHOR"];
                   author.IDOFAUTHOR = (int)reader["IDOFAUTHOR"];
                }

                listOfBooks = setAuthorsBooks(author);
            }
            return listOfBooks;
        }

        
        //Updating database with new book
        public void UpdateBook(BOOK book, string[] choosenAuthors, string nameOFGenre, string nameOfLanguage) { 
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("update BOOK set TITLE = '" + book.TITLE+"', DESCRIPTION = '"+book.DESCRIPTION+"' where IDOFBOOK = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                book.AUTHORs = updateAuthors(choosenAuthors, book);
                book.GENREs = updateGenre(nameOFGenre, book);
                book.LANGUAGEs = updateLanguage(nameOfLanguage, book);
                updateWrither(book.AUTHORs.ToList(), book);
                updateBelonging(book.GENREs.ToList(), book);
                updateBelonging2(book.LANGUAGEs.ToList(), book);
            }
        }

        private List<AUTHOR> updateAuthors(string[] namesOfAuthors, BOOK book)
        {
            List<AUTHOR> authors = new List<AUTHOR>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                for (int index = 0; index < namesOfAuthors.Length; index++)
                {
                    SqlCommand command1 = new SqlCommand("select * from AUTHOR where NAMEOFATUTHOR like '" + namesOfAuthors[index] + "'", connectionToBase);
                    SqlDataReader reader = command1.ExecuteReader();
                    AUTHOR author = new AUTHOR();
                    while (reader.Read())
                    {
                        author.NAMEOFATUTHOR = (String)reader["NAMEOFATUTHOR"];
                        author.IDOFAUTHOR = (int)reader["IDOFAUTHOR"];
                        author.IDOFNATIONALITY = (int)reader["IDOFNATIONALITY"];
                        author.BOOKs = GetAllBooksFromAuthor(author.NAMEOFATUTHOR);
                    }
                    authors.Add(author);
                    reader.Close();
                }
            }
            return authors;
        }

        private List<GENRE> updateGenre(string namesOfGenre, BOOK book)
        {
            List<GENRE> genres = new List<GENRE>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command1 = new SqlCommand("select * from GENRE where NAMEOFGENRE like '" + namesOfGenre + "'", connectionToBase);
                SqlDataReader reader = command1.ExecuteReader();
                GENRE genre = new GENRE();
                while (reader.Read())
                {
                    genre.NAMEOFGENRE = (String)reader["NAMEOFGENRE"];
                    genre.IDOFGENRE = (int)reader["IDOFGENRE"];
                    genre.BOOKs = GetAllBooksFromGenre(genre.NAMEOFGENRE);
                }
                genres.Add(genre);
            }
            return genres;
        }

        private List<LANGUAGE> updateLanguage(string namesOfLanguage, BOOK book)
        {
            List<LANGUAGE> languages = new List<LANGUAGE>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command1 = new SqlCommand("select * from LANGUAGE where NAMEOFTHELANGUAGE like '" + namesOfLanguage + "'", connectionToBase);
                SqlDataReader reader = command1.ExecuteReader();
                LANGUAGE language = new LANGUAGE();
                while (reader.Read())
                {
                    language.NAMEOFTHELANGUAGE = (String)reader["NAMEOFTHELANGUAGE"];
                    language.IDOFLANGUAGE = (int)reader["IDOFLANGUAGE"];
                    language.BOOKs = GetAllBooksFromLanguage(language.NAMEOFTHELANGUAGE);
                }
                languages.Add(language);
            }
            return languages;
        }

        public void updateWrither(List<AUTHOR> authors, BOOK book)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                foreach (AUTHOR author in authors)
                {
                    SqlCommand command1 = new SqlCommand("select * from WRITHER where IDOFBOOK = @IdB", connectionToBase);
                    command1.Parameters.AddWithValue("@IdB", book.IDOFBOOK);
                    SqlDataReader reader = command1.ExecuteReader();
                    List<AUTHOR> authors1 = new List<AUTHOR>();
                    while (reader.Read())
                    {
                        AUTHOR author1 = new AUTHOR();
                        author1.IDOFAUTHOR = (int)reader["IDOFAUTHOR"];
                        authors1.Add(author1);
                    }
                    reader.Close();
                    if(authors.Count == authors1.Count) { 
                        SqlCommand command2 = new SqlCommand("update WRITHER set IDOFAUTHOR = @IdA where IDOFBOOK = @IdB", connectionToBase);
                        command2.Parameters.AddWithValue("@IdA", author.IDOFAUTHOR);
                        command2.Parameters.AddWithValue("@IdB", book.IDOFBOOK);
                        reader = command2.ExecuteReader();
                        reader.Close();
                    }
                    else
                    {
                        List<AUTHOR> existingInWrither = new List<AUTHOR>();
                        SqlCommand command3 = new SqlCommand("select * from WRITHER where IDOFBOOK = @Id", connectionToBase);
                        command3.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                        reader = command3.ExecuteReader();
                        while (reader.Read())
                        {
                            AUTHOR authorInWriher = new AUTHOR();
                            authorInWriher.IDOFAUTHOR = (int)reader["IDOFAUTHOR"];
                            existingInWrither.Add(authorInWriher);
                        }
                        reader.Close();
                        foreach (AUTHOR existing in existingInWrither)
                        {
                            foreach (AUTHOR nonExist in authors1)
                            {
                                if(nonExist.IDOFAUTHOR != existing.IDOFAUTHOR)
                                {
                                    insertIntoWrither(nonExist, book);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void updateBelonging(List<GENRE> genres, BOOK book)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                GENRE genre = genres.First();
                SqlCommand command = new SqlCommand("update BELONGING set IDOFGENRE = @IdA where IDOFBOOK = @IdB", connectionToBase);
                command.Parameters.AddWithValue("@IdA", genre.IDOFGENRE);
                command.Parameters.AddWithValue("@IdB", book.IDOFBOOK);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }
        }

        public void updateBelonging2(List<LANGUAGE> languages, BOOK book)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                LANGUAGE language = languages.First();
                SqlCommand command = new SqlCommand("update BELONGING2 set IDOFLANGUAGE = @IdA where IDOFBOOK = @IdB", connectionToBase);
                command.Parameters.AddWithValue("@IdA", language.IDOFLANGUAGE);
                command.Parameters.AddWithValue("@IdB", book.IDOFBOOK);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }
        }

        //Deleting book from database
        public void DeleteBook(BOOK book)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("DELETE FROM BELONGING2 WHERE IDOFBOOK = @Id", connectionToBase);
                SqlCommand command1 = new SqlCommand("DELETE FROM BELONGING WHERE IDOFBOOK = @Id", connectionToBase);
                SqlCommand command2 = new SqlCommand("DELETE FROM WRITHER WHERE IDOFBOOK = @Id", connectionToBase);
                SqlCommand command3 = new SqlCommand("DELETE FROM  BOOK where IDOFBOOK = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                command1.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                command2.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                command3.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                reader = command1.ExecuteReader();
                reader.Close();
                reader = command2.ExecuteReader();
                reader.Close();
                reader = command3.ExecuteReader();
            }
        }





        //hellping method for GetAllBooksFromAuthor method
        private List<BOOK> setAuthorsBooks(AUTHOR author)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from WRITHER where IDOFAUTHOR = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", author.IDOFAUTHOR);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    int idOfBook = (int)reader["IDOFBOOK"];
                    BOOK book = findBookById(idOfBook);
                    listOfBooks.Add(book);
                }
            }
            return listOfBooks;
        }

        //return all book from database using just genre name
        public List<BOOK> GetAllBooksFromGenre(String name)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from GENRE where NAMEOFGENRE like '" + name + "'", connectionToBase);
                //command.Parameters.AddWithValue("@Name", name);
                SqlDataReader reader = command.ExecuteReader();
                int idOfGenre = 0;
                while (reader.Read())
                {
                    idOfGenre = (int)reader["IDOFGENRE"];
                }

                listOfBooks = setBooksFromGenre(idOfGenre);
            }
            return listOfBooks;
        }

        //hellping method for GetAllBooksFromGenre method
        private List<BOOK> setBooksFromGenre(int idOfGenre)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from BELONGING where IDOFGenre = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", idOfGenre);
                SqlDataReader reader = command.ExecuteReader();
                int idOfBook = 0;
                while (reader.Read())
                {
                    idOfBook = (int)reader["IDOFBOOK"];
                    BOOK book = findBookById(idOfBook);
                    listOfBooks.Add(book);
                }
            }
            return listOfBooks;
        }

        //return all book from database using just language name
        public List<BOOK> GetAllBooksFromLanguage(String name)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from LANGUAGE where NAMEOFTHELANGUAGE like '" + name + "'", connectionToBase);
                //command.Parameters.AddWithValue("@Name", name);
                SqlDataReader reader = command.ExecuteReader();
                int idOfLanguage = 0;
                while (reader.Read())
                {
                    idOfLanguage = (int)reader["IDOFLANGUAGE"];
                }

                listOfBooks = setBooksFromLanguage(idOfLanguage);
            }
            return listOfBooks;
        }

        //hellping method for GetAllBooksFromLanguage method
        private List<BOOK> setBooksFromLanguage(int idOfLanguage)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from BELONGING2 where IDOFLANGUAGE = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", idOfLanguage);
                SqlDataReader reader = command.ExecuteReader();
                int idOfBook = 0;
                while (reader.Read())
                {
                    idOfBook = (int)reader["IDOFBOOK"];
                    BOOK book = findBookById(idOfBook);
                    listOfBooks.Add(findBookById(idOfBook));
                }
            }
            return listOfBooks;
        }
        
        //finding books with idOfTheBook
        public BOOK findBookById(int id)
        {
            BOOK book = new BOOK();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from Book where IDOFBOOK = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    book.IDOFBOOK = id;
                    book.ISBNNUMBER = (int)reader["ISBNNUMBER"];
                    book.TITLE = (String)reader["TITLE"];
                    book.DESCRIPTION = (String)reader["Description"];
                    book.GENREs = SetGenres(book);
                    book.LANGUAGEs = SetLanguages(book);
                    book.AUTHORs = SetAuthors(book);
                }
            }
            return book;
        }

        //method for setting genres for book
        private List<GENRE> SetGenres(BOOK book)
        {
            List<GENRE> listOfGenres = new List<GENRE>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("Select * from BELONGING where IDOFBOOK = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    int idOfGenre = (int)reader["IDOFGENRE"];
                    GENRE genre = FindGenreByID(idOfGenre);
                    listOfGenres.Add(genre);
                }
            }
            return listOfGenres;
        }

        //helping method for SetGenres method
        private GENRE FindGenreByID(int idOfGenre)
        {
            GENRE genre = new GENRE();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from GENRE where IDOFGENRE = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", idOfGenre);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    genre.IDOFGENRE = idOfGenre;
                    genre.NAMEOFGENRE = (String)reader["NAMEOFGENRE"];
                }
            }
            return genre;
        }

        //method for setting author for book
        private List<AUTHOR> SetAuthors(BOOK book)
        {
            List<AUTHOR> listOfAuthors = new List<AUTHOR>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from WRITHER where IDOFBOOK = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int idOfAuthor = (int)reader["IDOFAUTHOR"];
                    AUTHOR author = FindAuthorByID(idOfAuthor);
                    listOfAuthors.Add(author);
                }
            }
            return listOfAuthors;
        }

        //helping method for SetAuthors method
        private AUTHOR FindAuthorByID(int idOfAuthor)
        {
            AUTHOR author = new AUTHOR();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from AUTHOR where IDOFAUTHOR = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", idOfAuthor);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    author.IDOFAUTHOR = idOfAuthor;
                    author.NAMEOFATUTHOR = (String)reader["NAMEOFATUTHOR"];
                }
            }
            return author;
        }

        //method for setting languages for book
        private List<LANGUAGE> SetLanguages(BOOK book)
        {
            List<LANGUAGE> listOfLanguages = new List<LANGUAGE>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from Belonging2 where IDOFBOOK = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", book.IDOFBOOK);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    int idOfLanguage = (int)reader["IDOFLANGUAGE"];
                    LANGUAGE language = FindLanguageByID(idOfLanguage);
                    listOfLanguages.Add(language);
                }
            }
            return listOfLanguages;
        }

        //helping method for SetLanguages method
        private LANGUAGE FindLanguageByID(int idOfLanguage)
        {
            LANGUAGE language = new LANGUAGE();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from language where IDOFLANGUAGE = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", idOfLanguage);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    language.IDOFLANGUAGE = idOfLanguage;
                    language.NAMEOFTHELANGUAGE = (String)reader["NAMEOFTHELANGUAGE"];
                }
            }
            return language;
        }

    }
}