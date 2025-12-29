using Archipelago.Core;
using MedievilArchipelago.Models;
using Archipelago.Core.Util;

namespace MedievilArchipelago.Helpers
{
    internal class GoalConditionHandlers
    {
        private static bool CheckZarokCondition(ArchipelagoClient client)
        {
            if (client?.LocationState?.CompletedLocations == null) return false;

            if (client?.LocationState?.CompletedLocations.Any(x => x != null && x.Name.Equals("Cleared: Zaroks Lair")) == true)
            {
                return true;
            }
            return false;
        }

        private static bool CheckChaliceCondition(ArchipelagoClient client)
        {
            int antOption = Int32.Parse(client.Options?.GetValueOrDefault("include_ant_hill_in_checks", "0").ToString());
            int maxChaliceCount = Int32.Parse(client.Options?.GetValueOrDefault("chalice_win_count", "0").ToString());

            if(antOption == 0)
            {
                if(maxChaliceCount > 19)
                {
                    maxChaliceCount = 19;
                }
            }
            int currentCount = ItemHandlers.GetChaliceCount(client);
            int currentLevel = Memory.ReadByte(Addresses.CurrentLevel);
            int currentMapPosition = Memory.ReadByte(Addresses.CurrentMapPosition);

            if (client?.LocationState == null || client.CurrentSession == null) return false;

            // There is an overlap with pumpkin serpent. This resolved the conflict and won't have it fire early.
            if (currentLevel == 10 && currentCount >= 20 && currentCount <= 24) return false;

            if (currentMapPosition == 8 && currentCount >= 20 && currentCount <= 24) return false;

            if (currentCount == maxChaliceCount)
            {
                return true;
            }
            return false;
        }



        public static bool CheckGoalCondition(ArchipelagoClient client)
        {

            if (client?.LocationState?.CompletedLocations == null)
            {
                return false;
            }

            if (client?.Options == null) { return false; }

            int goalCondition = Int32.Parse(client.Options?.GetValueOrDefault("goal", "0").ToString());

            if (goalCondition == PlayerGoals.DEFEAT_ZAROK)
            {
                bool goal = CheckZarokCondition(client);

                if (goal)
                {
                    Console.WriteLine("You've Defeated Zarok!");
                    client.SendGoalCompletion();
                    return true;
                }
            }

            if (goalCondition == PlayerGoals.CHALICE)
            {
                bool goal = CheckChaliceCondition(client);

                if (goal)
                {
                    Console.WriteLine("You collected all the chalices!");
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
                    Console.WriteLine("You defeated Zarok and collected all the chalices!");
                    client.SendGoalCompletion();
                    return true;
                }
            }

            return false;
        }
    }
}
