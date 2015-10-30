using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSeed.Repository.Entity
{
    public class Org
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public int Schoolcount { get; set; }

        public string Ename { get; set; }

        public string Validdate { get; set; }

        public string Operator { get; set; }

        public string Crmver { get; set; }

        public DateTime Adddate { get; set; }

        public bool Dealt { get; set; }

        public string Saler { get; set; }

        public string Isnew { get; set; }

        public bool SPH { get; set; }

        public bool YY { get; set; }

    }
}
