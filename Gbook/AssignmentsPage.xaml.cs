using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Gbook.ClassFiles;
using Gbook.Converters;
using Gbook.ViewModel;
using Syncfusion.SfNumericTextBox.XForms;
using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;

namespace Gbook
{
    public partial class AssignmentsPage : ContentPage
    {

        private static ObservableCollection<Assignments> Asses;
        private static List<string> Cats = new List<string>();
        public AssignmentsPage()
        {
            Asses = Globals.SelectedData.AssignmentsList;

            InitializeComponent();
            SetGradient();
            InitList();
            ListTemplate();
        }

        private void SetGradient()
        {
            grad1.Color = LoginPage.g1;
            grad1.Offset = LoginPage.o1;

            grad2.Color = LoginPage.g2;
            grad2.Offset = LoginPage.o2;
            AssList.SelectionMode = Syncfusion.ListView.XForms.SelectionMode.None;
        }

        private void InitList()
        {
            
            /*
            ObservableCollection<Assignments> asses = new ObservableCollection<Assignments>();

            //Asses.Clear();
            Cats.Clear();

            foreach (Assignments a in Globals.SelectedData.AssignmentsList)
            {
                asses.Add(a);
            }
            foreach(CategoryInfo c in Globals.SelectedData.CatInfoSet)
            {
                if(c.Description != "Overall")
                {
                    Cats.Add(c.Description);
                }
            }
            Asses = asses;
            */

            this.Title = Globals.SelectedData.CourseName;
            AssList.ItemsSource = Asses;
        }

        private void ListTemplate()
        {
            AssList.ItemSpacing = 0;
            AssList.AutoFitMode = Syncfusion.ListView.XForms.AutoFitMode.DynamicHeight;
            AssList.BindingContext = new Assignments();
            AssList.ItemTemplate = new DataTemplate(() =>
            {
            Grid MainGrid = new Grid() { VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromRgba(0, 0, 0, 0.5), Margin = new Thickness(0, 5), Padding = 0 };

            StackLayout main = new StackLayout() {Spacing = 5, Margin = 0, Padding = 0, Orientation = StackOrientation.Vertical, VerticalOptions= LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand };

            Label l = new Label() { TextColor = Color.White, FontSize = 17, FontAttributes = FontAttributes.Bold };
            l.SetBinding(Label.TextProperty, new Binding("Description"));

            StackLayout subMain = new StackLayout() { Spacing = 0, Margin = 0, Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand ,Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center };
            SfComboBox cat = new SfComboBox() {HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = 220, HeightRequest = 30, TextColor = Color.White };
            cat.ComboBoxSource = Cats;
            cat.IsEditableMode = false;
            
            Binding catBinding = new Binding("CatIndex");
            cat.SetBinding(SfComboBox.SelectedIndexProperty, catBinding);
            cat.SetBinding(SfComboBox.TextProperty, new Binding("AssignmentType"));

            StackLayout border = new StackLayout() {WidthRequest = 150, HeightRequest = 30, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };
            StackLayout score = new StackLayout() {Orientation = StackOrientation.Horizontal, WidthRequest=150, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };

            SfNumericTextBox points =new SfNumericTextBox() { TextAlignment = TextAlignment.Center ,WidthRequest = 70, HorizontalOptions = LayoutOptions.Start, TextColor = Color.White};
            points.SetBinding(SfNumericTextBox.ValueProperty, new Binding("Points", BindingMode.TwoWay));
            points.SetBinding(BackgroundColorProperty, new Binding("BackColor", BindingMode.TwoWay));
            points.MaximumNumberDecimalDigits = 1;
            points.Completed += Handle_ValueChanged;
            SfNumericTextBox possible = new SfNumericTextBox() { TextAlignment = TextAlignment.Center, WidthRequest = 70, HorizontalOptions = LayoutOptions.End, TextColor = Color.White};
            possible.SetBinding(SfNumericTextBox.ValueProperty, new Binding("Possible", BindingMode.TwoWay));
            possible.SetBinding(BackgroundColorProperty, new Binding("BackColor", BindingMode.TwoWay));
            possible.MaximumNumberDecimalDigits = 1;
            possible.Completed += Handle_ValueChanged;
            score.Children.Add(points);
            score.Children.Add(possible);
             
            border.Children.Add(score);
            border.BackgroundColor = Color.Transparent;

            subMain.Children.Add(cat);
            subMain.Children.Add(border);

            Binding dateNTime = new Binding("Date");
            dateNTime.Converter = new AssignmentDateConverter();

            Label date = new Label() { TextColor = Color.White, FontSize = 9, FontAttributes = FontAttributes.Italic, Margin = new Thickness(0,5) };
            date.SetBinding(Label.TextProperty, dateNTime);

            main.Children.Add(l);
            main.Children.Add(subMain);
            main.Children.Add(date);

            MainGrid.Children.Add(main);
            return MainGrid;
            });
        }

        private void Handle_ValueChanged(object sender, System.EventArgs e)
        {
            AssList.ItemsSource = null;
            AssList.ItemsSource = Asses;
            Console.WriteLine("Something happened");
        }
    }
}
