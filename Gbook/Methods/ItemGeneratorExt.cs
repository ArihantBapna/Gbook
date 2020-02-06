using System.Collections.ObjectModel;
using Gbook.ClassFiles;
using Gbook.ViewModel;
using Syncfusion.ListView.XForms;
namespace Gbook.Methods
{
    public class ItemGeneratorExt : ItemGenerator
    {
        public SfListView listView;

        public ItemGeneratorExt(SfListView listView) : base(listView)
        {
            this.listView = listView;
            
            string PageTerm = GradesPage.PageTermGlobal;
            ObservableCollection<Data> obsData = new ObservableCollection<Data>();
            foreach (Data x in Globals.Dataset)
            {
                if (x.TermCode == PageTerm)
                {
                    obsData.Add(x);
                }
            }
            //GradesPage.TermedData = obsData;
            this.listView.ItemsSource = obsData;
        }

        protected override ListViewItem OnCreateListViewItem(int itemIndex, ItemType type, object data = null)
        {
            if (type == ItemType.Record)
                return new ListViewItemExt(listView);
            return base.OnCreateListViewItem(itemIndex, type, data);
        }
    }
}
