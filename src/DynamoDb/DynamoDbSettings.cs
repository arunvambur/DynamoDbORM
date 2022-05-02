namespace DynamoDb
{
    public class DynamoDbSettings
    {
        public bool UseLocalDatabase { get; set; }
        public int Port { get; set; }
        public string EndpointUrl { get; set; }
        public string AccessKey { get; }
        public string SecretKey { get; }
        public string Token { get; }
    }
}
