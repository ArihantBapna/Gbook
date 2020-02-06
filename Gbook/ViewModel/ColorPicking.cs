using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using Gbook.ClassFiles;

namespace Gbook.ViewModel
{
    public class ColorPicking : INotifyPropertyChanged
    {
        private ObservableCollection<ColorClass> Colors;
        public ObservableCollection<ColorClass> colors
        {
            get
            {
                return colors;
            }
            set
            {
                Colors = value;
                OnPropertyChanged("Colors");
            }
        }

        public ColorPicking()
        {
            colors = new ObservableCollection<ColorClass>();
            colors.Add(new ColorClass() { col = Color.White });
            colors.Add(new ColorClass() { col = Color.Black });
            colors.Add(new ColorClass() { col = Color.Red });
            colors.Add(new ColorClass() { col = Color.Green });
            colors.Add(new ColorClass() { col = Color.Blue });
            colors.Add(new ColorClass() { col = Color.Yellow });
            colors.Add(new ColorClass() { col = Color.Orange });
            colors.Add(new ColorClass() { col = Color.Violet });
            colors.Add(new ColorClass() { col = Color.Blue });
            colors.Add(new ColorClass() { col = Color.Aqua });
            colors.Add(new ColorClass() { col = Color.Cyan });
            colors.Add(new ColorClass() { col = Color.Crimson });
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
