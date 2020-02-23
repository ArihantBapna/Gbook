using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfRangeSlider.XForms.iOS;
using Syncfusion.XForms.iOS.Backdrop;
using Syncfusion.XForms.iOS.EffectsView;
using Foundation;
using UIKit;
using Syncfusion.XForms.iOS.MaskedEdit;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.SfNumericUpDown.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using Syncfusion.SfBusyIndicator.XForms.iOS;

namespace Gbook.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Chips
            Syncfusion.XForms.iOS.Buttons.SfChipGroupRenderer.Init();

            //Radio Button
            Syncfusion.XForms.iOS.Buttons.SfRadioButtonRenderer.Init();

            //Popup Layout
            Syncfusion.XForms.iOS.PopupLayout.SfPopupLayoutRenderer.Init();

            //Picker
            SfPickerRenderer.Init();

            //Range slider
            new SfRangeSliderRenderer();

            //Combo box
            new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();

            //Mased Edit
            SfMaskedEditRenderer.Init();

            //Numeric Text Box
            new SfNumericTextBoxRenderer();

            //Numeric Up down
            SfNumericUpDownRenderer.Init();

            //Load Sf List view
            SfListViewRenderer.Init();
            SfEffectsViewRenderer.Init();  //Initialize only when effects view is added to Listview.

            //Progress Bars
            Syncfusion.XForms.iOS.ProgressBar.SfLinearProgressBarRenderer.Init();
            Syncfusion.XForms.iOS.ProgressBar.SfCircularProgressBarRenderer.Init();

            //Backdrop Page
            SfBackdropPageRenderer.Init();

            //Load gradience
            Syncfusion.XForms.iOS.Graphics.SfGradientViewRenderer.Init();

            //Border control
            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();

            //Load input fields
            Syncfusion.XForms.iOS.TextInputLayout.SfTextInputLayoutRenderer.Init();

            //Load Charts
            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();

            //Switch
            Syncfusion.XForms.iOS.Buttons.SfSwitchRenderer.Init();

            //Chips
            Syncfusion.XForms.iOS.Buttons.SfChipRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfChipGroupRenderer.Init();

            //Navigation drawer
            #pragma warning disable RECS0026 // Possible unassigned object created by 'new'

            new Syncfusion.SfNavigationDrawer.XForms.iOS.SfNavigationDrawerRenderer();

            #pragma warning restore RECS0026 // Possible unassigned object created by 'new'

            global::Xamarin.Forms.Forms.Init();

            //Busy Indicator
            new SfBusyIndicatorRenderer();

            LoadApplication(new App());

            //Load buttons
            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfButtonRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfCheckBoxRenderer.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}
