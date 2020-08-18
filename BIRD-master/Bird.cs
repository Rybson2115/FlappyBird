using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace BIRD
{
    public class Bird
    {
        private const int _x=10; //do zmiany w zależności od rozmiaru mapy 15
        private int MaxY;//do zmiany w zależności od rozmiaru mapy
        private  int MinY;
        private int _y;
        private int[,] KsztaltPtaka = new int[5, 5] { { 0, 1, 1, 0, 0 }, { 1, 1, 1, 1, 0 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 0 }, { 0, 1, 1, 0, 0 } };
        private int PoprzedniY;//temp
        private int WartoscSkoku;//do zmiany w klasach potomnych 
        private bool CzyZyje;
        public int Wynik=0;//score
        private int[,] TablicaPlansza = new int[Plansza.MaxPlanszaY, Plansza.MaxPlanszaX];
        private bool EfektNegatywny = false;
        private int LicznikPozytywn;
        private int LicznikNegatywny;
        private bool EfektPozytywny = false;

        public Bird()
        {   Plansza.RamkaPlanszy();
            _y = 20;//pozycja początkowa musi być wieksza od 7
            PoprzedniY = _y-1;
            CzyZyje = true;
            WartoscSkoku = 1;
            TablicaPlansza = Plansza.TablicaPoczątkowa();
            MaxY = Plansza.MaxPlanszaY+1;
            MinY = Plansza.MinPlanszaY;
         }
        private int Y
        {
            get
            {
                return _y;
            }
            set
            {
                if(value<MinY|| value > MaxY)
                {
                    CzyZyje = false;
                    KoncoweNapisy();
                }
                else
                 _y = value;
            }
        }
        private void RysujPtaka()
        {
            if (CzyZyje == true)
            {
                Console.SetCursorPosition(_x + 1, PoprzedniY);//maskuje góre ptaka 
                Console.Write("  ");
                Console.SetCursorPosition(_x + 1, PoprzedniY + 4);//maskuje dół ptaka 
                Console.Write("  ");
                Console.SetCursorPosition(_x, Y);
            }
           WygladPtaka();
           
        }
        private void WygladPtaka()//rysuje ptaka
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (KsztaltPtaka[i, j] == 1)
                    {  
                        if(EfektPozytywny==false)
                            Console.BackgroundColor = ConsoleColor.Yellow; //ciało
                       else
                            Console.BackgroundColor = ConsoleColor.Blue;

                        if (i == 1 && j == 2)
                        {
                            if (EfektPozytywny == false)
                                Console.BackgroundColor = ConsoleColor.Blue; //oko
                            else
                                Console.BackgroundColor = ConsoleColor.Yellow;
                        }
                        
                        
                        if (i == 2 && j == 4)
                        {
                            if (EfektPozytywny == false)
                                Console.BackgroundColor = ConsoleColor.Red;//dziub 
                            else
                                Console.BackgroundColor = ConsoleColor.Cyan;
                        }
                        
                    }
                    else
                        Console.ResetColor();
                    Console.SetCursorPosition(_x+j, Y + i);//rysuj go w odpowiedniej pozycji
                    Console.Write(" ");
                }
                
            }
        }
        
     
        public void WykonajRuch() 
        {
            while (SprawdzBird())
            {
                ConsoleKeyInfo wejscie;
                if (!Console.KeyAvailable)
                    Y += WartoscSkoku;
                else
                {
                    wejscie = Console.ReadKey(true);
                    if (wejscie.Key == ConsoleKey.Spacebar)
                        Y -= WartoscSkoku;
                }
                WyswietlPlansze();
                RysujPtaka();
                TablicaPlansza = Plansza.Przesun(TablicaPlansza);
                PoprzedniY = Y;
            }
            KoncoweNapisy();
        }
        
        private void WyswietlPlansze()
        {//+6 od góry 
            if (EfektNegatywny == false)
            {
                for (int i = 0; i < Plansza.MaxPlanszaY; i++)
                {
                    for (int j = 0; j < Plansza.MaxPlanszaX; j++)
                    {
                        Console.SetCursorPosition(j, i + 6);
                        if (TablicaPlansza[i, j] < 0)//maskowanie poprzedniej pozycji 
                            Console.Write(" ");
                        if (TablicaPlansza[i, j] > 2)
                        {//moneta 
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("C");
                            Console.ResetColor();
                        }
                        if (TablicaPlansza[i, j] == 1)//wyswietlanie rurki
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write(" ");
                            Console.ResetColor();
                        }
                    }
                    Console.WriteLine();
                }
            }
            else
                Thread.Sleep(150);

        }

        public void KoncoweNapisy()
        {
            string Tekst1, Tekst2;
            Tekst1 = $"Przegrałeś uzyskując:{Wynik} punktów";
            Tekst2 = "Naciśnij dowolny klawisz aby kontynuować";
            int n = 4;
            int frequency = 1000;
            int duration = 400;
            for (int i = 1; i < n; i++)
                Console.Beep(frequency, duration);
            Thread.Sleep(1000);
            Console.Clear();
            
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < Tekst2.Length+4; j++)
                {
                    if (i == 0 || i == 5|| j == 0 || j == Tekst2.Length + 3)
                    {
                        Console.SetCursorPosition(Console.LargestWindowWidth / 2 - (Tekst2.Length / 2) -2 + j, Console.LargestWindowHeight / 2 - 2 + i);
                        Console.Write("*");
                    }
                    
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(Console.LargestWindowWidth / 2 -Tekst1.Length/2 , Console.LargestWindowHeight/2);
            Console.WriteLine(Tekst1);
            Console.SetCursorPosition(Console.LargestWindowWidth / 2 - Tekst2.Length / 2, Console.LargestWindowHeight / 2+1);
            Console.Write(Tekst2);
            Thread.Sleep(1000);
            Console.ReadKey();
        }
        private bool SprawdzBird()
        {
            
            for (int i = 0; i < 5; i++)//indeksy dla kształtu ptaka
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i == 2 && j == 4)//pozycja dzioba 
                    {
                        if (TablicaPlansza[Y - 6 + i, _x + j] == 2)
                        {
                            Wynik++;
                            Console.SetCursorPosition(Console.LargestWindowWidth / 2 - 4, 1);
                            Console.Write(Wynik);
                        }
                    }
                    if (KsztaltPtaka[i, j] == 1)
                    {
                        switch (TablicaPlansza[Y - 6 + i, _x + j])
                        {
                            case 1: //dla indeksów w Planszy odpowiadających pozycji ptaka sprawdz czy któraść cześć ciała ptaka dotkneła rurki
                                CzyZyje = false;
                                break;
                            case 3:// negatywne 
                                LicznikNegatywny = 0;
                                   EfektNegatywny = true;
                                for (int w = 0; w < Plansza.MaxPlanszaY; w++)//skrócic zjadanie 
                                {
                                    for (int k = 0; k < Plansza.MaxPlanszaX; k++)
                                    {
                                        Console.SetCursorPosition(k, w + 6);
                                        Console.Write(" ");
                                    }
                                    Console.WriteLine();
                                }
                                TablicaPlansza[Y - 6 + i, _x + j] = 0;
                                break;
                            case 4://neutralne
                                Wynik++;
                                TablicaPlansza[Y - 6 + i, _x + j] = 0;
                                break;
                            case 5://pozytywne 
                                EfektPozytywny = true;
                                Wynik++;
                                TablicaPlansza[Y - 6 + i, _x + j] = 0;
                                break;
                            case -3:
                            case -4:
                            case -5:
                                if (TablicaPlansza[Y - 6 + i, _x + j-1]==0)
                                TablicaPlansza[Y - 6 + i, _x + j] = 0;
                                break;
                        }
                        
                    }
                    
                }
            }
            if (EfektPozytywny == true)
            {
                LicznikPozytywn++;
                CzyZyje = true;
                if (LicznikPozytywn == 40)//regulacja czasu trwania efektu
                {
                    EfektPozytywny = false;
                    LicznikPozytywn = 0;
                }
            }
            if (EfektNegatywny == true)
            {
                LicznikNegatywny++;
                if (LicznikNegatywny == 20)
                {
                    EfektNegatywny = false;
                    LicznikNegatywny = 0;
                }
            }
            Console.SetCursorPosition(Console.LargestWindowWidth / 2 - 4, 1);
            Console.Write(Wynik);
            return CzyZyje;
        }

    }
}
