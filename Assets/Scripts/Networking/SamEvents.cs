using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SamEvents
{
    public Evt[] events;
}

[Serializable]
public class Evt
{
    public string typ;
    public string val;
    public bool act;
    public int dur;
}
