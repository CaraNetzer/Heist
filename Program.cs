using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Heist
{
    class Program
    {   
        static void Main(string[] args)
        {
            Console.WriteLine("Plan Your Heist!");

            

            //Adds pre-loaded robbers to the rolodex
            List<IRobber> rolodex = new List<IRobber>() 
            {
                new Muscle("Bane", 70, 15),
                new Muscle("Harley Quinn", 80, 30),
                new Muscle("King Shark", 70, 15),
                new LockSpecialist("Poison Ivy", 70, 25),
                new Hacker("Clayface", 70, 15),
                new Hacker("Sy Borgman", 70, 10)
            };

            //Allows the user to add robbers to the rolodex
            while(true)
            {
                Console.WriteLine($"Current number of operatives in our rolodex: {rolodex.Count}");
                Console.Write($"\nEnter the name of a new crew member: ");
                string name = Console.ReadLine();
                if(name == "")
                {
                    break;
                }

                string specialty = "";
                while(true)
                {
                    Console.WriteLine($"\nSelect this new recruit's specialty");
                    Console.WriteLine("\th for Hacker (Disables alarms)");
                    Console.WriteLine("\tm for Muscle (Disarms guards)");
                    Console.Write("\tl for Lock Specialist (cracks vault): ");
                    specialty = Console.ReadLine();
                    if(specialty == "h" || specialty == "m" || specialty == "l")
                    {
                        break;
                    }
                }

                

                Console.Write("\nEnter this recruit's skill level (1-100): ");
                int skillLevel = int.Parse(Console.ReadLine());

                Console.Write("\nEnter their percentage cut demand: ");
                int percentageCut = int.Parse(Console.ReadLine());

                switch(specialty)
                {
                    case "h":
                        rolodex.Add(new Hacker(name, skillLevel, percentageCut));
                        break;
                    case "m":
                        rolodex.Add(new Muscle(name, skillLevel, percentageCut));
                        break;
                    case "l":
                        rolodex.Add(new LockSpecialist(name, skillLevel, percentageCut));
                        break;
                    default:
                        break;
                }
            }


            #region <Creates Recon Report>
            Bank bank = new Bank();
            //could also create a variable for a new random generator here and call .Next() on that variable instead of creating a new random generator each time
            bank.AlarmScore = new Random().Next(0,101);
            bank.VaultScore = new Random().Next(0,101);
            bank.SecurityGaurdScore = new Random().Next(0,101);
            bank.CashOnHand = new Random().Next(50000,1000001);


            /* Hacker Sarah = new Hacker("Sarah",100,35);
            Sarah.PerformSKill(bank);
            if(bank.IsSecure)
            {
                Console.WriteLine($"Bank is secure");
            }
            else
            {
                Console.WriteLine($"Bank is not secure");
            } */


            Console.WriteLine("\n---Recon Report---");
            Dictionary<string, int> bankProperties = new Dictionary<string, int>()
            {
                {"Alarm", bank.AlarmScore},
                {"Vault", bank.VaultScore},
                {"Gaurds", bank.SecurityGaurdScore}
            };


            bankProperties = bankProperties.OrderByDescending(kv => kv.Value).ToDictionary(x => x.Key, x => x.Value);
            /* foreach (KeyValuePair<string, int> kv in bankProperties) //making sure the values are sorted correctly
            {
                Console.WriteLine($"Key: {kv.Key} Value: {kv.Value}");
            } */

            //tommy's query version
            /* var orderQuery = (
                from kv in bankProperties
                orderby kv.Value
                select kv
            ).ToList(); */

            string maxScore = bankProperties.First().Key;
            string minScore = bankProperties.Last().Key;

            Console.WriteLine($"Most Secure: {maxScore}");
            Console.WriteLine($"Least Secure: {minScore}");

            //could also make this all a method in the Bank class ==> abstraction
            #endregion


            #region <Attempted to create Recon Report with GetProperty() + w/o a dictionary - failed>
            /* List<int> scores = new List<int>() {bank.AlarmScore, bank.VaultScore, bank.SecurityGaurdScore};
            int max = scores.Max();
            int min = scores.Min();
            List<string> scoreNames = new List<string>() {"AlarmScore","VaultScore","SecurityGaurdScore"};
            scoreNames.First(name => bank.GetProperty($"{name}"));

            //int mostSecureInt = Math.Max(bank.AlarmScore, Math.Max(bank.VaultScore, bank.SecurityGaurdScore));
            //int leastSecureInt = Math.Min(bank.AlarmScore, Math.Min(bank.VaultScore, bank.SecurityGaurdScore));

            PropertyInfo[] mostSecure = Type.GetType("Bank")?.GetProperties();
            for (int i = 0; i < mostSecure?.Length; i++)
            {
                Console.WriteLine(mostSecure[i].ToString());
            } */
            #endregion


            #region <Prints rolodex>
            Console.WriteLine("\nRolodex:");
            for(int i = 0; i < rolodex.Count; i++)
            {
                Console.WriteLine(@$"--------------------------
Name: {rolodex[i].Name} [{i+1}]
Specialty: {rolodex[i].GetType().Name}
Skill Level: {rolodex[i].SkillLevel}
Cut: {rolodex[i].PercentageCut}");
            }
            #endregion


            #region <Moves robbers out of the rolodex list into a crew list based on user input and accounts for each person's percentage cut>
            List<IRobber> crew = new List<IRobber>();
            int sumOfCrewCuts = 0;
            while(true)
            {
                try
                {
                    Console.Write("\nEnter the index number of a robber you want to add to your crew: ");
                    int crewMemberIndex = int.Parse(Console.ReadLine()) -1;

                    sumOfCrewCuts += rolodex[crewMemberIndex].PercentageCut;        

                           

                    if(sumOfCrewCuts > 100)
                    {
                        Console.WriteLine($"\nCannot add {rolodex[crewMemberIndex].Name}, their cut is too high.");
                        Console.WriteLine("Breakdown of current cuts:");
                        foreach (var robber in crew)
                        {
                            Console.WriteLine($"\t{robber.Name}: {robber.PercentageCut}");
                        }
                        Console.WriteLine($"You could add a robber with a cut of {100 - (sumOfCrewCuts-rolodex[crewMemberIndex].PercentageCut)}% or less. If there are none that meet this qualification, press enter");
                        sumOfCrewCuts -= rolodex[crewMemberIndex].PercentageCut;
                    } 
                    else if(sumOfCrewCuts == 100)
                    {
                        crew.Add(rolodex[crewMemberIndex]);
                        rolodex.RemoveAt(crewMemberIndex);
                        Console.WriteLine("\nYou've expended your budget, you cannot add any more crew members.");
                        Console.WriteLine("Breakdown of cuts:");
                        foreach (var robber in crew)
                        {
                            Console.WriteLine($"{robber.Name}: {robber.PercentageCut}");
                        }
                        break;
                    } 
                    else
                    {
                        crew.Add(rolodex[crewMemberIndex]);
                        rolodex.RemoveAt(crewMemberIndex);
                    }

                    Console.WriteLine("\nRolodex:");
                    for(int i = 0; i < rolodex.Count; i++)
                    {
                        //tommy's version: instead of removing from the rolodex && doesn't display robbers with 
                        //percentage cuts that are too high:
                        //if(!crew.Contains(rolodex[i]) && (currentCutCapacity + rolodex[i].PercentageCut) < 100)
                        Console.WriteLine(@$"--------------------------
Name: {rolodex[i].Name} [{i+1}]
Specialty: {rolodex[i].GetType().Name}
Skill Level: {rolodex[i].SkillLevel}
Cut: {rolodex[i].PercentageCut}");
                    }
                    //also could put this loop at the top of the while loop, in which case we wouldn't need the
                    //printed orlodex code before the loop either
                }
                catch(FormatException)
                {
                    break;
                }
            }
            #endregion
            
            //Runs the heist
            foreach (var c in crew)
            {
                c.PerformSKill(bank);
            }


            //Prints heist results
            if (bank.VaultScore <= 0 && bank.SecurityGaurdScore <= 0 && bank.AlarmScore <= 0) //if bank's security values are all below zero
            //if(bank.isSecure)
            {                
                Console.WriteLine("\n...");
                Console.WriteLine("...");
                Console.WriteLine($"You did it! You robbed the bank! You all scored ${bank.CashOnHand}!!!!!!!!!");

                Console.WriteLine("\n---Heist Report---");
                Console.WriteLine($"Total: ${bank.CashOnHand}");

                int totalPercentage = crew.Sum(r => r.PercentageCut);
                foreach (var c in crew)
                {
                    Console.WriteLine($"{c.Name} gets ${Math.Round(bank.CashOnHand*c.PercentageCut*.01,0)}");
                }
                Console.WriteLine($"${Math.Round(bank.CashOnHand - (bank.CashOnHand*totalPercentage*.01),0)} is left for you\n");
            }
            else
            {
                Console.WriteLine("\n...");
                Console.WriteLine("...");
                Console.WriteLine("Dr. Psycho teamed up with Joker to ruin your heist, you failed\n");
            }

            #region <Heist Part 1>
            /* Console.Write("\nEnter the difficulty level of the bank: ");
            int bankDifficulty = Int32.Parse(Console.ReadLine());

            List<Teammember> teamMembers = new List<Teammember>();            

            for(int i=0;i<4;i++)
            {
                Console.Write("Enter a team member's name: ");
                Teammember teammember = new Teammember(Console.ReadLine());
                if(teammember.Name == "")
                {
                    break;
                }
                Console.Write("Enter their skill level: ");
                teammember.SkillLevel = Int32.Parse(Console.ReadLine());
                Console.Write("Enter their courage factor: ");
                teammember.CourageFactor = Double.Parse(Console.ReadLine());

                
                teamMembers.Add(teammember);
            }

            Console.WriteLine($"\nThere are {teamMembers.Count} members of this team");
            Console.Write("How many trial runs would you like to run? ");
            
            int numOfTrialRuns = Int32.Parse(Console.ReadLine());
            int successes = 0;
            int failures = 0;

            for(int i=0; i< numOfTrialRuns; i++)
            {
                int luckValue = new Random().Next(-10, 10);
                bankDifficulty += luckValue;

                int skillSum = 0;
                foreach (Teammember m in teamMembers)
                {
                    // Console.WriteLine($"\n{m.Name}");
                    // Console.WriteLine($"Skill Level: {m.SkillLevel}");
                    // Console.WriteLine($"Courage Factor: {m.CourageFactor}");
                    skillSum += m.SkillLevel;
                }

                Console.WriteLine($"\nTrial #{i+1}");
                Console.WriteLine($"Team skill level: {skillSum}");
                Console.WriteLine($"Bank difficulty level: {bankDifficulty}");
                if(skillSum >= bankDifficulty)
                {
                    Console.WriteLine("You did it! You successfully robbed the bank!");
                    successes++;
                }
                else
                {
                    Console.WriteLine("So close! Better luck next time :(");
                    failures++;
                }
                
                bankDifficulty -= luckValue;                
            }

            Console.WriteLine($"\n# of successes: {successes}");
            Console.WriteLine($"# of failures: {failures}"); */
            #endregion

        }
    }
}
