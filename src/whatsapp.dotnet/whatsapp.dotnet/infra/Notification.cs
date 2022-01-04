using whatsapp.dotnet;

namespace whatsapp.dotnet
{
    public class Notification
    {
        public EventsMessage EventsMessage { get; set; }
        public ProcessingType ProcessingType { get; set; }

        public SubNotificationType SubNotificationType { get; set; }

        public object Container { get; set; }

        public Instance Instance { get; set; }
         
    }
}