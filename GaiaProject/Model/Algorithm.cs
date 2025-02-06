using System;
using System.Collections.Generic;
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
    }
}
