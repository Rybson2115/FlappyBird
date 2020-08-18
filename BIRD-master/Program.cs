using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.IO;
using System.Media;
using System.Globalization;
using System.Xml.Serialization;



namespace BIRD
{ //usunąć maskę monety po jej zjedzeniu 
    public class Program
    {
        public struct Gracz
        {
            public int Wynik;
            public string Imie;
        }
        static string WprowadzString(string Tekst)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo; 
            string zmienna;
            do
            {
                Console.SetCursorPosition((Console.LargestWindowWidth/2)-10,1);
                Console.WriteLine($"Wprowadz {Tekst}:");
                Console.SetCursorPosition((Console.LargestWindowWidth / 2) - 10, 2);
                zmienna = Console.ReadLine();
            } while (string.IsNullOrEmpty(zmienna));
            
            return ti.ToTitleCase(zmienna.Split(' ').First());//jak wprowadzi z małej litery to zmieni na dużą/po wpisaniu dwóch słów bierze tylko pierwsze
        }
        static void Menu()
        {
            //bool CzyPodanoImie = false;
            Gracz g1;
            bool Exit = true;
            List<Gracz> WynikiGraczy = new List<Gracz>();
            var serializer = new XmlSerializer(typeof(List<Gracz>));
            if (File.Exists("Wyniki.xml"))
            {
                var Odczyt = new StreamReader("Wyniki.xml");
                WynikiGraczy = (List<Gracz>)serializer.Deserialize(Odczyt);
                Odczyt.Close();
            }
            Muzyka();
            Plansza.Start();
            Console.ReadKey();
            Console.Clear();
            g1 = new Gracz();

            g1.Imie = WprowadzString("Imie");
            while (Exit)
            {
              
                switch (Plansza.UtworzMenu("Rozpocznij grę", "Wyświetl Wyniki", "Twórcy", "Wyjdz"))
                {
                    case 0:
                            
                        
                        Bird PtakPodstawowy = new Bird();
                        PtakPodstawowy.WykonajRuch();
                        g1.Wynik = PtakPodstawowy.Wynik;//sprawdzanie czy juz jest taki gracz na liście jak tak to zmień jego wynik to mają być tylko max score
                        if (WynikiGraczy.Count != 0 && WynikiGraczy.Exists(x => x.Imie == g1.Imie))
                        {
                            for (int i = 0; i < WynikiGraczy.Count; i++)//dodaje tylko największy wynik dla danego gracza 
                            {
                                if (WynikiGraczy[i].Imie == g1.Imie)
                                {
                                    if (WynikiGraczy[i].Wynik < g1.Wynik)
                                    {
                                        WynikiGraczy.Remove(WynikiGraczy[i]);
                                        WynikiGraczy.Add(g1);
                                    }
                                }
                            }
                        }
                        else
                            WynikiGraczy.Add(g1);

                        break;
                    case 1:
                        WynikiGraczy= WynikiGraczy.OrderBy(o => o.Wynik).ToList();  
                        for (int i = WynikiGraczy.Count-1; i>=0; i--)//wyswietla liste zawodników według punktów
                        {
                            Console.WriteLine($"Gracz {WynikiGraczy[i].Imie} Zdobył {WynikiGraczy[i].Wynik} Puntków");
                        }
                        Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine("Twórcy:");
                        Console.WriteLine("Kamil Matecki");
                        Console.WriteLine("Szymon Niemyt");
                        Console.ReadLine();
                        break;
                    case 3:
                        try
                        {
                            using (var Zapis = new StreamWriter("Wyniki.xml"))
                            {
                                serializer.Serialize(Zapis, WynikiGraczy);
                                Zapis.Close();
                            }
                        }
                        catch (Exception e)
                        {
                           Console.WriteLine($"Plik nie został zapisany! {e.Message}");                 
                        }
                        Exit = false;
                        break;
                }
            }
        }
        static void Muzyka()
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\tło.wav";
            player.PlayLooping();
        }

        static void Main(string[] args)
        {
         Menu();
        }
    }
}
