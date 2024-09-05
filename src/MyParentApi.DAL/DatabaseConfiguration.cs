namespace MyParentApi.DAL
{
    public record DatabaseConfiguration
    {
        public DatabaseConfiguration(string hostname, string schema, string username, string password)
        {
            HostName = hostname;
            Schema = schema;
            Username = username;
            Password = password;
        }

        public string HostName { get; init; }
        public string Schema { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
    }
}
