using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEventJson : MonoBehaviour
{

    public class SecondEvent
    {
        public string typ { get; set; }
        public string val { get; set; }
        public int act { get; set; }
        public int dur { get; set; }
    }

    public class RootObject
    {
        public SecondEvent Event { get; set; }
    }
}