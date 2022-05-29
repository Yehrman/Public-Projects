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
        public  string Equation { get; set; }
      

        Random r = new Random();
      public List<int> TempList(string id)
        {
            List<int> tempSolutions = new List<int>();
            string subject = db.FindSubject(id);
            int answer = PickMathType(subject);
            tempSolutions.Add(answer);
            for (int i = 1; i < 4; i++)
            {
               
                int guess = PickGuess(subject);
                if (tempSolutions.All(x => x != guess))
                {
                    tempSolutions.Add(guess);
                }
                else
                {
                    i = i - 1;
                }
            }
            return tempSolutions;
        }
        //public List<MathProblems>ShuffledList(string id)
        //{
        //    var temp = TempList(id);
        //    List<MathProblems> shuffled =new List<MathProblems>();
        //    shuffled.Add();
        //}
    
        public ArrayList ShuffledList(string id)
        {
            var temp = TempList(id);
            ArrayList shuffled = new ArrayList();
            shuffled.Add(Equation);
            while(temp.Count!=0)
            {
                int a = r.Next(0, temp.Count);
                shuffled.Add(temp[a]);
                temp.RemoveAt(a);
            }
            return shuffled;
        }
        private int Add()
        {
            Num1 = r.Next(1, 10);
            Num2 = r.Next(1, 10);
            Equation = Num1.ToString() + " +" + Num2.ToString();
            return Num1 + Num2;
        }
        private int Subtract()
        {
            Num1 = r.Next(2, 20);
            Num2 = r.Next(1, 10);
            Equation = Num1.ToString() + " -" + Num2.ToString();
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
            Equation = Num1.ToString() + " *" + Num2.ToString();
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
            Equation = Num1.ToString() + " /" + Num2.ToString();

            //We need to ensure this is a int or change to decimal 
            return Num1 / Num2;
        }
        //For some reason the pick guess method picks the same 
        //thing every time
        public int PickGuess(string subject)
        {       
                int guess = 0;
            switch (subject)
            {
                case "addition":
                    guess = r.Next(1, 20);
                    break;
                case "subtraction":
                   guess = r.Next(1, 19);
                    break;
                case "multiplication":
                    guess = r.Next(1,100);
                    break;
                case "division":
                    guess= r.Next(1, 100);
                    break;
            }
            return guess;
        }
        private int PickMathType(string subject)
        {
            int x = 0;
            switch (subject)
            {
                case "addition":
                    x = Add();
                    break;
                case "subtraction":
                    x = Subtract();
                    break;
                case "multiplication":
                    x = Multiply();
                    break;
                case "division":
                    x = Divide();
                    break;
            }
            return x;
        }
        public char Symbol(string subject)
        {
            char x = ' ';
            switch (subject)
            {
                case "addition":
                    x = '+';
                    break;
                case "subtraction":
                    x = '-';
                    break;
                case "multiplication":
                    x = '*';
                    break;
                case "division":
                    x = '/';
                    break;
            }
            return x;
        }
    }
}