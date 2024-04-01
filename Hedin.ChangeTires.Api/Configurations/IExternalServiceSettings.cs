namespace Hedin.ChangeTires.Api.Configurations
{
    public interface IExternalServiceSettings
    {
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}