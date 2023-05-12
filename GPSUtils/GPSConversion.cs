using System.Globalization;

namespace sim7600collector.GPSUtils
{
    public class GPSConversion
    {
        public static double[] ConvertGpsToDecimalDegrees(string gpsString)
        {
            // Split the GPS string into an array of strings.
            var gpsValues = gpsString.Split(',');

            // Get the latitude and longitude values.
            var latitudeString = gpsValues[0];
            var longitudeString = gpsValues[2];

            // Convert latitude and longitude strings to decimal degrees.
            double latitude = ParseGpsCoordinate(latitudeString);
            double longitude = ParseGpsCoordinate(longitudeString);

            // Check the direction of the latitude and longitude values.
            if (gpsValues[1] == "S")
            {
                latitude = -latitude;
            }
            if (gpsValues[3] == "E")
            {
                longitude = -longitude;
            }

            // Return the latitude and longitude values in decimal degrees.
            return new double[] { latitude, longitude };
        }

        private static double ParseGpsCoordinate(string coordinateString)
        {
            double coordinate = double.Parse(coordinateString, CultureInfo.InvariantCulture);

            int degrees = (int)(coordinate / 100);
            double minutes = coordinate % 100;
            double decimalDegrees = degrees + (minutes / 60);

            return decimalDegrees;
        }

        public static DateTime GetDateOnly(string gpsString)
        {
            var gpsValues = gpsString.Split(',');

            DateTime date = DateTime.ParseExact(gpsValues[4], "MMddyy", CultureInfo.InvariantCulture);

            return date.Date;
        }

        public static TimeSpan GetTimeOnly(string gpsString)
        {
            var gpsValues = gpsString.Split(',');

            TimeSpan timeSpan = TimeSpan.FromSeconds(Convert.ToDouble(gpsValues[5]));

            return timeSpan;
        }


        public static DateTime GetDateTime(string gpsString)
        {
            DateTime dateOnly = GetDateOnly(gpsString);
            TimeSpan timeOnly = GetTimeOnly(gpsString);

            DateTime dateTime = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day, timeOnly.Hours, timeOnly.Minutes, timeOnly.Seconds);

            return dateTime;
        }
    }
}
