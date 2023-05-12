using System.Globalization;

namespace sim7600collector.GPSUtils
{
    public class GPSConversion
    {
        public static void ConvertGpsToDecimalDegrees(string gpsString, out double decimalLatitute, out double decimalLongitude)
        {
            // Split the GPS string into an array of strings.
            var gpsValues = gpsString.Split(',');

            // Get the latitude and longitude values.
            var latitudeString = gpsValues[0];
            var longitudeString = gpsValues[2];

            // Convert latitude and longitude strings to decimal degrees.
            decimalLatitute = ParseGpsCoordinate(latitudeString);
            decimalLongitude = ParseGpsCoordinate(longitudeString);

            // Check the direction of the latitude and longitude values.
            if (gpsValues[1] == "S")
            {
                decimalLatitute = -decimalLatitute;
            }
            
            if (gpsValues[3] == "E")
            {
                decimalLongitude = decimalLongitude;
            }
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

            DateTime date = DateTime.ParseExact(gpsValues[4], "ddMMyy", CultureInfo.InvariantCulture);

            return date.Date;
        }

        public static TimeSpan GetTimeOnly(string gpsString)
        {
            var gpsValues = gpsString.Split(',');

            string timeString = gpsValues[5];
            int hours = int.Parse(timeString.Substring(0, 2));
            int minutes = int.Parse(timeString.Substring(2, 2));
            int seconds = int.Parse(timeString.Substring(4, 2));

            TimeSpan timeSpan = new TimeSpan(hours, minutes, seconds);

            return timeSpan;
        }

        public static DateTime GetDateTime(string gpsString)
        {
            DateTime dateOnly = GetDateOnly(gpsString);
            TimeSpan timeOnly = GetTimeOnly(gpsString);

            DateTime dateTime = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day, timeOnly.Hours, timeOnly.Minutes, timeOnly.Seconds);

            return dateTime;
        }

        public static void ConvertSpeed(string speed, out double speedInKnots, out double speedInKmph, out double speedInMph)
        {
            speedInKnots = double.Parse(speed);
            speedInKmph = Math.Round(speedInKnots * 1.852, 2);
            speedInMph = Math.Round(speedInKnots * 1.1508, 2);
        }

        public static double ParseDoubleValue(string value)
        {
            if (double.TryParse(value, out double result))
            {
                return result;
            }

            return 0.0; // Default value if parsing fails
        }
    }
}
