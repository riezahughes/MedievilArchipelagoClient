using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievilArchipelago.Models
{
    public class GenericItemsData
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public uint Address { get; set; }

        public string Check {  get; set; }

        public bool IsInChest { get; set; }
        public bool DynamicItem { get; set; }

        public GenericItemsData(string name, uint locationAddress, string check, bool isChest = false, bool isDynamicItem = false, int id = 0)
        {
            Name = name;
            Id = id;
            Address = locationAddress;
            Check = check;
            IsInChest = isChest;
            DynamicItem = isDynamicItem;
        }
    }
}
