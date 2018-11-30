using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CgiApiRework.EfModels
{
    [Table("Skill")]
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}
