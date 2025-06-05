using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archipelago.Core.Models;
using MedievilArchipelago.Models;

namespace MedievilArchipelago
{
    public class Helpers
    {
        public static List<Location> BuildLocationsList()
        {

        }
    }

    private static List<LevelData> GetLevelData()
        {
            return new List<LevelData>()
            {
                new LevelData("The Graveyard", 0x80000000, new List<string>() { "Shield of the Ancients", "Sword of the Ancients" }),
            }
        }
}
