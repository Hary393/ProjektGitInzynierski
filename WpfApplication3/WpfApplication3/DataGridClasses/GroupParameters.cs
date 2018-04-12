using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfInżynierka.DataGridClasses
{
    public class GroupParameters
    {
        public string paramName { get; set; }

        public int paramSize { get; set; }

        public int paramDensity { get; set; }

        public GroupParameters(string name, int size, int density)
        {
            paramName = name;
            paramSize = size;
            paramDensity = density;
        }
    }
}
