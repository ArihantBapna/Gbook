using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

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

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_PropertyName));
        }
    }
}
