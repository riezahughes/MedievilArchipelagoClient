using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievilArchipelago.Models
{
    public class HallOfHeroesData
    {
        public string Name { get; set; }
        public int LevelId { get; set; }

        public HallOfHeroesData(string name, int levelId)
        {
            Name = name;
            LevelId = levelId;
        }
    }
}
