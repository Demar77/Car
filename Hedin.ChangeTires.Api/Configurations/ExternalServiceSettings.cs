using Microsoft.Extensions.Options;

namespace Hedin.ChangeTires.Api.Configurations
{
    public class ExternalServiceSettings
    {
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
