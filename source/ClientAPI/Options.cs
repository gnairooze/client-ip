namespace ClientAPI
{
    public class Options
    {
        public string HeaderKey { get; set; } = string.Empty;
        public string AllowedOrigins { get; set; } = string.Empty;  
        public string AllowedMethods { get; set;} = string.Empty;
        public bool FirstIpOnly { get; set; }
    }
}
