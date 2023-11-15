using StackExchange.Redis;

namespace BlogService.Common.Utilities
{
    public class ConnectionHelper
    {
        private static ConfigurationOptions configurationOptions = new ConfigurationOptions
        {
            EndPoints = { ConfigurationManager.AppSetting["Radis:RedisUrl"] },
            Password = ConfigurationManager.AppSetting["Radis:RedisPassword"],
            Ssl = false,
            AbortOnConnectFail = false
        };

        static ConnectionHelper()
        {
            ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(configurationOptions);
            });
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
