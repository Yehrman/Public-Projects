using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace EducationAlarm.ViewModels
{
    public class MathProblems
    {
        ViewDbLookups db = new ViewDbLookups();
         int Num1 { get; set; }
         int Num2 { get; set; }
          char Symbol { get; set; }
      

        Random r = new Random();
       //List<int> TempList(string id)
       // {
       //     List<int> tempSolutions = new List<int>();
       //     string subject = db.FindSubject(id);
       //     int answer = PickMathType(subject);
       //     tempSolutions.Add(answer);
       //     for (int i = 1; i < 4; i++)
       //     {              
       //         int guess = PickGuess(subject);
       //         if (tempSolutions.All(x => x != guess))
       //         {
       //             tempSolutions.Add(guess);
       //         }
       //         else
       //         {
       //             i = i - 1;
       //         }
       //     }
       //     return tempSolutions;
       // }
        public ArrayList ShuffledList(string id)
        {
           // var temp = TempList(id);
            string subject = db.FindSubject(id);
            Symbol = PickSymbol(subject);
            int answer = PickMathType(subject);
            ArrayList shuffled = new ArrayList();
            shuffled.Add(Num1);
            shuffled.Add(Symbol);
            shuffled.Add(Num2);
            shuffled.Add(answer);
            //while(temp.Count!=0)
            //{
            //    int a = r.Next(0, temp.Count);
            //    shuffled.Add(temp[a]);
            //    temp.RemoveAt(a);
            //}
            return shuffled;
        }
        private int Add()
        {
            Num1 = r.Next(1, 10);
            Num2 = r.Next(1, 10);
         //   Equation = Num1.ToString() + " +" + Num2.ToString();
            return Num1 + Num2;
        }
        private int Subtract()
        {
            Num1 = r.Next(2, 20);
            Num2 = r.Next(1, 10);
           // Equation = Num1.ToString() + " -" + Num2.ToString();
            if(Num1<Num2)
            {
                Subtract();
            }
            return Num1 - Num2;
        }
        private int Multiply()
        {
            Num1 = r.Next(1, 10);
            Num2 = r.Next(1, 10);
            //Equation = Num1.ToString() + " *" + Num2.ToString();
            return Num1 * Num2;
        }
        private int Divide()
        {
            Num1 = r.Next(2, 100);
            Num2 = r.Next(1, 50);
            if(Num1<Num2||Num1%Num2!=0)
            {
                Divide();
            }
            //Equation = Num1.ToString() + " /" + Num2.ToString();

            //We need to ensure this is a int or change to decimal 
            return Num1 / Num2;
        }
   
        private int PickMathType(string subject)
        {
            int x = 0;
            switch (subject)
            {
                case "Simple Addition":
                    x = Add();
                    break;
                case "Simple Subtraction":
                    x = Subtract();
                    break;
                case "Simple Multiplication":
                    x = Multiply();
                    break;
                case "Simple Division":
                    x = Divide();
                    break;
            }
            return x;
        }
        private char x { get; set; }
           char PickSymbol(string subject)
        {
           
            switch (subject)
            {
                case "Simple Addition":
                    x = '+';
                    break;
                case "Simple Subtraction":
                    x = '-';
                    break;
                case "Simple Multiplication":
                   x = '*';
                    break;
                case "Simple Division":
                    x = '/';
                    break;
            }
            return x;
        }
    }
}