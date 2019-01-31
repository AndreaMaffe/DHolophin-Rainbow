using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//    N.B: "Send" messages must be called inside a Coroutine!
//     Example: StartCoroutine(HttpMessage.sendSingleColorAllLed(Color.red,"http://192.168.1.2:60000"));
//

public class HttpMessage
{
    public enum DolphinLed { parthead, partleftfin, partrightfin, partbelly };
    public enum DolphinMoves { moveEyes, moveMouth };
    private static string[] dolphinLed = { "parthead", "partleftfin", "partrightfin", "partbelly" };
    //  private static string[] dolphinMoves = {"moveEyes", "moveMouth"};
    //public string[] musicDetail = { "type", "track", "volume" };
    // public string[] moveDetail = { "type", "id", "direction", "speed" , "duration" };
    // public string[] changeDetail = { "ipTarget", "portTarget"};
    private static byte[] HttpMessageFormation(string requestType, string modeRequest, Dictionary<string, string> details)
    {
        string stringJson = "";
        string detail = "";
        switch (modeRequest)
        {
            case "lightControllerSetter":
                detail += SetterLedLight(details);
                stringJson += "{\"requestType\":\"" + requestType + "\",\"" + modeRequest + "\":[" + detail + "]}";
                break;

            case "soundControllerSetter":
                detail += SetterComponent(details);
                stringJson += "{\"requestType\":\"" + requestType + "\",\"" + modeRequest + "\":[{" + detail + "}]}";
                break;

            case "motorControllerSetter":
                detail += SetterComponent(details);
                stringJson += "{\"requestType\":\"" + requestType + "\",\"" + modeRequest + "\":[{" + detail + "}]}";
                break;

            case "changeHttp":
                detail += SetterComponent(details);
                stringJson += "{\"requestType\":\"" + requestType + "\"," + detail + "}";
                break;
        }

        var data = System.Text.Encoding.UTF8.GetBytes(stringJson);
        return data;
    }

    private static string AddLedComponent(string ledZone, string color)
    {
        string stringa = "{\"code\":\"" + ledZone + "\",\"color\":\"" + color + "\"}";
        return stringa;
    }

    private static string SetterLedLight(Dictionary<string, string> colorLed)
    {
        string stringa = "";
        int i = 1;
        foreach (string key in colorLed.Keys)
        {
            stringa += AddLedComponent(key, colorLed[key]);
            if (i < colorLed.Count)
            {
                stringa += ",";
            }
            i++;
        }
        return stringa;
    }
    private static string AddComponent(string field, string value)
    {
        string stringa = "\"" + field + "\":" + value;
        return stringa;
    }
    private static string SetterComponent(Dictionary<string, string> details)
    {
        string stringa = "";
        int i = 1;
        foreach (string key in details.Keys)
        {

            stringa += AddComponent(key, details[key]);
            if (i < details.Count)
            {
                stringa += ",";
            }
            i++;
        }
        return stringa;
    }
    private static byte[] CreateSingleColorAllLed(Color color)
    {
        string exaColor = ColorUtility.ToHtmlStringRGB(color);
        int i;
        Dictionary<string, string> colorLed = new Dictionary<string, string>();
        for (i = 0; i < dolphinLed.Length; i++)
        {
            colorLed.Add(dolphinLed[i], exaColor);
        }
        var data = HttpMessageFormation("set", "lightControllerSetter", colorLed);

        return data;
    }

    private static byte[] CreateOneColorForLed(Dictionary<string, Color> colorLedUnity)
    {

        Dictionary<string, string> colorLed = new Dictionary<string, string>();
        foreach (string key in colorLedUnity.Keys)
        {
            string exaColor = ColorUtility.ToHtmlStringRGB(colorLedUnity[key]);
            colorLed.Add(key, exaColor);
        }
        var data = HttpMessageFormation("set", "lightControllerSetter", colorLed);

        return data;
    }

    private static byte[] CreateMusicMessage(string track, string vol)
    {
        Dictionary<string, string> musicDetails = new Dictionary<string, string>();
        musicDetails.Add("type", "\"music\"");
        musicDetails.Add("track", "\"" + track + "\"");
        musicDetails.Add("volume", vol);
        var data = HttpMessageFormation("set", "soundControllerSetter", musicDetails);
        return data;
    }

    private static byte[] CreateMoveMessage(string idMove, string speed, string duration)
    {
        Dictionary<string, string> moveDetails = new Dictionary<string, string>();
        moveDetails.Add("type", "\"dc\"");
        moveDetails.Add("id", idMove);
        moveDetails.Add("direction", "\"cw\"");
        moveDetails.Add("speed", "\"" + speed + "\"");
        moveDetails.Add("duration", duration);
        var data = HttpMessageFormation("set", "motorControllerSetter", moveDetails);
        return data;
    }

    private static byte[] CreateHttpChange(string url, string port)
    {
        Dictionary<string, string> HttpDetails = new Dictionary<string, string>();
        HttpDetails.Add("ipTarget", "\"" + url + "\"");
        HttpDetails.Add("portTarget", port);
        var data = HttpMessageFormation("changeHttp", "changeHttp", HttpDetails);
        return data;
    }
    public static IEnumerator SendSingleColorAllLeds(Color color, string urlTarget)
    {
        Dictionary<string, string> headerD = new Dictionary<string, string>();
        headerD.Add("Content-Type", "application/json");
        WWW request = new WWW(urlTarget, CreateSingleColorAllLed(color), headerD);
        yield return request;
    }

    //insert part of the dolphin as keys ["parthead", "partleftfin", "partrightfin", "partbelly"]
    public static IEnumerator SendSingleColorSingleLed(Dictionary<DolphinLed, Color> colorLedUnityInput, string urlTarget)
    {
        Dictionary<string, string> headerD = new Dictionary<string, string>();
        Dictionary<string, Color> colorLedUnity = new Dictionary<string, Color>();
        foreach (DolphinLed key in colorLedUnityInput.Keys)
        {
            colorLedUnity.Add(key.ToString(), colorLedUnityInput[key]);
        }
        headerD.Add("Content-Type", "application/json");
        WWW request = new WWW(urlTarget, CreateOneColorForLed(colorLedUnity), headerD);
        yield return request;
    }

    public static IEnumerator SendMusicMessage(string track, string urlTarget)
    {
        Dictionary<string, string> headerD = new Dictionary<string, string>();
        headerD.Add("Content-Type", "application/json");
        WWW request = new WWW(urlTarget, CreateMusicMessage(track,"20"), headerD);
        yield return request;
    }

    //idMove=1(muove occhi),2(muove bocca)
    public static IEnumerator SendMoveMessage(DolphinMoves moves, string urlTarget)
    {
        string idMove = "";
        switch (moves)
        {
            case DolphinMoves.moveEyes:
                idMove = "1";
                break;
            case DolphinMoves.moveMouth:
                idMove = "2";
                break;

        }
        Dictionary<string, string> headerD = new Dictionary<string, string>();
        headerD.Add("Content-Type", "application/json");
        WWW request = new WWW(urlTarget, CreateMoveMessage(idMove, "90", "500"), headerD);
        yield return request;
    }

    public static IEnumerator SendHttpChange(string url, int port, string urlTarget)
    {
        Dictionary<string, string> headerD = new Dictionary<string, string>();
        headerD.Add("Content-Type", "application/json");
        WWW request = new WWW(urlTarget, CreateHttpChange(url, port.ToString()), headerD);
        yield return request;
    }
}