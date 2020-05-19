using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diplom
{
    public partial class ResetPassword : Form
    {
        public ResetPassword()
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

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Blue;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.DarkSlateGray;
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Blue;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.DarkSlateGray;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register register = new Register();
            register.Show();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            errorLable.Visible = false;
            String login = textBoxLogin.Text;
            String newpassword = textBoxPassword.Text;
            DAO db = new DAO();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("select `role` from `users` where `login` = @l", db.GetConnection());
            command.Parameters.Add("@l", MySqlDbType.VarChar).Value = login;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                command = new MySqlCommand("UPDATE `users` SET `password`=@p WHERE `login` = @l", db.GetConnection());
                command.Parameters.Add("@l", MySqlDbType.VarChar).Value = login;
                command.Parameters.Add("@p", MySqlDbType.VarChar).Value = newpassword;
                db.openConection();
                if (command.ExecuteNonQuery() == 1)
                {
                    this.Hide();
                    Login loginform = new Login();
                    loginform.Show();
                    MessageBox.Show("Операція успішна");
                }
                else
                {
                    MessageBox.Show("Операція відхилена");
                }
                db.closeConection();

            }
            else
            {
                errorLable.Text = "Логін " + textBoxLogin + " не існує в системі!";
                errorLable.Visible = true;
            }
        }
    }
}
