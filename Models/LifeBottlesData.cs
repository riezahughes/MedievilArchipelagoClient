using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// really dumb class to have, i know. will refactor later. trying to keep patterns going
namespace MedievilArchipelago.Models
{
    public class LifeBottlesData
    {
        public string Name { get; set; }
        public int LevelId { get; set; }

        public LifeBottlesData(string name, int levelId)
        {
            Name = name;
            LevelId = levelId;
        }
    }
}
