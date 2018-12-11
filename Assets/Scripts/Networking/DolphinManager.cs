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

#if !UNITY_EDITOR
using System.Threading.Tasks;
#endif

public class DolphinManager : MonoBehaviour
{
    private static string ipAddr = ""; //Dolphin IP address
    private static int dolphinPort = 0; //Dolphin port

    public delegate void ColorSubmittedEvent();
    public static event ColorSubmittedEvent OnColorSubmitted;

    public static Color CurrentDoplhinColor { get; private set; }

    public static IPAddress serverIP; //server IP address
    public static int serverPort = 60001; //server port

    byte[] myBuffer;

    private Stack<SamEvents> eventStack;


    private StreamReader reader;

    private HttpListener _listener;

#if UNITY_EDITOR
    private TcpListener server;
    private TcpClient client;
    private Thread thread;
#endif

#if !UNITY_EDITOR
    private Windows.Networking.Sockets.StreamSocket socket;
    private Task exchangeTask;
#endif


    void Start()
    {
        CurrentDoplhinColor = GameManager.PossibleColors[0];
        eventStack = new Stack<SamEvents>();
        myBuffer = new byte[1024];
#if UNITY_EDITOR
        InitializeUnityServer();
#else
        InitializeUWPServer();
#endif
    }

    void Update()
    {
        //temporary, they will substituted by dolphin inputs
        if (Input.GetKeyDown("r"))
            CurrentDoplhinColor = Color.red;

        if (Input.GetKeyDown("g"))
            CurrentDoplhinColor = Color.green;

        if (Input.GetKeyDown("b"))
            CurrentDoplhinColor = Color.blue;

        if (Input.GetKeyDown("w"))
            CurrentDoplhinColor = Color.white;

        if (Input.GetKeyDown("c"))
            CurrentDoplhinColor = Color.cyan;

        if (Input.GetKeyDown("y"))
            CurrentDoplhinColor = Color.yellow;

        if (Input.GetKeyDown("l") && GameManager.Mode == GameMode.MANUAL)
            OnNextColor();

        if (Input.GetKeyDown("k") && GameManager.Mode == GameMode.MANUAL)
            OnPreviousColor();

        if (Input.GetKeyDown(KeyCode.Space))
            OnColorSubmitted();

        if (eventStack.Count != 0)
            HandleDolphinEvent(eventStack.Pop());
    }

    //da chiamare quando il giocatore preme la pinna di destra
    void OnNextColor()
    {
        try
        {
            CurrentDoplhinColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentDoplhinColor) + 1];
        }  catch (IndexOutOfRangeException e) { CurrentDoplhinColor = GameManager.PossibleColors[0]; }

        Debug.Log("New color selected: " + CurrentDoplhinColor.ToString());

        SwitchDolphinOn();
    }

    //da chiamare quando il giocatore preme la pinna di sinistra
    void OnPreviousColor()
    {
        try
        {
            CurrentDoplhinColor = GameManager.PossibleColors[Array.IndexOf(GameManager.PossibleColors, CurrentDoplhinColor) - 1];
        }   catch (IndexOutOfRangeException e) { CurrentDoplhinColor = GameManager.PossibleColors[GameManager.PossibleColors.Length-1]; }

        Debug.Log("New color selected: " + CurrentDoplhinColor.ToString());

        SwitchDolphinOn();
    }

#if UNITY_EDITOR
    void InitializeUnityServer()
    {
        StartCoroutine(HttpMessage.SendHttpChange("127.0.0.1", 60001, "192.168.0.125"));
        serverIP = IPAddress.Parse("127.0.0.1");

        _listener = new HttpListener();
        _listener.Prefixes.Add("http://+:60000/");

        _listener.Start();

        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);

    }

    private void ListenerCallback(IAsyncResult result)
    {
        Debug.Log("Messaggio http ricevuto");
        HttpListener listener = (HttpListener)result.AsyncState;
        // Call EndGetContext to complete the asynchronous operation.
        HttpListenerContext context = listener.EndGetContext(result);
        HttpListenerRequest request = context.Request;
        // Obtain a response object.
        HttpListenerResponse response = context.Response;
        string contRead = new StreamReader(request.InputStream).ReadToEnd();
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        Debug.Log(contRead);
        SamEvents samEvents = new SamEvents();
        samEvents = JsonUtility.FromJson<SamEvents>(contRead);
        eventStack.Push(samEvents);
        Debug.Log("" + samEvents.events[0].act);
    }
#endif

#if !UNITY_EDITOR
    private async void InitializeUWPServer()
    {
        socket = new Windows.Networking.Sockets.StreamSocket();
        Windows.Networking.HostName serverHost = new Windows.Networking.HostName("127.0.0.1");
        await socket.ConnectAsync(serverHost, serverPort.ToString());
        Stream streamIn = socket.InputStream.AsStreamForRead();
        reader = new StreamReader(streamIn);
        exchangeTask = Task.Run(() => UWPServerTask());
    }
#endif

    public static void SwitchDolphinOn()
    {
        HttpMessage.SendSingleColorAllLeds(CurrentDoplhinColor, ipAddr);
    }

    public static void SwitchDolphinOff()
    {
        //spegni i led
    }

#if !UNITY_EDITOR
    public void UWPServerTask (){
        while(true){
            string received = reader.ReadLine();
            Debug.Log(received);
            JsonEvent jsonEvent = JsonEvent.ParseEventJson(received);
            eventStack.Push(jsonEvent);
        }
    }
#endif

    private void HandleDolphinEvent(SamEvents samEvent)
    {
        //da chiamare quando il giocatore preme la pinna centrale
        if (samEvent.events[0].typ == "touch" && samEvent.events[0].act == true)
        {
            switch (samEvent.events[0].val)
            {
                case "1": OnNextColor();
                    break;
                case "2": OnPreviousColor();
                    break;
                case "5": OnColorSubmitted();
                    break;
            }
        }
    }

}



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