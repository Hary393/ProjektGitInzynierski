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
using System.Windows.Shapes;
using WpfApplication3.ViewModels;
using WpfInżynierka.DataGridClasses;


namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<Groups> groupList;
        public List<GroupParameters> parametersList;
        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
            groupList = new List<Groups>();             //utworzenie listy grup i parametrów oraz ustawienie ich jako źródło odp datagridów
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
        private void ParametersDensityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) ///////////////////// Reagex so you can only use numbers in txt box and number max is 100
        {
            Regex regex = new Regex("[^0-9]+");
            if (!regex.IsMatch(e.Text))
            {
                if (Int32.Parse(((TextBox)sender).Text + e.Text) > 100)
                {
                    ParametersDensityTextBox.Text = "100";
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }


        private void buttonDodajParametr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GroupParameters nextParameter = new GroupParameters(ParametersNameTxtBox.Text.ToString(),
                                           Int32.Parse(ParametersSizeTextBox.Text.ToString()),
                                           Int32.Parse(ParametersDensityTextBox.Text.ToString()));
                parametersList.Add(nextParameter);
                ParametersDataGrid.Items.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Fill up rquied fields", "No text", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void buttonUsunZaznaczone_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (Groups)GroupDataGrid.SelectedItem;
            var selectedItem2 = (GroupParameters)ParametersDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                groupList.Remove(selectedItem);
                GroupDataGrid.Items.Refresh();
            }
            if (selectedItem2 != null)
            {
                parametersList.Remove(selectedItem2);
                ParametersDataGrid.Items.Refresh();
            }
        }

        private void buttonGeneruj_Click(object sender, RoutedEventArgs e)
        {
            if (groupList.Count == 0)
            {
                MessageBox.Show("Add group first", "Nothing to show", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (parametersList.Count == 0)
            {
                MessageBox.Show("Some parameters would be nice", "Nothing to report", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (fileNameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Name your file", "Nothing to do", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (caltulateDataSize())
            {
                MessageBox.Show("To much objects to create because of insuficient numbers of parameters", "Nothing to do", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                generateData();
            }
        }


        bool caltulateDataSize()
        {
            int groupSize = 0;
            int paramSize = 0;
            foreach (var group in groupList)
            {
                groupSize += group.groupSize;
            }
            foreach (var param in parametersList)
            {
                paramSize += param.paramSize;
            }

            if (groupSize <= Math.Pow(paramSize, 2))
                return false;
            return true;
        }


        void generateData()
        {
            DataGridWindow KEK = new DataGridWindow(groupList, parametersList);
            var dialogresult= KEK.ShowDialog();
            if (dialogresult == true)
            {
                DataViewModel GeneratingData = (DataViewModel)KEK.DataContext;

                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        try
                        {
                            int nrObiektu = 1;
                            string obiektyGrupy = "";
                            string obiektyParametry = "";
                            string newObject = "";
                            HashSet<string> uniqueObiekty = new HashSet<string>();
                            int gestoscXparam;
                            int groupnr = 0;
                            bool notunique = true;
                            List<int> groupPassList = new List<int>();
                            List<List<double>> paramPassList = new List<List<double>>();
                            


                            foreach (var group in GeneratingData.Rows)                //Tworzenie obiektu i przypisanie do grupy
                            {
                                for (int i = 0; i < groupList[groupnr].groupSize; i++)
                                {
                                    obiektyGrupy += "Obiekt" + nrObiektu.ToString() + " - " + group.Name + Environment.NewLine;
                                    obiektyParametry += "Obiekt" + nrObiektu.ToString() + " - "; //Tworzenie parametrów i przypisanie do obiektu
                                    groupPassList.Add(groupnr);
                                    notunique = true;
                                    while (notunique){
                                        List<double> nextPassList = new List<double>();
                                        newObject = "";
                                        foreach (var prop in group.Columns)
                                        {
                                            gestoscXparam = rnd.Next(0, 100);
                                            int value = 0;
                                            if (Int32.TryParse(prop.EditableValue, out value))
                                            {
                                                if (gestoscXparam <= value)               //Paramety o podanej gęstosci zasięgu 0-100 
                                                {///////////////////////////tutaj to ogólnie trzeba random dyskretny wjebac gdzie 100 to zawsze 0 to nigdy bo teraz to mi nie wychodzi i brane z Parametru
                                                    newObject += "1" + " ,";
                                                    nextPassList.Add(1);
                                                }
                                                else
                                                {
                                                    newObject += "0" + " ,";
                                                    nextPassList.Add(0);
                                                }
                                            }
                                            else
                                            {
                                                int valueRnd = rnd.Next(0, 2) ;
                                                newObject += valueRnd.ToString() + " , ";//jak nie ma gęstości to poprostu random
                                                nextPassList.Add(valueRnd);
                                            }
                                        }

                                        if (uniqueObiekty.Add(newObject))
                                        {
                                            notunique = false;
                                            obiektyParametry += newObject;
                                            paramPassList.Add(nextPassList);
                                        }

                                    }//while not unique


                                    obiektyParametry = obiektyParametry.TrimEnd(',');
                                    obiektyParametry += Environment.NewLine;
                                    ++nrObiektu;
                                }
                                groupnr++;
                            }///Rows
                            /*
                            
                             
                             foreach (var group in groupList)                //Tworzenie obiektu i przypisanie do grupy
                            {
                                for (int i = 0; i < group.groupSize; i++)
                                {
                                    obiektyGrupy += "Obiekt" + nrObiektu.ToString() + " - " + group.groupName + "\\n";
                                    obiektyParametry += "Obiekt" + nrObiektu.ToString() + " - "; //Tworzenie parametrów i przypisanie do obiektu
                                    foreach (var prop in parametersList)
                                    {
                                        gestoscXparam = rnd.Next(0, 100);
                                        if (gestoscXparam <= prop.paramDensity)               //Paramety o podanej gęstosci zasięgu
                                        {
                                            obiektyParametry += rnd.Next(0, prop.paramSize) + " , "; //Paramety o podanym zasięgu
                                        }
                                        else
                                        {
                                            obiektyParametry += "0" + " ,";
                                        }
                                    }
                                    obiektyParametry = obiektyParametry.TrimEnd(',');
                                    obiektyParametry += "\\n";
                                    ++nrObiektu;
                                }
                            }
                             
                             
                             
                             */
                            string path = dialog.SelectedPath + "\\\\" + fileNameTextBox.Text.ToString() + "Groups.txt";
                            System.IO.File.WriteAllText(path, obiektyGrupy);
                            path = dialog.SelectedPath + "\\\\" + fileNameTextBox.Text.ToString() + "Objects.txt";
                            System.IO.File.WriteAllText(path, obiektyParametry);
                            MessageBox.Show("Pliki zostały utworzone", "Nothing to acknowledge", MessageBoxButton.OK, MessageBoxImage.Information);
                            ValidationWindow validate = new ValidationWindow(groupPassList, paramPassList);
                            validate.Show();

                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Pliki nie zostały utworzone błąd", "Nothing to work properly", MessageBoxButton.OK, MessageBoxImage.Warning);

                            throw;
                        }
                    }//folderBrowserDialogResult
                }//folderBrowserDialogUsing
            }//resultWindowDialog
            else
            {
                MessageBox.Show("Jakiś Błąd w drugim oknie", "Nothing to work properly", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }///GenerateData
    }
}
