using System;
using DynamicCheck.IO;
using DynamicCheck.Testing;
using DynamicCheck.Tracking;
using static System.Console;

namespace DynamicCheck {
    internal class MessageWriter {

        private readonly TrackingManager _tracking;

        public MessageWriter(TestingLifeCycle lifeCycle, IStageProvider provider, TrackingManager tracking)
        {
            lifeCycle.OnRun += ShowStartUp(provider.GetStages().Count);
            lifeCycle.OnStageEnd += ShowStageComplete;
            lifeCycle.OnTestEnd += ShowCompletion;
            _tracking = tracking;
        }

        private void ShowCompletion()
        {
            Clear();
            WriteFormatted(@$"
                            Je hebt de test afgemaakt! Goed gedaan!

                        Druk op <DarkCyan>Enter</> om de test af te sluiten...
            ");

            while(ReadKey().Key != ConsoleKey.Enter) {}
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
            ");

            while(ReadKey().Key != ConsoleKey.Enter) {}
        };

        private void ShowStageComplete(Stage stage) {
            Clear();
            WriteFormatted($@"
            
                    Gefeliciteerd, je bent klaar met <DarkMagenta>{stage.Name}</>!
                    Je tijd voor dit deel is {_tracking.GetTime(stage)}
            Druk op <DarkCyan>Enter</> om te met het volgende onderdeel te beginnen ...
            ");

            while(ReadKey().Key != ConsoleKey.Enter) {}
        }

        public void WriteFormatted(string s) {
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