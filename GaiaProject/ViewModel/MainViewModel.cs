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
using System.Windows.Input;

namespace GaiaProject.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region property


        private double valueA;
        public double ValueA
        {
            get { return valueA; }
            set
            {
                valueA = value;
                OnPropertyChanged();
            }
        }

        private double valueB;
        public double ValueB
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

        #endregion

        #region command
        public ICommand CalculateCommand { get; set; }
        #endregion

        private DatabaseService _databaseService;
        public MainViewModel()
        {
            _databaseService = new DatabaseService();
            CalculateCommand = new RelayCommand(Calculate);
            History = new ObservableCollection<OperationModel>();
        }

        #region function
        private void Calculate()
        {
            switch (Operation)
            {
                case "+": Result = ValueA + ValueB; break;
                case "-": Result = ValueA - ValueB; break;
                case "*": Result = ValueA * ValueB; break;
                case "/": Result = ValueB != 0 ? ValueA / ValueB : 0; break;
                default: Result = 0; break;
            }

            var operationModel = new OperationModel
            {
                ValueA = this.ValueA,
                ValueB = this.ValueB,
                Operation = this.Operation,
                Result = this.Result,
                Timestamp = DateTime.Now
            };

            History.Add(operationModel);
            CreateTxt();
            _databaseService.SaveOperation(operationModel);
        }

        public void CreateTxt()
        {
            List<OperationModel> list = new List<OperationModel>();
            string projectPath = Directory.GetCurrentDirectory(); // הנתיב הנוכחי של הפרויקט
            string folderPath = Path.Combine(projectPath, "Data"); // הנתיב לתיקיית Data
            string filePath = Path.Combine(folderPath, "results.txt"); // הנתיב המלא לקובץ

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath); // יצירת תיקיית Data אם לא קיימת
            }

            list = _databaseService.GetHistory();



            // כתיבת התוצאה לקובץ (הוספה במקום דריסה)
            foreach (var item in list)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"ValueA: {item.ValueA}");
                sb.AppendLine($"Operation: {item.Operation}");
                sb.AppendLine($"ValueB: {item.ValueB}");
                sb.AppendLine($"Result: {item.Result}");
                sb.AppendLine($"Timestamp: {item.Timestamp}");
                sb.AppendLine("--------------------------------");

                File.AppendAllText(filePath, sb.ToString());
            }
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
