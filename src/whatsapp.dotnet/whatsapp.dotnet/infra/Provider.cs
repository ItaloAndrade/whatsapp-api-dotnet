using System.Reactive.Subjects;
using PuppeteerSharp;
using whatsapp.dotnet;

namespace whatsapp.dotnet
{
    public class Provider
    {
        public string id { get; set; }
        public Page page { get; set; }
        public Browser browser { get; set; }
        public Subject<Notification> eventNotification { get; set; }
        public Instance instance { get; set; }
    }
}