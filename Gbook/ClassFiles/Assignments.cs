using System;
using System.ComponentModel;
using Gbook.Methods;
using Xamarin.Forms;

namespace Gbook.ClassFiles
{
    public class Assignments : INotifyPropertyChanged
    {
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
                RaisedOnPropertyChanged("AssignmentsPossible");
            }
        }

        private double points { get; set; }
        public double Points
        {
            get { return points; }
            set
            {
                points = value;
                RaisedOnPropertyChanged("AssignmentsPoints");
            }
        }


        private double weight { get; set; }
        public double Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                RaisedOnPropertyChanged("AssignmentsWeight");
            }
        }

        private int catIndex { get; set; }
        public int CatIndex
        {
            get { return catIndex; }
            set
            {
                catIndex = value;
                RaisedOnPropertyChanged("CategoryIndexChanged");
            }
        }

        private Color backColor { get; set; }
        public Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                RaisedOnPropertyChanged("BackgroundColor");
            }
        }

        private double percent { get; set; }
        public double Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                RaisedOnPropertyChanged("AssignmentsPercent");
            }
        }

        private string grade { get; set; }
        public string Grade
        {
            get { return grade; }
            set
            {
                grade = value;
                RaisedOnPropertyChanged("AssignmentsGrade");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PropertyName));
            if(_PropertyName == "AssignmentsPoints" || _PropertyName == "AssignmentsPossible")
            {
                this.BackColor = ColorGet.ColorFromPercent((int)Math.Round(((this.Points / this.Possible)*100), 0));
            }
        }
    }


}
