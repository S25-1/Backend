using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CgiApiRework.Models
{
    public class Employer : User
    {
        public List<Vacancy> ListVacancies { get; private set; }

        static private string connectionString = @"Server=cgi-matchup.database.windows.net;Database=MatchUp;User ID = cgi; Password=Fontys12345;Trusted_Connection=False;";
        //static private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mike\OneDrive\school\mike_backend\CGIdatabase.mdf;Integrated Security=True;Connect Timeout=30";
        static private SqlConnection conn = new SqlConnection(connectionString);

        public Employer(int userID, string name, string email, string password, DateTime dateOfBirth, string phoneNumber, int userTypeID, decimal hourly_wage, Address address, Job_Type job, Branch branch, List<Skill> skillList, List<Vacancy> listVacancies) : base(userID, name, email, password, dateOfBirth, phoneNumber, hourly_wage, userTypeID, address, job, branch, skillList)
        {
            UserID = userID;
            Name = name;
            Email = email;
            Password = password;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Address = address;
            Job = job;
            Hourly_wage = hourly_wage;
            Branch = branch;
            UserTypeID = userTypeID;
            SkillList = skillList;
            ListVacancies = listVacancies;
        }

        public Employer() : base(0, "null", "null", "null", new DateTime(), "null",0 , 0, new Address(0), new Job_Type(0), new Branch(0), new List<Skill>())
        {

        }

        //public void CreateVacancy(string name, string requiredJob, List<SkillType> reqCompetence, string description, DateTime beginDate, DateTime endDate)
        //{
        //    JobOffer job = new JobOffer(this, name, requiredJob, reqCompetence, description, beginDate, endDate);

        //}

        public void CreateVacancy(Vacancy jobOffer)
        {
            
        }

        static public Employer GetEmployer(int userID)
        {
            Employer employer = new Employer();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    //Get User table
                    command.Parameters.AddWithValue("@UserID", userID);

                    command.CommandText = "SELECT * FROM (SELECT u.UserID,u.Name,u.Email,u.Password,u.Date_of_birth,u.Phone_number,u.UserTypeID,u.Hourly_wage,a.AddressID,a.City,a.Country,a.House_number,a.Postal_code,a.Province,a.Street_Name FROM [User] u, Address a WHERE u.AddressID = a.AddressID AND u.UserID = 1) as u, (SELECT j.Job_typeID, j.Job_name FROM [User] u, Job_Type j WHERE u.Job_TypeID = j.Job_typeID AND u.UserID = 1) as j ,(SELECT b.BranchID, b.Name, a.AddressID, a.City, a.Country, a.House_number, a.Postal_code,a.Province,a.Street_name FROM Branch b , Address a, [User] u WHERE b.AddressID = a.AddressID AND u.BranchID = b.BranchID AND u.UserID = 1) AS b ";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                employer = new Employer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                                        reader.GetDateTime(4), reader.GetString(5), reader.GetInt32(6),
                                                        reader.GetDecimal(7), new Address(reader.GetInt32(8), reader.GetString(9),
                                                        reader.GetString(10), reader.GetString(11), reader.GetString(12),
                                                        reader.GetString(13)), new Job_Type(reader.GetInt32(14), reader.GetString(15)),
                                                        new Branch(reader.GetInt32(16), reader.GetString(17),
                                                        new Address(reader.GetInt32(18), reader.GetString(19), reader.GetString(20),
                                                        reader.GetString(21), reader.GetString(22), reader.GetString(23))), 
                                                        new List<Skill>(), new List<Vacancy>()
                                                       );
                            }
                            command.Parameters.RemoveAt("@UserID");
                        }
                    }

                    //Get vacancy table
                    command.Parameters.AddWithValue("@UserID", userID);

                    command.CommandText = "SELECT * FROM (SELECT u.UserID,u.Name,u.Email,u.Password,u.Date_of_birth,u.Phone_number,u.UserTypeID,u.Hourly_wage,a.AddressID,a.City,a.Country,a.House_number,a.Postal_code,a.Province,a.Street_Name FROM [User] u, Address a WHERE u.AddressID = a.AddressID AND u.UserID = 1) as u, (SELECT j.Job_typeID, j.Job_name FROM [User] u, Job_Type j WHERE u.Job_TypeID = j.Job_typeID AND u.UserID = 1) as j ,(SELECT b.BranchID, b.Name, a.AddressID, a.City, a.Country, a.House_number, a.Postal_code,a.Province,a.Street_name FROM Branch b , Address a, [User] u WHERE b.AddressID = a.AddressID AND u.BranchID = b.BranchID AND u.UserID = 1) AS b ";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                employer = new Employer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                                        reader.GetDateTime(4), reader.GetString(5), reader.GetInt32(6),
                                                        reader.GetDecimal(7), new Address(reader.GetInt32(8), reader.GetString(9),
                                                        reader.GetString(10), reader.GetString(11), reader.GetString(12), reader.GetString(13)), 
                                                        new Job_Type(reader.GetInt32(14), reader.GetString(15)),
                                                        new Branch(reader.GetInt32(16), reader.GetString(17),
                                                        new Address(reader.GetInt32(18), reader.GetString(19),
                                                        reader.GetString(20), reader.GetString(21), reader.GetString(22),
                                                        reader.GetString(23))), new List<Skill>(), new List<Vacancy>()
                                                       );
                            }
                            command.Parameters.RemoveAt("@UserID");
                        }
                    }

                    //Get Address table
                    //command.Parameters.AddWithValue("@AddressID", employer.Address.AddressID);

                    //command.CommandText = "SELECT * FROM Address WHERE AddressID = @AddressID";
                    //using (SqlDataReader reader = command.ExecuteReader())
                    //{
                    //    if (reader.HasRows)
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            employer.Address.Country = reader.GetString(1);
                    //            employer.Address.Province = reader.GetString(2);
                    //            employer.Address.City = reader.GetString(3);
                    //            employer.Address.Street_name = reader.GetString(4);
                    //            employer.Address.House_number = reader.GetString(5);
                    //            employer.Address.Postal_code = reader.GetString(6);
                    //        }
                    //        command.Parameters.RemoveAt("@AddressID");
                    //    }
                    //}

                    // Attempt to commit the transaction.
                    transaction.Commit();

                    Console.WriteLine("Both records are written to database.");
                    return employer;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                    return employer;
                }
            }
        }
        //public ArrayList GetListRespondVacancyUser(int vacancyID, int statusID)
        //{
        //    return VacancyAPI.GetListRespondVacancyUser(vacancyID, statusID);
        //}


    }
}
