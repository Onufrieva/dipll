using diplom.Controller;
using diplom.Model;
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
    public partial class MainAdmin : Form
    {
        public UserModel user = new UserModel();
        public AdminController controller = new AdminController();
        List<string[]> list = new List<string[]>();

        public MainAdmin(UserModel userModel)
        {
            this.user = userModel;
            InitializeComponent();
            labelName.Visible = true;
            labelName.Text = labelName.Text + " " +  user.fio ;
            refillDataGrid();
            refillDataGridSubject();
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

        private void exit_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void exit_MouseEnter(object sender, EventArgs e)
        {
            exit.ForeColor = Color.Red;
        }

        private void exit_MouseLeave(object sender, EventArgs e)
        {
            exit.ForeColor = Color.Black;
        }

        private void refillDataGrid()
        {
            List<string[]> data = new List<string[]>();

            int i = 0;
            data = controller.TeacheTable();
            dataGridView1.Rows.Clear();
            comboBoxFIOTeacher.Items.Clear();
            foreach (string[] s in data)
            {
                dataGridView1.Rows.Add(s);
                comboBoxFIOTeacher.Items.Add(s[0]);
                list.Add(new string[2]);
                list[list.Count - 1][0] = Convert.ToString(i);
                list[list.Count - 1][1] = Convert.ToString(s[1]);
                i++;
            }
                
        }
        private void refillDataGridSubject()
        {
            List<string[]> subject = new List<string[]>();
            subject = controller.SubjectTable();
            dataSubject.Rows.Clear();
            foreach (string[] s in subject)
                dataSubject.Rows.Add(s);
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            errorLable.Visible = false;
            String login = textBoxLoginReg.Text;
            String password = textBoxPasswordReg.Text;
            String fio = textBoxFIOReg.Text;
            string message;
            if (login != "" && password != "" && fio != "")
            {
                if (!controller.CheckUser(login))
                {
                    errorLable.Text = errorLable.Text + "\n логін " + login + "вже присутній в системі!";
                    errorLable.Visible = true;
                }
                else
                {
                    message = controller.TeacherRegister(login, password, fio);
                    refillDataGrid();
                    controller.teacher.id = controller.FindIdByLogin(login);
                    controller.teacher.login = login;
                    controller.teacher.password = password;
                    controller.teacher.fio = fio;
                    MessageBox.Show(message);
                }
            }
            else
            {
                errorLable.Visible = true;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int j = e.RowIndex;
            string fio = Convert.ToString(dataGridView1[0, j].Value);
            string login = Convert.ToString(dataGridView1[1, j].Value);
            textBoxFIOReg.Text = fio;
            textBoxLoginReg.Text = login;
            controller.SelectTeacher(login, fio);
        }

        private void buttonUpdateTeacher_Click(object sender, EventArgs e)
        {
            if(textBoxLoginReg.Text!="")
            controller.TeacherUpdate(textBoxLoginReg.Text, textBoxFIOReg.Text);
            refillDataGrid();
        }

        private void buttonDeleteTeacher_Click(object sender, EventArgs e)
        {
            if (textBoxLoginReg.Text != "")
                controller.TeacherDelete();
            refillDataGrid();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            textBoxFIOReg.Text = "";
            textBoxLoginReg.Text = "";
            controller.teacher = new UserOperetion();
        }



        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Red;
            textBoxLoginReg.BackColor = Color.FromArgb(232, 204, 204);
            textBoxFIOReg.BackColor = Color.FromArgb(232, 204, 204);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
            textBoxLoginReg.BackColor = Color.White;
            textBoxFIOReg.BackColor = Color.White;
        }

        private void clearSubject_MouseEnter(object sender, EventArgs e)
        {
            clearSubject.ForeColor = Color.Red;
            textBoxNameSubject.BackColor = Color.FromArgb(232, 204, 204);
            comboBoxFIOTeacher.BackColor = Color.FromArgb(232, 204, 204);
        }

        private void clearSubject_MouseLeave(object sender, EventArgs e)
        {
            clearSubject.ForeColor = Color.Black;
            textBoxNameSubject.BackColor = Color.White;
            comboBoxFIOTeacher.BackColor = Color.White;
        }

        private void clearSubject_Click(object sender, EventArgs e)
        {
            textBoxNameSubject.Text = "";
            comboBoxFIOTeacher.SelectedIndex=-1;
            controller.subject = new SubjectOperation();
        }

        private void dataSubject_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int j = e.RowIndex;
            string name = Convert.ToString(dataSubject[0, j].Value);
            int NUMteacherId = comboBoxFIOTeacher.FindString(Convert.ToString(dataSubject[1, j].Value));
            comboBoxFIOTeacher.SelectedIndex = NUMteacherId;
            textBoxNameSubject.Text = name;
            controller.SelectSubject(name, list[NUMteacherId][1]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = list[comboBoxFIOTeacher.SelectedIndex][1];
            string name = textBoxNameSubject.Text;
            controller.InsertSubject(name, login);
            refillDataGridSubject();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string newteacherLogin = list[comboBoxFIOTeacher.SelectedIndex][1];
            string newName = textBoxNameSubject.Text;
            controller.SubjectUpdate(newName, newteacherLogin);
            refillDataGridSubject();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            controller.SubjectDelete();
            refillDataGridSubject();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.Hide();
            ResetPassword resetPassword = new ResetPassword();
            resetPassword.Show();
        }

        private void label9_MouseEnter(object sender, EventArgs e)
        {
            label9.ForeColor = Color.Green;
        }

        private void label9_MouseLeave(object sender, EventArgs e)
        {
            label9.ForeColor = Color.Black;
        }
    }
}
