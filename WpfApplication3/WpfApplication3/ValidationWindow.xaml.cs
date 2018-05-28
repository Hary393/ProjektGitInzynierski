using Accord.MachineLearning;
using Accord.MachineLearning.Bayes;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math.Optimization.Losses;
using Accord.Statistics.Analysis;
using Accord.Statistics.Distributions.Fitting;
using Accord.Statistics.Kernels;
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
    /// Interaction logic for ValidationWindow.xaml
    /// </summary>
    public partial class ValidationWindow : Window
    {

        int[] outputs;
        double[][] inputs;
        int[][] inputsInt;
        string message = "";
        public ValidationWindow(List<int> GroupList,List<List<double>> ParamList)
        {
            InitializeComponent();
            outputs = GroupList.ToArray();
            inputs = ParamList.Select(list => list.ToArray()).ToArray();
            inputsInt = ParamList.Select(x => x.Select(y => (int)y).ToArray())
                         .ToArray();

            knnValidation();
            multilabelSVM();
            nativeBayesValidation();
            MessageBox.Show(message, "wynik walidacji", MessageBoxButton.OK);
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        public void knnValidation()
        {
            
            var crossvalidation = CrossValidation.Create(
                k: 3,
                learner: (p) => new KNearestNeighbors(k: 4),
                loss: (actual, expected, p) => new ZeroOneLoss(expected).Loss(actual),
                fit: (teacher, x, y, w) => teacher.Learn(x, y, w),
                x: inputs, y: outputs
                );
            var result = crossvalidation.Learn(inputs, outputs);
            // We can grab some information about the problem:
            int numberOfSamples = result.NumberOfSamples; 
            int numberOfInputs = result.NumberOfInputs;   
            int numberOfOutputs = result.NumberOfOutputs; 

            double trainingError = result.Training.Mean; 
            double validationError = result.Validation.Mean; 

            // If desired, compute an aggregate confusion matrix for the validation sets:
            GeneralConfusionMatrix gcm = result.ToConfusionMatrix(inputs, outputs);
            double accuracy = gcm.Accuracy;
            message += "Knn Validacja\n";
            message += "trainingError " + trainingError.ToString() + "\n";
            message += "validationError " + validationError.ToString() + "\n";
            message += "accuracy " + accuracy.ToString() + "\n\n";

        }


        public void nativeBayesValidation()
        {
            
            var learn = new NaiveBayesLearning();
            NaiveBayes nb = learn.Learn(inputsInt, outputs);

            var cv = CrossValidation.Create(
            k: 3,

            learner: (p) => new NaiveBayesLearning(),

            loss: (actual, expected, p) => new ZeroOneLoss(expected).Loss(actual),

            fit: (teacher, x, y, w) => teacher.Learn(x, y, w),

            x: inputsInt, y: outputs
        );

            var result = cv.Learn(inputsInt, outputs);

            int numberOfSamples = result.NumberOfSamples;
            int numberOfInputs = result.NumberOfInputs;
            int numberOfOutputs = result.NumberOfOutputs;

            double trainingError = result.Training.Mean;
            double validationError = result.Validation.Mean;
            GeneralConfusionMatrix gcm = result.ToConfusionMatrix(inputsInt, outputs);
            double accuracy = gcm.Accuracy;
            message += "Native Bayes Validacja\n";
            message += "trainingError " + trainingError.ToString() + "\n";
            message += "validationError " + validationError.ToString() + "\n";
            message += "accuracy " + accuracy.ToString() + "\n\n";
        }

        public void multilabelSVM()
        {
            
            var teacher = new MulticlassSupportVectorLearning<Gaussian>()
            {
                // Configure the learning algorithm to use SMO to train the
                //  underlying SVMs in each of the binary class subproblems.
                Learner = (param) => new SequentialMinimalOptimization<Gaussian>()
                {
                    // Estimate a suitable guess for the Gaussian kernel's parameters.
                    // This estimate can serve as a starting point for a grid search.
                    UseKernelEstimation = true
                }
            };

            // Learn a machine
            var machine = teacher.Learn(inputs, outputs);


            // Create the multi-class learning algorithm for the machine
            var calibration = new MulticlassSupportVectorLearning<Gaussian>()
            {
                Model = machine, // We will start with an existing machine

                // Configure the learning algorithm to use Platt's calibration
                Learner = (param) => new ProbabilisticOutputCalibration<Gaussian>()
                {
                    Model = param.Model // Start with an existing machine
                }
            };


            // Configure parallel execution options
            calibration.ParallelOptions.MaxDegreeOfParallelism = 1;

            // Learn a machine
            calibration.Learn(inputs, outputs);

            // Obtain class predictions for each sample
            int[] predicted = machine.Decide(inputs);

            // Get class scores for each sample
            double[] scores = machine.Score(inputs);

            // Get log-likelihoods (should be same as scores)
            double[][] logl = machine.LogLikelihoods(inputs);

            // Get probability for each sample
            double[][] prob = machine.Probabilities(inputs);

            // Compute classification error
            double error = new ZeroOneLoss(outputs).Loss(predicted);
            double loss = new CategoryCrossEntropyLoss(outputs).Loss(prob);
            message += "SVM Validacja\n";
            message += "error " + error.ToString() + "\n";
            message += "loss " + loss.ToString() + "\n\n";
        }

    }
}
