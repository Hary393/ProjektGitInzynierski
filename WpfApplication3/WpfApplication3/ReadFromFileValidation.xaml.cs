using Accord.Statistics.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for ReadFromFileValidation.xaml
    /// </summary>
    public partial class ReadFromFileValidation : Window
    {
        public ReadFromFileValidation(GeneralConfusionMatrix originalData, GeneralConfusionMatrix testData, GeneralConfusionMatrix splitData,double testScore, double splitScore)
        {
            InitializeComponent(); 
            OrginalAcc.Content = "Accuracy: " + string.Format("{0:0.000}", originalData.Accuracy);
            OrginalError.Content="Error: "+ string.Format("{0:0.000}", originalData.Error);

            TestAcc.Content = "Accuracy: " + string.Format("{0:0.000}", testData.Accuracy);
            TestError.Content = "Error: " + string.Format("{0:0.000}", testData.Error);
            TestScore.Content = "Score: " + string.Format("{0:0.000}", testScore);

            SplitAcc.Content = "Accuracy: " + string.Format("{0:0.000}", splitData.Accuracy);
            SplitError.Content = "Error: " + string.Format("{0:0.000}", splitData.Error);
            SplitScore.Content = "Score: " + string.Format("{0:0.000}", splitScore);
        }
    }
}
