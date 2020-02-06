using System;
using System.Collections.ObjectModel;

namespace Gbook.ClassFiles
{
    public class NavList
    {
        public string NavTitle { get; set; }
        public string Desc { get; set; }
    }

    public class NavListRepo
    {
        private ObservableCollection<NavList> navy;

        public ObservableCollection<NavList> Navy
        {
            get { return navy; }
            set { this.Navy = value; }
        }

        public NavListRepo()
        {
            GenerateNavInfo();
        }

        internal void GenerateNavInfo()
        {
            navy = new ObservableCollection<NavList>();
            navy.Add(new NavList() { NavTitle = "Grades", Desc = "Look at your grades" });
            navy.Add(new NavList() { NavTitle = "Settings", Desc = "Change your preferences" });
            navy.Add(new NavList() { NavTitle = "About", Desc = "Who we are" });
            navy.Add(new NavList() { NavTitle = "Logout", Desc = "Don't want to save your info no " });
        }
    }
}
