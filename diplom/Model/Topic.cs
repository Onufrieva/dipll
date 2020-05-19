using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.Model
{
    public class Topic //товар
    {
        public int id { get; set; } //название товара
        public int time { get; set; } //вес товара
        public int price { get; set; } //цена товара
        public double us;
        public bool take; //берем ли товар


        public Topic(int _id, int _time, int _price)
        {
            if (_time != 0)
            {
                id = _id;
                time = _time;
                price = _price;
                us = price / time;
            }
            else
            {
                id = _id;
                time = _time;
                price = _price;
            }
        }
    }
}
