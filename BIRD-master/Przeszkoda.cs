using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIRD
{// metoda rysuj przeszkodę pole z informacją o y dla przerwy która jest stała 
    public  class Przeszkoda
    {//elementy tablicy:0-puste 1-rura 2- po przekroczeniu tego miejsca dostajesz punkt -1-przestrzen za rurką potrzebna do maskowania jej pozycji 3-moneta -3-maska dla monety
        public int[,] PrzeszkodaTab = new int[Plansza.MaxPlanszaY,_Przerwa+ _SzerokoscPrzeszkody];//5 szerokosc przeszkody
        private const int _Przerwa = 15;//długość przerwy 
        private const int _SzerokoscPrzeszkody = 5;
        static Random rnd = new Random();
        private int YPrzerwy = rnd.Next(2, Plansza.MaxPlanszaY - (_Przerwa + 2)); //losuje proporcje rur po przez wstawienie przerwy o stałych wymiarach
      
        public int Przerwa
        {
            get { return _Przerwa; }
        }
        public int SzerokoscPrzeszkody
        {
            get { return _SzerokoscPrzeszkody; }
        }

        public Przeszkoda() 
        {
            UtworzPrzeszkoda();
        }
       private void UtworzPrzeszkoda()
        {
            for (int i = 0; i < Plansza.MaxPlanszaY; i++)
            {
                for (int j = 0; j < _Przerwa + _SzerokoscPrzeszkody; j++)
                {
                    if (j <= _SzerokoscPrzeszkody)
                    {
                        if (i > YPrzerwy && i < YPrzerwy + _Przerwa)//zakres przerwy
                        {
                            if (j == _SzerokoscPrzeszkody - 1)
                                PrzeszkodaTab[i, j] = 2;
                            else
                                PrzeszkodaTab[i, j] = 0;
                        }
                        else if (j == _SzerokoscPrzeszkody)
                            PrzeszkodaTab[i, j] = -1;
                        else
                            PrzeszkodaTab[i, j] = 1;

                    }
                    else
                        PrzeszkodaTab[i, j] = 0;
                }
            }

            
        }
        public  void WyswietlPrzeszkode() //funkcja pomocnicza
        {
            for (int i = 0; i < Plansza.MaxPlanszaY; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Console.Write(PrzeszkodaTab[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
