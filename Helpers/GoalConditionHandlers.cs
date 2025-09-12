using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.Core.Models;
using Archipelago.Core;
using MedievilArchipelago.Models;

namespace MedievilArchipelago.Helpers
{
    internal class GoalConditionHandlers
    {
        private static bool CheckZarokCondition(ArchipelagoClient client)
        {
            if(client?.GameState?.CompletedLocations == null) return false;

            if (client?.GameState?.CompletedLocations.Any(x => x != null && x.Name.Equals("Cleared: Zaroks Lair")) == true)
            {
                Console.WriteLine("You've Defeated Zarok");
                return true;
            }
            return false;
        }

        private static bool CheckChaliceCondition(ArchipelagoClient client)
        {
            int antOption = Int32.Parse(client.Options?.GetValueOrDefault("include_ant_hill_in_checks", "0").ToString());
            int maxChaliceCount = antOption == 1 ? 20 : 19;
            int currentCount = 0;
            if (client?.GameState == null || client.CurrentSession == null) return false;

            foreach (CompositeLocation loc in client.GameState.CompletedLocations.Distinct())
            {
                if (loc.Name.Contains("Chalice: "))
                {
                    currentCount++;
                }
            }

            if (currentCount == maxChaliceCount)
            {
                client.SendGoalCompletion();
                Console.WriteLine("You got all the chalices!");
                return true;
            }
            return false;
        }



        public static bool CheckGoalCondition(ArchipelagoClient client)
        {

            if (client?.GameState?.CompletedLocations == null)
            {
                return false;
            }

            if(client?.Options == null) { return false; }

            int goalCondition = Int32.Parse(client.Options?.GetValueOrDefault("goal", "0").ToString());

            if (goalCondition == PlayerGoals.DEFEAT_ZAROK)
            {
                bool goal = CheckZarokCondition(client);

                if (goal)
                {
                    client.SendGoalCompletion();
                    return true;
                }
            }

            if (goalCondition == PlayerGoals.CHALICE)
            {
                bool goal = CheckChaliceCondition(client);

                if (goal)
                {
                    client.SendGoalCompletion();
                    return true;
                }
            }

            if (goalCondition == PlayerGoals.BOTH)
            {
                bool zarokGoal = CheckZarokCondition(client);
                bool chaliceGoal = CheckChaliceCondition(client);

                if (zarokGoal && chaliceGoal)
                {
                    client.SendGoalCompletion();
                    return true;
                }
            }

            return false;
        }
    }
}
