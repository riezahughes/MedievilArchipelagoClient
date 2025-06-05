using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievilArchipelago.Models
{
    public class LevelData
    {
        public string Name { get; set; }
        public uint Address { get; set; }

        public List<string> UniqueLevelItems { get; set; } = new List<string>();

        public List<string> RunesRequired { get; set; } = new List<string>();
        public LevelData(string name, uint address, List<string> uniqueLevelItems, List<string> runesRequired)
        {
            Name = name;
            Address = address;
            UniqueLevelItems = new List<string>();
            RunesRequired = new List<string>();
        }
    }
}
