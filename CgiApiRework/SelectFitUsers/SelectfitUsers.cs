using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CgiApiRework.SelectFitUsers;
using Vacancy = CgiApiRework.SelectFitUsers.Vacancy;

namespace CgiApiRework.ChooseFitUsers
{
    public class SelectFitUsers
    {
        public void SelectUser(Vacancy vacancy)
        {
            List<int> skillId = new List<int>();
            List<User> selectedUsers = new List<User>();
            List<User> finalUsers = new List<User>();

            SqlCommand command = new SqlCommand {Connection = new SqlConnection(/*connectionstring*/)};

            // als de jobtype niet word opgeslagen in vacancy dan moet je de volgende code hebben

            //command.CommandText = "SELECT Skill_ID FROM Skill User WHERE Skill_name = " + vacancy.JobType;

            //int jobId = (int)ExecuteCommand(command).Rows[0][0];

            foreach (var skill in vacancy.RequiredSkills)
            {
                command.CommandText = "SELECT Skill_ID FROM Skill User WHERE Skill_name = " + skill.SkillTypeName;
                skillId.Add((int)ExecuteCommandAndReturnResults(command).Rows[0][0]);
            }
            
            foreach (int skillid in skillId)
            {
                command.CommandText = "SELECT UserID FROM User_Skill WHERE Skill_ID = " + skillId;
                selectedUsers.Add(new User((int)ExecuteCommandAndReturnResults(command).Rows[0][0]));
            }

            foreach (var user in selectedUsers)
            {
                command.CommandText = "SELECT Experience FROM Userjob WHERE SkillID = " + vacancy.JobType + " AND UserID = " + user;
                int experience = (int)ExecuteCommandAndReturnResults(command).Rows[0][0];

                if (experience >= vacancy.MinimalExperience)
                {
                    finalUsers.Add(user);
                }
            }

            foreach (User finalUser in finalUsers)
            {
                command.CommandText = "INSERT INTO AcceptedUser (UserID, VacancyID) VALUES ( " + finalUser.UserId + "," + vacancy.VacancyId + " )";
                SqlConnection conn = new SqlConnection();

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        private DataTable ExecuteCommandAndReturnResults(SqlCommand command)
        {
            SqlConnection conn = new SqlConnection();

            conn.Open();
            DataTable result = new DataTable();
            SqlDataAdapter exec = new SqlDataAdapter(command);
            exec.Fill(result);
            conn.Close();
            return result;
        }
    }
}
