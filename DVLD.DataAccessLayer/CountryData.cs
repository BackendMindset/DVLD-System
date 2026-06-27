using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public class CountryData
    {
        public bool IsFound { get; set; } = false;
        public int ID { get; set; } = -1;
        public string CountryName { get; set; } = "";
    }
}
