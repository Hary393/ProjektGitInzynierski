namespace WpfApplication3.Models
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Properties;

    public class PropertyGrid : IColumn, INotifyPropertyChanged
    {
        private string _param_value;

        public event PropertyChangedEventHandler PropertyChanged;
        public string ParamName { get; set; }

        public string ParamValue
        {
            get { return this._param_value; }
            set
            {
                if (value == this._param_value) return;
                this._param_value = value;
                this.OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return this.ParamName; }
            set
            {
                this.ParamName = value;
                this.OnPropertyChanged();
            }
        }

        public dynamic EditableValue
        {
            get { return this.ParamValue; }
            set { this.ParamValue = value; }
        }

        public int Size { get; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}