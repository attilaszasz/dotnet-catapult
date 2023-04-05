using Interfaces;

namespace Redis
{
    public class RedisConfig
    {
        public List<string> Endpoints { get; }
        public string Password { get; set; }
        public int ConnectTimeout { get; set; }
        public bool UseSSL { get; set; }

        public RedisConfig(IConfigurationService configurationService)
        {
            if (configurationService == null) throw new ArgumentNullException(nameof(configurationService));

            var dto = configurationService.Get<RedisConfigDto>("Redis");
            Password = dto.PrimaryKey;
            ConnectTimeout = dto.ConnectionTimeout;
            Endpoints = dto.EndPointsCSV.Split(',').ToList();
            UseSSL = dto.UseSSL;
        }

        public class RedisConfigDto
        {
            public string EndPointsCSV { get; set; } = string.Empty;
            public string PrimaryKey { get; set; } = string.Empty;
            public int ConnectionTimeout { get; set; }
            public bool UseSSL { get; set; }
        }
    }
}