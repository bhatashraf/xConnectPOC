using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xConnectPOC.Models
{
    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UrlRedirect { get; set; }
    }
}