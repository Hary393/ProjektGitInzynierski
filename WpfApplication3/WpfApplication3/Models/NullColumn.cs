using System;

namespace WpfApplication3.Models
{
    public class NullColumn : IColumn
    {
        public string Name { get; set; }
        public dynamic EditableValue { get; set; }

    }
}