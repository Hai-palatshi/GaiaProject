using GaiaProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GaiaProject.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region property

        private string valueA;
        public string ValueA
        {
            get { return valueA; }
            set
            {
                valueA = value;
                OnPropertyChanged();
            }
        }

        private string valueB;
        public string ValueB
        {
            get { return valueB; }
            set
            {
                valueB = value;
                OnPropertyChanged();
            }
        }

        private double result;
        public double Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }

        private string operation;
        public string Operation
        {
            get { return operation; }
            set
            {
                operation = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<OperationModel> history;
        public ObservableCollection<OperationModel> History
        {
            get { return history; }
            set
            {
                history = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<OperationModel> showListHstory;
        public ObservableCollection<OperationModel> ShowListHstory
        {
            get { return showListHstory; }
            set
            {
                showListHstory = value;
                OnPropertyChanged();
            }
        }
        private string error;
        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                OnPropertyChanged();
            }
        }

        private Visibility valid;
        public Visibility Valid
        {
            get { return valid; }
            set
            {
                valid = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<string, int> allOperation;
        public Dictionary<string, int> AllOperation
        {
            get { return allOperation; }
            set
            {
                allOperation = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region command
        public ICommand CalculateCommand { get; set; }
        #endregion

        private DatabaseService _databaseService;

        public MainViewModel()
        {
            Valid = Visibility.Collapsed;
            _databaseService = new DatabaseService();
            CalculateCommand = new RelayCommand(Calculate);
            History = new ObservableCollection<OperationModel>();
            ShowListHstory = new ObservableCollection<OperationModel>(_databaseService.GetHistory());
            AllOperation = Algorithm.DetailOperation(ShowListHstory);

        }

        #region function
        private void Calculate()
        {
            if (Algorithm.IsValidNumber(this.ValueA, this.ValueB))
            {
                Valid = Visibility.Collapsed;
                switch (Operation)
                {
                    case "+": Result = Convert.ToDouble(ValueA) + Convert.ToDouble(ValueB); break;
                    case "-": Result = Convert.ToDouble(ValueA) - Convert.ToDouble(ValueB); break;
                    case "*": Result = Convert.ToDouble(ValueA) * Convert.ToDouble(ValueB); break;
                    case "/":
                        if (Convert.ToDouble(ValueB) != 0)
                            Result = Convert.ToDouble(ValueA) / Convert.ToDouble(ValueB);
                        else
                        {
                            Valid = Visibility.Visible;
                            Error = "Invalid value";
                            return; //  display an error message to the client.
                        }

                        break;

                    default: Result = 0; break;
                }

                var operationModel = new OperationModel
                {
                    ValueA = Convert.ToDouble(this.ValueA),
                    ValueB = Convert.ToDouble(this.ValueB),
                    Operation = this.Operation,
                    Result = this.Result,
                    Timestamp = DateTime.Now
                };

                History.Add(operationModel);
                _databaseService.SaveOperation(operationModel);
                CreateTxt();
            }
            else
            {
                Valid = Visibility.Visible;
                Error = "Invalid value";
            }
        }

        public void CreateTxt()
        {
            string projectPath = Directory.GetCurrentDirectory(); // The current project path.
            string folderPath = Path.Combine(projectPath, "Data"); // The path to the Data folder.
            string filePath = Path.Combine(folderPath, "results.txt"); // The full path to the file.

            // Creating the Data folder if it does not exist.
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Retrieving the calculation history and saving it to an ObservableCollection.
            ShowListHstory = new ObservableCollection<OperationModel>(_databaseService.GetHistory());
            AllOperation = Algorithm.DetailOperation(showListHstory);
            // Using a single StringBuilder to collect all the data.
            StringBuilder sb = new StringBuilder();

            foreach (var item in ShowListHstory)
            {
                sb.AppendLine($"ValueA: {item.ValueA}, Operation: {item.Operation}, ValueB: {item.ValueB}, Result: {item.Result}, Timestamp: {item.Timestamp}");
                sb.AppendLine("--------------------------------");
            }

            // Writing the data to a file.
            File.WriteAllText(filePath, sb.ToString());
        }

        #endregion
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
