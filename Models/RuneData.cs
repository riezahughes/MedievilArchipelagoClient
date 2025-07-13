using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievilArchipelago.Models
{
    public class RuneData
    {
        public string Name { get; set; }
        public int LevelId { get; set; }

        public RuneData(string name, int levelId)
        {
            Name = name;
            LevelId = levelId;
        }
    }
}
