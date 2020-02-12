using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Gbook.ViewModel
{
    public class OverallTracker : INotifyPropertyChanged
    {
        private List<double> overallInCats { get; set; }
        public List<double> OverallInCats
        {
            get { return overallInCats; }
            set
            {
                overallInCats = value;
                RaisedOnPropertyChanged("OverallInCats");
            }
        }

        private double overallAll { get; set; }
        public double OverallAll
        {
            get { return overallAll; }
            set
            {
                overallAll = value;
                RaisedOnPropertyChanged("OverallAll");
            }
        }

        private Color overallColor { get; set; }
        public Color OverallColor
        {
            get { return overallColor; }
            set
            {
                overallColor = value;
                RaisedOnPropertyChanged("OverallColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PropertyName));
        }
    }
}
