using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonEvent
{
    public string typ { get; set; }
    public string val { get; set; }
    public int act { get; set; }
    public int dur { get; set; }

    public JsonEvent(string typ, string val, int act, int dur)
    {
        this.typ = typ;
        this.val = val;
        this.act = act;
        this.dur = dur;
    }

    public JsonEvent(string typ, string val, int act)
    {
        this.typ = typ;
        this.val = val;
        this.act = act;
        this.dur = 0;
    }


}

