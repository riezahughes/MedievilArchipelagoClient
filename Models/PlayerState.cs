using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Archipelago.Core.Util;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct2D1;
using GameOverlay.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;

// can ignore. was playing with this idea, but because it took forever i'm leaving it here for now anyway
namespace MedievilArchipelago.Models
{
    internal class PlayerState
    {

        public static Dictionary<string, int> CurrentGoals { get; private set; }

        public static Dictionary<string, int> GoalsCompleted()
        {
            return new Dictionary<string, int>
            {
                {"Cleared: Dan's Crypt", 0 },
                {"Cleared: The Graveyard", 0},
                {"Cleared: Return to the Graveyard", 0},
                {"Cleared: Cemetery Hill",0},
                {"Cleared: The Hilltop Mausoleum", 0},
                {"Cleared: Scarecrow Fields", 0},
                {"Cleared: Ant Hill", 0},
                {"Cleared: The Crystal Caves", 0},
                {"Cleared: The Lake", 0},
                {"Cleared: Pumpkin Gorge", 0},
                {"Cleared: Pumpkin Serpent", 0},
                {"Cleared: Sleeping Village", 0},
                {"Cleared: Pools of the Ancient Dead", 0},
                {"Cleared: Asylum Grounds", 0},
                {"Cleared: Inside the Asylum", 0},
                {"Cleared: Enchanted Earth", 0},
                {"Cleared: The Gallows Gauntlet", 0},
                {"Cleared: The Haunted Ruins", 0},
                {"Cleared: Ghost Ship", 0},
                {"Cleared: The Entrance Hall", 0},
                {"Cleared: The Time Device", 0},
                {"Chalice: The Graveyard", 0},
                {"Chalice: Return to the Graveyard", 0},
                {"Chalice: Cemetery Hill",0},
                {"Chalice: The Hilltop Mausoleum", 0},
                {"Chalice: Scarecrow Fields", 0},
                {"Chalice: Ant Hill", 0},
                {"Chalice: The Crystal Caves", 0},
                {"Chalice: The Lake", 0},
                {"Chalice: Pumpkin Gorge", 0},
                {"Chalice: Pumpkin Serpent", 0},
                {"Chalice: Sleeping Village", 0},
                {"Chalice: Pools of the Ancient Dead", 0},
                {"Chalice: Asylum Grounds", 0},
                {"Chalice: Inside the Asylum", 0},
                {"Chalice: Enchanted Earth", 0},
                {"Chalice: The Gallows Gauntlet", 0},
                {"Chalice: The Haunted Ruins", 0},
                {"Chalice: Ghost Ship", 0},
                {"Chalice: The Entrance Hall", 0},
                {"Chalice: The Time Device", 0},
                {"Hall of Heroes: Canny Tim 1", 0},
                {"Hall of Heroes: Canny Tim 2", 0},
                {"Hall of Heroes: Stanyer Iron Hewer 1", 0},
                {"Hall of Heroes: Stanyer Iron Hewer 2", 0},
                {"Hall of Heroes: Woden The Mighty 1", 0},
                {"Hall of Heroes: Woden The Mighty 2", 0},
                {"Hall of Heroes: RavenHooves The Archer 1", 0},
                {"Hall of Heroes: RavenHooves The Archer 2", 0},
                {"Hall of Heroes: RavenHooves The Archer 3", 0},
                {"Hall of Heroes: RavenHooves The Archer 4", 0},
                {"Hall of Heroes: Imanzi 1", 0},
                {"Hall of Heroes: Imanzi 2", 0},
                {"Hall of Heroes: Dark Steadfast 1", 0},
                {"Hall of Heroes: Dark Steadfast 2", 0},
                {"Hall of Heroes: Karl Stungard 1", 0},
                {"Hall of Heroes: Karl Stungard 2", 0},
                {"Hall of Heroes: Bloodmonath Skill Cleaver 1", 0},
                {"Hall of Heroes: Bloodmonath Skill Cleaver 2", 0},
                {"Hall of Heroes: Megwynne Stormbinder 1", 0},
                {"Hall of Heroes: Megwynne Stormbinder 1", 0 },
                {"Equipment: Small Sword",0 },
                {"Equipment: Broadsword",0 },
                {"Equipment: Magic Swords",0 },
                {"Equipment: Club",0 },
                {"Equipment: Hammer",0 },
                {"Equipment: Daggers",0 },
                {"Equipment: Axe",0 },
                {"Equipment: Chicken Drumsticks",0 },
                {"Equipment: Crossbow",0 },
                {"Equipment: Longbow",0 },
                {"Equipment: Fire Longbow",0 },
                {"Equipment: Magic Longbow",0 },
                {"Equipment: Spear",0 },
                {"Equipment: Lightning",0 },
                {"Equipment: Good Lightning",0 },
                {"Equipment: Copper Shield",0 },
                {"Equipment: Silver Shield",0 },
                {"Equipment: Gold Shield",0 },
                {"Equipment: Dragon Armour",0 },
                {"Life Bottle: Dan's Crypt", 0 } ,
                {"Life Bottle: The Graveyard", 0 } ,
                {"Life Bottle: Hall of Heroes (Canny Tim)", 0 } ,
                {"Life Bottle: Dan's Crypt - Behind Wall", 0 } ,
                {"Life Bottle: Scarecrow Fields", 0 } ,
                {"Life Bottle: Pools of the Ancient Dead", 0 } ,
                {"Life Bottle: Hall of Heroes (Ravenhooves The Archer)", 0 } ,
                {"Life Bottle: Hall of Heroes (Dirk Steadfast)", 0 } ,
                {"Life Bottle: The Time Device", 0 } ,
                {"Skill: Daring Dash", 0 } ,
                {"Chaos Rune: The Graveyard",0},
                {"Chaos Rune: The Hilltop Mausoleum",0},
                {"Chaos Rune: Scarecrow Fields",0},
                {"Chaos Rune: The Lake",0},
                {"Chaos Rune: Pumpkin Gorge",0},
                {"Chaos Rune: Sleeping Village",0},
                {"Chaos Rune: Pools of the Ancient Dead",0},
                {"Chaos Rune: Asylum Grounds",0},
                {"Chaos Rune: The Haunted Ruins",0},
                {"Chaos Rune: Ghost Ship",0},
                {"Chaos Rune: The Time Device",0},
                {"Earth Rune: The Graveyard",0},
                {"Earth Rune: The Hilltop Mausoleum",0},
                {"Earth Rune: Scarecrow Fields",0},
                {"Earth Rune: The Crystal Caves",0},
                {"Earth Rune: The Lake",0},
                {"Earth Rune: Pumpkin Gorge",0},
                {"Earth Rune: Sleeping Village",0},
                {"Earth Rune: Inside the Asylum",0},
                {"Earth Rune: Enchanted Earth",0},
                {"Earth Rune: The Haunted Ruins",0},
                {"Earth Rune: The Entrance Hall",0},
                {"Earth Rune: The Time Device",0},
                {"Moon Rune: The Hilltop Mausoleum",0},
                {"Moon Rune: Scarecrow Fields",0},
                {"Moon Rune: Pumpkin Gorge",0},
                {"Moon Rune: Ghost Ship",0},
                {"Moon Rune: The Time Device",0},
                {"Star Rune: Return to the Graveyard",0},
                {"Star Rune: Dan's Crypt",0},
                {"Star Rune: The Crystal Caves",0},
                {"Star Rune: The Lake",0},
                {"Star Rune: Enchanted Earth",0},
                {"Star Rune: The Gallows Gauntlet",0},
                {"Star Rune: Ghost Ship",0},
                {"Time Rune: The Lake",0},
                {"Time Rune: Pumpkin Gorge",0},
                {"Time Rune: The Time Device",0},
            };
        }

        public static void UpdateGoal(string key, int value)
        {
            var goals = GoalsCompleted();
            goals[key] = value;
        }

        public static Dictionary<string, int> GetSingleGoal(string key)
        {
            // Return a copy to prevent external modification if desired
            return new Dictionary<string, int>(GoalsCompleted()[key]);
        }

        public static Dictionary<string, int> FilterInactiveGoals()
        {
            Dictionary<string, int> allGoals = PlayerState.GoalsCompleted();

            Dictionary<string, int> goalsWithZero = allGoals
                .Where(pair => pair.Value == 0)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            return goalsWithZero;
        }


        public static Dictionary<string, int> GetListOfGoals()
        {
            // Return a copy to prevent external modification if desired
            return new Dictionary<string, int>(GoalsCompleted());
        }
    }
}
