using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Gbook.Methods;
using Xamarin.Forms;

namespace Gbook.ClassFiles
{
    public class Assignments : INotifyPropertyChanged
    {
        private int id { get; set; }
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                RaisedOnPropertyChanged("Id");
            }
        }

        private string description { get; set; }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                RaisedOnPropertyChanged("Description");
            }
        }

        private string date { get; set; }
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisedOnPropertyChanged("Date");
            }
        }

        private string assignmentType { get; set; }
        public string AssignmentType
        {
            get { return assignmentType; }
            set
            {
                assignmentType = value;
                RaisedOnPropertyChanged("AssignmentType");
            }
        }

        private double possible { get; set; }
        public double Possible
        {
            get { return possible; }
            set
            {
                possible = value;
                this.BackColor = ColorGet.ColorFromPercent((int)Math.Round(((this.Points / this.Possible) * 100), 0));
                this.Percent = ((this.Points / this.Possible) * 100);
                EvaluateGrade();
                RaisedOnPropertyChanged("Possible");
            }
        }

        private double points { get; set; }
        public double Points
        {
            get { return points; }
            set
            {
                points = value;
                this.BackColor = ColorGet.ColorFromPercent((int)Math.Round(((this.Points / this.Possible) * 100), 0));
                this.Percent = ((this.Points / this.Possible) * 100);
                EvaluateGrade();
                RaisedOnPropertyChanged("Points");
            }
        }

        private void EvaluateGrade()
        {
            if(this.Grade == "NG")
            {
                if(this.Points != 0)
                {
                    this.Grade = GradeFromScore.GetGrade(Percent);
                }
            }
            else
            {
                this.Grade = GradeFromScore.GetGrade(Percent);
            }
        }

        private double weight { get; set; }
        public double Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                RaisedOnPropertyChanged("Weight");
            }
        }

        private int catIndex { get; set; }
        public int CatIndex
        {
            get { return catIndex; }
            set
            {
                catIndex = value;
                RaisedOnPropertyChanged("CatIndex");
            }
        }

        private Color backColor { get; set; }
        public Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                RaisedOnPropertyChanged("BackColor");
            }
        }

        private double percent { get; set; }
        public double Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                RaisedOnPropertyChanged("Percent");
            }
        }

        private string grade { get; set; }
        public string Grade
        {
            get { return grade; }
            set
            {
                grade = value;
                RaisedOnPropertyChanged("Grade");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PropertyName));
        }
    }


}
