using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gbook.ClassFiles;
using Gbook.Converters;
using Gbook.ViewModel;
using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;

namespace Gbook
{
    public partial class AssignmentsPage : ContentPage
    {

        private static ObservableCollection<Assignments> Asses = new ObservableCollection<Assignments>();
        private static List<string> Cats = new List<string>();
        public AssignmentsPage()
        {
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
            Asses.Clear();
            Cats.Clear();
            foreach(Assignments a in Globals.SelectedData.AssignmentsList)
            {
                Asses.Add(a);
            }
            foreach(CategoryInfo c in Globals.SelectedData.CatInfoSet)
            {
                if(c.Description != "Overall")
                {
                    Cats.Add(c.Description);
                }
            }
            this.Title = Globals.SelectedData.CourseName;
            AssList.ItemsSource = Asses;
        }

        private void ListTemplate()
        {
            AssList.ItemSpacing = 0;
            AssList.AutoFitMode = Syncfusion.ListView.XForms.AutoFitMode.DynamicHeight;

            AssList.ItemTemplate = new DataTemplate(() =>
            {
            Grid MainGrid = new Grid() { VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromRgba(0, 0, 0, 0.5), Margin = new Thickness(0, 5), Padding = 0 };

            StackLayout main = new StackLayout() {Spacing = 5, Margin = 0, Padding = 0, Orientation = StackOrientation.Vertical, VerticalOptions= LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand };

            Label l = new Label() { TextColor = Color.White, FontSize = 17, FontAttributes = FontAttributes.Bold };
            l.SetBinding(Label.TextProperty, new Binding("Description"));

            StackLayout subMain = new StackLayout() { Spacing = 0, Margin = 0, Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand ,Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center };
            SfComboBox cat = new SfComboBox() {HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = 200, HeightRequest = 40, TextColor = Color.White };
            cat.ComboBoxSource = Cats;
            cat.IsEditableMode = false;
            
            Binding catBinding = new Binding("CatIndex");
            cat.SetBinding(SfComboBox.SelectedIndexProperty, catBinding);
            cat.SetBinding(SfComboBox.TextProperty, new Binding("AssignmentType"));


            Frame border = new Frame() {Margin = new Thickness(3,0), Padding = 0, WidthRequest = 40, HeightRequest = 30, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.End };
            StackLayout score = new StackLayout() { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center };
            Label scoreLabel = new Label() { FontSize = 13 ,TextColor = Color.White, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center };
            scoreLabel.SetBinding(Label.TextProperty, new Binding("Percent", stringFormat: "{0}%"));
            score.Children.Add(scoreLabel);
            Binding binding3 = new Binding("Percent");
            binding3.Converter = new ColorGradientConverter();
            border.SetBinding(BackgroundColorProperty, binding3);
            border.Content = score;

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
    }
}
