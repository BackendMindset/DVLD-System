using DVLD.DataAccessLayer;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace DVLD.BusinessLayer
{
    public sealed class CountryService
    {
        public int ID { get; private set; } = -1;
        public string CountryName { get; set; } = "";
        private CountryService(CountryData data)
        {
            ID = data.ID;
            CountryName = data.CountryName ?? "";
        }
        public static async Task<CountryService> FindByNameAsync(string name)
        {
            CountryData data = await CountriesDataAccess.GetCountryInfoByNameAsync(name);
            if (!data.IsFound)
                return null;
            return new CountryService(data);
        }
        public static async Task<CountryService> FindByIdAsync(int ID)
        {
            CountryData data = await CountriesDataAccess.GetCountryInfoByIDAsync(ID);
            if (!data.IsFound)
                return null;
            return new CountryService(data);
        }

        public static async Task<List<CountryService>> GetAllAsync()
        {
            List<CountryData> dataList = await CountriesDataAccess.GetAllCountriesAsync();
            List<CountryService> countries = new List<CountryService>();
            return dataList
                   .Select(data => new CountryService(data))
                   .ToList();
        }
    }
}
