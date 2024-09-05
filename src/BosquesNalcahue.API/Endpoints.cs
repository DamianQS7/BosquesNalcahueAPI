namespace BosquesNalcahue.API
{
    public static class Endpoints
    {
        public static class Reports
        {
            private const string Base = "api/reports";
            public const string GetAll = Base;
            public const string Create = Base;
            public const string GetById = Base + "/{id}";
            public const string Replace = Base + "/{id}";
            public const string Delete = Base + "/{id}";
            public const string UploadReport = Base + "/upload-report";
        }

        public static class Analytics
        {
            private const string Base = "api/analytics";
            public const string ReportsCountByPeriod = Base + "/reports-count-period";
            public const string ReportsCountByMonth = Base + "/reports-count-month/{month}";
            public const string MonthlyCountBreakdown = Base + "/monthly-breakdown";
        }

        public static class Identity
        {
            private const string Base = "api/identity";
            public const string Register = Base + "/register";
            public const string Login = Base + "/login";
            public const string Refresh = Base + "/refresh";
        }

        public static class Blob
        {
            private const string Base = "api/blob";
            public const string Upload = Base + "/upload";
            public const string Delete = Base + "/{blobId}";
            public const string GetUri = Base + "/{blobId}";
        }
    }
}
