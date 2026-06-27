using System;

namespace DVLD.DataAccessLayer
{
    public class RoleData
    {
        public int ID { get; set; } = -1;
        public string RoleName { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsActive { get; set; } = false;
    }
}