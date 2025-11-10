namespace Economy.Core.Helpers
{
    public static class ConnectionStringHelper
    {
        public static string Build(
            string server,
            string database,
            bool isSqlAuth,
            string? user,
            string? password)
        {
            if (isSqlAuth)
            {
                return $"Data Source={server};Initial Catalog={database};" +
                       $"User ID={user};Password={password};" +
                       "Encrypt=True;TrustServerCertificate=True;" +
                       "MultipleActiveResultSets=True;Connect Timeout=30;";
            }

            return $"Data Source={server};Initial Catalog={database};" +
                   "Integrated Security=True;Encrypt=True;" +
                   "TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=30;";
        }
    }
}
