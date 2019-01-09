using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CgiApiRework.Models;

namespace CgiApiRework.Models
{
    public class Vacancy
    {
        public int VacancyID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public int JobType { get; set; }
        public List<Skill> RequiredSkills { get; set; }
        public string Description { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MinimalExperience { get; set; }
        public List<RespondVacancyUser> RespondVacancyUserList { get; set; }

        private static readonly string ConnectionString = Startup.ConnectionString;

        public Vacancy(string userID, string name, int jobType, List<Skill> requiredSkills, string description, DateTime beginDateTime, DateTime endDateTime, int minimalExperience)
        {
            this.VacancyID = -1;
            this.UserID = userID;
            this.Name = name;
            this.JobType = jobType;
            this.RequiredSkills = requiredSkills;
            this.Description = description;
            this.BeginDateTime = beginDateTime;
            this.EndDateTime = endDateTime;
            this.MinimalExperience = minimalExperience;
        }

        [JsonConstructor]
        public Vacancy(string userID, string name, int jobType, string description, int minimalExperience, DateTime beginDateTime, DateTime endDateTime, List<int> requiredSkills)
        {
            this.VacancyID = -1;
            this.UserID = userID;
            this.Name = name;
            this.JobType = jobType;
            this.RequiredSkills = new List<Skill>();
            foreach (int item in requiredSkills)
            {
                RequiredSkills.Add(new Skill(item));
            }
            this.Description = description;
            this.BeginDateTime = beginDateTime;
            this.EndDateTime = endDateTime;
            this.MinimalExperience = minimalExperience;
        }


        public Vacancy(int vacancyID, string userID, string name, int jobType,string description, int minimalExperience, DateTime beginDateTime, DateTime endDateTime, List<int> requiredSkills)
        {
            this.VacancyID = vacancyID;
            this.UserID = userID;
            this.Name = name;
            this.JobType = jobType;
            this.RequiredSkills = new List<Skill>();
            foreach (int item in requiredSkills)
            {
                RequiredSkills.Add(new Skill(item));
            }
            this.Description = description;
            this.BeginDateTime = beginDateTime;
            this.EndDateTime = endDateTime;
            this.MinimalExperience = minimalExperience;
        }

        //add methods
        public static string AddVacancy(Vacancy Vacancy)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.Parameters.AddWithValue("@UserID", Vacancy.UserID);
                    command.Parameters.AddWithValue("@Job_TypeID", Vacancy.JobType);
                    command.Parameters.AddWithValue("@Date_begin", Vacancy.BeginDateTime);
                    command.Parameters.AddWithValue("@Date_end", Vacancy.EndDateTime);
                    command.Parameters.AddWithValue("@Description", Vacancy.Description);
                    command.Parameters.AddWithValue("@MinMonthsExperience", Vacancy.MinimalExperience);
                    command.Parameters.AddWithValue("@Name", Vacancy.Name);

                    command.CommandText =
                        "INSERT INTO Vacancy (UserID, Job_TypeID, Date_begin, Date_end, Description, MinMonthsExperience, Name) " + "VALUES (@UserID, @Job_TypeID, @Date_begin, @Date_end, @Description, @MinMonthsExperience, @Name)";
                    command.ExecuteNonQuery();

                    foreach (Skill skill in Vacancy.RequiredSkills)
                    {
                        command.CommandText =
                        "INSERT INTO Skill_Vacancy (Skill_ID, VacancyID) SELECT @SkillTypeID, VacancyID FROM Vacancy WHERE UserID=@UserID AND Job_TypeID=@Job_TypeID AND Date_begin=@Date_begin AND Date_end=@Date_end AND Description=@Description AND MinMonthsExperience=@MinMonthsExperience AND Name=@Name";
                        command.Parameters.AddWithValue("@SkillTypeID", skill.SkillTypeID);
                        command.ExecuteNonQuery();
                        command.Parameters.RemoveAt("@SkillTypeID");
                    }

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                    return "Record added to the database";
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
                    return ex.Message;
                }
            }
        }

        public static bool AddRespondVacancyUser(RespondVacancyUser user)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.Parameters.AddWithValue("@VacancyID", user.VacancyID);
                    command.Parameters.AddWithValue("@UserID", user.UserID);
                    command.Parameters.AddWithValue("@StatusID", user.UserStatusID);
                    command.CommandText =
                        "INSERT INTO AcceptedUser (UserID, VacancyID, StatusID) " + "VALUES (@UserID, @VacancyID, @StatusID)";
                    command.ExecuteNonQuery();

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                    return true;
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
                    return false;
                }
            }
        }


        //update methods

        static public bool UpdateRespondVacancyUser(RespondVacancyUser employee)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.Parameters.AddWithValue("@VacancyID", employee.VacancyID);
                    command.Parameters.AddWithValue("@UserID", employee.UserID);
                    command.Parameters.AddWithValue("@StatusID", employee.UserStatusID);
                    command.CommandText = "UPDATE AcceptedUser SET StatusID = @StatusID WHERE VacancyID = @VacancyID AND UserID = @UserID";
                    command.ExecuteNonQuery();

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                    return true;
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
                    return false;
                }
            }
        }

        static public bool UpdateRespondVacancyUser(List<RespondVacancyUser> userList)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    foreach (RespondVacancyUser user in userList)
                    {
                        command.Parameters.AddWithValue("@VacancyID", user.VacancyID);
                        command.Parameters.AddWithValue("@UserID", user.UserID);
                        command.Parameters.AddWithValue("@StatusID", user.UserStatusID);
                        command.CommandText = "UPDATE AcceptedUser SET StatusID = @StatusID WHERE VacancyID = @VacancyID AND UserID = @UserID";
                        command.ExecuteNonQuery();
                        command.Parameters.RemoveAt("@VacancyID");
                        command.Parameters.RemoveAt("@UserID");
                        command.Parameters.RemoveAt("@StatusID");
                    }
                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                    
                    return true;
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
                    return false;
                }
            }
        }
        //Get methods
        public static ArrayList GetListRespondVacancyUser()
        {
            ArrayList RespondVacancyUserList = new ArrayList();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.CommandText = "SELECT v.VacancyID, v.Name, v.Description, j.Job_name, au.UserID, u.UserName, s.StatusID ,s.Status_name, u.PhoneNumber, u.Email, v.Date_begin, v.Date_end FROM AcceptedUser au, AspNetUsers u, Vacancy v, Job_Type j, Status s WHERE au.UserID = u.Id AND v.VacancyID = au.VacancyID AND v.Job_TypeID = j.Job_typeID";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                RespondVacancyUser RespondVacancyUser = new RespondVacancyUser(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)
                                    , reader.GetString(5), reader.GetInt32(6), reader.GetString(7), reader.GetString(8), reader.GetString(9),
                                    reader.GetDateTime(10), reader.GetDateTime(11));
                                RespondVacancyUserList.Add(RespondVacancyUser);
                            }
                        }
                    }
                    // Attempt to commit the transaction.
                    transaction.Commit();

                    Console.WriteLine("Both records are written to database.");

                    return RespondVacancyUserList;

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
                    return RespondVacancyUserList;
                }
            }
        }

        public static ArrayList GetListRespondVacancyUser(string userID)
        {
            ArrayList RespondVacancyUserList = new ArrayList();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.Parameters.AddWithValue("@userID", userID);
                    command.CommandText = "SELECT v.VacancyID, v.Name, v.Description, j.Job_name, au.UserID, u.UserName, s.StatusID ,s.Status_name, u.PhoneNumber, u.Email, v.Date_begin, v.Date_end FROM AcceptedUser au, AspNetUsers u, Vacancy v, Job_Type j, Status s WHERE au.UserID = u.Id AND v.VacancyID = au.VacancyID AND v.Job_TypeID = j.Job_typeID AND au.StatusID = s.StatusID AND au.UserID = @userID";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                RespondVacancyUser RespondVacancyUser = new RespondVacancyUser(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)
                                    , reader.GetString(5), reader.GetInt32(6), reader.GetString(7), reader.GetString(8), reader.GetString(9),
                                    reader.GetDateTime(10), reader.GetDateTime(11));
                                RespondVacancyUserList.Add(RespondVacancyUser);
                            }
                        }
                    }
                    // Attempt to commit the transaction.
                    transaction.Commit();

                    Console.WriteLine("Both records are written to database.");

                    return RespondVacancyUserList;

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
                    return RespondVacancyUserList;
                }
            }
        }
        public static ArrayList GetListVacancy()
        {
            ArrayList vacancyList = new ArrayList();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.CommandText = "SELECT v.VacancyID, v.UserID, v.Name, v.Job_typeID, v.Description, v.MinMonthsExperience ,v.Date_begin, v.Date_end FROM Vacancy v";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Vacancy vacancy = new Vacancy(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetInt32(5), reader.GetDateTime(6), reader.GetDateTime(7), new List<int>());
                                vacancyList.Add(vacancy);
                            }
                        }
                    }

                    foreach (Vacancy v in vacancyList)
                    {
                        v.RequiredSkills = new List<Skill>();

                        command.CommandText = "SELECT Skill.Skill_ID, Skill.Skill_name FROM Skill_Vacancy, Skill WHERE Skill_Vacancy.Skill_ID = Skill.Skill_ID AND Skill_Vacancy.VacancyID = @VacancyID";
                        command.Parameters.AddWithValue("@VacancyID", v.VacancyID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    v.RequiredSkills.Add(new Skill(reader.GetInt32(0), reader.GetString(1)));
                                }
                            }
                            command.Parameters.RemoveAt("@VacancyID");
                        }

                        command.Parameters.AddWithValue("@Job_TypeID", v.JobType);

                        command.CommandText = "SELECT Job_TypeID FROM Job_Type WHERE Job_TypeID = @Job_TypeID";
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    v.JobType = reader.GetInt32(0);
                                }
                            }
                            command.Parameters.RemoveAt("@Job_TypeID");
                        }

                        v.RespondVacancyUserList = new List<RespondVacancyUser>();

                        //casting arraylist to class type
                        foreach (RespondVacancyUser item in Vacancy.GetListRespondVacancyUser(v.VacancyID))
                        {
                            v.RespondVacancyUserList.Add(item);
                        }

                        //command.CommandText = "SELECT * FROM dbo.AcceptedUser WHERE VacancyID = @VacancyID";
                        //command.Parameters.AddWithValue("@VacancyID", v.VacancyID);
                        //using (SqlDataReader reader = command.ExecuteReader())
                        //{
                        //    if (reader.HasRows)
                        //    {
                        //        while (reader.Read())
                        //        {
                        //            v.RespondVacancyUserList.Add(new RespondVacancyUser(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)));
                        //        }
                        //    }
                        //    command.Parameters.RemoveAt("@VacancyID");
                        //}
                    }


                    // Attempt to commit the transaction.
                    transaction.Commit();

                    Console.WriteLine("Both records are written to database.");
                    return vacancyList;

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
                    return vacancyList;
                }
            }
        }

        public static ArrayList GetListVacancyFilterASC(string columnName)
        {
            ArrayList vacancyList = new ArrayList();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.Parameters.AddWithValue("@columnName", columnName);
                    command.CommandText = "EXECUTE GetVacancyListASC @columnName";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Vacancy vacancy = new Vacancy(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetInt32(5), reader.GetDateTime(6), reader.GetDateTime(7), new List<int>());
                                vacancyList.Add(vacancy);
                            }
                        }
                    }

                    foreach (Vacancy v in vacancyList)
                    {
                        v.RequiredSkills = new List<Skill>();

                        command.CommandText = "SELECT Skill.Skill_ID, Skill.Skill_name FROM Skill_Vacancy, Skill WHERE Skill_Vacancy.Skill_ID = Skill.Skill_ID AND Skill_Vacancy.VacancyID = @VacancyID";
                        command.Parameters.AddWithValue("@VacancyID", v.VacancyID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    v.RequiredSkills.Add(new Skill(reader.GetInt32(0), reader.GetString(1)));
                                }
                            }
                            command.Parameters.RemoveAt("@VacancyID");
                        }

                        command.Parameters.AddWithValue("@Job_TypeID", v.JobType);

                        command.CommandText = "SELECT Job_TypeID FROM Job_Type WHERE Job_TypeID = @Job_TypeID";
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    v.JobType = reader.GetInt32(0);
                                }
                            }
                            command.Parameters.RemoveAt("@Job_TypeID");
                        }

                        v.RespondVacancyUserList = new List<RespondVacancyUser>();

                        //casting arraylist to class type
                        foreach (RespondVacancyUser item in Vacancy.GetListRespondVacancyUser(v.VacancyID))
                        {
                            v.RespondVacancyUserList.Add(item);
                        }

                        //command.CommandText = "SELECT * FROM dbo.AcceptedUser WHERE VacancyID = @VacancyID";
                        //command.Parameters.AddWithValue("@VacancyID", v.VacancyID);
                        //using (SqlDataReader reader = command.ExecuteReader())
                        //{
                        //    if (reader.HasRows)
                        //    {
                        //        while (reader.Read())
                        //        {
                        //            v.RespondVacancyUserList.Add(new RespondVacancyUser(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)));
                        //        }
                        //    }
                        //    command.Parameters.RemoveAt("@VacancyID");
                        //}
                    }


                    // Attempt to commit the transaction.
                    transaction.Commit();

                    Console.WriteLine("Both records are written to database.");
                    return vacancyList;

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
                    return vacancyList;
                }
            }
        }

        public static ArrayList GetListVacancyFilterDESC(string columnName)
        {
            ArrayList vacancyList = new ArrayList();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.Parameters.AddWithValue("@columnName", columnName);
                    command.CommandText = "EXECUTE GetVacancyListDESC @columnName";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Vacancy vacancy = new Vacancy(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetInt32(5), reader.GetDateTime(6), reader.GetDateTime(7), new List<int>());
                                vacancyList.Add(vacancy);
                            }
                        }
                    }

                    foreach (Vacancy v in vacancyList)
                    {
                        v.RequiredSkills = new List<Skill>();

                        command.CommandText = "SELECT Skill.Skill_ID, Skill.Skill_name FROM Skill_Vacancy, Skill WHERE Skill_Vacancy.Skill_ID = Skill.Skill_ID AND Skill_Vacancy.VacancyID = @VacancyID";
                        command.Parameters.AddWithValue("@VacancyID", v.VacancyID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    v.RequiredSkills.Add(new Skill(reader.GetInt32(0), reader.GetString(1)));
                                }
                            }
                            command.Parameters.RemoveAt("@VacancyID");
                        }

                        command.Parameters.AddWithValue("@Job_TypeID", v.JobType);

                        command.CommandText = "SELECT Job_TypeID FROM Job_Type WHERE Job_TypeID = @Job_TypeID";
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    v.JobType = reader.GetInt32(0);
                                }
                            }
                            command.Parameters.RemoveAt("@Job_TypeID");
                        }

                        v.RespondVacancyUserList = new List<RespondVacancyUser>();

                        //casting arraylist to class type
                        foreach (RespondVacancyUser item in Vacancy.GetListRespondVacancyUser(v.VacancyID))
                        {
                            v.RespondVacancyUserList.Add(item);
                        }

                        //command.CommandText = "SELECT * FROM dbo.AcceptedUser WHERE VacancyID = @VacancyID";
                        //command.Parameters.AddWithValue("@VacancyID", v.VacancyID);
                        //using (SqlDataReader reader = command.ExecuteReader())
                        //{
                        //    if (reader.HasRows)
                        //    {
                        //        while (reader.Read())
                        //        {
                        //            v.RespondVacancyUserList.Add(new RespondVacancyUser(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)));
                        //        }
                        //    }
                        //    command.Parameters.RemoveAt("@VacancyID");
                        //}
                    }


                    // Attempt to commit the transaction.
                    transaction.Commit();

                    Console.WriteLine("Both records are written to database.");
                    return vacancyList;

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
                    return vacancyList;
                }
            }
        }

        public static ArrayList GetListVacancy(int vacancyID)
        {
            ArrayList vacancyList = new ArrayList();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.Parameters.AddWithValue("@VacancyID", vacancyID);

                    command.CommandText = "SELECT v.VacancyID, v.UserID, v.Name, v.Job_typeID, v.Description, v.MinMonthsExperience ,v.Date_begin, v.Date_end FROM Vacancy v WHERE VacancyID = @VacancyID";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Vacancy vacancy = new Vacancy(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetInt32(5), reader.GetDateTime(6), reader.GetDateTime(7), new List<int>());
                                vacancyList.Add(vacancy);
                            }
                            command.Parameters.RemoveAt("@VacancyID");
                        }
                    }

                    foreach (Vacancy v in vacancyList)
                    {
                        v.RequiredSkills = new List<Skill>();

                        command.CommandText = "SELECT Skill.Skill_ID, Skill.Skill_name FROM Skill_Vacancy, Skill WHERE Skill_Vacancy.Skill_ID = Skill.Skill_ID AND Skill_Vacancy.VacancyID = @VacancyID";
                        command.Parameters.AddWithValue("@VacancyID", v.VacancyID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    v.RequiredSkills.Add(new Skill(reader.GetInt32(0), reader.GetString(1)));
                                }
                            }
                            command.Parameters.RemoveAt("@VacancyID");
                        }

                        command.Parameters.AddWithValue("@Job_TypeID", v.JobType);

                        command.CommandText = "SELECT Job_TypeID FROM Job_Type WHERE Job_TypeID = @Job_TypeID";
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    v.JobType = reader.GetInt32(0);
                                }
                            }
                            command.Parameters.RemoveAt("@Job_TypeID");
                        }

                        v.RespondVacancyUserList = new List<RespondVacancyUser>();
                        //casting arraylist to class type
                        foreach (RespondVacancyUser item in GetListRespondVacancyUser(vacancyID))
                        {
                            v.RespondVacancyUserList.Add(item);
                        }
                    }


                    // Attempt to commit the transaction.
                    transaction.Commit();

                    Console.WriteLine("Both records are written to database.");
                    return vacancyList;

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
                    return vacancyList;
                }
            }
        }


        static public ArrayList GetListRespondVacancyUser(int vacancyID, int statusID)
        {
            ArrayList RespondVacancyUserList = new ArrayList();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.Parameters.AddWithValue("@VacancyID", vacancyID);
                    command.Parameters.AddWithValue("@StatusID", statusID);

                    command.CommandText = "SELECT v.VacancyID, v.Name, v.Description, j.Job_name, au.UserID, u.UserName, s.StatusID ,s.Status_name, u.PhoneNumber, u.Email, v.Date_begin, v.Date_end FROM AcceptedUser au, AspNetUsers u, Vacancy v, Job_Type j, Status s WHERE au.UserID = u.Id AND v.VacancyID = au.VacancyID AND v.Job_TypeID = j.Job_typeID AND au.StatusID = s.StatusID AND au.VacancyID = @VacancyID AND au.StatusID = @StatusID";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                RespondVacancyUser RespondVacancyUser = new RespondVacancyUser(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)
                                , reader.GetString(5), reader.GetInt32(6), reader.GetString(7), reader.GetString(8), reader.GetString(9),
                                reader.GetDateTime(10), reader.GetDateTime(11));
                                RespondVacancyUserList.Add(RespondVacancyUser);
                            }
                        }
                    }
                    // Attempt to commit the transaction.
                    transaction.Commit();

                    Console.WriteLine("Both records are written to database.");

                    return RespondVacancyUserList;

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
                    return RespondVacancyUserList;
                }
            }
        }

        static public ArrayList GetListRespondVacancyUser(int vacancyID)
        {
            ArrayList RespondVacancyUserList = new ArrayList();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                    command.Parameters.AddWithValue("@VacancyID", vacancyID);

                    command.CommandText = "SELECT v.VacancyID, v.Name, v.Description, j.Job_name, au.UserID, u.UserName, s.StatusID ,s.Status_name, u.PhoneNumber, u.Email, v.Date_begin, v.Date_end FROM AcceptedUser au, AspNetUsers u, Vacancy v, Job_Type j, Status s WHERE au.UserID = u.Id AND v.VacancyID = au.VacancyID AND v.Job_TypeID = j.Job_typeID AND au.StatusID = s.StatusID AND au.VacancyID = @VacancyID ";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                RespondVacancyUser RespondVacancyUser = new RespondVacancyUser(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)
                                , reader.GetString(5), reader.GetInt32(6), reader.GetString(7), reader.GetString(8), reader.GetString(9),
                                reader.GetDateTime(10), reader.GetDateTime(11));
                                RespondVacancyUserList.Add(RespondVacancyUser);
                            }
                        }
                    }
                    // Attempt to commit the transaction.
                    transaction.Commit();

                    Console.WriteLine("Both records are written to database.");

                    return RespondVacancyUserList;

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
                    return RespondVacancyUserList;
                }
            }
        }
    }
}
