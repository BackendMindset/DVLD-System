using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace DVLD.DataAccessLayer
{
    static class DataAccessSettings
    {
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["DVLDConnection"]?.ConnectionString
            ?? "Server=.;Database=MeDVLD;Integrated Security=True;";
    }
}