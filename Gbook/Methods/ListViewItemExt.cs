using System;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace Gbook.Methods
{
    public class ListViewItemExt : ListViewItem
    {
        private SfListView listView;

        public ListViewItemExt(SfListView l)
        {
            this.listView = l;
        }

        protected override void OnItemAppearing()
        {
            Opacity = 0;
            this.FadeTo(1, 600, Easing.SinInOut);
            base.OnItemAppearing();
        }
    }
}
