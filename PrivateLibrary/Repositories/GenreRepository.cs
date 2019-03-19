using PrivateLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PrivateLibrary.Repositories
{
    public class GenreRepository
    {

        static string ConnectionString = "data source=.; database=PrivateLibrary; integrated security = SSPI";

        internal List<GENRE> GetAllGenres()
        {
            List<GENRE> genres = new List<GENRE>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("select * from GENRE", connectionToBase);
                connectionToBase.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    GENRE genre = new GENRE();
                    genre.IDOFGENRE = (int)reader["IDOFGENRE"];
                    genre.NAMEOFGENRE = (string)reader["NAMEOFGENRE"];
                    genre.BOOKs = setBooks(genre.IDOFGENRE);
                    genres.Add(genre);
                }
            }
            return genres;
        }

        private List<BOOK> setBooks(int idOfGenre)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from BELONGING where IDOFGENRE = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", idOfGenre);
                SqlDataReader reader = command.ExecuteReader();
                int idOfBook = 0;
                while (reader.Read())
                {
                    idOfBook = (int)reader["IDOFBOOK"];
                }
                listOfBooks = getAllBooksFromId(idOfBook);
            }
            return listOfBooks;
        }

        public void CreateNewGenre(GENRE genre)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("insert into GENRE (IDOFGENRE, NAMEOFGENRE)  VALUES (@Id, '" + genre.NAMEOFGENRE + "')", connectionToBase);
                command.Parameters.AddWithValue("@Id", genre.IDOFGENRE);
                SqlDataReader reader = command.ExecuteReader();
            }
        }

        private List<BOOK> getAllBooksFromId(int id)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from Book where IDOFBOOK = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BOOK book = new BOOK();
                    book.IDOFBOOK = id;
                    book.ISBNNUMBER = (int)reader["ISBNNUMBER"];
                    book.TITLE = (String)reader["TITLE"];
                    book.DESCRIPTION = (String)reader["Description"];
                    book.GENREs = SetGenres(book);
                    book.LANGUAGEs = SetLanguages(book);
                    book.AUTHORs = SetAuthors(book);
                    listOfBooks.Add(book);
                }
            }
            return listOfBooks;
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

        internal int addIdOfGenre()
        {
            int id = 0;
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from Genre", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int idOfBook = (int)reader["IDOFGENRE"];
                    if (id < idOfBook)
                    {
                        id = idOfBook;
                    }
                }
            }
            return id + 1;
        }

        internal GENRE findById(int id)
        {
            GENRE genre = new GENRE();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from GENRE where IDOFGENRE = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    genre.IDOFGENRE = (int)reader["IDOFGENRE"];
                    genre.NAMEOFGENRE = (string)reader["NAMEOFGENRE"];
                }
            }
            return genre;
        }

        public void UpdateGenre(GENRE genre)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("update GENRE set NAMEOFGENRE = '" + genre.NAMEOFGENRE + "' where IDOFGENRE = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", genre.IDOFGENRE);
                SqlDataReader reader = command.ExecuteReader();
            }
        }

        public void DeleteGenre(GENRE genre)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("DELETE FROM BELONGING WHERE IDOFGENRE = @Id", connectionToBase);
                SqlCommand command1 = new SqlCommand("DELETE FROM  GENRE where IDOFGENRE = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", genre.IDOFGENRE);
                command1.Parameters.AddWithValue("@Id", genre.IDOFGENRE);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                reader = command1.ExecuteReader();
                reader.Close();
            }
        }

    }
}