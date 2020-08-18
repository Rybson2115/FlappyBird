using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIRD
{//
    class Moneta
    {
        private int MonetaX;
        private int MonetaY;
        static private Random rnd = new Random();
        public int WlasnoscMonety;
       
        public Moneta()
        {
            WlasnoscMonety = rnd.Next(3,6);
            MonetaX = 0;
            MonetaY = 0;
        }
        public int[,] DodajMonete (int[,] Tab)
        {
            do
            {
                MonetaY = rnd.Next(3, Tab.GetLength(0)-3);
                MonetaX = rnd.Next(Tab.GetLength(1)-20, Tab.GetLength(1)-2);

            }
            while (Tab[MonetaY, MonetaX] != 0);
            Tab[MonetaY, MonetaX] = WlasnoscMonety; //wlasnosc 3- negatywna 4- neutralna 5-pozytywna
            if(Tab[MonetaY, MonetaX + 1]!=1)
            Tab[MonetaY, MonetaX+1] = WlasnoscMonety*-1;
            return Tab;
        }
    }
}
