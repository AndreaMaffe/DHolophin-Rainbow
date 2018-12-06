using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventJson
{


    public class Event
    {
        public string type { get; set; }
        public string id { get; set; }
        public string val { get; set; }
        public string dur { get; set; }
        public string SemVal { get; set; }
    }

    public class RootObject
    {
        public Event Event { get; set; }
    }
}

