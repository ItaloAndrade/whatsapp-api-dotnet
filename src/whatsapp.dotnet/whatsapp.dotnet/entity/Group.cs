using System.Collections.Generic;

namespace whatsapp.dotnet
{
    public class Group
    {
        public GroupId id { get; set; }
        public string size { get; set; }
        public string status { get; set; }
        public GroupId owner { get; set; }
        public string subject { get; set; }
        public string creation { get; set; }
        public List<Participant> participants { get; set; }
    }


    public class Participant
    {
        public GroupId id { get; set; }

        public bool isAdmin { get; set; }

        public bool isSuperAdmin { get; set; }
    }

    public class GroupId
    {
        public string user { get; set; }
        public string _serialized { get; set; }
        public string server { get; set; }
    }
}