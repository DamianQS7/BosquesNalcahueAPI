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
        }
    }
}
