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

        public GenericItemsData(string name, int id, uint locationAddress, string check)
        {
            Name = name;
            Id = id;
            Address = locationAddress;
            Check = check;
        }
    }
}
