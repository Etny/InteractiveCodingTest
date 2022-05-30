using System;

namespace DeelC
{
    internal class TuringOpdracht
    {
        /* Turing 0.1
         * 
         * Omdat volgens Turing computers simpele dingen zijn, die werken zolang je kan optellen en springen,
         * besluit iemand bij ITvitae een Turingmachine te bouwen.
         * 
         * De code voor de machine bestaat uit brokken van vier getallen.
         * 
         * Het eerste getal geeft het type instructie aan: stoppen (0), optellen (1) of springen (2)
         * 
         * Zowel optel-instructies als spring-instructies worden gevolgd door drie getallen, die de positie
         * geven van het eerste argument, de positie van het tweede argument, en de positie van het derde argument.
         * 
         * Dus:
         * 
         * operatie positie_eerste_argument positie_tweede_argument positie_derde_argument
         * 
         * Optellen is simpel: op de positie aangegeven door het derde argument wordt de som van de getallen
         * die op de posities van het eerste en tweede argument staan aangegeven neergezet. Als het optellen is 
         * afgerond, gaat het programma verder naar het volgende instructiebrok (dus 4 posities verder dan het
         * begin van de optelinstructie)
         * 
         * "1 10 11 15" betekent dus: tel het getal dat op positie 10 staat op bij het getal op positie 11, en
         * schrijf het resultaat naar positie 15
         * 
         * Springen is iets ingewikkelder: als het getal op de locatie aangegeven door het eerste argument
         * kleiner is dan het getal op de locatie aangegeven door het tweede argument, springt het programma
         * naar de locatie aangegeven door het derde argument; anders gaat het programma gewoon verder naar de 
         * volgende instructie.
         * 
         * "2 5 7 20" betekent dus: als het getal dat op positie 5 staat kleiner is dan het getal dat op positie 7 staat, 
         * spring naar positie 20. Als het getal op positie 5 groter of gelijk is aan dat op positie 7, ga dan gewoon
         * door naar het volgende brok van 4 instructies.
         * 
         * Als voorbeeld een programma om het getal 2 tot een bepaalde macht te verheffen:
         * 
         * 1, 0, 13, 13, 1, 14, 14, 14, 2, 13, 15, 0, 0, 0, 1, 4
         * 
         * Als ik hierbij de posities weergeef (als goede programmeurs beginnen we bij 0 te tellen)
         * 
         *  !0: 1       1: 0        2: 13       3: 13   // dit is de eerste instructie: 1, 0, 13, 13
         *  4: 1        5: 14       6: 14       7: 14   // dit is de tweede instructie: 1, 14, 14, 14
         *  8: 2        9: 13       10: 15      11: 0   // dit is de derde instructie: 2, 13, 15, 0
         *  12:0        13: 0       14: 1       15: 4   // dit is de vierde instructie: 0, 0, 1, 4
         * 
         * We beginnen bij positie 0 (aangegeven door de !): 1 betekent "optellen". We tellen het getal 
         * op locatie 0, wat 1 is [we gebruiken de 1 dus zowel als instructietype voor optellen als als waarde!]
         * op bij het getal op locatie 13, wat 0 is, en slaan de som daarvan, 1, op op locatie 13. 
         * Dan verschuiven we de programmateller gewoon naar de volgende positie, 4.
         * De reeks ziet er nu uit als 
         * 
         *  0: 1        1: 0        2: 13       3: 13   // dit is de eerste instructie: 1, 0, 13, 13
         *  !4: 1       5: 14       6: 14       7: 14   // dit is de tweede instructie: 1, 14, 14, 14
         *  8: 2        9: 13       10: 15      11: 0   // dit is de derde instructie: 2, 13, 15, 0
         *  12:0        13: 1       14: 1       15: 4   // dit is de vierde instructie: 0, 0, 1, 4
         *                  
         *                  
         * We gaan door met de volgende instructie, op plaats 4. Dit is ook optellen, het getal op locatie 14,
         * wat 1 is, wordt opgeteld bij zichzelf. De som is dus 2, en die som wordt teruggezet op locatie 14. 
         * Dan wordt de programmapointer opgeschoven naar het begin van de volgende instructie, positie 8.
         * De reeks komt er dan uit te zien als 
         * 
         *  0: 1        1: 0        2: 13       3: 13   // dit is de eerste instructie: 1, 0, 13, 13
         *  4: 1        5: 14       6: 14       7: 14   // dit is de tweede instructie: 1, 14, 14, 14
         *  !8: 2       9: 13       10: 15      11: 0   // dit is de derde instructie: 2, 13, 15, 0
         *  12:0        13: 1       14: 2       15: 4   // dit is de vierde instructie: 0, 0, 1, 4
         *                                  
         * De volgende instructie (op psitie 8) is een 2, dus een sprong. Het getal op locatie 13, wat 1 is,
         * wordt vergeleken met het getal op locatie 15, wat 4 is. 1 is kleiner dan 4, dus de programmawijzer 
         * wordt teruggezet op naar de positie aangegeven door het derde argument (positie 11, die 0 bevat). 
         * De programmawijzer gaat dus terug naar het 0 / het begin!
         * 
         *  !0: 1       1: 0        2: 13       3: 13   // dit is de eerste instructie: 1, 0, 13, 13
         *  4: 1        5: 14       6: 14       7: 14   // dit is de tweede instructie: 1, 14, 14, 14
         *  8: 2        9: 13       10: 15      11: 0   // dit is de derde instructie: 2, 13, 15, 0
         *  12:0        13: 1       14: 2       15: 4   // dit is de vierde instructie: 0, 0, 1, 4
         *  
         *  Weer wordt 1 bij het getal op 13 opgeteld (tot 2) het getal op 14 bij zichzelf (tot 4), en 2 
         *  wordt met 4 vergeleken.
         *  De programmawijzer wordt daarom teruggezet op 0. 1 wordt opgeteld bij het getal op 13 (tot 3),
         *  14 bij zichzelf (tot 8), en 3 wordt met 4 vergeleken. Tenslotte wordt 1 opgeteld bij het getal op 13
         *  (tot 4), het getal op 14 wordt bij zichzelf opgeteld (tot 16). Dan wordt het getal op 13 vergeleken
         *  met het getal op 15. De getallen zijn nu allebei 4, dus er wordt niet meer gesprongen, maar gewoon
         *  doorgegaan met de instructie op 12. Dit is een 0, dus het programma stopt, met als eindtoestand
         *  
         *  0: 1        1: 0        2: 13       3: 13   // dit is de eerste instructie: 1, 0, 13, 13
         *  4: 1        5: 14       6: 14       7: 14   // dit is de tweede instructie: 1, 14, 14, 14
         *  8: 2        9: 13       10: 15      11: 0   // dit is de derde instructie: 2, 13, 15, 0
         *  !12:0       13: 4       14: 16      15: 4   // dit is de vierde instructie: 0, 0, 1, 4
         *  
         *  Hierbij zijn 13 operaties uitgevoerd (4x 0, 4 en 8, en 1x 12, wat het programma stopt)
         *  
         *  Natuurlijk zijn ook grotere programma's mogelijk. Van het programma in OpgaveCInput, wat is 
         *  het produkt van het aantal stappen dat het programma neemt voordat het ophoudt, en het nieuwe
         *  getal op de vijfde positie (index 5)?
         *  
         *  Bij het bovenstaande programma zou de methode dus het element op index 5 (14) maal het aantal
         *  uitgevoerde stappen (13), dus (14 * 13 =) 182 terug moeten geven.                                          
        */
        public static int Get5CodeTimesNumSteps()
        {
            int finalNumberAtPosition5 = 0;
            int numberOfStepsTaken = 0;

            return finalNumberAtPosition5 * numberOfStepsTaken;
        }
    }
}
