using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CgiApiRework.Models
{
    public class Branch
    {
        public int BranchID { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }

        public Branch(int branchID, string name, Address address)
        {
            BranchID = branchID;
            Name = name;
            Address = address;
        }
        public Branch(int branchID)
        {
            BranchID = branchID;
            Name = "null";
            Address = new Address(0);
        }
    }
}