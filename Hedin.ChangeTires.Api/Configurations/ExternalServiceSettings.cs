namespace Hedin.ChangeTires.Api.Configurations
{
    public class ExternalServiceSettings : IExternalServiceSettings
    {
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}