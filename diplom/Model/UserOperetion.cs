using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.Model
{
    public class UserOperetion
    {
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string fio { get; set; }
        public string role { get; set; }
        public string parentid { get; set; }
    }
}
