using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Gbook.ClassFiles;
using Xamarin.Forms;

namespace Gbook.ViewModel
{
    public class ColorInfo : INotifyPropertyChanged
    {
        private ObservableCollection<Color> colors;
        private object selectedItem;

        public ObservableCollection<Color> Colors
        {
            get
            {
                return colors;
            }
            set
            {
                colors = value;
                OnPropertyChanged("Colors");
            }
        }
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");

            }
        }
        public Color SelectedColor { get; set; }
        public ColorInfo()

        {
            Colors = new ObservableCollection<Color>();

            Colors.Add(Color.FromHex("4568dc"));
            Colors.Add(Color.FromHex("b06ab3"));
            Colors.Add(Color.FromHex("000428"));
            Colors.Add(Color.FromHex("004e92"));

            Colors.Add(Color.FromHex("02aab0"));
            Colors.Add(Color.FromHex("00cdac"));

            Colors.Add(Color.FromHex("ff9966"));
            Colors.Add(Color.FromHex("ff5e62"));
            Colors.Add(Color.FromHex("ff5f6d"));

            Colors.Add(Color.FromHex("00416A"));
            Colors.Add(Color.FromHex("E4E5E6"));
            Colors.Add(Color.FromHex("ffafbd"));
            Colors.Add(Color.FromHex("ffc3a0"));
            Colors.Add(Color.FromHex("2193b0"));
            Colors.Add(Color.FromHex("6dd5ed"));
            Colors.Add(Color.FromHex("bdc3c7"));
            Colors.Add(Color.FromHex("2c3e50"));
            Colors.Add(Color.FromHex("614385"));
            Colors.Add(Color.FromHex("516395"));
            Colors.Add(Color.FromHex("2b5876"));
            Colors.Add(Color.FromHex("4e4376"));




        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
