using PrivateLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PrivateLibrary.Repositories
{
    public class NationalityRepository
    {

        static string ConnectionString = "data source=.; database=PrivateLibrary; integrated security = SSPI";

        internal List<NATIONALITY> GetAllNationalities()
        {
            List<NATIONALITY> nationalities = new List<NATIONALITY>();
            using (SqlConnection connectionToBase = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("select * from NATIONALITY", connectionToBase);
                connectionToBase.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    NATIONALITY nationality = new NATIONALITY();
                    nationality.IDOFNATIONALITY = (int)reader["IDOFNATIONALITY"];
                    nationality.NAMEOFNATIONALITY = (string)reader["NAMEOFNATIONALITY"];
                    nationalities.Add(nationality);
                }
            }
            return nationalities;
        }


    }
}