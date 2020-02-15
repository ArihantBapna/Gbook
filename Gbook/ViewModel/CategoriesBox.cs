using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Gbook.Methods;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;

namespace Gbook.ViewModel
{
    public class CategoriesBox : INotifyPropertyChanged
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

        private double delWeight { get; set; }
        public double DelWeight
        {
            get { return delWeight; }
            set
            {
                delWeight = value;
                RaisedOnPropertyChanged("DelWeight");
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

        private double catPoints { get; set; }
        public double CatPoints
        {
            get { return catPoints; }
            set
            {
                catPoints = value;
                RaisedOnPropertyChanged("CatPoints");
            }
        }

        private double catPossible { get; set; }
        public double CatPossible
        {
            get { return catPossible; }
            set
            {
                catPossible = value;
                RaisedOnPropertyChanged("CatPossible");
            }
        }

        private double weightPercent { get; set; }
        public double WeightPercent
        {
            get { return weightPercent; }
            set
            {
                weightPercent = value;
                RaisedOnPropertyChanged("WeightPercent");
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

        private Color catColor { get; set; }
        public Color CatColor
        {
            get { return catColor; }
            set
            {
                catColor = value;
                RaisedOnPropertyChanged("CatColor");
            }
        }

        private ChartColorModel colorModel { get; set; }
        public ChartColorModel ColorModel
        {
            get { return colorModel;  }
            set
            {
                colorModel = value;
                RaisedOnPropertyChanged("ColorModel");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PropertyName));
        }
    }
}
