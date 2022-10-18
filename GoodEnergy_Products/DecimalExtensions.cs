using System.Globalization;

namespace GoodEnergy_Products
{
    public static class DecimalExtensions
    {
        public static string ToGBString(this decimal val)
        {
            return val.ToString("C", CultureInfo.CreateSpecificCulture("en-GB"));
        }
    }
}
