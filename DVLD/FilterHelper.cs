using System;
using System.Collections.Generic;
using System.Linq;

namespace DVLD
{
    public static class FilterHelper
    {
        public static List<T> ApplyFilter<T>(
            List<T> source,
            string selectedFilter,
            string searchValue,
            Dictionary<string, Func<T, string>> map)
        {
            if (selectedFilter == "None" || string.IsNullOrWhiteSpace(searchValue))
                return source;

            searchValue = searchValue.Trim();

            return source.Where(item =>
            {
                if (!map.ContainsKey(selectedFilter))
                    return false;

                string field = map[selectedFilter]?.Invoke(item) ?? "";

                return field.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0;
            }).ToList();
        }
    }
}