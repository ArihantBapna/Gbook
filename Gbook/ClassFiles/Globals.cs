using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gbook.ClassFiles;
using Gbook.ViewModel;

namespace Gbook.ClassFiles
{
    public static class Globals
    {
        public static ObservableCollection<Data> Dataset = new ObservableCollection<Data>();
        public static List<Terms> TermsData = new List<Terms>();
        public static Data SelectedData = new Data();
    }
}
