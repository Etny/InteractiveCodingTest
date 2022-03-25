using System;
using static System.Console;

namespace DynamicCheck {
    public static class UX {

        public static void ShowStartUp() {
            Clear();
            WriteFormatted(@"
                         Welkom bij de <Cyan>ITvitae</> <Yellow>Niveau Test</>!

                        De test bestaat (voor nu) uit 1 deel.

                Wanneer je de test begint, wordt er een file aangemaakt 
                in de huidige folder. Open de file en maak de opdrachten
                zoals aangegeven door de comments. Iedere keer dat je de
                file veranderd en opslaat wordt gecheckt of alles klopt,
                check de console voor je progress. 

                            Druk op <DarkCyan>Enter</> om te beginnen...
            ");

            while(ReadKey().Key != ConsoleKey.Enter) {}
        }

        public static void ShowStageComplete(string stageName) {
            Clear();
            WriteFormatted($@"
            
                    Gefeliciteerd, je bent klaar met <DarkMagenta>{stageName}</>!

            Druk op <DarkCyan>Enter</> om te met het volgende onderdeel te beginnen ...
            ");

            while(ReadKey().Key != ConsoleKey.Enter) {}
        }

        public static void WriteFormatted(string s) {
            var parts = s.Split('<','>');
            
            foreach(var part in parts) {
                if(part == "/")
                    ResetColor();
                else if(Enum.TryParse(part.Trim(), out ConsoleColor c)) 
                    ForegroundColor = c;
                else 
                    Write(part);
            }
        }

    }
}