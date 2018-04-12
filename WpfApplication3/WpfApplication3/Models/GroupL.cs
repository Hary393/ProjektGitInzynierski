namespace WpfApplication3.Models
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Properties;

    public class GroupGrid : IRow, INotifyPropertyChanged
    {
        private string _name;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name
        {
            get { return this._name; }
            set
            {
                if (value == this._name) return;
                this._name = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<IColumn> Columns
        {
            get { return this.Params; }
            set
            {
                this.Params = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<IColumn> Params { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}