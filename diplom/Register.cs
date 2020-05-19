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
    public partial class Register : Form
    {
        public Register()
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

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            errorLable.Visible = false;
            String loginParent = textBoxLoginRegParent.Text;
            String passwordParent = textBoxPasswordRegParent.Text;
            String nameParent = textBoxFIORegParent.Text;
            String loginStudent = textBoxLoginRegStudent.Text;
            String passwordStudent = textBoxPasswordRegStudent.Text;
            String nameStudent = textBoxFIORegStudent.Text;
            if (loginParent != "" && passwordParent != "" && nameParent != "" && loginStudent != "" && passwordStudent != "" && nameStudent != "" && !CheckUser(loginParent) && !CheckUser(loginStudent) && loginParent != loginStudent)
            {

            
            DAO db = new DAO();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("INSERT INTO `users` ( `login`, `password`, `fio`, `role`, `parentId`) VALUES (@loginParent, @passwordParent, @nameParent, 'PARENT', NULL);", db.GetConnection());
            command.Parameters.Add("@loginParent", MySqlDbType.VarChar).Value = loginParent;
            command.Parameters.Add("@passwordParent", MySqlDbType.VarChar).Value = passwordParent;
            command.Parameters.Add("@nameParent", MySqlDbType.VarChar).Value = nameParent;

            db.openConection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Реєстрація успішна");
            }
            else
            {
                MessageBox.Show("Реєстрація не успішна");
            }
            db.closeConection();

                MySqlCommand command2 = new MySqlCommand("select `id` from  `users` where `login`= @loginParent", db.GetConnection());
                command2.Parameters.Add("@loginParent", MySqlDbType.VarChar).Value = loginParent;
                db.openConection();
                string idparent = command2.ExecuteScalar().ToString();
                db.closeConection();
                MySqlCommand command1 = new MySqlCommand("INSERT INTO `users` ( `login`, `password`, `fio`, `role`, `parentId`) VALUES (@loginStudent, @passwordStudent, @nameStudent, 'STUDENT', @idparent);", db.GetConnection());
                command1.Parameters.Add("@loginStudent", MySqlDbType.VarChar).Value = loginStudent;
                command1.Parameters.Add("@passwordStudent", MySqlDbType.VarChar).Value = passwordStudent;
                command1.Parameters.Add("@nameStudent", MySqlDbType.VarChar).Value = nameStudent;
                command1.Parameters.Add("@idparent", MySqlDbType.Int32).Value = Convert.ToInt32(idparent);
                db.openConection();
                if (command1.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Реєстрація успішна");
                }
                else
                {
                    MessageBox.Show("Реєстрація не успішна");
                }
                db.closeConection();
            }
            else
            {
                errorLable.Visible = true;
            }

        }
        public Boolean CheckUser(String login)
        {
            Boolean check;
            DAO db = new DAO();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("select * from `users` where `login` = @l", db.GetConnection());
            command.Parameters.Add("@l", MySqlDbType.VarChar).Value = login;
            
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                check = true;
                errorLable.Text = errorLable.Text + "\n логін "+ login  + "вже присутній в системі!";
            }
            else
            {
                check = false;
            }
            return check;
        }

        private void label10_MouseEnter(object sender, EventArgs e)
        {
            label10.ForeColor = Color.Green;
        }

        private void label10_MouseLeave(object sender, EventArgs e)
        {
            label10.ForeColor = Color.Black;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();

        }
    }
}
