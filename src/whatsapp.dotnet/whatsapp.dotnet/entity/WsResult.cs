namespace whatsapp.dotnet
{
    public class WsResult
    {
        public ContentTypeWS ContentTypeWS { get; set; }

        public object Result { get; set; }

        public StatusTransaction StatusTransaction { get; set; }
    }
}