namespace WpfApplication3.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Extensions;
    using Models;
    using Properties;

    public class DataViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _columns;
        private ObservableCollection<IRow> _rows;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<IRow> Rows
        {
            get { return this._rows; }
            set
            {
                if (Equals(value, this._rows)) return;
                this._rows = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Columns
        {
            get { return this._columns; }
            set
            {
                if (Equals(value, this._columns)) return;
                this._columns = value;
                this.OnPropertyChanged();
            }
        }

        public void PopulateViewModel(ObservableCollection<IRow> observableCollection)
        {
            this.Columns = observableCollection
                .SelectMany(r => r.Columns)
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(c => c)
                .ToObservableCollection();

            this.FillTheGaps(observableCollection);

            this.Rows = observableCollection;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FillTheGaps(IEnumerable<IRow> observableCollection)
        {
            foreach (var row in observableCollection)
            {
                foreach (var column in this.Columns.Where(c => !row.Columns.Select(rc => rc.Name)
                                                              .Contains(c)))
                {
                    row.Columns.Insert(this.Columns.IndexOf(column), new NullColumn {Name = column});
                }

                row.Columns = row.Columns.OrderBy(c => c.Name)
                    .ToObservableCollection();
            }
        }
    }
}