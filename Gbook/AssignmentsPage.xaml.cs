using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Gbook.ClassFiles;
using Gbook.Converters;
using Gbook.Methods;
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

        private static OverallTracker Ot = new OverallTracker();
        private static List<double> CatsOverall = new List<double>();
        private static double oPa = 0;

        public AssignmentsPage()
        {
            Asses = Globals.SelectedData.AssignmentsList;

            InitializeComponent();

            oPa = 0;
            CatsOverall.Clear();

            double heightSide = Application.Current.MainPage.Height;
            AssList.HeightRequest = heightSide;

            SetGradient();
            InitList();
            ListTemplate();
            SetOverallDescription();
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
            Cats.Clear();
            this.Title = Globals.SelectedData.CourseName;
            AssList.ItemsSource = Asses;
            foreach(CategoryInfo c in Globals.SelectedData.UCatInfoSet)
            {
                Cats.Add(c.Description);
            }
            Cats.Add("Ungraded");
        }

        private void SetOverallDescription()
        {
            
            double netWeight = 0;
            List<CategoryInfo> unboundCats = Globals.SelectedData.UCatInfoSet;
            
            foreach(CategoryInfo cats in unboundCats)
            {
                var a = Asses.Where(x => x.Weight == cats.Weight);
                double totPoints = 0;
                foreach(Assignments ass in a)
                {
                    totPoints += ass.Points;
                }
                if(cats.PointsPossible > 0)
                {
                    oPa = oPa + (totPoints / cats.PointsPossible) * cats.Weight;
                    netWeight += cats.Weight;
                    CatsOverall.Add(((totPoints / cats.PointsPossible) * 100));
                }
                Console.WriteLine(cats.Description +":=" +oPa);
            }
            /*
             * x/a = y/100
             * 100x = ay
             * y = 100x/a
             */
            //Color c = ColorGet.ColorFromPercent((int)Math.Round(oPa, 0));
            
            borderFrame.BindingContext = Ot;
            StackLayout score = new StackLayout() { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.End };
            Label scoreLabel = new Label() { TextColor = Color.White, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment= TextAlignment.Center };
            scoreLabel.SetBinding(Label.TextProperty, new Binding("OverallAll"));
            score.Children.Add(scoreLabel);
            borderFrame.Content = score;
            borderFrame.SetBinding(Frame.BackgroundColorProperty, new Binding("OverallColor"));
            borderFrame.Parent = overallScore;

            oPa = (oPa * 100) / netWeight;
            oPa = Math.Round(oPa, 2);
            Ot.OverallInCats = CatsOverall;
            Ot.OverallColor = ColorGet.ColorFromPercent((int)Math.Round(oPa, 0));
            Ot.OverallAll = oPa;
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

            StackLayout subMain = new StackLayout() { Spacing = 0, Margin = 0, Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand ,Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.CenterAndExpand };
            SfComboBox cat = new SfComboBox() {HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = 220, HeightRequest = 50, TextColor = Color.White, VerticalOptions=LayoutOptions.CenterAndExpand };
            cat.ComboBoxSource = Cats;
            cat.IsEditableMode = false;
            
            Binding catBinding = new Binding("CatIndex");
            cat.SetBinding(SfComboBox.SelectedIndexProperty, catBinding);
            cat.SetBinding(SfComboBox.TextProperty, new Binding("AssignmentType"));

            StackLayout border = new StackLayout() {WidthRequest = 150, HeightRequest = 30, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.EndAndExpand };
            StackLayout score = new StackLayout() {Orientation = StackOrientation.Horizontal, WidthRequest=150, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };

            SfNumericTextBox points =new SfNumericTextBox() { TextAlignment = TextAlignment.Center ,WidthRequest = 70, HorizontalOptions = LayoutOptions.Start, TextColor = Color.White};
            points.SetBinding(SfNumericTextBox.ValueProperty, new Binding("Points"));
            points.SetBinding(BackgroundColorProperty, new Binding("BackColor"));
            points.MaximumNumberDecimalDigits = 1;
            points.Completed += Handle_ValueChanged;
            points.Minimum = 0;

            SfNumericTextBox possible = new SfNumericTextBox() { TextAlignment = TextAlignment.Center, WidthRequest = 70, HorizontalOptions = LayoutOptions.End, TextColor = Color.White};
            possible.SetBinding(SfNumericTextBox.ValueProperty, new Binding("Possible"));
            possible.SetBinding(BackgroundColorProperty, new Binding("BackColor"));
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
            oPa = 0;
            CatsOverall.Clear();
            double netWeight = 0;
            List<CategoryInfo> unboundCats = Globals.SelectedData.UCatInfoSet;
            foreach (CategoryInfo cats in unboundCats)
            {
                var a = Asses.Where(x => x.Weight == cats.Weight);
                double totPoints = 0;
                foreach (Assignments ass in a)
                {
                    totPoints += ass.Points;
                }
                if (cats.PointsPossible > 0)
                {
                    oPa = oPa + (totPoints / cats.PointsPossible) * cats.Weight;
                    netWeight += cats.Weight;
                    CatsOverall.Add(((totPoints / cats.PointsPossible) * 100));

                }
                Console.WriteLine(cats.Description + ":=" + oPa);
            }
            oPa = (oPa * 100) / netWeight;
            oPa = Math.Round(oPa, 2);

            Ot.OverallAll = oPa;
            Ot.OverallColor = ColorGet.ColorFromPercent((int)Math.Round(oPa, 0));
            Ot.OverallInCats = CatsOverall;


        }
    }
}
