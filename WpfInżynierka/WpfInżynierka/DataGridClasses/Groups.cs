using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfInżynierka.DataGridClasses
{
    public class Groups
    {
        public string groupName { get; set; }

        public int groupSize { get; set; }

        public Groups(string name,int size) {
            groupName = name;
            groupSize = size;
        }
    }
}
