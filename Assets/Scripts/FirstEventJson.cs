using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEventJson
{

    public class FirstEvent
    {
        public string typ { get; set; }
        public string val { get; set; }
        public int act { get; set; }
    }

    public class RootObject
    {
        public FirstEvent events { get; set; }
    }

}