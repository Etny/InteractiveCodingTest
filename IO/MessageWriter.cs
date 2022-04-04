using System.Linq;
using DynamicCheck.IO;
using DynamicCheck.Testing;
using DynamicCheck.Tracking;
using static System.Console;

namespace DynamicCheck {
    internal class MessageWriter {

        private readonly ProgressTracker _tracking;

        public MessageWriter(TestingLifeCycle lifeCycle, IStageProvider provider, ProgressTracker tracking)
        {
            lifeCycle.OnRun += ShowStartUp(provider.GetStages().Count);
            lifeCycle.OnStageStart += ShowStageStart;
            lifeCycle.OnStageEnd += ShowStageComplete;
            lifeCycle.OnTestEnd += ShowCompletion;
            _tracking = tracking;
        }

        private void ShowStageStart(Stage stage)
        {
            Clear();
            WriteFormatted(@$"
                Je start zo met deel {stage.Name}

                    {stage.Description}
                
            Druk op <DarkCyan>Enter</> om te beginnen ...
            ", true);

            WaitForEnter();
        }

        private void ShowCompletion()
        {
            Clear();
            WriteFormatted(@$"
                            Je hebt de test afgemaakt! Goed gedaan!

                        Druk op <DarkCyan>Enter</> om de test af te sluiten...
            ", true);

            WaitForEnter();
        }

        private Action ShowStartUp(int stageCount) => () => {
            Clear();
            WriteFormatted(@$"
                         Welkom bij de <Cyan>ITvitae</> <Yellow>Niveau Test</>!

                           De test bestaat uit {stageCount} delen.

                Wanneer je de test begint, wordt er een file aangemaakt 
                in de huidige folder. Open de file en maak de opdrachten
                zoals aangegeven door de comments. Iedere keer dat je de
                file veranderd en opslaat wordt gecheckt of alles klopt,
                check de console voor je progress. 

                            Druk op <DarkCyan>Enter</> om te beginnen...
            ", true);

            WaitForEnter();
        };

        public static void WaitForEnter() {
            while(ReadKey().Key != ConsoleKey.Enter) {}
        }


        private void ShowStageComplete(Stage stage) {
            Clear();
            WriteFormatted($@"
            
                    Gefeliciteerd, je bent klaar met <DarkMagenta>{stage.Name}</>!
                    Je tijd voor dit deel is {_tracking.LastStageTime()}
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
                else if(Enum.TryParse(part.Trim(), out ConsoleColor c)) 
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