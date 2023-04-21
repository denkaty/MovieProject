namespace MovieProject.Data
{
    public static class Configuration
    {
        public const string CONNECTION_STRING = "Server=DESKTOP-P0HDHCB\\SQLEXPRESS;Database=MovieDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
