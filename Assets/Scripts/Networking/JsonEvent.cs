﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonEvent
{
    public int dur { get; set; }
    public string typ { get; set; }
    public string val { get; set; }
    public int act { get; set; }

    public JsonEvent(int dur, string typ, string val, int act)
    {
        this.dur = dur;
        this.typ = typ;
        this.val = val;
        this.act = act;
    }

    public JsonEvent(string typ, string val, int act)
    {
        this.typ = typ;
        this.val = val;
        this.act = act;
        this.dur = 0;
    }

    public static JsonEvent ParseEventJson(string JsonString)
    {
        try
        {
            JsonEvent jsonEvent;

            int beginIndex = JsonString.IndexOf("\"typ\"") + 7;
            int endIndex = JsonString.IndexOf("\"val\"") - 2;
            string eventTyp = JsonString.Substring(beginIndex, endIndex - beginIndex);

            beginIndex = JsonString.IndexOf("\"val\"") + 7;
            endIndex = JsonString.IndexOf("\"act\"") - 2;
            string eventVal = JsonString.Substring(beginIndex, endIndex - beginIndex);

            beginIndex = JsonString.IndexOf("\"act\"") + 6;
            endIndex = JsonString.IndexOf("}]}");
            string eventAct = JsonString.Substring(beginIndex, endIndex - beginIndex);
            jsonEvent = new JsonEvent(eventTyp, eventVal, int.Parse(eventAct));


            if (JsonString.Contains("dur"))
            {
                beginIndex = JsonString.IndexOf("\"dur\"") + 6;
                endIndex = JsonString.IndexOf("\"typ\"") - 1;
                string eventDur = JsonString.Substring(beginIndex, endIndex - beginIndex);

                jsonEvent = new JsonEvent(int.Parse(eventDur), eventTyp, eventVal, int.Parse(eventAct));

            }

            return jsonEvent;
        }
        catch (Exception e) { Debug.Log(e.ToString()); }

        return null;
    }


}

