using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.Model
{
    class MVIG
    {

        public List<Topic> bestItems = null;

        private double maxW;

        private double bestPrice;

        public MVIG(double _maxW)
        {
            maxW = _maxW;
        }

        //вычисляет общий вес набора предметов
        public double CalcWeigth(List<Topic> items)
        {
            double sumW = 0;

            foreach (Topic i in items)
            {
                sumW += i.time;
            }

            return sumW;
        }

        //вычисляет общую стоимость набора предметов
        public double CalcPrice(List<Topic> items)
        {
            double sumPrice = 0;

            foreach (Topic i in items)
            {
                sumPrice += i.price;
            }

            return sumPrice;
        }


        //проверка, является ли данный набор лучшим решением задачи
        private void CheckSet(List<Topic> items)
        {

            if (bestItems == null)
            {
                if (CalcWeigth(items) <= maxW)
                {
                    bestItems = items;
                    bestPrice = CalcPrice(items);
                }
            }
            else
            {
                if (CalcWeigth(items) <= maxW && CalcPrice(items) > bestPrice)
                {
                    bestItems = items;
                    bestPrice = CalcPrice(items);

                }
            }
        }

        //создание всех наборов перестановок значений
        public void MakeAllSets(List<Topic> items, List<Themes> themes)
        {
            if (items.Count > 0)
                if (CheckC(themes, items))
                    CheckSet(items);

            for (int i = 0; i < items.Count; i++)
            {
                List<Topic> newSet = new List<Topic>(items);

                newSet.RemoveAt(i);
                MakeAllSets(newSet, themes);
            }

        }


        //возвращает решение задачи (набор предметов)
        public List<Topic> GetBestSet()
        {
            return bestItems;
        }
        public bool CheckC(List<Themes> themes, List<Topic> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                ///////////////////////////////////////////////////////////////
                for (int j = 0; j < items.Count; j++)
                {
                    Console.WriteLine(items[j].id);
                }
                Console.WriteLine("333333-------------");
                /////////////////////////////////////////////////////////////////
                for (int k = 0; k < themes.Count; k++)
                    if (items[i].id == themes[k].second)
                    {
                        for (int j = 0; j < items.Count; j++)
                        {
                            if (themes[k].first == items[j].id)
                            {
                                return true;
                            }
                            else return false;
                        }
                    }
                    else return true;

            }
            ////////////////////////////////////////////////////////////
            for (int i = 0; i < themes.Count; i++)
            {
                Console.WriteLine(themes.Count);
                Console.WriteLine(themes[i].first);
                Console.WriteLine(themes[i].second);
                Console.WriteLine("------------");

            }
            ////////////////////////////////////////////////////////

            return false;
        }
    }
}
