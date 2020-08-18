using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

namespace BIRD
{
    static class Plansza
    {
        public static int MinPlanszaY = (Console.LargestWindowHeight / 10)+1;
        public static int MaxPlanszaY = Console.LargestWindowHeight - 9;
        public static int MaxPlanszaX = (Console.LargestWindowWidth-4)/2; //dla optymalizacja pół orginalnego zakresu 
        private static  int[,] Flappybird = new int[5, 65] { {1,1,1,1,1,1,0,1,1,0,0,0,0,0,1,1,1,1,1,1,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,1,1,0,0,1,1,0,1,1,1,1,0,0,0,1,1,0,1,1,1,1,1,1,0,1,1,1,1,0,0 },
                                                 {1,1,0,0,0,0,0,1,1,0,0,0,0,0,1,1,0,0,1,1,0,1,1,0,0,1,1,0,1,1,0,0,1,1,0,1,1,0,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,0,1,1 },
                                                 {1,1,1,1,1,1,0,1,1,0,0,0,0,0,1,1,1,1,1,1,0,1,1,1,1,0,0,0,1,1,1,1,0,0,0,0,0,1,1,0,0,0,1,1,1,1,0,0,0,1,1,0,1,1,1,1,0,0,0,1,1,0,0,1,1 },
                                                 {1,1,0,0,0,0,0,1,1,0,0,0,0,0,1,1,0,0,1,1,0,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,1,1,0,0,1,1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,0,1,1 },
                                                 {1,1,0,0,0,0,0,1,1,1,1,1,1,0,1,1,0,0,1,1,0,1,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,1,1,1,1,0,0,0,1,1,0,1,1,0,0,1,1,0,1,1,1,1,0,0 } };

        private static ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor)); // 0 to czarny nie losować
        private static Random rnd = new Random();

        [DllImport("kernel32.dll", ExactSpelling = true)]        

        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int MAXIMIZE = 3;
        static public void Start()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.Title = "FlappyBird_by_Rybson_&_Kamson";
            WyswietlStart(Flappybird);

        }
        static public void RamkaPlanszy()
        {
            Console.SetCursorPosition(0, Console.LargestWindowHeight / 10);
            for (int i = 0; i <= Console.LargestWindowWidth - 4; i++)
                Console.Write("#");
            Console.SetCursorPosition(0, Console.LargestWindowHeight - 3);
            for (int i = 0; i <= Console.LargestWindowWidth - 4; i++)
                Console.Write("#");
            Console.SetCursorPosition(Console.LargestWindowWidth / 2 - 10, 0);
            Console.Write("FLAPPY BIRD by KAMIL&Rybson2115");
            Console.SetCursorPosition(Console.LargestWindowWidth / 2 - 10, 1);
            Console.Write("SCORE:");
            WyswietlLogo(Flappybird);
        }
        static public int[,] TablicaPoczątkowa()// pierwsze utworzenie tablicy przeszkód z odpowiednia iloscia miejsca po lewej stronie 
        {
              int[,] TablicaPlansza = new int[Plansza.MaxPlanszaY, Plansza.MaxPlanszaX];
            //x=25 start
            int IlePrzeszkod = ((Plansza.MaxPlanszaX - 25) / 15);
             int licznik = 0;
            int k = 0;
            
            List<Przeszkoda> Przeszkody = new List<Przeszkoda>();
            Przeszkoda p1;
            for (int i = 0; i < IlePrzeszkod; i++)
            {
                p1 = new Przeszkoda();
                Przeszkody.Add(p1);

            }
            int SzerTabPrzeszkoda = Przeszkody[0].Przerwa+ Przeszkody[0].SzerokoscPrzeszkody;
            for (int i = 0; i < Plansza.MaxPlanszaY; i++)
             {
                 for (int j = 0; j+ SzerTabPrzeszkoda < Plansza.MaxPlanszaX; j++)
                 {
                    if (j < 25) //do indeksu 25 puste miejsca 
                         TablicaPlansza[i, j] = 0;
                    else if (licznik<= IlePrzeszkod)//do momentu az limit przeszkód nie zostanie osiągniety
                    {   
                         for (k = 0; k < SzerTabPrzeszkoda; k++)//dodaje pojedynczą przeszkode z listy
                             TablicaPlansza[i, j+k] = Przeszkody[licznik].PrzeszkodaTab[i, k]; //dodawaj elementy z listy przeszkód do tablicy plansza
                         licznik++;//przeszkoda została dodana
                         j += k;//aktualizuj licznik 
                    }
                    
                 }
                licznik = 0;//reset licznika przed przejściem do następnego wiersza 
             }

            return TablicaPlansza;  
        }
        static private int[,] DodajPrzeszkode(int[,] Tablica)//dodaje pojedynczą przeszkode do tablicy //>>>>>>zrobić dodawanie całego obiektu a nie tylko rurki<<<<
        { int Licznik = 0;
            int k = 0;
            int MiejsceNaPrzeszkode = 0;
            Przeszkoda P1 = new Przeszkoda();
            Moneta M1 = new Moneta();
            for (int i = Tablica.GetLength(1)-(P1.SzerokoscPrzeszkody+P1.Przerwa); i < Tablica.GetLength(1); i++) //sprawdz tylko ostatnie 20 pozycji 
            {
                if (Tablica[0, i] == 0)
                    Licznik++;
                if (Tablica[0, i] == 1)
                    Licznik = 0;
                if (Licznik == P1.Przerwa)//szerokosc przerwy miedzy przeszkodami
                {
                    MiejsceNaPrzeszkode = i; //szukanie indeksu który wystepuje po odpowiedniej przerwie miedzy przeszkodami
                    break;
                }
                    
            }

            if (Tablica.GetLength(1)-MiejsceNaPrzeszkode==(P1.SzerokoscPrzeszkody + 1))//sprawdzanie czy przeszkoda zmieści się do tablicy
            {
                for (int i = 0; i < Tablica.GetLength(0); i++)
                {
                    for (int j = MiejsceNaPrzeszkode; j < Tablica.GetLength(1); j++) //wstawia elementy przeszkody od wolnego miejsca do końca tablicy 
                    {

                        for (k = 0; k < 6; k++)
                        {
                            Tablica[i, j + k] = P1.PrzeszkodaTab[i, k];
                        }
                        j += k;

                    }

                }
                Tablica=M1.DodajMonete(Tablica);
            }

            return Tablica;
        }
        static public int[,] Przesun(int[,] Tablica)
        {
            for (int i = 0; i < Tablica.GetLength(0); i++)
            {
                for (int j = 0; j < Tablica.GetLength(1); j++)
                {

                    if (j + 1 == Tablica.GetLength(1)) 
                        Tablica[i, j] = 0; //wstaw na koniec 0

                    else if (i + 1 == Tablica.GetLength(0))//zeby indeks nie przekroczył zadeklarowanej wartości
                        Tablica[i, j] = Tablica[i, j + 1];
                    else
                        Tablica[i, j] = Tablica[i , j + 1];//przesunięcie elementu tablicy w lewo
                }

            }
            return DodajPrzeszkode(Tablica);
           
        }
        static public int UtworzMenu(params string[] Menu)
        {
            return WyświetlListe(Menu.ToList());
        }
        static private int WyświetlListe(List<string> Menu)
        {
            Console.CursorVisible = false;
            Console.Clear(); 
            if (Menu.Count == 0)
                throw new Exception("Lista jest pusta");
            string T1 = "Menu:";
            int i = 0;
            Console.SetCursorPosition((Console.LargestWindowWidth -T1.Length)/2 , 4 );
            Console.WriteLine(T1);
            foreach (string o in Menu)
            {
                Console.SetCursorPosition((Console.LargestWindowWidth -T1.Length)/2 , 6 +i*2);
                if (i == 0)
                    Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write($"{++i}");
                Console.ResetColor();
                Console.Write($".");
                Console.WriteLine($"{o}");
                
            }
            int left = (Console.LargestWindowWidth - T1.Length)/2, top = 1;
            int WartoscMax = Menu.Count * 2;
            ConsoleKeyInfo wejscie;
            do
            {
                Console.SetCursorPosition(left, top+5);
                wejscie = Console.ReadKey(true);
                Console.Write($"{(top / 2) + 1}.");
                switch (wejscie.Key.ToString())
                {
                    case "UpArrow":
                        top -= 2;
                        if (top <= 0)
                            top = WartoscMax - 1;
                        break;
                    case "DownArrow":
                        top += 2;
                        if (top >= WartoscMax)
                            top = 1;
                        break;

                }
                Console.SetCursorPosition(left,top+5);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write($"{(top / 2) + 1}");
                Console.ResetColor();

            } while (wejscie.Key != ConsoleKey.Enter);
            int indeks = (top / 2);
            Console.Clear();
            return indeks;
        }
        static public void WyswietlStart(int[,] litera)
        {
            bool dyskoteka = false;
            int x = 0; //Console.LargestWindowWidth;
            int y = 0;//Console.LargestWindowHeight;
            for (int k = 0; k < 30; k++) //10 liter
            {
                for (int i = 0; i < 5; i++)
                {
                    int kolor = rnd.Next(1, colors.Length);
                    for (int l = 0; l < 65; l++)
                    {
                    Console.SetCursorPosition(l + x, y);
                        if (litera[i, l] == 1)
                        {
                            Console.BackgroundColor = colors[kolor];
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        else
                            Console.Write(" ");
                    }
                    if (y < 26)
                        y++;

                    Thread.Sleep(15);
                }
                if (dyskoteka == false) Console.Clear();
                Console.WriteLine();
                if (x < Console.LargestWindowWidth / 2 - 32)
                    x += 12;
                if (y >= 20)
                {
                    dyskoteka = true;
                    y -= 5;
                }

            }
            int collor = rnd.Next(1, colors.Length);
            Console.ForegroundColor = colors[collor];
            Console.SetCursorPosition(Console.LargestWindowWidth / 2  - 20 , Console.LargestWindowHeight / 2 - 2 );
            Console.Write("Naciśnij dowolny klawisz aby kontynuować");
            Console.ResetColor();

        }

        static private void WyswietlLogo(int[,] litera)
        {
            int x = Console.LargestWindowWidth - 70;
            int y = Console.LargestWindowHeight / 2 - 3;
            for (int i = 0; i < 5; i++)
            {
                int kolor = rnd.Next(1, colors.Length);
                for (int l = 0; l < 65; l++)
                {
                    if (l > 41)
                    {
                        x -= 32;
                        y += 5;
                    }
                    Console.SetCursorPosition(x + l, y + i);
                    if (litera[i, l] == 1)
                    {
                        Console.BackgroundColor = colors[kolor];
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                        Console.Write(" ");
                    if (l > 41)
                    {
                        x += 32;
                        y -= 5;
                    }

                }
            }
        }

    }

}
