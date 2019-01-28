using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using UnityEngine.UI;

#if !UNITY_EDITOR
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
#endif

public class DolphinManager : MonoBehaviour
{
    private Text text;
    private static string anyIp = IPAddress.Any.ToString();
    private static string dolphinIpAddr = "192.168.0.125";
    private static string thisIpAddr = "192.168.0.147";
    private static int thisPort = 22112;
    private Stack<SamEvents> eventStack;

#if UNITY_EDITOR
    private HttpListener _listener;
#endif

#if !UNITY_EDITOR
    private StreamReader reader;
    private StreamSocketListener listener;
    private Task exchangeTask;
#endif

    void Start()
    {
        eventStack = new Stack<SamEvents>();
        Invoke("SetDebugText", 2f);


#if UNITY_EDITOR
        Invoke("InitializeUnityServer", 2.5f);
#else
        InitializeUWPServer();
#endif
    }

    void Update()
    {
        if (eventStack.Count != 0)
            HandleDolphinEvent(eventStack.Pop());
    }        


#if UNITY_EDITOR
    void InitializeUnityServer()
    {
        text.text = "Im starting the server";
        StartCoroutine(HttpMessage.SendHttpChange(thisIpAddr, thisPort, dolphinIpAddr));

        _listener = new HttpListener();
        _listener.Prefixes.Add("http://+:" + thisPort.ToString() + "/");

        _listener.Start();
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
    }

    private void ListenerCallback(IAsyncResult result)
    {
        HttpListener listener = (HttpListener)result.AsyncState;
        HttpListenerContext context = listener.EndGetContext(result);

        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        string contRead = new StreamReader(request.InputStream).ReadToEnd();
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        SamEvents samEvents = new SamEvents();
        samEvents = JsonUtility.FromJson<SamEvents>(contRead);
        eventStack.Push(samEvents);
    }
#endif

#if !UNITY_EDITOR
    private async void InitializeUWPServer()
    {
        StartCoroutine(HttpMessage.SendHttpChange(thisIpAddr, thisPort, dolphinIpAddr));
        StartCoroutine(HttpMessage.SendSingleColorAllLeds(InputHandler.CurrentColor, dolphinIpAddr));

        listener = new StreamSocketListener();
        //forse non serve più: Windows.Networking.HostName serverHost = new Windows.Networking.HostName(anyIp);
        listener.ConnectionReceived += OnConnection;
        listener.Control.KeepAlive = false;

        try
        {
            await listener.BindServiceNameAsync(thisPort.ToString());
        } catch(Exception e) { text.text = e.Message; }
    }
#endif


#if !UNITY_EDITOR
    private async void OnConnection(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            text.text = "OnConnection() chiamata!";

            /*
            DataReader reader = new DataReader(args.Socket.InputStream);
            try 
            {   
                while (true)
                {
                    uint stringLength = reader.ReadUInt32();
                    uint actualStringLength = await reader.LoadAsync(stringLength);
                    text.text = "RECEIVED: " + reader.ReadString(actualStringLength);

                }
            } catch(Exception e) { text.text = e.Message; }

            */

            //reader.ReadToEnd();  per leggere 
        }
#endif

    //call methods of InputHandler according to the message received from the dolphin
    private void HandleDolphinEvent(SamEvents samEvent)
    {
        if (samEvent.events[0].typ == "touch" && samEvent.events[0].act == true)
        {
            switch (samEvent.events[0].val)
            {
                case "1": InputHandler.OnNextColor();
                          StartCoroutine(HttpMessage.SendSingleColorAllLeds(InputHandler.CurrentColor, dolphinIpAddr));
                    break;
                case "2": InputHandler.OnPreviousColor();
                          StartCoroutine(HttpMessage.SendSingleColorAllLeds(InputHandler.CurrentColor, dolphinIpAddr));
                    break;
                case "5": InputHandler.SubmitColor();
                    break;
            }
        }
    }

    void SetDebugText()
    {
        text = GameObject.Find("DEBUG_TEXT").GetComponent<Text>();
    }

        
}

