namespace WpfApplication3.Models
{
    using Properties;

    public interface IColumn
    {
        string Name { get; }

        [CanBeNull]
        dynamic EditableValue { get; }
    }
}