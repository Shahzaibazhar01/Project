using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.BusinessObject
{
    public class User
    {
        public int UID { get; set; }
        public string UName { get; set; }
        public string UAddress { get; set; }
        public Nullable<long> UCNIC { get; set; }
        public Nullable<long> UPhoneNumber { get; set; }
        public string UUserName { get; set; }
        public string UPassword { get; set; }
        public Nullable<int> URoleID { get; set; }
        public string URole { get; set; }
    }
}