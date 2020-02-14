using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gbook.ClassFiles;
using Gbook.Methods;
using Gbook.ViewModel;
using Newtonsoft.Json;

namespace Gbook.Comms
{
    public class DataFetcher
    {
        public static List<Classes> myClasses;
        public static bool loggedIn;
        public static async Task AttemptLogIn()
        {
            bool x = await ClientInitializor.ReturnTheClient();
            if (x)
            {
                await SetDataset();
                loggedIn = true;
            }
        }

        private static async Task SetDataset()
        {
            var TempClass = await StoreClassesAsync();
            var TermsData = await GetTermsAsync();
            Globals.TermsData = TermsData;
            ClientInitializor.CurrentTerm = TermsData[TermsData.Count - 1].Code;
            foreach (Classes c in TempClass)
            {
                Data obj = new Data();

                obj.StudentName = c.Student;
                obj.SectionCode = c.SectionId;

                String name = c.CourseName;
                if (name.Contains("AP "))
                {
                    obj.ClassType = "AP";
                    obj.CourseName = name.Replace("AP ", "");
                }
                else if (name.Contains("HON "))
                {
                    obj.ClassType = "HON";
                    obj.CourseName = name.Replace("HON ", "");
                }
                else if (name.Contains("ADV "))
                {
                    obj.ClassType = "ADV ";
                    obj.CourseName = name.Replace("ADV ", "");
                }
                else
                {
                    obj.ClassType = "REG";
                    obj.CourseName = name;
                }

                obj.TermCode = c.Termid;
                foreach (Terms t in TermsData)
                {
                    if (t.Code == obj.TermCode)
                    {
                        obj.TermName = t.Termname;
                    }
                }

                obj.OverallGrade = c.OverallGrade;

                obj.OverallPercent = await GetSubjectOverview(c.SectionId, c.Termid);
                obj.OverallColor = ColorGet.ColorFromPercent((int)Math.Round(obj.OverallPercent, 0));

                obj.TeachersName = c.Teacher;
                obj.TeachersEmail = c.Email_Addr;
                obj.Period = c.Period;

                var Cats = await SetCategoriesDataAsync(c.SectionId, c.Termid);

                foreach (CategoryInfo cats1 in Cats)
                {
                    cats1.WeightPercent = (cats1.Percent * cats1.Weight) / 100;
                }

                var Asss = await SetAssignmentsData(c.SectionId, c.Termid);

                foreach (Assignments asses in Asss)
                {
                    asses.AssignmentType = Regex.Replace(asses.AssignmentType, "[^a-zA-Z]", "");
                    if(asses.Grade == "" && asses.Points == 0)
                    {
                        asses.Grade = "NG";
                    }
                    if (Math.Abs(asses.Possible) > 0)
                    {
                        asses.Percent = (asses.Points / asses.Possible) * 100;
                    }
                    else
                    {
                        asses.Percent = 100;
                    }
                    
                }

                for(int i = 0; i < Asss.Count; i++)
                {
                    Asss[i].Id = i;
                }
                foreach(Assignments asses in Asss)
                {
                    int counter = 0;
                    foreach (CategoryInfo cats2 in Cats)
                    {
                        if (cats2.Description == asses.AssignmentType)
                        {
                            asses.CatIndex = counter;
                            asses.Weight = cats2.Weight;
                        }
                        counter += 1;
                    }
                    if (asses.AssignmentType.Contains("Ungraded"))
                    {
                        asses.AssignmentType = "Ungraded";
                        asses.Weight = 0;
                        asses.CatIndex = (counter);
                    }

                    asses.BackColor = ColorGet.ColorFromPercent((int)Math.Round(asses.Percent, 0));
                }
                if (Asss.Where(x => x.AssignmentType.Contains("Ungraded")).Count() > 0)
                {
                    CategoryInfo uCat = new CategoryInfo();
                    uCat.Description = "Ungraded";
                    uCat.Weight = 0;
                    Cats.Add(uCat);
                }

                obj.NoOfCat = Cats.Count;
                obj.UCatInfoSet = Cats;

                List<CategoryInfo> tempCats = new List<CategoryInfo>();
                foreach(CategoryInfo cats3 in Cats)
                {
                    if(cats3.PointsPossible != 0)
                    {
                        tempCats.Add(cats3);
                    }
                }
                if (tempCats.Count > 1)
                {
                    CategoryInfo finalCat = new CategoryInfo();
                    finalCat.Description = "Overall";
                    finalCat.Percent = obj.OverallPercent;
                    finalCat.Weight = 100;
                    finalCat.WeightPercent = obj.OverallPercent;
                    tempCats.Add(finalCat);

                }

                obj.CatInfoSet = tempCats;
                
                obj.AssignmentsList = Asss;

                Globals.Dataset.Add(obj);
            }
        }


        private static async Task<List<Terms>> GetTermsAsync()
        {
            var jsonFileResponse = await ClientInitializor.Client.GetAsync("https://portal.mcpsmd.org/guardian/prefs/termsData.json?schoolid=" + ClientInitializor.schoolId);
            jsonFileResponse.EnsureSuccessStatusCode();
            string cont = await jsonFileResponse.Content.ReadAsStringAsync();
            var x = JsonConvert.DeserializeObject<List<Terms>>(cont);
            List<Terms> y = new List<Terms>();
            foreach (Terms z in x)
            {
                if (!(string.IsNullOrWhiteSpace(z.Id)))
                {
                    y.Add(z);
                }
            }
            return y;
        }

        private static async Task<double> GetSubjectOverview(string sectionid, string termid)
        {
            List<Overview> OverviewCleanedList = new List<Overview>();
            string modifier = "";
            if (termid == ClientInitializor.CurrentTerm)
            {
                modifier = "https://portal.mcpsmd.org/guardian/prefs/assignmentGrade_CourseDetail.json?";
            }
            else
            {
                modifier = "https://portal.mcpsmd.org/guardian/prefs/assignmentGrade_CourseDetail_prior.json?";
            }
            var jsonFileResponse = await ClientInitializor.Client.GetAsync(modifier + "secid=" + sectionid + "&student_number=" + ClientInitializor.username + "&schoolid=" + ClientInitializor.schoolId + "&termid=" + termid);
            jsonFileResponse.EnsureSuccessStatusCode();
            string cont = await jsonFileResponse.Content.ReadAsStringAsync();
            Overview OverviewList = JsonConvert.DeserializeObject<Overview>(cont);

            return OverviewList.Percent;

        }

        private static async Task<List<Classes>> StoreClassesAsync()
        {
            var jsonFileResponse = await ClientInitializor.Client.GetAsync("https://portal.mcpsmd.org/guardian/prefs/gradeByCourseSecondary.json?schoolid=" + ClientInitializor.schoolId + "&student_number=" + ClientInitializor.username);
            jsonFileResponse.EnsureSuccessStatusCode();
            string cont = await jsonFileResponse.Content.ReadAsStringAsync();
            var x = JsonConvert.DeserializeObject<List<Classes>>(cont);
            List<Classes> y = new List<Classes>();
            foreach (Classes z in x)
            {
                if (!(string.IsNullOrWhiteSpace(z.OverallGrade)))
                {
                    y.Add(z);
                }
            }
            return y;
        }

        public static async Task<List<CategoryInfo>> SetCategoriesDataAsync(string sectionid, string termid)
        {
            List<CategoryInfo> CatInfoCleaned = new List<CategoryInfo>();
            string modifier = "";
            if (termid == ClientInitializor.CurrentTerm)
            {
                modifier = "https://portal.mcpsmd.org/guardian/prefs/assignmentGrade_CategoryDetail.json?";
            }
            else
            {
                modifier = "https://portal.mcpsmd.org/guardian/prefs/assignmentGrade_CategoryDetail_prior.json?";
            }
            var jsonFileResponse = await ClientInitializor.Client.GetAsync(modifier + "secid=" + sectionid + "&student_number=" + ClientInitializor.username + "&schoolid=" + ClientInitializor.schoolId + "&termid=" + termid);
            jsonFileResponse.EnsureSuccessStatusCode();
            string cont = await jsonFileResponse.Content.ReadAsStringAsync();
            List<CategoryInfo> CatInfo = JsonConvert.DeserializeObject<List<CategoryInfo>>(cont);
            foreach (CategoryInfo y in CatInfo)
            {
                if (!string.IsNullOrWhiteSpace(y.Description))
                {
                    if (y != null)
                    {
                        if (!string.IsNullOrEmpty(y.Description))
                        {
                            y.Description = Regex.Replace(y.Description, "[^a-zA-Z]", "");
                            /*
                            if ((int)y.PointsPossible != 0)
                            {
                                CatInfoCleaned.Add(y);
                            }
                            */
                            CatInfoCleaned.Add(y);

                        }
                    }
                }
            }
            return CatInfoCleaned;
        }

        private static async Task<ObservableCollection<Assignments>> SetAssignmentsData(string sectionid, string termid)
        {
            List<AssignmentsRaw> AssignmentTable = new List<AssignmentsRaw>();
            ObservableCollection<Assignments> AssignmentsTableModified = new ObservableCollection<Assignments>();
            string modifier = "";
            if (termid == ClientInitializor.CurrentTerm)
            {
                modifier = "https://portal.mcpsmd.org/guardian/prefs/assignmentGrade_AssignmentDetail.json?";
            }
            else
            {
                modifier = "https://portal.mcpsmd.org/guardian/prefs/assignmentGrade_AssignmentDetail_prior.json?";
            }
            var jsonFileResponse = await ClientInitializor.Client.GetAsync(modifier + "secid=" + sectionid + "&student_number=" + ClientInitializor.username + "&schoolid=" + ClientInitializor.schoolId + "&termid=" + termid);
            jsonFileResponse.EnsureSuccessStatusCode();
            string cont = await jsonFileResponse.Content.ReadAsStringAsync();
            List<AssignmentsRaw> RawAssignmentTable = JsonConvert.DeserializeObject<List<AssignmentsRaw>>(cont);
            foreach (AssignmentsRaw y in RawAssignmentTable)
            {
                if (!string.IsNullOrWhiteSpace(y.Description))
                {
                    if (y != null)
                    {
                        if (!string.IsNullOrEmpty(y.DueDate))
                        {
                            AssignmentTable.Add(y);
                        }
                    }
                }
            }
            foreach (AssignmentsRaw ass in AssignmentTable)
            {
                Assignments obj = new Assignments();
                obj.Description = ass.Description;
                obj.Date = ass.DueDate;
                obj.AssignmentType = ass.AssignmentType;

                double x;
                double.TryParse(ass.Possible, out x);
                obj.Possible = x;

                double y;
                double.TryParse(ass.Points, out y);
                obj.Points = y;

                double z;
                double.TryParse(ass.Weight, out z);
                obj.Weight = z;

                obj.Grade = ass.Grade;

                AssignmentsTableModified.Add(obj);
            }
            return AssignmentsTableModified;
        }


    }
}

