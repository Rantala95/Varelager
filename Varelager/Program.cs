using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml.Linq;
using Varelager.DVIService;

namespace Varelager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lav variabler for temp og fugt, både indenfor og udenfor
            //Sæt dem til dynamisk opdatering senere
            DVIService.monitorSoapClient ds = new DVIService.monitorSoapClient();

            double LagerTemp = ds.StockTemp();
            double LagerFugt = ds.StockHumidity();
            double UdenforTemp = ds.OutdoorTemp();
            double UdenforFugt = ds.OutdoorHumidity();

            // Få den til at vise hvad temp og fugt er, både inde og udenfor
            Console.WriteLine("Temp / Fugt");
            Console.WriteLine("Lager");
            Console.WriteLine("Temp: " + LagerTemp + "°C");
            Console.WriteLine("Fugt: " + LagerFugt + "%");
            Console.WriteLine(" ");
            Console.WriteLine("Udenfor");
            Console.WriteLine("Temp: " + UdenforTemp + "°C");
            Console.WriteLine("Fugt: " + UdenforFugt + "%");

            Console.WriteLine(" ");

            // Større end >
            // Mindre end <

            //Vare som er under minimum
            Console.WriteLine("Vare under minumum");
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

            Console.WriteLine(" ");

            //Vare over maximum
            Console.WriteLine("Vare over maksimum");
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

            Console.WriteLine(" ");

            //Mest solgte vare
            Console.WriteLine("Mest solgte vare");
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

            Console.WriteLine(" ");

            string rssFeedUrl = "https://nordjyske.dk/rss/nyheder";
            WebClient client = new WebClient();
            string rssXml = client.DownloadString(rssFeedUrl);

            // Parse det til en XML
            XDocument rssDocument = XDocument.Parse(rssXml);

            // Vælg hvad den skal tage
            Console.WriteLine(rssDocument.Root.Element("channel").Element("title").Value);

            int count = 0;
            // Køre item 3 gange
            foreach (var element in rssDocument.Root.Element("channel").Elements())
            {
                // Vi er interrasserede i Item
                if (element.Name == "item")
                {
                    // Print overskriften ud fra RSS
                    Console.WriteLine(element.Element("title").Value);
                    count++;
                }
                if (count == 3) break;
            }

            Console.WriteLine(" ");

            Console.WriteLine("Dato / Tid");

            do
            {
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
                Console.SetCursorPosition(0, 27);
                Console.WriteLine("København: " + copenhagenTime.ToString(CultureInfo.InvariantCulture));
                Console.WriteLine("London: " + londonTime.ToString(CultureInfo.InvariantCulture));
                Console.WriteLine("Singapore: " + singaporeTime.ToString(CultureInfo.InvariantCulture));
                Console.CursorVisible = !false;
                Thread.Sleep(1000);
            }
            while (true);

            Console.ReadLine();
        }
    }
}