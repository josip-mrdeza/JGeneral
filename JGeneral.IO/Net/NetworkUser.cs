using System;

namespace JGeneral.IO.Net
{
    public class NetworkUser
    {
        public string Id { get; set; }
        public DateTime LastSeen { get; set; }

        public NetworkUser(string id)
        {
            Id = id;
            LastSeen = DateTime.Now;
        }
    }
}