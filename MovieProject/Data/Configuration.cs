using System.Text.RegularExpressions;

namespace MovieProject.Data
{
    public static class Configuration
    {
        public const string CONNECTION_STRING = "Server=DESKTOP-P0HDHCB\\SQLEXPRESS;Database=MovieDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
        public static bool IsValidReleasedFormat(string released)
        {
            string pattern = @"^\d{4}-\d{2}-\d{2}$";

            return Regex.IsMatch(released, pattern);
        }
        public static bool IsValidYearFormat(string year)
        {
            string pattern = @"^\d{4}$";

            return Regex.IsMatch(year, pattern);
        }
        public static bool IsValidDirectorFullNameFormat(string directorFullName)
        {
            string pattern = @"^[a-zA-Z]+ [a-zA-Z]+$";

            return Regex.IsMatch(directorFullName, pattern);
        }
        public static bool IsValidGenresFormat(string genres)
        {
            string pattern = @"^[a-zA-Z]+(?:,\s*[a-zA-Z\s]+)*$";

            return Regex.IsMatch(genres, pattern);
        }
    }
}
