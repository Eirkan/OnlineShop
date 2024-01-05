namespace Order.Infrastructure.Persistence.Settings
{

    public sealed class ConnectionStringSettings
    {
        public const string SettingsKey = "ConnectionStrings";

        public string Sql { get; set; }


        public string Redis { get; set; }
    }
}
