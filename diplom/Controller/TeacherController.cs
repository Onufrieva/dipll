using diplom.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.Controller
{
    public class TeacherController
    {
        public DAO db = new DAO();
        public SubjectOperation subject = new SubjectOperation();
        public UserModel curentUser = new UserModel();
        public UserOperetion usedUser = new UserOperetion();
        public TopicOperation curentTopic;
        List<int[]> idTopicNumberList = new List<int[]>();
        public List<string[]> FindTeacherSubject(int id)
        {
            MySqlCommand command = new MySqlCommand("select `subjects`.`name`, `subjects`.`id` from `subjects` left join `users` on `users`.`id`=`teacherId` where `teacherId`=@id", db.GetConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
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


        public void CreateGroup(string name, string countLesson, int idScheme)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `groups`(`name`, `teacherId`, `subjectId`, `lessonsCount`, `schemeId`) VALUES (@name,@teacherId,@subjectId,@lessonCount,@schemeId)", db.GetConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            command.Parameters.Add("@teacherId", MySqlDbType.Int32).Value = curentUser.id;
            command.Parameters.Add("@subjectId", MySqlDbType.Int32).Value = subject.id;
            command.Parameters.Add("@lessonCount", MySqlDbType.Int32).Value = Convert.ToInt32(countLesson);
            command.Parameters.Add("@schemeId", MySqlDbType.Int32).Value = idScheme;
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
        }

        public void CreateTopic(string nameTopic, int cost, int time, string dependency, string parth)
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `topics`(`name`,`subjectId`, `time`, `cost`,`dependents`) VALUES (@name,@subjectId,@time,@cost,@dependency)", db.GetConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameTopic;
            command.Parameters.Add("@subjectId", MySqlDbType.Int32).Value = subject.id;
            command.Parameters.Add("@time", MySqlDbType.Int32).Value = time;
            command.Parameters.Add("@cost", MySqlDbType.Int32).Value = cost;
            command.Parameters.Add("@dependency", MySqlDbType.VarChar).Value = StringNumberToStringIdTopics(dependency);
            string message;
            int id;
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

            command = new MySqlCommand("select `id` from `topics` where `name`=@name and `subjectId`=@subjectId and `time`=@time and `cost`=@cost", db.GetConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameTopic;
            command.Parameters.Add("@subjectId", MySqlDbType.Int32).Value = subject.id;
            command.Parameters.Add("@time", MySqlDbType.Int32).Value = time;
            command.Parameters.Add("@cost", MySqlDbType.Int32).Value = cost;
            db.openConection();
            id = Convert.ToInt32(command.ExecuteScalar().ToString());
            db.closeConection();

            command = null;
            FileStream fsObj = null;
            BinaryReader binRdr = null;
                //converting image to bytes
                fsObj = File.OpenRead(parth);
                byte[] imgContent = new byte[fsObj.Length];
                binRdr = new BinaryReader(fsObj);
                imgContent = binRdr.ReadBytes((int)fsObj.Length);
                db.openConection();

                //inserting into MySQL db
                command = new MySqlCommand("insert into `texttopic`  (`topicId`, `document`) values (@id,@document)", db.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.Parameters.Add(new MySqlParameter("@document", (object)imgContent));
                command.ExecuteNonQuery();
                db.closeConection();
                if (binRdr != null) binRdr.Close();
                binRdr = null;
                if (fsObj != null) fsObj.Close();
                fsObj = null;

                if (command != null) command.Dispose();
                command = null;
                db.closeConection();
        }

        public List<string[]> TopicTable()
        {
            idTopicNumberList = new List<int[]>();
            MySqlCommand command = new MySqlCommand("select `topics`.`id`,`topics`.`name`,`subjects`.`name`,`topics`.`time`,`topics`.`cost`, `topics`.`dependents`  from `topics` left join `subjects` on `topics`.`subjectId` = `subjects`.`id` where `subjects`.`teacherId` = @teacherId order by `topics`.`id`", db.GetConnection());
            command.Parameters.Add("@teacherId", MySqlDbType.Int32).Value = curentUser.id;
            db.openConection();
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            int i=0;
            while (reader.Read())
            {
                data.Add(new string[6]);
                idTopicNumberList.Add(new int[2]);

                idTopicNumberList[idTopicNumberList.Count - 1][0] = i+1;
                idTopicNumberList[idTopicNumberList.Count - 1][1] = Convert.ToInt32(reader[0].ToString());
                int id = Convert.ToInt32(reader[0].ToString());
                data[data.Count - 1][0] = Convert.ToString(i+1);
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = StringIdTopicsToStringNumber(reader[5].ToString());
                i++;
            }
            reader.Close();
            db.closeConection();
            return data;
        }
        public void SelectTopic(string name)
        {
            MySqlCommand command = new MySqlCommand("select `topics`.`id`,`topics`.`name`,`topics`.`time`,`topics`.`cost`,`topics`.`dependents`   from `topics`  where `topics`.`name` = @name order by `topics`.`id`", db.GetConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            db.openConection();
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            int i = 0;
            while (reader.Read())
            {
                data.Add(new string[6]);
                data[data.Count - 1][0] = Convert.ToString(i + 1);
                data[data.Count - 1][1] = reader[0].ToString();
                data[data.Count - 1][2] = reader[1].ToString();
                data[data.Count - 1][3] = reader[2].ToString();
                data[data.Count - 1][4] = reader[3].ToString();
                data[data.Count - 1][5] = StringIdTopicsToStringNumber(reader[4].ToString());
                i++;
            }
            reader.Close();
            db.closeConection();
            int topicId = Convert.ToInt32(data[0][1]);
            int time = Convert.ToInt32(data[0][3]);
            int cost = Convert.ToInt32(data[0][4]);
            string nameTopic = data[0][2];
            string dependents = data[0][5];
            curentTopic = new TopicOperation(topicId, time, cost, nameTopic, subject.id, dependents);
        }
        public string StringNumberToStringIdTopics(string dependents) //повертає строку, що записується в базу даних
        {
            if (dependents == "")
            {
                return dependents;
            }
            List<int> dep = new List<int>(dependents.Split(',').Select(int.Parse));
            dependents = "";
            foreach (int number in dep)
            {
                foreach (int[] id in idTopicNumberList)
                {
                    if (number == id[0])
                    {
                        dependents = dependents + id[1] + ",";
                    }
                }
            }
            char[] MyChar = { ',' };
            dependents = dependents.TrimEnd(MyChar);
            return dependents;
        }
        public string StringIdTopicsToStringNumber(string dependents) // повертає строку, що показується користувачу
        {
            if (dependents == "")
            {
                return dependents;
            }
            List<int> dep = new List<int>(dependents.Split(',').Select(int.Parse));
            dependents = "";
            foreach (int id in dep)
            {
                
                foreach (int[] number in idTopicNumberList)
                {
                    if (id == number[1])
                    {
                        dependents = dependents + number[0]+ ",";
                    }
                }
            }
            char[] MyChar = { ',' };
            dependents = dependents.TrimEnd(MyChar);
            return dependents;
        }
        public TopicList topicList = new TopicList();
        public List<string[]> SelectTopicForSchemes()
        {
            MySqlCommand command = new MySqlCommand("select `topics`.`id`,`topics`.`name`,`topics`.`time`,`topics`.`cost`,`topics`.`dependents`   from `topics`  where `topics`.`subjectId`=@subjectId order by `topics`.`id`", db.GetConnection());
            command.Parameters.Add("@subjectId", MySqlDbType.Int32).Value = subject.id;
            db.openConection();
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            int i = 0;
            while (reader.Read())
            {
                data.Add(new string[6]);
                data[data.Count - 1][0] = Convert.ToString(i + 1);
                data[data.Count - 1][1] = reader[0].ToString();
                data[data.Count - 1][2] = reader[1].ToString();
                data[data.Count - 1][3] = reader[2].ToString();
                data[data.Count - 1][4] = reader[3].ToString();
                data[data.Count - 1][5] = StringIdTopicsToStringNumber(reader[4].ToString());
                i++;
            }
            reader.Close();
            db.closeConection();
            //int topicId = Convert.ToInt32(data[0][1]);
            //int time = Convert.ToInt32(data[0][3]);
            //int cost = Convert.ToInt32(data[0][4]);
            //string nameTopic = data[0][2];
            //string dependents = data[0][5];
            //curentTopic = new TopicOperation(topicId, time, cost, nameTopic, subject.id, dependents);
            return data;
        }

        public void UpdateTopic(string nameTopic, int time, string dependency, int cost)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `topics` SET `name`=@name,`subjectId`=@subjectId,`time`=@time, `dependents`=@dependency, `cost`=@cost WHERE `id`= @id", db.GetConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameTopic;
            command.Parameters.Add("@subjectId", MySqlDbType.Int32).Value = subject.id;
            command.Parameters.Add("@time", MySqlDbType.Int32).Value = time;
            command.Parameters.Add("@cost", MySqlDbType.Int32).Value = cost;
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = curentTopic.id;
            command.Parameters.Add("@dependency", MySqlDbType.VarChar).Value = StringNumberToStringIdTopics(dependency);
            db.openConection();
            command.ExecuteNonQuery();
            db.closeConection();
        }

        public void DeleteTopic()
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `texttopic` WHERE `topicId`=@id", db.GetConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = curentTopic.id;
            db.openConection();
            command.ExecuteNonQuery();
            db.closeConection();
            command = new MySqlCommand("DELETE FROM `topics` WHERE `id`=@id", db.GetConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = curentTopic.id;
            db.openConection();
            command.ExecuteNonQuery();
            db.closeConection();
            command = new MySqlCommand("DELETE FROM `topicsinschemes` WHERE `topicId`=@id", db.GetConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = curentTopic.id;
            db.openConection();
            command.ExecuteNonQuery();
            db.closeConection();
        }

        public void UpdateTopicWithFile(string nameTopic, int time, int cost, string dependency, string parth)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `topics` SET `name`=@name,`subjectId`=@subjectId,`time`=@time,`cost`=@cost,`dependents`=@dependency WHERE `id`= @id", db.GetConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameTopic;
            command.Parameters.Add("@subjectId", MySqlDbType.Int32).Value = subject.id;
            command.Parameters.Add("@time", MySqlDbType.Int32).Value = time;
            command.Parameters.Add("@cost", MySqlDbType.Int32).Value = cost;
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = curentTopic.id;
            command.Parameters.Add("@dependency", MySqlDbType.VarChar).Value = StringNumberToStringIdTopics(dependency);
            db.openConection();
            command.ExecuteNonQuery();
            db.closeConection();

            command = null;
            FileStream fsObj = null;
            BinaryReader binRdr = null;
            //converting image to bytes
            fsObj = File.OpenRead(parth);
            byte[] imgContent = new byte[fsObj.Length];
            binRdr = new BinaryReader(fsObj);
            imgContent = binRdr.ReadBytes((int)fsObj.Length);
            db.openConection();

            //inserting into MySQL db
            command = new MySqlCommand("UPDATE `texttopic`  SET  `document` = @document where `topicId`=@id", db.GetConnection());
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = curentTopic.id;
            command.Parameters.Add(new MySqlParameter("@document", (object)imgContent));
            command.ExecuteNonQuery();
            db.closeConection();
            if (binRdr != null) binRdr.Close();
            binRdr = null;
            if (fsObj != null) fsObj.Close();
            fsObj = null;

            if (command != null) command.Dispose();
            command = null;
            db.closeConection();
        }
         
        public int SelectSchemeByName(string name)
        {
            int id;
            MySqlCommand command = new MySqlCommand("SELECT `id`, `name`, `countLessons`, `subjectId` FROM `schemes` WHERE `name`=@name and `subjectId`= @subjectId", db.GetConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
            command.Parameters.Add("@subjectId", MySqlDbType.Int32).Value = subject.id;
            db.openConection();
            id = Convert.ToInt32(command.ExecuteScalar().ToString());
            db.closeConection();
            return id;
        }
        public void CreateSchemes(List<string[]> topicList, string name, int countLessons)//Створення запису в таблиці Schemes
        {
            List<int> topicIdList = MVIG(topicList, countLessons);
            if (topicIdList != null)
            {
                MySqlCommand command = new MySqlCommand("INSERT INTO `schemes`(`name`, `countLessons`, `subjectId`) VALUES(@name, @countLessons, @subjectId)", db.GetConnection());
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@subjectId", MySqlDbType.Int32).Value = subject.id;
                command.Parameters.Add("@countLessons", MySqlDbType.Int32).Value = countLessons;
                db.openConection();
                command.ExecuteNonQuery();
                db.closeConection();
                int schemesId = SelectSchemeByName(name);
                CreateListTopicForBDSchemes(topicIdList, schemesId);
            }
        }

        public void CreateListTopicForBDSchemes(List<int> topicIdList, int schemesId)//Створення запису в таблиці TopicInSchemes
        {
            foreach (int topicId in topicIdList)
            {
            MySqlCommand command = new MySqlCommand("INSERT INTO `topicsinschemes`(`topicId`, `schemesId`) VALUES(@topicId,@schemesId)", db.GetConnection());
            command.Parameters.Add("@topicId", MySqlDbType.Int32).Value = topicId;
            command.Parameters.Add("@schemesId", MySqlDbType.Int32).Value = schemesId;
            db.openConection();
            command.ExecuteNonQuery();
            db.closeConection();
            }
        }
        public List<int> MVIG(List<string[]> idtopictimecostdep, int countLessons)
        {
            List<int> topicIdList = topicList.startMVIG(idtopictimecostdep, countLessons);
            return topicIdList;
        }

        //public List<string[]> GroupTable()
        //{

        //}
    }
}
