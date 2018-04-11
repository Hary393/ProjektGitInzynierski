using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            else if (parametersList.Count==0)
            {
                MessageBox.Show("Some parameters would be nice", "Nothing to report", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (fileNameTextBox.Text.Length==0)
            {
                MessageBox.Show("Name your file", "Nothing to do", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                generateData();
            }
        }

        void generateData()
        {
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
                        int gestoscXparam;

                        foreach (var group in groupList)                //Tworzenie obiektu i przypisanie do grupy
                        {
                            for (int i = 0; i < group.groupSize; i++)
                            {
                                obiektyGrupy += "Obiekt" + nrObiektu.ToString() + " - " + group.groupName + "\n";
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
                                obiektyParametry += "\n";
                                ++nrObiektu;
                            }
                        }
                        string path = dialog.SelectedPath + "\\\\" + fileNameTextBox.Text.ToString() + "Groups.txt";
                        System.IO.File.WriteAllText(path, obiektyGrupy);
                        path = dialog.SelectedPath + "\\\\" + fileNameTextBox.Text.ToString() + "Objects.txt";
                        System.IO.File.WriteAllText(path, obiektyParametry);
                        MessageBox.Show("Pliki zostały utworzone", "Nothing to acknowledge", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Pliki nie zostały utworzone błąd", "Nothing to work properly", MessageBoxButton.OK, MessageBoxImage.Warning);

                        throw;
                    }
                }
            }
            
        }///GenerateData
    }
}
