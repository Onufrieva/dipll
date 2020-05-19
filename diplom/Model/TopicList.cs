using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.Model
{
    public class TopicList
    {
       public List<string[]> topicList = new List<string[]>();
        
        public List<int> startMVIG(List<string[]> idtopictimecost, int countLessons)//должен возвращать список айдишок
        {
            List<Themes> themesM = new List<Themes>(); 
            topicList = idtopictimecost;
            List<int> idTopics = Mvig(countLessons, AddItems(), idtopictimecost, themesM);
            return idTopics;
        }

        public List<int> Mvig(int time, List<Topic> items, List<string[]> idtopictimecost, List<Themes> themesM)
        {
            List<int> idTopics = new List<int>();
            conditionsCheck(themesM, items);
            MVIG bp = new MVIG(time);
            int[,] C = CreateMatrix(idtopictimecost);
            bp.MakeAllSets(items, themesM);
            List<Topic> solve = bp.GetBestSet();
            string message="";
            if (solve == null)
            {
                message = "Метод Ветвей и Границ:\nНет решения!";
                return null;
            }
            else
            {
                message = "";
                idTopics = ShowItems(solve);
                message = "Кількість витрачених занять: " + bp.CalcWeigth(solve).ToString();
                message = message + "Важливіть схеми вцілому: " + bp.CalcPrice(solve).ToString();//количество потраченых занятий
                
            }
            return idTopics;

        }

        private List<int> ShowItems(List<Topic> _items)
        {
            List<int>  idTopics = new List<int>();

            foreach (Topic i in _items)
            {
                    idTopics.Add(i.id);
                
            }
            return idTopics;
        }

        public int[,] CreateMatrix(List<string[]> idtopictimecost)
        {
            int size = idtopictimecost.Count;
            int[,] C = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        C[i, j] = 1;
                    }
                    else
                    {
                        C[i, j] = 0;
                    }
                }
            }

            for (int j = 0; j < size; j++)
            {
                string dependents = idtopictimecost[j][4];
                if (dependents != "")
                {
                    List<int> dep = new List<int>(dependents.Split(',').Select(int.Parse));
                    foreach(int d in dep)
                    {
                        C[d-1, j] = 1;
                    }
                }
            }

            return C;
        }

        private List<Topic> AddItems()
        {
            List<Topic> topics = new List<Topic>();
            foreach (string[] top in topicList)
            {
                topics.Add(new Topic(Convert.ToInt32(top[1]), Convert.ToInt32(top[2]), Convert.ToInt32(top[3])));

            }
            return topics;
        }

        private List<Topic> conditionsCheck(List<Themes> themesM, List<Topic> topic)////запускать по нажатию на "решить"
        {
            int size = topicList.Count;
            int[,] C = CreateMatrix(topicList);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (C[i, j] == 1)
                    {
                        themesM.Add(new Themes(i + 1, j + 1, topic[i].id, topic[j].id));
                    }
                    if (C[i, j] == 1 && i < j && i != j)
                    {
                        Topic top = topic[j];
                        topic[j] = topic[i];
                        topic[i] = top;
                    }
                }
            }
            return topic;
        }

    }
}