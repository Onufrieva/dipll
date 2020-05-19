using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.Model
{
    public class TopicOperation
    {
        public int id { get; set; }
        public int time { get; set; }
        public int cost { get; set; }
        public string name { get; set; }
        public int subjectId { get; set; }
         
        public string dependency { get; set; }

        public TopicOperation(int id, int time, int cost, string name, int subjectId, string dependency)
        {
            this.id = id;
            this.time = time;
            this.cost = cost;
            this.name = name;
            this.subjectId = subjectId;
            this.dependency = dependency;
        }
    }
}
