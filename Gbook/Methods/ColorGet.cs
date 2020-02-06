using System.Collections.Generic;
using Color = System.Drawing.Color;

namespace Gbook.Methods
{
    public static class ColorGet
    {
        static byte interpolate(byte a, byte b, double p)
        {
            return (byte)(a * (1 - p) + b * p);
        }
        public static Color ColorFromPercent(int v)
        {

            SortedDictionary<int, Color> d = new SortedDictionary<int, Color>();
            d.Add(0, Color.Violet);
            d.Add(50, Color.OrangeRed);
            d.Add(69, Color.Orchid);
            d.Add(79, Color.Orange);
            d.Add(89, Color.Blue);
            d.Add(95, Color.Green);
            d.Add(100, Color.DarkGreen);

            KeyValuePair<int, Color> kvp_previous = new KeyValuePair<int, Color>(-1, Color.Black);
            foreach (KeyValuePair<int, Color> kvp in d)
            {
                if (kvp.Key > v)
                {
                    double p = (v - kvp_previous.Key) / (double)(kvp.Key - kvp_previous.Key);
                    Color a = kvp_previous.Value;
                    Color b = kvp.Value;
                    Color c = Color.FromArgb(
                        interpolate(a.R, b.R, p),
                        interpolate(a.G, b.G, p),
                        interpolate(a.B, b.B, p));
                    return c;
                }
                else if (kvp.Key == v)
                {
                    return kvp.Value;
                }
            }

            return Color.Black;
        }
    }
}
