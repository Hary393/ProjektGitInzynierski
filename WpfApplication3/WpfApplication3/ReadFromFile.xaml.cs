using Accord.MachineLearning;
using Accord.Statistics.Analysis;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using WpfInżynierka.DataGridClasses;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for ReadFromFile.xaml
    /// </summary>
    public partial class ReadFromFile : Window
    {
        public List<Groups> groupList = new List<Groups>();
        public List<GroupsScore> groupScoreList = new List<GroupsScore>();
        List<List<double>> paramPassList = new List<List<double>>();
        int paramSize = 0;
        int objNumber = 0;
        Random rnd = new Random();


        public ReadFromFile()
        {
            InitializeComponent();
            scoreDataGrid.ItemsSource = groupScoreList;
        }

        private void selectFileButton_Click(object sender, RoutedEventArgs e)
        {
            string nextLine = "";
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                using (Stream s = openFileDialog.OpenFile())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        while (sr.Peek() >= 0)
                        {
                            nextLine = sr.ReadLine();
                            List<double> nextPassList = new List<double>();
                            Char delimiter = '-';
                            string[] firstSub = nextLine.Split(delimiter);
                            string paramsAndClass = firstSub[1].TrimStart();
                            Char delimiter2 = ',';
                            string[] secondSub = paramsAndClass.Split(delimiter2);
                            for (int i = 1; i < secondSub.Length; i++)
                            {
                                nextPassList.Add(double.Parse(secondSub[i]));
                            }


                            bool existFlag = false;
                            foreach (var group in groupList)
                            {
                                if (group.groupName.Equals(secondSub[0]))
                                {
                                    group.groupSize += 1;
                                    existFlag = true;
                                    break;
                                }
                            }
                            if (!existFlag)
                            {
                                groupList.Add(new Groups(secondSub[0], 1));
                            }
                            paramPassList.Add(nextPassList);
                            paramSize = nextPassList.Count;
                        }
                    }
                    groupDataGrid.ItemsSource = groupList;
                    paramLabel.Content = "Size of Parameters = " + paramSize;
                }
            }
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> classesDict = new Dictionary<string, int>();
            double[][] inputs;
            inputs = paramPassList.Select(list => list.ToArray()).ToArray();
            var knn = new KNearestNeighbors(k: 4);
            List<int> groupClasses = new List<int>();
            int clasessCount = 0;
            string currGroup = groupList[0].groupName;
            classesDict.Add(currGroup, clasessCount);
            foreach (var group in groupList)
            {
                if (!currGroup.Equals(group.groupName))
                {
                    clasessCount++;
                    currGroup = group.groupName;
                    classesDict.Add(currGroup, clasessCount);
                }
                for (int i = 0; i < group.groupSize; i++)
                {
                    groupClasses.Add(clasessCount);
                    objNumber++;
                }
                
            }
            int[] outputs;
            outputs = groupClasses.ToArray();
            // We learn the algorithm:
            knn.Learn(inputs, outputs);
            var cm = GeneralConfusionMatrix.Estimate(knn, inputs, outputs);

            // We can use it to estimate measures such as 
            double error = cm.Error;  // should be 
            double acc = cm.Accuracy; // should be 
            double kappa = cm.Kappa;  // should be

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string resultsToWrite = "";
                    foreach (var item in groupScoreList)
                    {
                        int counter = 0;
                        for (int i = 0; i < item.groupSize; i++)
                        {
                            while (counter!=100)
                            {
                                List<double> next = new List<double>();
                                for (int j = 0; j < paramSize; j++)
                                {
                                    next.Add(rnd.Next(2));
                                }
                                double[] paramValues = next.ToArray();
                                double scoreValue = knn.Score(paramValues, classesDict[item.groupName]);
                                if ((scoreValue*100)>item.groupScore)
                                {
                                    resultsToWrite += "Obiekt" + objNumber + " - " + item.groupName + ",";
                                    objNumber++;
                                    foreach (var para in paramValues)
                                    {
                                        resultsToWrite += para.ToString() + ",";
                                    }
                                    resultsToWrite+=Environment.NewLine;
                                    break;
                                }

                            }
                            counter = 0;
                        }

                    }

                    try
                    {
                        string path = dialog.SelectedPath + "\\\\" + "ExtendedExamples.txt";
                        System.IO.File.WriteAllText(path, resultsToWrite);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Coś Poszło nie tak", "Wynik Generacji", MessageBoxButton.OK,MessageBoxImage.Warning);
                        throw;
                    }
                    MessageBox.Show("Wygenerowano Plik", "Wynik Generacji", MessageBoxButton.OK);
                }
            }

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (groupDataGrid.SelectedItem != null)
            {
                Groups group = (Groups)groupDataGrid.SelectedItem;
                groupScoreList.Add(new GroupsScore(group.groupName, Int32.Parse(sizeTextBox.Text), Int32.Parse(scoreTextBox.Text)));
                scoreDataGrid.Items.Refresh();
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
                    scoreTextBox.Text = "100";
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
