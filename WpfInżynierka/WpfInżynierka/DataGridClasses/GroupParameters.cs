using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfInżynierka.DataGridClasses
{
    class GroupParameters
    {
        public string paramName { get; set; }

        public int paramSize { get; set; }

        public GroupParameters(string name, int size)
        {
            paramName = name;
           paramSize = size;
        }
    }
}
