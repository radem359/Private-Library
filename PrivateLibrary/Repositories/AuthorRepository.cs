using PrivateLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace PrivateLibrary.Repositories
{
    public class AuthorRepository
    {
        private NationalityRepository nationalityRepo = new NationalityRepository();

        static string ConnectionString = "server = localhost; database = privatelibrary; user id = root; password = root;";
        
        //getting all authors
        internal List<AUTHOR> GetAllAuthors()
        {
            List<AUTHOR> authors = new List<AUTHOR>();
            using (MySqlConnection connectionToBase = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand("select * from AUTHOR", connectionToBase);
                connectionToBase.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    AUTHOR author = new AUTHOR();
                    author.IDOFAUTHOR = (int)reader["IDOFAUTHOR"];
                    author.NAMEOFATUTHOR = (string)reader["NAMEOFATUTHOR"];
                    author.IDOFNATIONALITY = (int)reader["IDOFNATIONALITY"];
                    author.NATIONALITY = setNationalityByID(author.IDOFNATIONALITY);
                    author.BOOKs = GetAllBooksFromAuthor(author.NAMEOFATUTHOR);
                    authors.Add(author);
                }
             }
            return authors;
        }
        
        //set nationality by id
        private NATIONALITY setNationalityByID(int idOfNationality)
        {
            NATIONALITY nationality = new NATIONALITY();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from NATIONALITY where IDOFNATIONALITY = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", idOfNationality);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nationality.NAMEOFNATIONALITY = (string)reader["NAMEOFNATIONALITY"];
                    nationality.IDOFNATIONALITY = (int)reader["IDOFNATIONALITY"];
                }
            }
            return nationality;
        }

        public void UpdateAuthor(AUTHOR author, string nameOfNationality)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("update AUTHOR set IDOFNATIONALITY = @IdN, NAMEOFATUTHOR = '" + author.NAMEOFATUTHOR + "' where IDOFAUTHOR = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", author.IDOFAUTHOR);
                author.NATIONALITY = getNationality(nameOfNationality);
                author.IDOFNATIONALITY = author.NATIONALITY.IDOFNATIONALITY;
                command.Parameters.AddWithValue("@IdN", author.IDOFNATIONALITY);
                SqlDataReader reader = command.ExecuteReader();
            }
        }

        public NATIONALITY findNationalityById(int id)
        {
            NATIONALITY nationality = new NATIONALITY();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from NATIONALITY where IDOFNATIONALITY = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nationality.IDOFNATIONALITY = (int)reader["IDOFNATIONALITY"];
                    nationality.NAMEOFNATIONALITY = (string)reader["NAMEOFNATIONALITY"];
                }
            }
            return nationality;
        }

        public void DeleteAuthor(AUTHOR author)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("DELETE FROM WRITHER WHERE IDOFAUTHOR = @Id", connectionToBase);
                SqlCommand command1 = new SqlCommand("DELETE FROM  AUTHOR where IDOFAUTHOR = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", author.IDOFAUTHOR);
                command1.Parameters.AddWithValue("@Id", author.IDOFAUTHOR);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                reader = command1.ExecuteReader();
                reader.Close();
            }
        }

        internal AUTHOR findAuthorById(int id)
        {
            AUTHOR author = new AUTHOR();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from AUTHOR where IDOFAUTHOR = @Id", connectionToBase);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    author.IDOFAUTHOR = id;
                    author.NAMEOFATUTHOR = (string)reader["NAMEOFATUTHOR"];
                    author.IDOFNATIONALITY = (int)reader["IDOFNATIONALITY"];
                    author.NATIONALITY = setNationalityByID(author.IDOFNATIONALITY);
                    author.BOOKs = GetAllBooksFromAuthor(author.NAMEOFATUTHOR);
                }
            }
            return author;
        }


        //adding idOfAuthor because for some reason I cant send it from Update view to AuthorsController method update
        internal int addIdOfAuthor(AUTHOR author)
        {
            int id = 0;
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from AUTHOR where NAMEOFATUTHOR like '" + author.NAMEOFATUTHOR + "'", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = (int)reader["IDOFAUTHOR"];
                }
            }
            return id;
        }

        //return all book from database using just authors name
        private List<BOOK> GetAllBooksFromAuthor(String name)
        {
            List<BOOK> listOfBooks = new List<BOOK>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from AUTHOR where NAMEOFATUTHOR like '" + name + "'", connectionToBase);
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
                BOOK book = new BOOK();
                while (reader.Read())
                {

                    int idOfBook = (int)reader["IDOFBOOK"];
                    book = findBookById(idOfBook);
                    listOfBooks.Add(book);
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

        //Create Author
        public void CreateAuthor(AUTHOR aUTHOR, string nameOfNationality)
        {
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("insert into AUTHOR (IDOFAUTHOR, NAMEOFATUTHOR, IDOFNATIONALITY)  VALUES (@Id, '" + aUTHOR.NAMEOFATUTHOR + "',  @IdN)", connectionToBase);
                command.Parameters.AddWithValue("@Id", aUTHOR.IDOFAUTHOR);
                aUTHOR.NATIONALITY = getNationality(nameOfNationality);
                aUTHOR.IDOFNATIONALITY = aUTHOR.NATIONALITY.IDOFNATIONALITY;
                command.Parameters.AddWithValue("@IdN", aUTHOR.IDOFNATIONALITY);
                SqlDataReader reader = command.ExecuteReader();
            }
        }

        internal NATIONALITY getNationality(string nameOfNationality)
        {
            NATIONALITY nationality = new NATIONALITY();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from NATIONALITY where NAMEOFNATIONALITY like '" + nameOfNationality + "'", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nationality.IDOFNATIONALITY = (int)reader["IDOFNATIONALITY"];
                    nationality.NAMEOFNATIONALITY = (String)reader["NAMEOFNATIONALITY"];
                }
            }
            return nationality;
        }

        internal int addIdOfAuthor()
        {
            int id = 0;
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                connectionToBase.Open();
                SqlCommand command = new SqlCommand("select * from Author", connectionToBase);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int idOfBook = (int)reader["IDOFAUTHOR"];
                    if (id < idOfBook)
                    {
                        id = idOfBook;
                    }
                }
            }
            return id + 1;
        }

    }
}