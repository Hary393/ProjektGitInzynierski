namespace WpfApplication3.Extensions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class IEnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}