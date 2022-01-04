using System;

namespace whatsapp.dotnet
{
   public class MessageResume
    {
        public string Id { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
        public bool HasMedia { get; set; }
        public DateTime CreatedIn { get; set; }
        public string Body { get; set; }
        public string ChatFk { get; set; }
        public bool FromMe { get; set; }
        public string AsFrom { get; set; }
        public string AsTo { get; set; }
    }
}
