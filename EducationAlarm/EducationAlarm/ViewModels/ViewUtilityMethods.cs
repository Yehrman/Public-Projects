using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EducationAlarm.ViewModels
{
    public class ViewUtilityMethods
    {

        string _Symbol { get; set; }
        public string Symbol(int ans = 0, int? myAns = 0)
        {

            if (ans == myAns)
            {
                _Symbol = "\u2713";
            }
            else
            {
                _Symbol = "x";
            }
            return _Symbol;
        }
        public int MathOp(string subject, int x, int y)
        {
            int answer = 0;
            switch (subject)
            {
                case "Simple Addition":
                    answer = x + y;
                    break;
                case "Simple Subtraction":
                    answer = x - y;
                    break;
                case "Simple Multiplication":
                    answer = x * y;
                    break;
                case "Simple Division":
                    answer = x / y;
                    break;
            }
            return answer;
        }
    }
}