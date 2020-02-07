using System;
using System.Collections.ObjectModel;
using System.Linq;
using Gbook.ClassFiles;
using Gbook.Converters;
using Gbook.Methods;
using Gbook.ViewModel;
using Syncfusion.DataSource;
using Syncfusion.DataSource.Extensions;
using Syncfusion.ListView.XForms;
using Syncfusion.SfChart.XForms;
using Syncfusion.XForms.Backdrop;
using Xamarin.Forms;
using Syncfusion.XForms.Border;
using System.Threading.Tasks;

namespace Gbook
{
    public partial class GradesPage : SfBackdropPage
    {
        public static string PageTermGlobal;
        public static ObservableCollection<Data> TermedData;

        private static GroupResult expandedGroup;

        public GradesPage()
        {
            InitializeComponent();
            this.OpenIcon = "ArrowDown.png";

            NavListRepo r = new NavListRepo();
            navList.ItemsSource = r.Navy;

            if (!LoginPage.LoggedIn)
            {
                Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            }
            else
            {
                string currentTerm = Globals.TermsData.Last<Terms>().Code;
                Title = Globals.TermsData.Last<Terms>().Termname + " Overview";
                PageTermGlobal = currentTerm;

                StudentName.Text = Globals.Dataset[0].StudentName;
                navList.SelectionChanged += navListTapped;

                BackLayerRevealOption = RevealOption.Auto;
                BackLayer = new BackdropBackLayer();
                BackLayer.VerticalOptions = LayoutOptions.Start;
                BackLayer.BackgroundColor = LoginPage.g1;
                listView.CollapseAll();
            }
        }

        protected override void OnAppearing()
        {

            if (LoginPage.LoggedIn)
            {
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("00416A");
                ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;

                double heightSide = Application.Current.MainPage.Height;
                double tHeightSide = heightSide + 500.0;
                navDraw.HeightRequest = tHeightSide;

                SetBackLayerContent();

                grad1.Color = LoginPage.g1;
                grad1.Offset = LoginPage.o1;

                grad2.Color = LoginPage.g2;
                grad2.Offset = LoginPage.o2;

                listView.ItemGenerator = new ItemGeneratorExt(listView);
                SetListViewListeners();



                listView.ItemTemplate = SetListViewItemTemplate();
                listView.GroupHeaderTemplate = DefGroupHeaderTemplate();
            }

        }

        private void SetBackLayerContent()
        {
            var grid = new Grid() { VerticalOptions = LayoutOptions.Start };
            grid.HeightRequest = Globals.TermsData.Count * 40;
            SfListView backList = new SfListView();
            backList.ItemTapped += backListTappedAsync;
            backList.ItemsSource = Globals.TermsData;
            backList.ItemTemplate = new DataTemplate(() =>
            {
                Grid MainGrid = new Grid() { VerticalOptions = LayoutOptions.Start, Margin = 5, Padding = 2 };

                StackLayout myLayout = new StackLayout() { HeightRequest = 35, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
                Label l = new Label() { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, FontSize = 14, TextColor = Color.White };
                l.SetBinding(Label.TextProperty, new Binding("Termname"));
                myLayout.Children.Add(l);

                MainGrid.Children.Add(myLayout);
                return MainGrid;
            });
            grid.Children.Add(backList);
            BackLayer.Content = grid;
        }


        // Group header Template
        private DataTemplate DefGroupHeaderTemplate()
        {
            DataTemplate t = new DataTemplate(() =>
            {
                SfBorder HeaderBorder = new SfBorder() { CornerRadius = new Thickness(10, 10, 10, 0), BorderWidth = 0 };
                var MainGrid = new Grid() { VerticalOptions = LayoutOptions.Center, HeightRequest = 58, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 0), Padding = 0, RowSpacing = 0, ColumnSpacing = 0 };

                MainGrid.BackgroundColor = Xamarin.Forms.Color.FromRgba(0, 0, 0, 0.70);

                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                Binding binding1 = new Binding("Key");
                binding1.Converter = new GroupHeaderConverter();
                binding1.ConverterParameter = 0;

                var label = new Label() { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Start, FontSize = 17, FontAttributes = FontAttributes.Bold, TextColor = Color.White, Margin = new Thickness(5, 0, 0, 0) };
                label.SetBinding(Label.TextProperty, binding1);

                Binding binding4 = new Binding("Key");
                binding4.Converter = new GroupHeaderConverter();
                binding4.ConverterParameter = 3;
                var classType = new Label() { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, FontSize = 10, TextColor = Color.White };
                classType.SetBinding(Label.TextProperty, binding4);

                var stackLayout = new StackLayout();
                stackLayout.Orientation = StackOrientation.Horizontal;
                stackLayout.Children.Add(label);
                stackLayout.Children.Add(classType);

                Binding binding2 = new Binding("Key");
                binding2.Converter = new GroupHeaderConverter();
                binding2.ConverterParameter = 1;
                binding2.StringFormat = "{0}%";

                Frame border = new Frame() { Padding = 0, WidthRequest = 75, HeightRequest = 50, Margin = 10, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.End };
                StackLayout score = new StackLayout() { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center };
                Label scoreLabel = new Label() { TextColor = Color.White, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center };
                scoreLabel.SetBinding(Label.TextProperty, binding2);
                score.Children.Add(scoreLabel);
                Binding binding3 = new Binding("Key");
                binding3.Converter = new GroupHeaderConverter();
                binding3.ConverterParameter = 2;
                border.SetBinding(BackgroundColorProperty, binding3);
                border.Content = score;

                MainGrid.Children.Add(stackLayout);
                Grid.SetColumn(stackLayout, 0);
                Grid.SetColumnSpan(stackLayout, 2);

                MainGrid.Children.Add(border);
                Grid.SetColumn(border, 2);

                HeaderBorder.Content = MainGrid;

                return HeaderBorder;
            });
            return t;
        }
        //End of GroupHeader Templates


        //List ItemTemplate

        private DataTemplate SetListViewItemTemplate()
        {
            listView.CollapseAll();
            DataTemplate t = new DataTemplate(() => {

                SfBorder Border = new SfBorder() { CornerRadius = new Thickness(0, 0, 10, 10), BorderWidth = 0, VerticalOptions = LayoutOptions.FillAndExpand };

                var grid = new StackLayout() { VerticalOptions = LayoutOptions.StartAndExpand, Spacing = 0 };
                grid.BackgroundColor = Xamarin.Forms.Color.FromRgba(0, 0, 0, 0.35);


                var HolderInside = new Grid() { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.Start, Margin = 10, Padding = 10 };
                HolderInside.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                HolderInside.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                SfChart chart = PrefChart(Xamarin.Essentials.Preferences.Get("chartType", 0));
                HolderInside.Children.Add(chart);
                Grid.SetRow(chart, 0);

                grid.Children.Add(HolderInside);

                StackLayout labelHolder = new StackLayout();
                Label l = new Label() { FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.End, FontSize = 13, TextColor = Color.White, Margin = new Thickness(5, 0, 0, 0) };
                l.SetBinding(Label.TextProperty, new Binding("TeachersName"));
                Label l2 = new Label() { FontAttributes = FontAttributes.Italic, VerticalOptions = LayoutOptions.Center, FontSize = 12, TextColor = Color.White, Margin = new Thickness(5, 0, 0, 2) };
                Binding periodB = new Binding("TeachersEmail");
                periodB.StringFormat = "Email: {0}";
                l2.SetBinding(Label.TextProperty, periodB);
                labelHolder.Orientation = StackOrientation.Vertical;
                labelHolder.Children.Add(l);
                labelHolder.Children.Add(l2);

                grid.Children.Add(labelHolder);

                Border.Content = grid;
                return Border;
            });
            return t;
        }

        private SfChart PrefChart(int type)
        {
            if (type == 1)
            {
                SfChart chart = new SfChart() { SideBySideSeriesPlacement = false, HeightRequest = 370, BackgroundColor = Color.Transparent };

                Binding chartBind1 = new Binding("CatInfoSet");
                chartBind1.Converter = new BarChartConverter();
                chartBind1.ConverterParameter = 1;

                ColumnSeries series1 = new ColumnSeries();
                series1.SetBinding(ChartSeries.ItemsSourceProperty, new Binding("CatInfoSet"));
                series1.XBindingPath = "Description";
                series1.YBindingPath = "Weight";
                series1.SetBinding(ColumnSeries.WidthProperty, chartBind1);
                series1.Color = Xamarin.Forms.Color.Transparent;
                series1.StrokeColor = Color.White;

                series1.CornerRadius = new ChartCornerRadius(5, 5, 0, 0);
                series1.StrokeWidth = 0.3;
                series1.EnableAnimation = true;
                series1.AnimationDuration = 0.8;
                series1.DataMarker = new ChartDataMarker();
                DataTemplate dataMarkerTemplate2 = new DataTemplate(() =>
                {
                    StackLayout stack = new StackLayout();
                    stack.Orientation = StackOrientation.Horizontal;
                    Label label = new Label() { TextColor = Color.White };
                    label.SetBinding(Label.TextProperty, "Weight", stringFormat: "Total: {0}");
                    label.FontSize = 12;
                    label.VerticalOptions = LayoutOptions.Center;
                    stack.Children.Add(label);
                    return stack;
                });
                series1.DataMarker.LabelTemplate = dataMarkerTemplate2;

                Binding chartBind2 = new Binding("CatInfoSet");
                chartBind2.Converter = new BarChartConverter();
                chartBind2.ConverterParameter = 1;

                ColumnSeries series2 = new ColumnSeries();
                series2.SetBinding(ChartSeries.ItemsSourceProperty, new Binding("CatInfoSet"));
                series2.XBindingPath = "Description";
                series2.YBindingPath = "WeightPercent";

                series2.SetBinding(ColumnSeries.WidthProperty, chartBind2);

                series2.Opacity = 1;
                series2.CornerRadius = new ChartCornerRadius(5, 5, 0, 0);

                series2.DataMarker = new ChartDataMarker();
                series2.DataMarker.ShowLabel = true;
                series2.DataMarker.LabelStyle.LabelPosition = DataMarkerLabelPosition.Inner;

                Binding backCol = new Binding("CatInfoSet");
                backCol.Converter = new BarChartConverter();
                backCol.ConverterParameter = 4;

                series2.SetBinding(ChartSeries.ColorModelProperty, backCol);

                Binding labelOffsetBind = new Binding("CatInfoSet")
                {
                    Converter = new BarChartConverter(),
                    ConverterParameter = 2
                };

                DataTemplate dataMarkerTemplate = new DataTemplate(() =>
                {
                    Frame f = new Frame() { Padding = 0, Margin = 0 };
                    Binding bCol = new Binding("Percent");
                    bCol.Converter = new BarChartConverter();
                    bCol.ConverterParameter = 3;

                    f.BackgroundColor = Color.Transparent;

                    StackLayout stack = new StackLayout() { VerticalOptions = LayoutOptions.EndAndExpand };
                    stack.Orientation = StackOrientation.Horizontal;
                    Label label = new Label() { HorizontalOptions = LayoutOptions.Center, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center };
                    label.SetBinding(Label.TextProperty, "WeightPercent", stringFormat: "You: \n{0}");
                    label.FontSize = 10;
                    label.VerticalOptions = LayoutOptions.Center;

                    stack.Children.Add(label);
                    f.Content = stack;
                    return f;
                });
                series2.DataMarker.LabelTemplate = dataMarkerTemplate;

                series2.EnableAnimation = true;
                series2.AnimationDuration = 1;

                chart.Series.Add(series2);
                chart.Series.Add(series1);

                ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior();
                chart.ChartBehaviors.Add(zoomPanBehavior);
                zoomPanBehavior.EnableDirectionalZooming = true;

                NumericalAxis numericalAxis = new NumericalAxis() { ShowMajorGridLines = true, ShowMinorGridLines = true, Minimum = 0, Maximum = 119, Name = "Score", Interval = 20, LabelStyle = new ChartAxisLabelStyle() { FontSize = 13, TextColor = Color.White, FontAttributes = FontAttributes.Bold } };

                chart.SecondaryAxis = numericalAxis;
                chart.PrimaryAxis = new CategoryAxis() { Interval = 1, LabelPlacement = LabelPlacement.BetweenTicks, Name = "Prim" };

                chart.PrimaryAxis.LabelStyle.FontAttributes = FontAttributes.Bold;
                chart.PrimaryAxis.LabelStyle.FontSize = 16;
                chart.SecondaryAxis.LabelStyle.FontSize = 12;
                chart.PrimaryAxis.LabelStyle.TextColor = Color.White;
                chart.PrimaryAxis.LabelsIntersectAction = AxisLabelsIntersectAction.MultipleRows;

                return chart;
            }
            else
            {
                SfChart chart = new SfChart() { HeightRequest = 230, BackgroundColor = Xamarin.Forms.Color.FromRgba(0, 0, 0, 0), HorizontalOptions = LayoutOptions.Start };
                DoughnutSeries dSeries = new DoughnutSeries();
                dSeries.SetBinding(ChartSeries.ItemsSourceProperty, new Binding("CatInfoSet"));
                dSeries.XBindingPath = "Description";
                dSeries.YBindingPath = "Percent";
                dSeries.IsStackedDoughnut = true;
                dSeries.Spacing = 0.6;
                Binding localBind = new Binding("CatInfoSet");
                localBind.Converter = new ListViewConverterItem();
                dSeries.SetBinding(DoughnutSeries.DoughnutCoefficientProperty, localBind);
                dSeries.MaximumValue = 100;
                dSeries.CapStyle = DoughnutCapStyle.BothCurve;
                dSeries.EnableAnimation = true;
                chart.Series.Add(dSeries);

                ChartLegend legend = new ChartLegend();
                chart.Legend = legend;
                chart.Legend.DockPosition = LegendPlacement.Right;
                chart.Legend.Orientation = ChartOrientation.Vertical;
                chart.Legend.LabelStyle.TextColor = Color.White;

                legend.ItemTemplate = new DataTemplate(() =>
                {
                    StackLayout stack = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        Spacing = 5,
                        VerticalOptions = LayoutOptions.StartAndExpand
                    };

                    StackLayout SubStack = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = 200, Spacing = 2 };

                    BoxView boxView = new BoxView()
                    {
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 15,
                        HeightRequest = 15
                    };
                    boxView.SetBinding(BackgroundColorProperty, "IconColor");

                    Label name = new Label()
                    {
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = 13,
                        TextColor = Color.White
                    };
                    name.SetBinding(Label.TextProperty, "DataPoint.Description");

                    Frame frame = new Frame() { Padding = 1, Margin = 10, HeightRequest = 25, WidthRequest = 50, HorizontalOptions = LayoutOptions.EndAndExpand };
                    Label value = new Label()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        TextColor = Color.White,
                        FontAttributes = FontAttributes.Bold,
                    };

                    Binding TextBind = new Binding("DataPoint.Percent");
                    TextBind.Converter = new LegendItemConverter();
                    TextBind.StringFormat = "{0}%";

                    value.SetBinding(Label.TextProperty, TextBind);
                    frame.Content = value;

                    Binding colorBind = new Binding("DataPoint.Percent");
                    colorBind.Converter = new LegendItemConverter();
                    frame.SetBinding(BackgroundColorProperty, colorBind);

                    SubStack.Children.Add(boxView);
                    SubStack.Children.Add(name);
                    SubStack.Children.Add(frame);

                    stack.Children.Add(SubStack);
                    return stack;
                });

                return chart;
            }
        }
        //End of list of 


        private void SetListViewListeners()
        {
            if (listView.DataSource.GroupDescriptors.Count == 0)
            {
                listView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
                {
                    PropertyName = "CourseName",
                    KeySelector = (object obj1) =>
                    {
                        var item = (obj1 as Data);
                        return item;
                    }
                });
            }

            listView.ItemSpacing = 0;
            listView.RowSpacing = 0;

            listView.AllowGroupExpandCollapse = true;
            listView.SelectionGesture = TouchGesture.Tap;
            listView.SelectionMode = Syncfusion.ListView.XForms.SelectionMode.Single;
            listView.AutoFitMode = AutoFitMode.Height;

            listView.Loaded += ListView_Loaded;
            listView.PropertyChanged += ListView_PropertyChanged;
            listView.SelectionChanged += ListView_OnSelectionChanged;
            listView.GroupExpanding += ListView_GroupExpanding;
        }

        ///--- Listeneres ---///
        private async void ListView_OnSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            Data dat = (Data)listView.SelectedItem;
            Globals.currentAss = dat;
            await NavigationExtensions.PushAsyncSingle(Navigation,new AssignmentsPage(), true);
        }

        private void ListView_Loaded(object sender, ListViewLoadedEventArgs e)
        {
            listView.CollapseAll();
            var group = listView.DataSource.Groups[0];
            listView.ExpandGroup(group);
        }

        private void ListView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                listView.CollapseAll();

            }
        }

        private void ListView_GroupExpanding(object sender, GroupExpandCollapseChangingEventArgs e)
        {
            if (e.Groups.Count > 0)
            {
                var group = e.Groups[0];
                if (expandedGroup == null || group.Key != expandedGroup.Key)
                {
                    foreach (var otherGroup in listView.DataSource.Groups)
                    {
                        if (group.Key != otherGroup.Key)
                        {
                            listView.CollapseGroup(otherGroup);
                        }
                    }

                    expandedGroup = group;
                    listView.ExpandGroup(expandedGroup);
                    int index = listView.DataSource.DisplayItems.IndexOf(expandedGroup);
                    listView.LayoutManager.ScrollToRowIndex(index, Syncfusion.ListView.XForms.ScrollToPosition.Center, true);
                }
            }
        }

        private void backListTappedAsync(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            Terms term = (Terms)e.ItemData;
            PageTermGlobal = term.Code;
            ObservableCollection<Data> obsData = new ObservableCollection<Data>();
            foreach (Data x in Globals.Dataset)
            {
                if (x.TermCode == PageTermGlobal)
                {
                    obsData.Add(x);
                }
            }
            listView.ItemsSource = obsData;
            this.IsBackLayerRevealed = false;
            this.Title = term.Termname + " Overview";
        }

        private void navListTapped(object sender, ItemSelectionChangedEventArgs e)
        {

            NavList r = (NavList)navList.SelectedItem;
            String m = r.NavTitle;
            if (m == "Grades")
            {
                Application.Current.MainPage = new NavigationPage(new GradesPage()) { BarBackgroundColor = LoginPage.g1, BarTextColor = Color.White };
            }
            else if (m == "Settings")
            {
                Application.Current.MainPage = new NavigationPage(new Settings()) { BarBackgroundColor = LoginPage.g1, BarTextColor = Color.White };
            }
            else if (m == "Logout")
            {
                Globals.Dataset.Clear();
                Xamarin.Essentials.Preferences.Remove("username");
                Xamarin.Essentials.Preferences.Remove("password");
                Application.Current.MainPage = new NavigationPage(new LoginPage());

            }
        }

        void hamburgerButton_Clicked(object sender, EventArgs e)
        {
            navDraw.ToggleDrawer();
        }

        //---------------//
    }

}
