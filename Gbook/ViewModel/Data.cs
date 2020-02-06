using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Gbook.ClassFiles;
using Gbook.Methods;

namespace Gbook.ViewModel
{
    public class Data : INotifyPropertyChanged
    {
        public string StudentName { get; set; }

        public string CourseName { get; set; }

        public string SectionCode { get; set; }

        private string classtype { get; set; }
        public string ClassType
        {
            get { return classtype; }
            set
            {
                classtype = value;
                this.RaisedOnPropertyChanged("ClassType");
            }
        }

        public string TermCode { get; set; }
        public string TermName { get; set; }

        public string OverallGrade { get; set; }

        public string Period { get; set; }

        private double overallpercent { get; set; }
        public double OverallPercent
        {
            get { return overallpercent; }
            set
            {
                if (overallpercent != value)
                {
                    overallpercent = value;
                    this.RaisedOnPropertyChanged("OverallPercent");
                    SetNewOverallColor();
                }

            }
        }

        private Color overallcolor { get; set; }
        public Color OverallColor
        {
            get { return overallcolor; }
            set
            {
                if (overallcolor != value)
                {
                    overallcolor = value;
                    this.RaisedOnPropertyChanged("OverallColor");
                }
            }
        }

        void SetNewOverallColor()
        {
            OverallColor = ColorGet.ColorFromPercent((int)Math.Round(OverallPercent, 0));
        }

        public string TeachersName { get; set; }
        public string TeachersEmail { get; set; }

        public int NoOfCat { get; set; }
        public List<CategoryInfo> CatInfoSet { get; set; }
        public List<Assignments> AssignmentsList { get; set; }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                this.RaisedOnPropertyChanged("IsExpanded");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PropertyName));
        }
    }

}
