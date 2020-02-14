using System;
namespace Gbook.Methods
{
    public class GradeFromScore
    {
        public static string GetGrade(double score)
        {
            if(score > 50)
            {
                if(score > 69.5)
                {
                    if(score > 79.5)
                    {
                        if(score > 89.5)
                        {
                            return "A";
                        }
                        else
                        {
                            return "B";
                        }
                    }
                    else
                    {
                        return "C";
                    }
                }
                else
                {
                    return "D";
                }
            }
            else
            {
                return "E";
            }
        }
    }
}
