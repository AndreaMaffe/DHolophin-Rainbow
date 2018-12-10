using System;
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

            if (JsonString.Contains("dur"))
            {
                beginIndex = JsonString.IndexOf("\"act\"") + 6;
                endIndex = JsonString.IndexOf("\"dur\"") - 1;
                string eventAct = JsonString.Substring(beginIndex, endIndex - beginIndex);

                beginIndex = JsonString.IndexOf("\"dur\"") + 6;
                endIndex = JsonString.IndexOf("}]}");
                string eventDur = JsonString.Substring(beginIndex, endIndex - beginIndex);

                jsonEvent = new JsonEvent(eventTyp, eventVal, int.Parse(eventAct), int.Parse(eventDur));

            }
            else
            {
                beginIndex = JsonString.IndexOf("\"act\"") + 6;
                endIndex = JsonString.IndexOf("}]}");
                string eventAct = JsonString.Substring(beginIndex, endIndex - beginIndex);
                jsonEvent = new JsonEvent(eventTyp, eventVal, int.Parse(eventAct));

            }

            return jsonEvent;
        }
        catch (Exception e) { Debug.Log(e.ToString()); }

        return null;
    }


}

