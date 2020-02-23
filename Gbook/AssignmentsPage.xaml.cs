using System;
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
using Syncfusion.SfBusyIndicator.XForms;
using Syncfusion.SfChart.XForms;
using Syncfusion.SfNumericTextBox.XForms;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.ComboBox;
using Syncfusion.XForms.PopupLayout;
using Syncfusion.XForms.TextInputLayout;
using Xamarin.Forms;

namespace Gbook
{
    public partial class AssignmentsPage : ContentPage
    {
        private static ObservableCollection<Assignments> OgAsses;
        private static ObservableCollection<Assignments> Asses;
        private static ObservableCollection<CategoriesBox> catBoxes = new ObservableCollection<CategoriesBox>();
        private static List<string> Cats = new List<string>();
        private static List<double> CatsOverall = new List<double>();
        private static List<string> gradesPoss = new List<string>() { "Graded", "NG", "X", "Z" };
        private static SfChart chart = new SfChart();
        private static ChartColorModel ColorModel = new ChartColorModel();
        private static OverallTracker Ot = new OverallTracker();
        public static  SfComboBox cat = new SfComboBox();
        public static SfComboBox gradeBox = new SfComboBox();
        private static SfPopupLayout bumpPop = new SfPopupLayout();
        private static SfPopupLayout loadPop = new SfPopupLayout();
        private static SfComboBox gradeSel = new SfComboBox();
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
                var l1 = new Label() { Text = "Delete", FontSize=18f, FontAttributes = FontAttributes.Bold, TextColor = Color.White, VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center}    ;
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
                AssList.ResetSwipe();
                Asses.RemoveAt(e.ItemIndex);
                AssList.ItemsSource = Asses;
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
            chart = new SfChart() { SideBySideSeriesPlacement = false, HeightRequest = 150, BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = Application.Current.MainPage.Width };
            //Initializing Primary Axis
           
            chart.SideBySideSeriesPlacement = false;
            CategoryAxis primaryAxis = new CategoryAxis() { LabelStyle = new ChartAxisLabelStyle() { TextColor = Color.White ,LabelAlignment = ChartAxisLabelAlignment.Center, LabelsPosition = AxisElementPosition.Outside, WrappedLabelAlignment = ChartAxisLabelAlignment.Center }, OpposedPosition = false };

            //Initializing Secondary Axis
            NumericalAxis secondaryAxis = new NumericalAxis() { Minimum = 0, Maximum = 119, Interval = 20, RangePadding = NumericalPadding.None, LabelStyle = new ChartAxisLabelStyle() { TextColor = Color.White }, IsInversed = false };
            chart.SecondaryAxis = secondaryAxis;
            chart.PrimaryAxis = primaryAxis;
            chart.BindingContext = Ot;
            BarSeries series2 = new BarSeries()
            {
                XBindingPath = "Description",
                YBindingPath = "WeightPercent"
            };
            series2.SetBinding(ChartSeries.ItemsSourceProperty, new Binding("CatsOverall"));
            series2.ColorModel = ColorModel;
            series2.StrokeWidth = 0.3;
            series2.EnableAnimation = true;
            series2.AnimationDuration = 0.8;
            ChartDataMarker cDm = new ChartDataMarker();
            cDm.LabelTemplate = new DataTemplate(() =>
            {
                StackLayout layout = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0 };
                Label l1 = new Label() { TextColor = Color.White, FontSize = 10f };
                l1.SetBinding(Label.TextProperty, new Binding("WeightPercent", stringFormat: "{0}"));
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

            chart.PrimaryAxis.ShowMajorGridLines = false;
            chart.SecondaryAxis.ShowMajorGridLines = false;
            chart.SecondaryAxis.ShowMinorGridLines = false;

            Holder.Children.Add(chart);
            CategoryHolder.Children.Add(Holder);
        }


        private void ListTemplate()
        {
            AssList.ItemSpacing = 0;
            AssList.AutoFitMode = Syncfusion.ListView.XForms.AutoFitMode.DynamicHeight;
            AssList.BindingContext = new Assignments();
            AssList.ItemTemplate = new DataTemplate(() =>
            {
            Grid MainGrid = new Grid() { VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromRgba(0, 0, 0, 0.5), Margin = new Thickness(0), Padding = 0 };

            StackLayout main = new StackLayout() {Spacing = 5, Margin = 0, Padding = 0, Orientation = StackOrientation.Vertical, VerticalOptions= LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

            StackLayout firstRow = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0, VerticalOptions=LayoutOptions.Fill, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest=50 };
                Label l = new Label() { TextColor = Color.White, FontSize = 18, WidthRequest=200, LineBreakMode= Xamarin.Forms.LineBreakMode.TailTruncation ,FontAttributes = FontAttributes.Bold, HorizontalOptions=LayoutOptions.StartAndExpand, VerticalOptions=LayoutOptions.Center,VerticalTextAlignment=TextAlignment.Center,Margin = new Thickness(0, 0) };
                l.SetBinding(Label.TextProperty, new Binding("Description"));
                firstRow.Children.Add(l);

                StackLayout border = new StackLayout() {WidthRequest = 150, HeightRequest = 50, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0,0) };
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
            firstRow.Children.Add(border);

            StackLayout subMain = new StackLayout() { Spacing = 0, Margin = 0, Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand ,Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
                cat = new SfComboBox() {HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest=200 ,HeightRequest = 50, TextColor = Color.White, VerticalOptions=LayoutOptions.CenterAndExpand };
                cat.ComboBoxSource = Cats;
                cat.SelectionChanged += ComboBox_SelectionChanged;
                cat.SetBinding(SfComboBox.TextProperty, new Binding("AssignmentType", BindingMode.TwoWay));
            subMain.Children.Add(cat);
                gradeBox = new SfComboBox() { HorizontalOptions = LayoutOptions.End, WidthRequest = 150, TextColor = Color.White };
                gradeBox.SetBinding(SfComboBox.TextProperty, new Binding("Grade", BindingMode.TwoWay));
                gradeBox.ComboBoxSource = gradesPoss;
                gradeBox.SelectionChanged += Gradebox_SelectionChanged;
            subMain.Children.Add(gradeBox);

            StackLayout last = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0,0) };
            Binding dateNTime = new Binding("Date");
            dateNTime.Converter = new AssignmentDateConverter();
            Label date = new Label() { TextColor = Color.White, VerticalOptions = LayoutOptions.End ,FontSize = 15f, FontAttributes = FontAttributes.Bold, Margin = new Thickness(0,0), HorizontalOptions = LayoutOptions.EndAndExpand };
            date.SetBinding(Label.TextProperty, dateNTime);
            last.Children.Add(date);

            main.Children.Add(firstRow);
            main.Children.Add(subMain);
            main.Children.Add(last);
            main.Children.Add(new BoxView() { Color = Color.Black, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 2 });

            MainGrid.Children.Add(main);
            return MainGrid;
            });
        }

        private void Gradebox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            UpdateOverallScore();
        }

        private void ComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            /*
            if (Globals.SelectedData != null && Globals.SelectedData.UCatInfoSet.Count > 0)
            {
                SfComboBox temp = (SfComboBox)sender;
                var x = temp.Parent;
                Console.WriteLine("Automation Id here: " + x.AutomationId);
                if (int.TryParse(x.AutomationId, out int id))
                {
                    Asses[id].AssignmentType = catBoxes[temp.SelectedIndex].Description;
                    Asses[id].Weight = catBoxes[temp.SelectedIndex].Weight;
                }
                
            }
            */
            UpdateOverallScore();

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
                catBoxes.Add(b);
                count += 1;
            }
        }

        private static void UpdateOverallScore()
        {
            chart.SuspendSeriesNotification();
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
            chart.ResumeSeriesNotification();
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
            newAss.Id = Asses.Count();
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

        private string GetOverallFromPoint()
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
            foreach(Assignments ass in Asses)
            {
                if (ass.Grade == "NG") count++;
            }

            DataTemplate popupView = new DataTemplate(() =>
            {
                var m = new StackLayout() { Orientation = StackOrientation.Vertical, Margin = 5, Spacing = 5, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = LoginPage.g1};

                var topRow = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
                var l = new Label() { Text = "Bump your grade", TextColor = Color.White, FontSize = 20f, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center };
                topRow.Children.Add(l);

                var midRow = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand };
                var l2 = new Label() { Text = "Select the grade that you want", TextColor = Color.White, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Start };
                gradeSel = new SfComboBox() { WidthRequest = 200, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.End };
                gradeSel.ComboBoxSource = new List<string>() { "A","B","C","D","E" };
                gradeSel.Watermark = "Grade";
                gradeSel.WatermarkColor = Color.White;
                gradeSel.TextColor = Color.White;
                midRow.Children.Add(l2);
                midRow.Children.Add(gradeSel);

                var botRow = new StackLayout() { Orientation = StackOrientation.Vertical, Spacing = 5, VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
                var l3 = new Label() { TextColor = Color.White, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, FontSize = 15f };
                l3.Text = count + " assignments found that can be bumped";
                var b = new SfButton() { HorizontalOptions = LayoutOptions.Center, WidthRequest = 80, VerticalOptions = LayoutOptions.End, BackgroundColor = Color.DarkGray, Margin = new Thickness(5,5) };
                b.Clicked += B_Clicked;
                b.Text = "Bump";
                b.TextColor = Color.White;
                botRow.Children.Add(l3);
                botRow.Children.Add(b);

                m.Children.Add(topRow);
                m.Children.Add(midRow);
                m.Children.Add(botRow);
                return m;
            });
            bumpPop.PopupView.ContentTemplate = popupView;
            bumpPop.PopupView.AutoSizeMode = AutoSizeMode.Both;
            bumpPop.PopupView.ShowHeader = false;
            bumpPop.PopupView.ShowFooter = false;
            bumpPop.ClosePopupOnBackButtonPressed = true;
            bumpPop.Show();
        }

        private async void B_Clicked(object sender, EventArgs e)
        {
            await PreBumpWork();
            Device.BeginInvokeOnMainThread(() => {
                loadPop.Dismiss();
                loadPop.IsOpen = false;
                loadPop.IsVisible = false;
            });
        }

        private Task PreBumpWork()
        {
            return Task.Factory.StartNew(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    bumpPop.Dismiss();
                    DataTemplate loadView = new DataTemplate(() =>
                    {
                        SfBusyIndicator busy = new SfBusyIndicator();
                        busy.IsBusy = true;
                        return busy;
                    });
                    loadPop.PopupView.ContentTemplate = loadView;
                    loadPop.PopupView.ShowFooter = false;
                    loadPop.PopupView.ShowHeader = false;
                    loadPop.ClosePopupOnBackButtonPressed = false;
                    loadPop.Show();
                });

                if (String.IsNullOrEmpty((string)gradeSel.SelectedValue) || String.IsNullOrWhiteSpace((string)gradeSel.SelectedValue))
                {
                    Device.BeginInvokeOnMainThread(() => {
                        gradeSel.Watermark = "Please select a grade";
                        loadPop.Dismiss();
                        bumpPop.Show();
                    });
                }
                else
                {
                    string grade = (string)gradeSel.SelectedValue;

                    Task.Run(async () => await BumpGrade(grade));
                }
            });
        }

        private async Task BumpGrade(string grade)
        {
            int count = 0;
            List<int> pos = new List<int>();
            pos.Clear();
            int iterate = 0;
            foreach (Assignments a in Asses)
            {
                if (a.Grade == "NG" && a.Possible > 0)
                {
                    count++;
                    pos.Add(iterate);
                }
                iterate++;
            }
            await Task.FromResult<bool>(DoBump(grade, pos));
            Device.BeginInvokeOnMainThread(() => {
                UpdateOverallScore();
                AssList.ItemsSource = Asses;
            });
        }

        private bool DoBump(string grade, List<int> pos)
        {
            foreach (int k in pos)
            {
                int subCount = 0;
                foreach (CategoriesBox cb in catBoxes)
                {
                    if (cb.Description == Asses[k].AssignmentType)
                    {
                        Asses[k].Grade = "Graded";
                        string currGrade = grade;
                        cb.CatPossible += Asses[k].Possible;

                        Asses[k].Points = 0;
                        cb.CatPoints += Asses[k].Points;
                        cb.Percent = cb.CatPoints / cb.CatPossible;
                        string gradeNew = GetOverallFromPoint();
                        while (!currGrade.Equals(gradeNew))
                        {
                            Asses[k].Points += 1;
                            cb.CatPoints += 1;
                            cb.Percent = cb.CatPoints / cb.CatPossible;
                            gradeNew = GetOverallFromPoint();
                        }
                        break;
                    }
                    subCount++;
                }
            }
            loadPop.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                loadPop.Dismiss();
            });
            return true;
        }
    }
}
