using System;
namespace Gbook.ClassFiles
{
    public class CategoryInfo
    {
        public int Id { get; set; }
        public string CategoryGrade { get; set; }
        public string Description { get; set; }
        public int OrganizationId { get; set; }
        public double Percent { get; set; }
        public double PointsEarned { get; set; }
        public double PointsPossible { get; set; }
        public string SectionId { get; set; }
        public int StudentId { get; set; }
        public string TermId { get; set; }
        public double Weight { get; set; }
        public double WeightPercent { get; set; }
    }
}
