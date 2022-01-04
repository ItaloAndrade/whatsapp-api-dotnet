using System.Collections.Generic;

namespace whatsapp.dotnet
{
    public class MessageOption
    {
        public bool? linkPreview { get; set; } 
        public bool? sendAudioAsVoice { get; set; } 
        public bool? sendMediaAsSticker { get; set; } 
        public bool? sendMediaAsDocument { get; set; }  
        public string caption { get; set; } 
        public string quotedMessageId { get; set; }
        public List<Contact> mentions { get; set; }
        public bool sendSeen { get; set; }
        public MessageMedia media { get; set; }
    }
}