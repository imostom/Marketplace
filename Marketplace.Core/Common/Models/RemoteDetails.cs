namespace Marketplace.Core.Common.Models
{
    public class RemoteDetails
    {
        public string ApiKey { get; set; } = "";
        public string Authorization { get; set; } = "";
        public string Channel { get; set; } = "";
        public string Port { get; set; }
        public string IpAddress { get; set; } = "";
        public string Path { get; set; } = "";
    }
}
