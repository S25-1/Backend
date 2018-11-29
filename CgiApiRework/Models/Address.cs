using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CgiApiRework.Models
{
    public class Address
    {
        public int AddressID { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("house_number")]
        public string House_number { get; set; }
        [JsonProperty("postal_code")]
        public string Postal_code { get; set; }
        [JsonProperty("street_name")]
        public string Street_name { get; set; }

        public Address(int addressID, string city, string country, string house_number, string postal_code, string street_name)
        {
            AddressID = addressID;
            Country = country;
            City = city;
            Street_name = street_name;
            House_number = house_number;
            Postal_code = postal_code;
        }
        [JsonConstructor]
        public Address(string city, string country, string house_number, string postal_code, string street_name)
        {
            AddressID = -1;
            Country = country;
            City = city;
            Street_name = street_name;
            House_number = house_number;
            Postal_code = postal_code;
        }
        public Address(int addressID)
        {
            AddressID = addressID;
            Country = "null";
            City = "null";
            Street_name = "null";
            House_number = "null";
            Postal_code = "null";
        }
    }
}