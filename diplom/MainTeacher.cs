using diplom.Controller;
using diplom.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diplom
{
    public partial class MainTeacher : Form
    {
        public TeacherController controller = new TeacherController();
        List<string[]> list = new List<string[]>();
        public MainTeacher(Model.UserModel userModel)
        {
            controller.curentUser = userModel;
            InitializeComponent();
            List<string[]> data = new List<string[]>();
            data = controller.FindTeacherSubject(controller.curentUser.id);
            controller.subject.name = data[0][0];
            controller.subject.id = Convert.ToInt32(data[0][1]);
            controller.subject.teacherId = controller.curentUser.id;
            textBoxFIOTeacher.Text = controller.curentUser.fio;
            textBoxNameSubj.Text = controller.subject.name;
            textBox3.Text = controller.subject.name;
            labelName.Visible = true;
            labelName.Text = labelName.Text + " " + controller.curentUser.fio;
            RefilDataTopics();
            ListBoxChekedAll();

        }
        public void ListBoxChekedAll()
        {
            List<string[]> data = new List<string[]>();
            data = controller.TopicTable();
            checkedListBox1.Items.Clear();
            int i = 0;
            foreach (string[] s in data)
            {
                checkedListBox1.Items.Add(s[0]+". "+s[1]+" "+s[5]);
                checkedListBox1.SetItemChecked(i, true);
                i++;
            }
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

        private void label1_Click(object sender, EventArgs e)
        {
            textBoxNameGroup.Text = "";
            textBoxLessonsCount.Text = "";
            comboBoxScheme.SelectedIndex = -1;
            //controller.teacher = new UserOperetion();
        }
        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Red;
            textBoxNameGroup.BackColor = Color.FromArgb(232, 204, 204);
            textBoxLessonsCount.BackColor = Color.FromArgb(232, 204, 204);
            comboBoxScheme.BackColor = Color.FromArgb(232, 204, 204);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Black;
            textBoxNameGroup.BackColor = Color.White;
            textBoxLessonsCount.BackColor = Color.White;
            comboBoxScheme.BackColor = Color.White;

        }

        private void buttonCreateGroup_Click(object sender, EventArgs e)
        {
            string name = textBoxNameGroup.Text;
            string countLesson = textBoxLessonsCount.Text;

            //controller.CreateGroup(name, countLesson, idScheme);
        }
        private void refillDataGrid()
        {
            List<string[]> data = new List<string[]>();

            int i = 0;
            //data = controller.GroupTable();
            dataGridView1.Rows.Clear();
            comboBoxFIOTeacher.Items.Clear();
            foreach (string[] s in data)
            {
                dataGridView1.Rows.Add(s);
                comboBoxFIOTeacher.Items.Add(s[0]);
                list.Add(new string[2]);
                list[list.Count - 1][0] = Convert.ToString(i);
                list[list.Count - 1][1] = Convert.ToString(s[4]);
                i++;
            }

        }

        private void buttonDowloadTopic_Click(object sender, EventArgs e)
        {
            string imgLoc;
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "jpg Files|*.jpg| Text Files|*.doc;*.docx;*.txt;*.text";
                dlg.Title = "Select Employee Picture";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    imgLoc = dlg.FileName.ToString();
                    textBoxpicEmp1.Text = imgLoc;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void RefilDataTopics()
        {
            List<string[]> data = new List<string[]>();
            data = controller.TopicTable();
            dataTopic.Rows.Clear();
            foreach (string[] s in data)
            {
                dataTopic.Rows.Add(s);
            }
            ListBoxChekedAll();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string nameTopic = textBoxNameTopic.Text;
            int cost = Convert.ToInt32(textBoxCostTopic.Text);
            int time = Convert.ToInt32(textBoxTime.Text);
            string parth = textBoxpicEmp1.Text;
            string dependency = textBoxDependency.Text;
            controller.CreateTopic(nameTopic, cost, time, dependency, parth);
            RefilDataTopics();
        }

        private void dataTopic_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int j = e.RowIndex;
            if (j >= 0)
            {
                string name = Convert.ToString(dataTopic[1, j].Value);
                controller.SelectTopic(name);
                textBoxNameTopic.Text = controller.curentTopic.name;
                textBoxTime.Text = Convert.ToString(controller.curentTopic.time);
                textBoxCostTopic.Text = Convert.ToString(controller.curentTopic.cost);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string nameTopic = textBoxNameTopic.Text;
            int time = Convert.ToInt32(textBoxTime.Text);
            int cost = Convert.ToInt32(textBoxCostTopic.Text);
            string dependency = textBoxDependency.Text;
            if (textBoxpicEmp1.Text != "")
            {
                string parth = textBoxpicEmp1.Text;
                controller.UpdateTopicWithFile(nameTopic, time, cost, dependency, parth);
            }
            else
            {
                controller.UpdateTopic(nameTopic, time, dependency, cost);
            }
            RefilDataTopics();
        }

        private void button4_Click(object sender, EventArgs e)//delete Topic
        {
            controller.DeleteTopic();
            RefilDataTopics();
        }
        private List<string[]> ChekedTopics()
        {
            List<string[]> data = new List<string[]>();
            List<string[]> topicList = new List<string[]>();
            data = controller.SelectTopicForSchemes();
            int i = 0;
            foreach (string[] s in data)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                   topicList.Add(new string[5]);
                    string num = data[i][0]; 
                    string id = data[i][1];
                    string time = data[i][3];
                    string cost = data[i][4];
                    string dep = data[i][5];
                   topicList[topicList.Count - 1][0] = num;//номер темы
                   topicList[topicList.Count - 1][1] = id;//айди теми
                   topicList[topicList.Count - 1][2] = time;//количество занятий на тему
                   topicList[topicList.Count - 1][3] = cost;//важность теми
                   topicList[topicList.Count - 1][4] = dep;//зависимости номеров теми
                }

                //controller.topicList.topicList.Add(s);
                i++;
            }
            return topicList;
        }

        private void button8_Click(object sender, EventArgs e)//Створення схеми підготовки
        {
            string name = textBoxNameScheme.Text;
            int countLessons = Convert.ToInt32(textBoxSchemeLessons.Text);

            controller.CreateSchemes(ChekedTopics(), name, countLessons);
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }
    }
}
