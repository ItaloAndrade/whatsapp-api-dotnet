using System.Collections.Generic;

namespace whatsapp.dotnet
{
    public class GroupCreatedResult
    {
        public string status { get; set; }
        public GroupId gid { get; set; }
        public List<dynamic> participants { get; set; }
    }
}