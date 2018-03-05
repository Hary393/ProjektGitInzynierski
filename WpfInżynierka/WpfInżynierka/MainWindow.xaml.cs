using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfInżynierka.DataGridClasses;

namespace WpfInżynierka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Groups> groupList;
        List<GroupParameters> parametersList;
        public MainWindow()
        {
            InitializeComponent();
            groupList = new List<Groups>();
            GroupDataGrid.ItemsSource = groupList;

            parametersList = new List<GroupParameters>();
            ParametersDataGrid.ItemsSource = parametersList;

        }

        private void buttonDodajGrupe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Groups nextGroup = new Groups(GroupNameTxtBox.Text.ToString(),
                                           Int32.Parse(GroupSizeTextBox.Text.ToString()));
                groupList.Add(nextGroup);
                GroupDataGrid.Items.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Fill up rquied fields", "No text", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void NumberValidation(object sender, TextCompositionEventArgs e) ///////////////////// Reagex so you can only use numbers in txt box
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }

        private void buttonDodajParametr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GroupParameters nextParameter = new GroupParameters(ParametersNameTxtBox.Text.ToString(),
                                           Int32.Parse(ParametersSizeTextBox.Text.ToString()));
                parametersList.Add(nextParameter);
                ParametersDataGrid.Items.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Fill up rquied fields", "No text", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

    }
}
