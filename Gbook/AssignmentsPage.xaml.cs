using System;
using System.Collections.Generic;
using Gbook.ClassFiles;
using Gbook.ViewModel;
using Xamarin.Forms;

namespace Gbook
{
    public partial class AssignmentsPage : ContentPage
    {
        public AssignmentsPage()
        {
            InitializeComponent();
        }

        private void InitToolbar()
        {
            Data currentAss = Globals.currentAss;
            this.Title = currentAss.CourseName;
        }
    }
}
