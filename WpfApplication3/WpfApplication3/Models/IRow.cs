namespace WpfApplication3.Models
{
    using System.Collections.ObjectModel;

    public interface IRow
    {
        string Name { get; }
        ObservableCollection<IColumn> Columns { get; set; }
    }
}