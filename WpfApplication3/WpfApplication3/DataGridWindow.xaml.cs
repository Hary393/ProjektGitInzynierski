namespace WpfApplication3
{
    using System.Collections.ObjectModel;
    using Models;
    using ViewModels;
    using WpfInżynierka.DataGridClasses;
    using System.Collections.Generic;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DataGridWindow
    {
        public DataGridWindow(List<Groups> groupList,List<GroupParameters> parametersList)
        {
            this.InitializeComponent();

            var dataViewModel = new DataViewModel();
            this.DataContext = dataViewModel;

            
            var Groups = new ObservableCollection<IRow>();
            foreach (var gitem in groupList)
            {
                var ParamsToAdd = new ObservableCollection<IColumn>();
                foreach (var item in parametersList)
                {
                    for (int i = 1; i < item.paramSize + 1; i++)
                    {
                        ParamsToAdd.Add(new PropertyGrid { ParamName = item.paramName + i, ParamValue = item.paramDensity.ToString() });
                    }
                }
                Groups.Add(new GroupGrid
                {
                    Name = gitem.groupName,
                    Params = ParamsToAdd
                });
            }
            dataViewModel.PopulateViewModel(Groups);
        }

        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.GridKek.Items.Refresh();
            this.DialogResult = true;
        }
    }
}