using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Varelager
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Større end >
            // Mindre end <
            // <>
            // ||

            //Ticks til DateTime
            long timer1 = DateTime.Now.Ticks;
            long timer2 = DateTime.Now.Ticks;
            //Do-while løkke som køre forevigt pga. af (True)
            do
            {
                //Køre hvert 5 min
                if ((DateTime.Now.Ticks - timer2) >= 3000000000 || (DateTime.Now.Ticks - timer2) == 0)
                {
                    #region TempFugt

                    Console.SetCursorPosition(0, 1);

                    // Variable for temp og fugt, udenfor, indenfor, varelager og salg.
                    DVIService.monitorSoapClient ds = new DVIService.monitorSoapClient();

                    double LagerTemp = ds.StockTemp();
                    double LagerFugt = ds.StockHumidity();
                    double UdenforTemp = ds.OutdoorTemp();
                    double UdenforFugt = ds.OutdoorHumidity();

                    // Få den til at vise hvad temp og fugt er, både inde og udenfor
                    ColorMethodCyan(); Console.WriteLine("Temperatur og fugtighed\n");

                    ColorMethodCyan(); Console.WriteLine("Lager:");
                    ColorMethodWhite();
                    Console.WriteLine("Temp: " + LagerTemp + "°C");
                    Console.WriteLine("Fugt: " + LagerFugt + "%\n");

                    ColorMethodCyan(); Console.WriteLine("Udenfor");
                    ColorMethodWhite();
                    Console.WriteLine("Temp: " + UdenforTemp + "°C");
                    Console.WriteLine("Fugt: " + UdenforFugt + "%\n");

                    #endregion TempFugt

                    #region Lagerstatus

                    //Opdatering omkring varelager status
                    ColorMethodCyan(); Console.WriteLine("Lagerstatus\n");

                    //Vare som er under minimum
                    Console.WriteLine("Vare under minumum");
                    Console.ForegroundColor = ConsoleColor.Red;
                    List<String> VareUnderMin = new List<string>();
                    VareUnderMin = ds.StockItemsUnderMin();

                    if (VareUnderMin.Count == 0)
                        Console.WriteLine("Ingen vare under minimum");
                    else
                    {
                        for (int i = 0; i < VareUnderMin.Count; i++)
                        {
                            Console.WriteLine(VareUnderMin.ElementAt(i));
                        }
                    }

                    //Vare over maximum
                    ColorMethodCyan(); Console.WriteLine("\nVare over maksimum");
                    Console.ForegroundColor = ConsoleColor.Green;
                    List<String> VareOverMaks = new List<string>();
                    VareOverMaks = ds.StockItemsOverMax();

                    if (VareOverMaks.Count == 0)
                        Console.WriteLine("Ingen vare under maksimum");
                    else
                    {
                        for (int j = 0; j < VareOverMaks.Count; j++)
                        {
                            Console.WriteLine(VareOverMaks.ElementAt(j));
                        }
                    }

                    //Mest solgte vare
                    ColorMethodCyan(); Console.WriteLine("\nMest solgte vare");
                    ColorMethodWhite();
                    List<String> MestSolgtVare = new List<string>();
                    MestSolgtVare = ds.StockItemsMostSold();

                    if (VareOverMaks.Count == 0)
                        Console.WriteLine("Ingen mest solgte vare :(");
                    else
                    {
                        for (int z = 0; z < MestSolgtVare.Count; z++)
                        {
                            Console.WriteLine(MestSolgtVare.ElementAt(z));
                        }
                    }

                    #endregion Lagerstatus

                    #region RSS

                    ColorMethodCyan(); string rssFeedUrl = "https://nordjyske.dk/rss/nyheder";
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;  // Set the encoding to UTF8
                    string rssXml = client.DownloadString(rssFeedUrl);

                    // Parse det til en XDocument
                    XDocument rssDocument = XDocument.Parse(rssXml);

                    Console.SetCursorPosition(0, 32);
                    Console.WriteLine("\n" + rssDocument.Root.Element("channel").Element("title").Value);
                    Console.WriteLine("                                                                                                                                                                     ");
                    Console.WriteLine("                                                                                                                                                                     ");
                    Console.WriteLine("                                                                                                                                                                     ");
                    Console.WriteLine("                                                                                                                                                                     ");
                    Console.SetCursorPosition(0, 34);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    int count;
                    count = 0;
                    // Køre item 3 gange
                    foreach (var element in rssDocument.Root.Element("channel").Elements())
                    {
                        // Vi er interrasserede i Item
                        if (element.Name == "item" && count < 3)
                        {
                            // Print overskriften ud fra RSS
                            Console.WriteLine(element.Element("title").Value);
                            count++;
                        }
                    }

                    #endregion RSS

                    timer2 = DateTime.Now.Ticks;
                }
                if ((DateTime.Now.Ticks - timer1) >= 10000000/*1 sekund i ticks*/)
                {
                    #region Tidszone

                    Console.SetCursorPosition(0, 24);
                    ColorMethodCyan(); Console.WriteLine("\nDato / Tid");
                    // Sæt tidszone for København
                    TimeZoneInfo copenhagenTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                    // Sæt tidszone for London
                    TimeZoneInfo londonTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                    // Sæt tidszone for singapore
                    TimeZoneInfo singaporeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");

                    // Hent tidszoner de forskellige byer
                    DateTime copenhagenTime = TimeZoneInfo.ConvertTime(DateTime.Now, copenhagenTimeZone);
                    DateTime londonTime = TimeZoneInfo.ConvertTime(DateTime.Now, londonTimeZone);
                    DateTime singaporeTime = TimeZoneInfo.ConvertTime(DateTime.Now, singaporeTimeZone);

                    // Få den til at vise de forskellige tidszoner
                    Console.WriteLine("København:"); ColorMethodWhite(); Console.Write(copenhagenTime.DayOfWeek + " "); Console.WriteLine(copenhagenTime.ToString(CultureInfo.InstalledUICulture)); ;
                    ColorMethodCyan();
                    Console.WriteLine("London:"); ColorMethodWhite(); Console.Write(londonTime.DayOfWeek + " "); Console.WriteLine(londonTime.ToString(CultureInfo.InstalledUICulture));
                    ColorMethodCyan();
                    Console.WriteLine("Singapore:"); ColorMethodWhite(); Console.Write(singaporeTime.DayOfWeek + " "); Console.WriteLine(singaporeTime.ToString(CultureInfo.InstalledUICulture));
                    Console.CursorVisible = false;

                    #endregion Tidszone

                    timer1 = DateTime.Now.Ticks;
                }
            } while (true);
        }

        /// <summary>
        /// Denne kode definerer en funktion kaldet "ColorMethodCyan". En funktion er som en lille "opskrift" på noget, man kan gøre igen og igen. "private static" betyder, at denne funktion kun kan bruges inde i denne klasse(en slags "beholder" for kode). "void" betyder, at funktionen ikke returnerer noget når den er færdig. Inde i funktionen er der kun en linje kode.Den linje sætter farven på teksten, der bliver skrevet ud på skærmen, til "cyan". Når du kalder denne funktion, vil den sætte farven på teksten til "cyan". Du kan kalde den ved at skrive "ColorMethodCyan();" et andet sted i programmet.
        /// </summary>
        private static void ColorMethodCyan()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        /// <summary>
        /// Metode som gør tekst farven hvid
        /// </summary>
        private static void ColorMethodWhite()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}