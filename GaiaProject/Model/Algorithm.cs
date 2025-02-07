using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GaiaProject.Model
{
    public class Algorithm
    {
        public static bool IsValidNumber(string valueA, string valueB)
        {
            // תבנית לבדיקת מספרים שלמים ועשרוניים 
            string pattern = @"^-?\d*\.?\d+$";

            // בדיקה אם שני המספרים חוקיים
            return Regex.IsMatch(valueA, pattern) && Regex.IsMatch(valueB, pattern);
        }

        public static Dictionary<string, int> DetailOperation(ObservableCollection<OperationModel> operations)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            operations = new ObservableCollection<OperationModel>(operations.Where(op => op.Timestamp.Month == currentMonth && op.Timestamp.Year == currentYear).ToList());

            Dictionary<string,int> AllOperation = new Dictionary<string, int>();
            foreach (OperationModel operation in operations)
            {
                if (AllOperation.ContainsKey(operation.Operation))
                {
                    AllOperation[operation.Operation]++;
                }
                else
                {
                    AllOperation[operation.Operation] = 1;
                }
            }

            return AllOperation;
        }
    }
}
