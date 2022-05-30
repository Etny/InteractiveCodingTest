using System;
using System.Linq;

namespace DeelB
{
    internal class OpdrachtB
    {
       
        // implementeer de DemandCookie()-methode, die een koekje eist (iets van 
        // "Geef me een koekje! Ik wil een koekje!", waarop de gebruiker een woord intypt
        // (gebruik Console.ReadLine();, bv gift = Console.ReadLine();
        // Als het gegevene GEEN koekje is, maar bijvoorbeeld een fiets, druk af
        // "Ik wil geen fiets! Ik wil een koekje!" - en ga weer terug naar het koekjes vragen
        // Zeg anders "Dank je! Mjam mjam mjam" en verlaat/beëindig de methode
        public static void DemandCookie()
        {
            
        }
        

        public class ToyCar
        {
            public int Charge { get; set; } // in percent, 0-100
            public int DistanceDriven { get; set; } // in meters

            // opgave B1: implementeer de Buy() methode zodat ToyCar.Buy() een speelgoedauto oplevert
            // met volle (100% = 100) charge en 0 afgelegde afstand.
            public static ToyCar Buy()
            {
                return null;
            }

            // opgave B2: implementeer de Report() methode die laat zien wat de charge van het autootje is,
            // en hoever hij gereden heeft, iets als "De auto heeft 0 meter gereden, en is voor 100% geladen"
            public void Report()
            {
            
            }

            // opgave B3: implementeer de Drive() methode die, als de charge 5 of groter is, de auto 20 meter 
            // verder laat rijden en de charge met 5 verminderd. Als de charge lager dan 5 is, moet niks veranderen.
            public void Drive()
            {
                
            }

        }

        public class BirdCounter
        {
            public int[] Sightings { get; set; } = Array.Empty<int>();

            // implemententeer GetCountOnDay, dat het aantal vogels op dag "day" teruggeeft
            // (de eerste dag is dag 0)
            public int GetCountOnDay(int day)
            {
                return -1;
            }

            // implementeer GetTotalDays, welke het aantal dagen teruggeeft dat je vogels hebt gespot
            public int GetTotalDays()
            {
                return -1;
            }

            // implementeer GetCountYesterday, welke teruggeeft welke je op de meest recente (laatste) dag zag
            public int GetCountYesterday()
            {
                return -1;
            }

            // return als een tupple (int, int) de dag waarop je de meeste vogels zag, en hoeveel vogels je op
            // die dag zag, dus (beste dag, hoeveel vogels)
            public (int, int) MostSuccessfulDay()
            {
                return (-1, -1);
            }

            // implementeer TotalSightings, wat teruggeeft hoeveel vogels je in het totaal hebt gezien.
            public int TotalSightings()
            {
                return -1;
            }

            // implementeer CountBusyDays, wat teruggeeft op hoeveel dagen je vijf vogels (of meer) hebt gespot.
            public int CountBusyDays()
            {
                return -1;
            }
        }
    }
}
