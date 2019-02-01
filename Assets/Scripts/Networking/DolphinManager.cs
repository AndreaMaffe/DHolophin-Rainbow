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
    using Windows.Networking.Connectivity;
    using Windows.Foundation.Diagnostics;
#endif

public class DolphinManager : MonoBehaviour
{
    private Text text;
    private static string anyIp = IPAddress.Any.ToString();
    private static string dolphinIpAddr = "192.168.0.125"; //177 per Phil 125 per il cartone
    private static string holoLensIpAddr = "192.168.0.147";
    private static string unityIpAddr = "192.168.0.173";
    private static Uri uri;
    private static int unityPort = 60000;
    private static int holoLensPort = 9000;
    private Stack<SamEvents> eventStack;

#if UNITY_EDITOR
    private HttpListener _listener;
#endif

#if !UNITY_EDITOR
    private DatagramSocket socket;
    private StreamReader reader;
    private StreamSocketListener listener;
    //private Task exchangeTask;
    private MessageWebSocket messageWebSocket;
#endif

    void Start()
    {
        uri = new Uri("wss://192.168.0.173");
        eventStack = new Stack<SamEvents>();
        Invoke("SetDebugText", 2f);


#if UNITY_EDITOR
        Invoke("InitializeUnityServer", 4f);

#else
        //Invoke("InitializeUWPServer", 4f);
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
        text.text = "Im starting the Unity server!";

        Invoke("connect", 4f);
        //StartCoroutine(HttpMessage.SendSingleColorAllLeds(Color.yellow, dolphinIpAddr));

        /*
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://+:" + unityPort.ToString() + "/");

        _listener.Start();
        
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        */
        
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
            text.text = "Im starting the HoloLens server!";
            
            /*
            try 
            {
                Invoke("connect", 4f);
                StartCoroutine(HttpMessage.SendSingleColorAllLeds(Color.yellow, dolphinIpAddr));

                socket = new DatagramSocket();
                socket.MessageReceived += Socket_MessageReceived;

                listener.Control.KeepAlive = true;
                await socket.BindEndpointAsync(new HostName(holoLensIpAddr.ToString()), holoLensPort.ToString());
                text.text = "bindato";

                await SendMessage("ciao");
            }

            catch (Exception e) {  text.text = e.Message;  }
            */

    
            /*
            try
            {
                Invoke("connect", 4f);
                StartCoroutine(HttpMessage.SendSingleColorAllLeds(Color.green, dolphinIpAddr));

                listener = new StreamSocketListener();
                listener.ConnectionReceived += Listener_ConnectionReceived;
                await listener.BindServiceNameAsync(holoLensPort.ToString());

            } catch(Exception e) { text.text = e.Message; }
            */

            /*
            try
            {
                messageWebSocket = new MessageWebSocket();
                messageWebSocket.Control.MessageType = SocketMessageType.Utf8;
                messageWebSocket.MessageReceived += MessageReceived;
                await messageWebSocket.ConnectAsync(uri);
                text.text = "CONNESSO!";
            }
            catch (Exception e) // For debugging
            {
                text.text = e.Message;
            }
            */
    }
#endif


#if !UNITY_EDITOR
    private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            text.text = "messaggio ricevuto";

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


    private async System.Threading.Tasks.Task SendMessage(string message)
    {
        using (var stream = await socket.GetOutputStreamAsync(new Windows.Networking.HostName(unityIpAddr), unityPort.ToString()))
        {
            using (var writer = new Windows.Storage.Streams.DataWriter(stream))
            {
                var data = Encoding.UTF8.GetBytes(message);

                writer.WriteBytes(data);
                await writer.StoreAsync();
               text.text = "Sent: " + message;
            }
        }
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

    private void connect()
    {
        StartCoroutine(HttpMessage.SendHttpChange(unityIpAddr, unityPort, dolphinIpAddr));
    }



#if !UNITY_EDITOR
    private void Socket_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender,
        Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
    {
        text.text = "Messaggio ricevuto!";
    }

     private void MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
           text.text = "messaggio ricevuto";
        }


#endif
}

