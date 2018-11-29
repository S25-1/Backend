using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cgiAPI.Controllers;

namespace CgiApiRework.Models
{
    public class Employee : User
    {
        public List<Availability> AvailabilityList { get; set; }
        public List<Vacancy> ListAppliedJobs { get; set; }

        public Employee(int userID, string name, string email, string password, DateTime dateOfBirth, string phoneNumber, decimal hourly_wage, int userTypeID, Address address, Job_Type job, Branch branch, List<Skill> skillList, List<Availability> availabilityList, List<Vacancy> listAppliedJobs) : base(userID, name, email, password, dateOfBirth, phoneNumber, hourly_wage, userTypeID, address, job, branch, skillList)
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
            AvailabilityList = availabilityList;
            ListAppliedJobs = listAppliedJobs;
        }

        public Employee(string name, string email, string password, DateTime dateOfBirth, string phoneNumber, decimal hourly_wage, int userTypeID, Address address, Job_Type job, Branch branch, List<Skill> skillList, List<Availability> availabilityList, List<Vacancy> listAppliedJobs) : base(-1, name, email, password, dateOfBirth, phoneNumber, hourly_wage, userTypeID, address, job, branch, skillList)
        {
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
            AvailabilityList = availabilityList;
            ListAppliedJobs = listAppliedJobs;
        }
    }
}