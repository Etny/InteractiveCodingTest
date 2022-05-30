using System.Linq;
using DynamicCheck.IO;
using DynamicCheck.Testing;
using DynamicCheck.Tracking;
using static System.Console;

namespace DynamicCheck {
    internal class MessageWriter {

        public void ShowStageStart(Stage stage)
        {
            Clear();
            WriteFormatted(@$"
                Je start zo met deel {stage.Name}

                    {stage.Description}
                
            Druk op <DarkCyan>Enter</> om te beginnen ...
            ", true);

            WaitForEnter();
        }

        public void ShowCompletion()
        {
            Clear();
            WriteFormatted(@$"
                            Je hebt de test afgemaakt! Goed gedaan!

                        Druk op <DarkCyan>Enter</> om de test af te sluiten...
            ", true);

            WaitForEnter();
        }

        public void ShowStartUp(int stageCount) {
            Clear();
            WriteFormatted(@$"
                         Welkom bij de <Cyan>ITvitae</> <Yellow>Niveau Test</>!

                           De test bestaat uit {stageCount} delen.

                Wanneer je een deel van de test begint, wordt er een file 
                aangemaakt in de workspace folder. Open de file in je editor 
                naar keuze en vul de code in iedere class/method in als aangegeven 
                in de comments. Iedere keer dat je de file veranderd en opslaat 
                wordt gecheckt of alles klopt, check de console voor je progress. 

                            Druk op <DarkCyan>Enter</> om te beginnen...
            ", true);

            WaitForEnter();
        }

        public static void WaitForEnter() {
            while(ReadKey().Key != ConsoleKey.Enter) {}
        }


        public void ShowStageComplete(Stage stage, TimeSpan time) {
            Clear();
            WriteFormatted($@"
            
                    Gefeliciteerd, je bent klaar met <DarkMagenta>{stage.Name}</>!
                    Je tijd voor dit deel is {time}
            Druk op <DarkCyan>Enter</> om te met door te gaan...
            ", true);

            while(ReadKey().Key != ConsoleKey.Enter) {}
        }

        public void WriteFormatted(string s, bool space_evenly = false, int indent = -1) {
            var str = space_evenly ? EvenlySpace(s, indent) : s;
            var parts = str.Split('<','>');
            
            foreach(var part in parts) {
                if(part == "/")
                    ResetColor();
                else if(!int.TryParse(part, out int _) && Enum.TryParse(part.Trim(), out ConsoleColor c)) 
                    ForegroundColor = c;
                else 
                    Write(part);
            }
        }

        public static string EvenlySpace(string s, int indent = -1) {
            indent = indent == -1 ? WindowWidth / 2 : indent;

            var lines = s.Split('\n');
            var lengths = lines.Select(line => 
                line.Trim().Split('<','>').Select((l, i) => i % 2 == 0 ? l.Length : 0).Sum()
            );

            var indents = lengths.Select(l => indent - l / 2);

            return string.Join('\n', lines.Zip(indents).Select(l => new string(' ', l.Second) + l.First.Trim()));
        }

    }
}