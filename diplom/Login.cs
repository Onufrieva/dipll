using diplom.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diplom
{
    public partial class Login : Form
    {
        
        public Login()
        {
            InitializeComponent();

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void closeButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Red;

        }

        private void closeButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Black;
        }
        Point lastPoint;
        private void MainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void MainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            errorLable.Visible = false;
            String login = textBoxLogin.Text;
            String password = textBoxPassword.Text;
            DAO db = new DAO();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("select `role` from `users` where `login` = @l and `password` = @p", db.GetConnection());
            command.Parameters.Add("@l", MySqlDbType.VarChar).Value = login;
            command.Parameters.Add("@p", MySqlDbType.VarChar).Value = password;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                db.openConection();
                string role = command.ExecuteScalar().ToString();
                db.closeConection();
                command = new MySqlCommand("select `fio` from `users` where `login` = @l and `password` = @p", db.GetConnection());
                command.Parameters.Add("@l", MySqlDbType.VarChar).Value = login;
                command.Parameters.Add("@p", MySqlDbType.VarChar).Value = password;
                db.openConection();
                string fio = command.ExecuteScalar().ToString();
                db.closeConection();
                command = new MySqlCommand("select `id` from `users` where `login` = @l and `password` = @p", db.GetConnection());
                command.Parameters.Add("@l", MySqlDbType.VarChar).Value = login;
                command.Parameters.Add("@p", MySqlDbType.VarChar).Value = password;
                db.openConection();
                int id = Convert.ToInt32(command.ExecuteScalar().ToString());
                db.closeConection();
                UserModel userModel = new UserModel(id, login, password, fio, role);
                switch (role)
                {
                    case "ADMIN":
                        this.Hide();
                        MainAdmin adminForm = new MainAdmin(userModel);
                        adminForm.Show();
                        break;
                    case "TEACHER":
                        this.Hide();
                        MainTeacher teacherForm = new MainTeacher(userModel);
                        teacherForm.Show();
                        break;
                    //case "STUDENT":
                    //    this.Hide();
                    //    MainStudent studentForm = new MainStudent(userModel);
                    //    adminForm.Show();
                    //    break;
                    //case "PARENT":
                    //    this.Hide();
                    //    MainParent parentForm = new MainParent(userModel);
                    //    adminForm.Show();
                    //    break;
                    default:
                        errorLable.Visible = true;
                        break;
                }

            }
            else
                errorLable.Visible = true;
        }

        private void buttonRegForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register registerForm = new Register();
            registerForm.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ResetPassword resetPassword = new ResetPassword();
            resetPassword.Show();
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Blue;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.DarkSlateGray;
        }
    }
}
