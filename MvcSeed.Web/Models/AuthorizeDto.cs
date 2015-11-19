using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSeed.Web.Models
{
    public class AuthorizeDto
    {
        public string code { get; set; }
        public string state { get; set; }
    }
}