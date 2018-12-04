using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CgiApiRework.Models
{
    public class User
    {
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; protected set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public Job_Type Job { get; set; }
        public decimal Hourly_wage { get; set; }
        public List<Skill> SkillList { get; set; }
        public Branch Branch { get; set; }

        static private string connectionString = Startup.ConnectionString;
        //static private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mike\OneDrive\school\mike_backend\CGIdatabase.mdf;Integrated Security=True;Connect Timeout=30";
        static private SqlConnection conn = new SqlConnection(connectionString);

        public User(int userID, string name, string email, string password, DateTime dateOfBirth, string phoneNumber, decimal hourly_wage, int userTypeID, Address address, Job_Type job, Branch branch, List<Skill> skillList)
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
        }

        [JsonConstructor]
        public User(string name, string email, string password, DateTime dateOfBirth, string phoneNumber, decimal hourly_wage, Address address, Job_Type job, List<int> skillList)
        {
            UserID = -1;
            Name = name;
            Email = email;
            Password = password;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Address = address;
            Job = job;
            Hourly_wage = hourly_wage;
            Branch = new Branch(1);
            UserTypeID = 1;
            SkillList = new List<Skill>();
            foreach (int item in skillList)
            {
                SkillList.Add(new Skill(item));
            }
        }

        public static void test(User user)
        {
            User kek = user;
        }

        public static string AddUser(User user)
        {
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
                    command.Parameters.AddWithValue("@Country", user.Address.Country);
                    command.Parameters.AddWithValue("@City", user.Address.City);
                    command.Parameters.AddWithValue("@Street_name", user.Address.Street_name);
                    command.Parameters.AddWithValue("@House_number", user.Address.House_number);
                    command.Parameters.AddWithValue("@Postal_code", user.Address.Postal_code);

                    command.CommandText =
                        "INSERT INTO Address (Country, City, Street_name, House_number, Postal_code) " + "VALUES (@Country, @City, @Street_name, @House_number, @Postal_code)";
                    command.ExecuteNonQuery();

                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Date_of_birth", user.DateOfBirth);
                    command.Parameters.AddWithValue("@Phone_number", user.PhoneNumber);
                    command.Parameters.AddWithValue("@Job_TypeID", user.Job.Job_typeID);
                    command.Parameters.AddWithValue("@Hourly_wage", user.Hourly_wage);
                    command.Parameters.AddWithValue("@Branch_ID", user.Branch.BranchID);
                    command.Parameters.AddWithValue("@UserTypeID", user.UserTypeID);

                    command.CommandText =
                        "INSERT INTO [User] (Name, Email, Password, Date_of_birth, Phone_number, AddressID, Job_TypeID, Hourly_wage, BranchID, UserTypeID) SELECT @Name, @Email, @Password, @Date_of_birth, @Phone_number, a.AddressID, @Job_TypeID, @Hourly_wage, @Branch_ID, @UserTypeID FROM (SELECT AddressID FROM Address a WHERE a.City = @City AND a.Country = @Country AND a.House_number = @House_number AND a.Postal_code = @Postal_code AND a.Street_name = @Street_name) as a ";
                    command.ExecuteNonQuery();

                    command.Parameters.AddWithValue("@UserID", user.UserID);
                    foreach (Skill skill in user.SkillList)
                    {
                        command.CommandText =
                        "INSERT INTO User_Skill (Skill_ID, UserID) SELECT @Skill_ID, u.UserID FROM [User] u WHERE u.Name = @Name AND u.Email = @Email AND u.Password = @Password AND u.Date_of_birth = @Date_of_birth AND u.Phone_number = @Phone_number AND u.Job_TypeID = @Job_TypeID AND u.Hourly_wage = @Hourly_wage AND u.BranchID = @Branch_ID AND u.UserTypeID = @UserTypeID";
                        command.Parameters.AddWithValue("@Skill_ID", skill.SkillTypeID);
                        command.ExecuteNonQuery();
                        command.Parameters.RemoveAt("@Skill_ID");
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
    }
}
