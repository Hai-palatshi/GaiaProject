using GaiaProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public MainViewModel()
        {
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

            History.Add(
                new OperationModel
                {
                    ValueA = this.ValueA,
                    ValueB = this.ValueB,
                    Operation = this.Operation,
                    Result = this.Result,
                    Timestamp = DateTime.Now
                });
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
