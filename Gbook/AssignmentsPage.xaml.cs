using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Gbook.ClassFiles;
using Gbook.Converters;
using Gbook.Methods;
using Gbook.ViewModel;
using Syncfusion.ListView.XForms;
using Syncfusion.SfChart.XForms;
using Syncfusion.SfNumericTextBox.XForms;
using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;

namespace Gbook
{
    public partial class AssignmentsPage : ContentPage
    {

        private static ObservableCollection<Assignments> Asses = new ObservableCollection<Assignments>();
        private static List<string> Cats = new List<string>();

        private static ObservableCollection<CategoriesBox> catBoxes = new ObservableCollection<CategoriesBox>();

        private static OverallTracker Ot = new OverallTracker();
        private static List<double> CatsOverall = new List<double>();
        public static  SfComboBox cat = new SfComboBox();
        private static double oPa = 0;
        public ObservableCollection<CategoriesBox> nCatBox = new ObservableCollection<CategoriesBox>();
        public AssignmentsPage()
        {
            Asses = Globals.SelectedData.AssignmentsList;

            InitializeComponent();

            oPa = 0;
            CatsOverall.Clear();
            catBoxes.Clear();

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
            int count = 0;
            foreach(CategoryInfo c in Globals.SelectedData.UCatInfoSet)
            {
                CategoriesBox b = new CategoriesBox();
                b.Id = count;
                b.Weight = c.Weight;
                b.Description = c.Description;
                //b.CatPoints = c.PointsEarned;
                //b.CatPossible = c.PointsPossible;
                //b.Percent = c.Percent;

                catBoxes.Add(b);

                Cats.Add(c.Description);
                count += 1;
            }
        }

        private void SetOverallDescription()
        {
            /*
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
            }
            */

            /*
             * x/a = y/100
             * 100x = ay
             * y = 100x/a
             */
            //Color c = ColorGet.ColorFromPercent((int)Math.Round(oPa, 0));
            UpdateOverallScore();
            borderFrame.BindingContext = Ot;
            StackLayout score = new StackLayout() { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center };
            Label scoreLabel = new Label() { TextColor = Color.White, FontAttributes = FontAttributes.Bold,FontSize = 12f , VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center };
            Binding b = new Binding("OverallAll");
            b.StringFormat = "{0}%";
            scoreLabel.SetBinding(Label.TextProperty, b);
            score.Children.Add(scoreLabel);
            borderFrame.Content = score;
            borderFrame.SetBinding(Frame.BackgroundColorProperty, new Binding("OverallColor"));
            borderFrame.Parent = overallScore;


            StackLayout Holder = new StackLayout();
            SfChart chart = new SfChart() { HeightRequest = 150, BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = Application.Current.MainPage.Width };
            //Initializing Primary Axis
            
            foreach(CategoriesBox cv in catBoxes){
                nCatBox.Add(cv);
            }

            CategoryAxis primaryAxis = new CategoryAxis();
            
            //Initializing Secondary Axis
            NumericalAxis secondaryAxis = new NumericalAxis() { Minimum = 0, Maximum = 100, Interval = 20, RangePadding =NumericalPadding.None};
            chart.SecondaryAxis = secondaryAxis;
            chart.PrimaryAxis = primaryAxis;
            chart.BindingContext = Ot;
            StackingBarSeries series2 = new StackingBar100Series()
            {
                XBindingPath = "Description",
                YBindingPath = "CatPossible",
                Color = Color.Transparent
            };
            series2.SetBinding(StackingBarSeries.ItemsSourceProperty, new Binding("CatsOverall"));
            StackingBarSeries series1 = new StackingBar100Series()
            {
                XBindingPath = "Description",
                YBindingPath = "CatPoints",
                Color = Color.Transparent
            };
            series1.SetBinding(StackingBarSeries.ItemsSourceProperty, new Binding("CatsOverall"));
            series2.Color = Xamarin.Forms.Color.Transparent;
            series2.StrokeColor = Color.White;
            series2.StrokeWidth = 0.3;
            series2.EnableAnimation = true;
            series2.AnimationDuration = 0.8;
            series2.Color = Color.Transparent;
            series2.DataMarker = new ChartDataMarker();

            series1.ListenPropertyChange = true;
            series2.ListenPropertyChange = true;

            series1.Color = Color.Wheat;
            series1.StrokeColor = Color.Blue;
            series1.StrokeWidth = 0.3;
            series1.EnableAnimation = true;
            series1.AnimationDuration = 0.8;
            //series2.DataMarker = new ChartDataMarker();

            chart.Series.Add(series1);
            chart.Series.Add(series2);

            Holder.Children.Add(chart);
            CategoryHolder.Children.Add(Holder);
            

            /*
            oPa = (oPa * 100) / netWeight;
            oPa = Math.Round(oPa, 2);
            Ot.OverallInCats = CatsOverall;
            Ot.OverallColor = ColorGet.ColorFromPercent((int)Math.Round(oPa, 0));
            Ot.OverallAll = oPa;
            */
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
            subMain.SetBinding(Element.AutomationIdProperty, new Binding("Id"));
            cat = new SfComboBox() {HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = 220, HeightRequest = 50, TextColor = Color.White, VerticalOptions=LayoutOptions.CenterAndExpand };
            cat.ComboBoxSource = Cats;
            cat.IsEditableMode = false;
            cat.SetBinding(SfComboBox.SelectedValueProperty, new Binding("Description"));
            cat.SelectionChanged += ComboBox_SelectionChanged;

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
        private void ComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            
            if (Globals.SelectedData != null && Globals.SelectedData.UCatInfoSet.Count > 0)
            {
                int r = cat.SelectedIndex;
                SfComboBox temp = (SfComboBox)sender;
                var x = temp.Parent;
                int id = 0;
                if (int.TryParse(x.AutomationId, out id))
                {
                    Asses[id].AssignmentType = catBoxes[temp.SelectedIndex].Description;
                    Asses[id].Weight = catBoxes[temp.SelectedIndex].Weight;
                    //Console.WriteLine("Selected: " +temp.SelectedIndex);
                    //Console.WriteLine("Weight: " +catBoxes[temp.SelectedIndex].Weight);
                }
                UpdateOverallScore();
            }
            
        }

        private static void InitCatList()
        {
            if (catBoxes.Count > 0)
            {
                catBoxes.Clear();
                Ot.OverallAll = 0;
            }
            
            int count = 0;
            foreach (CategoryInfo c in Globals.SelectedData.UCatInfoSet)
            {
                CategoriesBox b = new CategoriesBox();
                b.Id = count;
                b.Weight = c.Weight;
                b.Description = c.Description;
                //b.CatPoints = c.PointsEarned;
                //b.CatPossible = c.PointsPossible;
                //b.Percent = c.Percent;

                catBoxes.Add(b);

                count += 1;
            }
            Console.WriteLine("filled the list " +catBoxes.Count());
        }

        private static void UpdateOverallScore()
        {
            //Console.WriteLine("Updating index score");
            oPa = 0;
            double netWeight = 0;
            CatsOverall.Clear();
            List<CategoryInfo> unboundCats = Globals.SelectedData.UCatInfoSet;
            //Console.WriteLine("Clearing the list");
            
            Console.WriteLine("Filling the list");
            InitCatList();
            /*
            foreach (CategoryInfo cats in unboundCats.Where(x => x.Weight > 0 && x.Description != "Ungraded"))
            {
                //Console.WriteLine(cats.Description);
                var a = Asses.Where(x => x.Weight == cats.Weight);
                double totPoints = 0;
                foreach (Assignments ass in a)
                {
                    totPoints += ass.Points;
                }
                if (cats.PointsPossible > 0 && cats.Weight > 0)
                {
                    oPa += (totPoints / cats.PointsPossible) * cats.Weight;
                    netWeight += cats.Weight;
                    CatsOverall.Add(((totPoints / cats.PointsPossible) * 100));
                    //Console.WriteLine("Overall points: " +oPa);
                    //Console.WriteLine("Net Weight: " + netWeight);
                    
                }
                //Console.WriteLine("Finished category");
                //Console.WriteLine("----------");
            }

            foreach(double d in CatsOverall)
            {
                //Console.WriteLine("Category score: " + d);
            }
            oPa = (oPa * 100) / netWeight;
            oPa = Math.Round(oPa, 2);

            Ot.OverallAll = oPa;
            Ot.OverallColor = ColorGet.ColorFromPercent((int)Math.Round(oPa, 0));
            Ot.OverallInCats = CatsOverall;
            for(int i = 0; i < CatsOverall.Count; i++)
            {
                catBoxes[i].Percent = CatsOverall[i];
            }
            */
            Console.WriteLine("Writing assignments");
            foreach (Assignments a in Asses)
            {
                if(a.Grade != "" && a.Grade != "NG")
                {
                    foreach(CategoriesBox cb1 in catBoxes)
                    {
                        if(cb1.Description == a.AssignmentType)
                        {
                            cb1.CatPossible += a.Possible;
                            cb1.CatPoints += a.Points;
                            cb1.Percent = (cb1.CatPoints / cb1.CatPossible);
                            Console.WriteLine("Category: " + cb1.Description);
                            Console.WriteLine("\t" + cb1.CatPossible);
                            Console.WriteLine("---------------");
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("LOOOOK " + a.Description);
                }
            }
            Console.WriteLine("Done doing everything \n \n ----------------");
            ObservableCollection<CategoriesBox> tempCats = new ObservableCollection<CategoriesBox>();
            foreach(CategoriesBox cb1 in catBoxes)
            {
                Ot.OverallAll += cb1.Percent * cb1.Weight;
                if(cb1.Weight > 0 && cb1.CatPossible > 0)
                {
                    tempCats.Add(cb1);
                    netWeight += cb1.Weight;
                    Console.WriteLine("Weight: " + netWeight);
                }
            }
            Ot.OverallAll = Math.Round((100 * Ot.OverallAll) / netWeight, 2);
            Ot.OverallColor = ColorGet.ColorFromPercent((int)Math.Round(Ot.OverallAll, 0));
            Ot.CatsOverall = tempCats;
            //Console.WriteLine("");
            //Console.WriteLine("");
        }

        private void Handle_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateOverallScore();
        }
    }
}
