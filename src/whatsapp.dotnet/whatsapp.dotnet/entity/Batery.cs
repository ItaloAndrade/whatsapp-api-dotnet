namespace whatsapp.dotnet
{
    public class Batery
    {
        public string batteryValue { get; set; }

        public bool isPlugged { get; set; }
    }

    public class BateryInfo
    {
        public string battery { get; set; }
        public string plugged { get; set; }
    }
}