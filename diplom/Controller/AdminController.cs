using diplom.Model;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.Controller
{
    public class AdminController
    {
        public DAO db = new DAO();
        public UserOperetion teacher = new UserOperetion();
        public SubjectOperation subject = new SubjectOperation();
        public List<string[]> TeacheTable()
        {
            MySqlCommand command = new MySqlCommand("select  `fio`, `login`,`id` from `users` where `role` = 'TEACHER'", db.GetConnection());
            db.openConection();
            MySqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[2]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
            }
            reader.Close();
            db.closeConection();
            return data;
        }
        public int FindIdByLogin(string login)
        {
            MySqlCommand command2 = new MySqlCommand("select `id` from  `users` where `login`= @login", db.GetConnection());
            command2.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
            db.openConection();
            int id = Convert.ToInt32(command2.ExecuteScalar().ToString());
            db.closeConection();
            return id;
        }
        public List<string[]> SubjectTable()
        {
            MySqlCommand command = new MySqlCommand("select `subjects`.`name`, `users`.`fio` from `subjects` left join `users` on `users`.`id`=`teacherId`", db.GetConnection());
            db.openConection();
            MySqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[2]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();

            }
            reader.Close();
            db.closeConection();
            return data;
        }
        public string TeacherRegister(string login, string password, string fio)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `users` ( `login`, `password`, `fio`, `role`, `parentId`) VALUES (@login, @password, @fio, 'TEACHER', NULL);", db.GetConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;
            command.Parameters.Add("@fio", MySqlDbType.VarChar).Value = fio;
            string message;
            db.openConection();
            if (command.ExecuteNonQuery() == 1)
            {
                message = "Реєстрація успішна";
            }
            else
            {
                message = "Реєстрація не успішна";
            }
            db.closeConection();
            return message;
        }
        public string InsertSubject(string name, string login)
        {
            int teacherId;
            teacherId = Convert.ToInt32(FindIdByLogin(login));
            MySqlCommand command = new MySqlCommand("INSERT INTO `subjects` ( `name`, `teacherId`) VALUES (@name, @teacherId);", db.GetConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            command.Parameters.Add("@teacherId", MySqlDbType.Int32).Value = teacherId;
            string message;
            db.openConection();
            if (command.ExecuteNonQuery() == 1)
            {
                message = "Реєстрація успішна";
            }
            else
            {
                message = "Реєстрація не успішна";
            }
            db.closeConection();
            return message;
        }
        public string TeacherUpdate( string newlogin, string newfio)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `login`=@newlogin,`fio`=@newfio WHERE `id`=@id", db.GetConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = teacher.id;
            command.Parameters.Add("@newlogin", MySqlDbType.VarChar).Value = newlogin;
            command.Parameters.Add("@newfio", MySqlDbType.VarChar).Value = newfio;
            string message;
            db.openConection();
            if (command.ExecuteNonQuery() == 1)
            {
                message = "Операція успішна";
            }
            else
            {
                message = "Операція відхилена";
            }
            db.closeConection();
            return message;
        }
        public string SubjectUpdate(string newName, string newteacherLogin)
        {
            int newteacherId = FindIdByLogin(newteacherLogin);
            MySqlCommand command = new MySqlCommand("UPDATE `subjects` SET `name`=@newName,`teacherId`=@newteacherId WHERE `id`=@id", db.GetConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = subject.id;
            command.Parameters.Add("@newName", MySqlDbType.VarChar).Value = newName;
            command.Parameters.Add("@newteacherId", MySqlDbType.Int32).Value = newteacherId;
            string message;
            db.openConection();
            if (command.ExecuteNonQuery() == 1)
            {
                message = "Операція успішна";
            }
            else
            {
                message = "Операція відхилена";
            }
            db.closeConection();
            return message;
        }
        public string SubjectDelete()
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `subjects` WHERE `id`=@id", db.GetConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = subject.id;
            string message;
            db.openConection();
            if (command.ExecuteNonQuery() == 1)
            {
                message = "Операція успішна";
            }
            else
            {
                message = "Операція відхилена";
            }
            db.closeConection();
            return message;
        }
        public string TeacherDelete()
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `users` WHERE `id`=@id", db.GetConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = teacher.id;
            string message;
            db.openConection();
            if (command.ExecuteNonQuery() == 1)
            {
                message = "Операція успішна";
            }
            else
            {
                message = "Операція відхилена";
            }
            db.closeConection();
            return message;
        }
        public void SelectTeacher(string login, string fio)
        {
            teacher.id = FindIdByLogin(login);
            teacher.login = login;
            teacher.fio = fio;
        }
        public void SelectSubject(string name, string login)
        {
            subject.teacherId = Convert.ToInt32(FindIdByLogin(login));
            subject.name = name;
            subject.id = FindByNameTeacher();
        }
        public int FindByNameTeacher()
        {
            MySqlCommand command2 = new MySqlCommand("select `id` from  `subjects` where `name`= @name and `teacherId`= @teacherId", db.GetConnection());
            command2.Parameters.Add("@teacherId", MySqlDbType.Int32).Value = subject.teacherId;
            command2.Parameters.Add("@name", MySqlDbType.VarChar).Value = subject.name;
            db.openConection();
            int id = Convert.ToInt32(command2.ExecuteScalar().ToString());
            db.closeConection();
            return id;
        }
        public Boolean CheckUser(String login)
        {
            Boolean check;
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("select * from `users` where `login` = @l", db.GetConnection());
            command.Parameters.Add("@l", MySqlDbType.VarChar).Value = login;

            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                check = false;

            }
            else
            {
                check = true;
            }
            return check;
        }
    }
}
