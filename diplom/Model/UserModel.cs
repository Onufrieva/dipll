using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.Model
{
    public class UserModel
    {
        public int id;
        public string login;
        public string password;
        public string fio;
        public string role;
        public string parentid;
        public string subject { get; set; }

        public UserModel(int id, string login, string password, string fio, string role, string parentid)
        {
            this.id = id;
            this.login = login;
            this.password = password;
            this.fio = fio;
            this.role = role;
            this.parentid = parentid;
    }
        public UserModel(int id, string login, string password, string fio, string role)
        {
            this.id = id;
            this.login = login;
            this.password = password;
            this.fio = fio;
            this.role = role;
        }
        public UserModel()
        {

        }

    }
}
