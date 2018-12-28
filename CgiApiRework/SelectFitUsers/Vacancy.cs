using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CgiApiRework.Models;

namespace CgiApiRework.SelectFitUsers
{
    public class Vacancy
    {
        public int VacancyId { get; set; }
        public int JobType { get; set; }
        public List<Skill> RequiredSkills { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MinimalExperience { get; set; }
    }
}
