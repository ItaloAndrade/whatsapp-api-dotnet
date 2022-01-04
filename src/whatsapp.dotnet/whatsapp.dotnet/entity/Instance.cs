namespace whatsapp.dotnet
{
    public class Instance
    {
        public string id { get; set; }
        public QrCode QrCode { get; set; } = new QrCode();

        public InstanceType StatusInstance { get; set; } = InstanceType.notstarted;
    }
}