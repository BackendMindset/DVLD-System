using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public class PersonData
    {
        public int PersonID { get; set; }
        public string NationalID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public byte Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public bool IsFound { get; set; }
        public PersonData()
        {
            PersonID = -1;
            NationalID = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            Gender = 0;
            DateOfBirth = DateTime.Now;
            CountryID = -1;
            CountryName = "";
            Address = "";
            Phone = "";
            Email = "";
            ImagePath = "";
            IsFound = false;
        }
    }
}
