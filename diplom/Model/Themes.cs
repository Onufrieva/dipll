using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.Model
{

       public class Themes
        {
            public int first { get; set; }
            public int second { get; set; }
            public int firstID { get; set; }
            public int secondID { get; set; }

            public Themes(int _first, int _second, int _firstID, int _secondID)
            {
                first = _first;
                second = _second;
                firstID = _firstID;
                secondID = _secondID;
            }
        }
    
}
