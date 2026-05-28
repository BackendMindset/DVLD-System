using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD
{
    public class PersonListView
    {
        [DisplayName("Person ID")]
        public int PersonID { get; set; }

        [DisplayName("National NO")]
        public string NationalNO { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Second Name")]
        public string SecondName { get; set; }

        [DisplayName("Third Name")]
        public string ThirdName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string Gender { get; set; }

        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Nationality")]
        public string CountryName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
