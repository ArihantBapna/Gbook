﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Gbook.ClassFiles;
using Gbook.Converters;
using Gbook.Methods;
using Gbook.ViewModel;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using Syncfusion.SfChart.XForms;
using Syncfusion.SfNumericTextBox.XForms;
using Syncfusion.XForms.ComboBox;
using Syncfusion.XForms.TextInputLayout;
using Xamarin.Forms;

namespace Gbook
{
    public partial class AssignmentsPage : ContentPage
    {

        private static ObservableCollection<Assignments> OgAsses;
        private static ObservableCollection<Assignments> Asses;
        private static List<string> Cats = new List<string>();

        private static ObservableCollection<CategoriesBox> catBoxes = new ObservableCollection<CategoriesBox>();
        private static ChartColorModel ColorModel = new ChartColorModel();
        private static OverallTracker Ot = new OverallTracker();
        private static List<double> CatsOverall = new List<double>();
        private static List<string> gradesPoss = new List<string>() { "Graded","NG","X","Z" };
        public static  SfComboBox cat = new SfComboBox();
        public static SfComboBox gradeBox = new SfComboBox();
        private static double oPa = 0;
        public AssignmentsPage()
        {
            Asses = new ObservableCollection<Assignments>();
            Asses.Clear();
            OgAsses = new ObservableCollection<Assignments>(Globals.SelectedData.AssignmentsList);
            foreach(Assignments a in OgAsses)
            {
                Assignments b = new Assignments();
                b.Points = a.Points;
                b.Possible = a.Possible;
                b.Description = a.Description;
                b.AssignmentType = a.AssignmentType;
                b.Percent = a.Percent;
                b.CatIndex = a.CatIndex;
                b.BackColor = a.BackColor;
                b.Date = a.Date;
                b.Grade = a.Grade;
                Asses.Add(b);
            }

            InitializeComponent();



            oPa = 0;
            CatsOverall.Clear();
            catBoxes.Clear();

            double heightSide = Application.Current.MainPage.Height;
            AssList.HeightRequest = heightSide;

            SetGradient();
            SetSwiping();
            InitList();
            ListTemplate();
            SetOverallDescription();

            
            
        }

        private void SetSwiping()
        {
            AssList.AllowSwiping = true;
            AssList.SelectionMode = Syncfusion.ListView.XForms.SelectionMode.None;
            AssList.SwipeOffset = 100;
            AssList.SwipeThreshold = 70;
            AssList.SwipeEnded += ListView_SwipeEnded;
            AssList.RightSwipeTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();

                var grid1 = new Grid()
                {
                    BackgroundColor = Color.FromHex("#DC595F"),
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                var deleteGrid = new Grid() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.FillAndExpand };
                var l1 = new Label() { Text = "Delete", FontSize=18f, FontAttributes = FontAttributes.Bold, TextColor = Color.White}    ;
                deleteGrid.Children.Add(l1);
                grid1.Children.Add(deleteGrid);

                grid.Children.Add(grid1);

                return grid1;
            });

        }

        private void ListView_SwipeEnded(object sender, Syncfusion.ListView.XForms.SwipeEndedEventArgs e)
        {
            if (e.SwipeOffset >= 100)
            {
                Asses.RemoveAt(e.ItemIndex);
                AssList.ResetSwipe();
                UpdateOverallScore();
            }
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

                catBoxes.Add(b);

                Cats.Add(c.Description);
                count += 1;
            }
        }

        private void SetOverallDescription()
        {
 
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
            SfChart chart = new SfChart() { SideBySideSeriesPlacement = false, HeightRequest = 150, BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = Application.Current.MainPage.Width };
            //Initializing Primary Axis
           
            chart.SideBySideSeriesPlacement = false;
            CategoryAxis primaryAxis = new CategoryAxis() { LabelStyle = new ChartAxisLabelStyle() { TextColor = Color.White } };

            //Initializing Secondary Axis
            NumericalAxis secondaryAxis = new NumericalAxis() { Minimum = 0, Maximum = 100, Interval = 20, RangePadding = NumericalPadding.None, LabelStyle = new ChartAxisLabelStyle() { TextColor = Color.White } };
            chart.SecondaryAxis = secondaryAxis;
            chart.PrimaryAxis = primaryAxis;
            chart.BindingContext = Ot;
            BarSeries series2 = new BarSeries()
            {
                XBindingPath = "Description",
                YBindingPath = "WeightPercent"
            };
            series2.SetBinding(ChartSeries.ItemsSourceProperty, new Binding("CatsOverall"));
            //series2.BindingContext = Ot.CatsOverall;
            series2.ColorModel = ColorModel;
            //series2.SetBinding(ChartSeries.ColorProperty, new Binding("CatColor"));
            //series2.SetBinding(BarSeries.StrokeColorProperty, new Binding("CatColor"));
            series2.StrokeWidth = 0.3;
            series2.EnableAnimation = true;
            series2.AnimationDuration = 0.8;
            ChartDataMarker cDm = new ChartDataMarker();
            cDm.LabelTemplate = new DataTemplate(() =>
            {
                StackLayout layout = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0 };
                Label l1 = new Label() { TextColor = Color.White, FontSize = 10f };
                l1.SetBinding(Label.TextProperty, new Binding("WeightPercent"));
                Label l2 = new Label() { TextColor = Color.White, FontSize = 10f };
                l2.SetBinding(Label.TextProperty, new Binding("Weight", stringFormat: "/{0}"));
                layout.Children.Add(l1);
                layout.Children.Add(l2);
                return layout;
            });
            cDm.LabelStyle.TextColor = Color.White;
            series2.DataMarker = cDm;

            BarSeries series1 = new BarSeries()
            {
                XBindingPath = "Description",
                YBindingPath = "Weight",
            };
            series1.SetBinding(ChartSeries.ItemsSourceProperty, new Binding("CatsOverall"));
            series1.StrokeColor = Color.White;
            series1.StrokeWidth = 0.3;
            series1.EnableAnimation = true;
            series1.AnimationDuration = 0.8;
            series1.Color = Color.Transparent;

            series1.ListenPropertyChange = true;
            series2.ListenPropertyChange = true;

            chart.Series.Add(series2);
            chart.Series.Add(series1);

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

            StackLayout main = new StackLayout() {Spacing = 5, Margin = 0, Padding = 0, Orientation = StackOrientation.Vertical, VerticalOptions= LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

            Label l = new Label() { TextColor = Color.White, FontSize = 17, FontAttributes = FontAttributes.Bold };
            l.SetBinding(Label.TextProperty, new Binding("Description"));

            StackLayout subMain = new StackLayout() { Spacing = 0, Margin = 0, Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand ,Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
            subMain.SetBinding(Element.AutomationIdProperty, new Binding("Id"));
            cat = new SfComboBox() {HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = 220, HeightRequest = 50, TextColor = Color.White, VerticalOptions=LayoutOptions.CenterAndExpand };
            cat.ComboBoxSource = Cats;
            cat.SelectionChanged += ComboBox_SelectionChanged;

            DropDownButtonSettings dropDownButtonSettings = new DropDownButtonSettings();
            dropDownButtonSettings.Height = 20;
            dropDownButtonSettings.Width = 20;
            dropDownButtonSettings.HighlightedBackgroundColor = Color.White;
            dropDownButtonSettings.BackgroundColor = Color.Transparent;
            dropDownButtonSettings.HighlightFontColor = Color.Black;
            cat.DropDownButtonSettings = dropDownButtonSettings;
            
            cat.SetBinding(SfComboBox.TextProperty, new Binding("AssignmentType"));

            StackLayout border = new StackLayout() {WidthRequest = 150, HeightRequest = 30, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0,5) };
            StackLayout score = new StackLayout() {Orientation = StackOrientation.Horizontal, WidthRequest=150, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };

            StackLayout holder1 = new StackLayout() { VerticalOptions = LayoutOptions.Start,HorizontalOptions=LayoutOptions.Center,WidthRequest = 105,HeightRequest=40 };
            var inputLayout1 = new SfTextInputLayout() { ContainerType = ContainerType.Outlined, VerticalOptions = LayoutOptions.StartAndExpand,HorizontalOptions = LayoutOptions.Center, HeightRequest = 40, InputViewPadding = 0, ReserveSpaceForAssistiveLabels = false };
            SfNumericTextBox points =new SfNumericTextBox() { TextAlignment = TextAlignment.Center , VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = Color.White, HeightRequest = 40, Margin = 0, SelectAllOnFocus = true };
            points.SetBinding(SfNumericTextBox.ValueProperty, new Binding("Points", BindingMode.TwoWay));
            inputLayout1.SetBinding(SfTextInputLayout.ContainerBackgroundColorProperty, new Binding("BackColor"));
            points.MaximumNumberDecimalDigits = 1;
            points.Completed += Handle_ValueChanged;
            points.Minimum = 0;
            inputLayout1.InputView = points;
            holder1.Children.Add(inputLayout1);

            StackLayout holder2 = new StackLayout() { VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.Center, WidthRequest = 105, HeightRequest = 40 };
            var inputLayout2 = new SfTextInputLayout() { ContainerType = ContainerType.Outlined, VerticalOptions = LayoutOptions.StartAndExpand,HorizontalOptions = LayoutOptions.Center, HeightRequest = 40, InputViewPadding = 0, ReserveSpaceForAssistiveLabels = false };
            SfNumericTextBox possible = new SfNumericTextBox() { TextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = Color.White, HeightRequest = 40, Margin = 0, SelectAllOnFocus = true };
            possible.SetBinding(SfNumericTextBox.ValueProperty, new Binding("Possible", BindingMode.TwoWay));
            inputLayout2.SetBinding(SfTextInputLayout.ContainerBackgroundColorProperty, new Binding("BackColor"));
            possible.MaximumNumberDecimalDigits = 1;
            possible.Completed += Handle_ValueChanged;
            inputLayout2.InputView = possible;
            holder2.Children.Add(inputLayout2);

            score.Children.Add(holder1);
            score.Children.Add(holder2);
             
            border.Children.Add(score);
            border.BackgroundColor = Color.Transparent;

            subMain.Children.Add(cat);
            subMain.Children.Add(border);

            StackLayout last = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(5,10) };
            Binding dateNTime = new Binding("Date");
            dateNTime.Converter = new AssignmentDateConverter();

            Label date = new Label() { TextColor = Color.White, VerticalOptions = LayoutOptions.End ,FontSize = 15f, FontAttributes = FontAttributes.Bold, Margin = new Thickness(0,0), HorizontalOptions = LayoutOptions.StartAndExpand };
            date.SetBinding(Label.TextProperty, dateNTime);

            gradeBox = new SfComboBox() { HorizontalOptions = LayoutOptions.End, WidthRequest = 150, TextColor = Color.White };
            gradeBox.SetBinding(SfComboBox.TextProperty, new Binding("Grade"));
            gradeBox.ComboBoxSource = gradesPoss;
            gradeBox.SetBinding(SfComboBox.AutomationIdProperty, new Binding("Description"));
            gradeBox.SelectionChanged += Gradebox_SelectionChanged;

            last.Children.Add(date);
            last.Children.Add(gradeBox);

            main.Children.Add(l);
            main.Children.Add(subMain);
            main.Children.Add(last);

            MainGrid.Children.Add(main);
            return MainGrid;
            });
        }

        private void Gradebox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

            SfComboBox temp = (SfComboBox)sender;
            foreach(Assignments ass in Asses)
            {
                if(ass.Description == temp.AutomationId)
                {
                    ass.Grade = (string) e.Value;
                }
            }
            UpdateOverallScore();

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
        }

        private static void UpdateOverallScore()
        {
            oPa = 0;
            double netWeight = 0;
            CatsOverall.Clear();
            List<CategoryInfo> unboundCats = Globals.SelectedData.UCatInfoSet;
            InitCatList();

            foreach (Assignments a in Asses)
            {
                if(a.Grade != "" && a.Grade != "NG" && a.Grade != "X")
                {
                    foreach(CategoriesBox cb1 in catBoxes)
                    {
                        if(cb1.Description == a.AssignmentType)
                        {
                            if(a.Grade == "Z")
                            {
                                a.Points = 0;
                            }
                            cb1.CatPossible += a.Possible;
                            cb1.CatPoints += a.Points;
                            cb1.Percent = (cb1.CatPoints / cb1.CatPossible);
                            break;
                        }
                    }
                }
            }
            ObservableCollection<CategoriesBox> tempCats = new ObservableCollection<CategoriesBox>();
            foreach(CategoriesBox cb1 in catBoxes)
            {
                Ot.OverallAll += cb1.Percent * cb1.Weight;
                if(cb1.Weight > 0 && cb1.CatPossible > 0)
                {
                    CategoriesBox cbTemp = new CategoriesBox();
                    cbTemp.CatPoints = cb1.CatPoints;
                    cbTemp.Percent = Math.Round(cb1.Percent, 2);
                    cbTemp.CatPossible = cb1.CatPossible;
                    cbTemp.Weight = cb1.Weight;
                    cbTemp.Description = cb1.Description;
                    cbTemp.WeightPercent = Math.Round((cbTemp.Percent * cbTemp.Weight), 2);
                    cbTemp.DelWeight = cbTemp.Weight - cbTemp.WeightPercent;
                    cbTemp.CatColor = ColorGet.ColorFromPercent((int)Math.Round(cbTemp.Percent * 100, 0));
                    tempCats.Add(cbTemp);
                    netWeight += cb1.Weight;
                }
            }
            Ot.OverallAll = Math.Round((100 * Ot.OverallAll) / netWeight, 2);

            if(catBoxes.Count >= 2)
            {
                if(tempCats.Count >= 2)
                {
                    CategoriesBox overall = new CategoriesBox();
                    overall.Percent = Ot.OverallAll;
                    overall.Weight = 100;
                    overall.Description = "Overall";
                    overall.WeightPercent = overall.Percent;
                    overall.CatColor = ColorGet.ColorFromPercent((int)Math.Round(Ot.OverallAll, 0));

                    tempCats.Add(overall);
                }

            }
            ColorModel.Palette = ChartColorPalette.Custom;
            ChartColorCollection cmc = new ChartColorCollection();
            foreach (CategoriesBox cv in tempCats)
            {
                cmc.Add(cv.CatColor);
            }
            ColorModel.CustomBrushes = cmc;
            Ot.OverallColor = ColorGet.ColorFromPercent((int)Math.Round(Ot.OverallAll, 0));
            Ot.CatsOverall = tempCats;
        }

        private void Handle_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateOverallScore();
        }

        void addButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Assignments newAss = new Assignments();
            newAss.Description = "Added just now";
            newAss.Date = DateTime.Now.ToString();
            newAss.Points = 0;
            newAss.Possible = 0;
            newAss.AssignmentType = "Ungraded";
            Asses.Insert(0,newAss);
            AssList.ItemsSource = Asses;
            UpdateOverallScore();
        }

        private void ResetData()
        {
            Asses.Clear();

            foreach (Assignments a in OgAsses)
            {
                Assignments b = new Assignments();
                b.Points = a.Points;
                b.Possible = a.Possible;
                b.Description = a.Description;
                b.AssignmentType = a.AssignmentType;
                b.Percent = a.Percent;
                b.CatIndex = a.CatIndex;
                b.BackColor = a.BackColor;
                b.Date = a.Date;
                b.Grade = a.Grade;
                Asses.Add(b);
            }

            AssList.ItemsSource = null;
            AssList.ItemsSource = Asses;
            UpdateOverallScore();
        }

        void resetButton_Clicked(System.Object sender, System.EventArgs e)
        {

            ResetData();
    
        }

        private string GetOverallFromPoint(int i)
        {
            ObservableCollection<CategoriesBox> tempCats = new ObservableCollection<CategoriesBox>();
            double netWeight = 0;
            double tempOverall = 0;
            foreach (CategoriesBox cb1 in catBoxes)
            {
                tempOverall += cb1.Percent * cb1.Weight;
                if (cb1.Weight > 0 && cb1.CatPossible > 0)
                {
                    CategoriesBox cbTemp = new CategoriesBox();
                    cbTemp.CatPoints = cb1.CatPoints;
                    cbTemp.Percent = Math.Round(cb1.Percent, 2);
                    cbTemp.CatPossible = cb1.CatPossible;
                    cbTemp.Weight = cb1.Weight;
                    cbTemp.Description = cb1.Description;
                    cbTemp.WeightPercent = Math.Round((cbTemp.Percent * cbTemp.Weight), 2);
                    cbTemp.DelWeight = cbTemp.Weight - cbTemp.WeightPercent;
                    cbTemp.CatColor = ColorGet.ColorFromPercent((int)Math.Round(cbTemp.Percent * 100, 0));
                    tempCats.Add(cbTemp);
                    netWeight += cb1.Weight;
                }
            }
            tempOverall = Math.Round((100 * tempOverall) / netWeight, 2);
            return GradeFromScore.GetGrade(tempOverall);
            }

        void bumpButton_Clicked(System.Object sender, System.EventArgs e)
        {
            int count = 0;
            List<int> pos = new List<int>();
            int iterate = 0;
            foreach(Assignments a in Asses)
            {
                if(a.Grade == "NG" && a.Possible > 0)
                {
                    count++;
                    pos.Add(iterate);
                }
                iterate++;
            }
            if(count == 1)
            {
                int subCount = 0;
                foreach(CategoriesBox cb in catBoxes)
                {
                    if(cb.Description.Equals(Asses[pos[0]].AssignmentType))
                    {
                        Console.WriteLine(Asses[pos[0]].Description);
                        Asses[pos[0]].Grade = "Graded";
                        string currGrade = GradeFromScore.GetGrade(Ot.OverallAll);
                        cb.CatPossible += Asses[pos[0]].Possible;

                        Asses[pos[0]].Points = 0;
                        cb.CatPoints += Asses[pos[0]].Points;
                        cb.Percent = cb.CatPoints / cb.CatPossible;
                        string gradeNew = GetOverallFromPoint(subCount);
                        while (!currGrade.Equals(gradeNew))
                        {
                            Asses[pos[0]].Points += 1;
                            cb.CatPoints += 1;
                            cb.Percent = cb.CatPoints / cb.CatPossible;
                            gradeNew = GetOverallFromPoint(subCount);
                        }
                        UpdateOverallScore();
                        break;
                    }
                    subCount++;
                }
            }            
        }
    }
}
