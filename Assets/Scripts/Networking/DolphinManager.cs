using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;


public class DolphinManager : MonoBehaviour
{
    private static string ipAddr = ""; //Dolphin IP address
    private static int dolphinPort = 0; //Dolphin port

    public delegate void ColorSubmittedEvent();
    public static event ColorSubmittedEvent OnColorSubmitted;

    public static Color CurrentDoplhinColor { get; private set; }

    public static IPAddress serverIP; //server IP address
    public static int serverPort = 60001; //server port
    
    private TcpListener server;
    private TcpClient client;
    private Thread thread;

    private Stack<JsonEvent> eventStack;    


    void Start()
    {
        CurrentDoplhinColor = GameManager.PossibleColors[0];
        eventStack = new Stack<JsonEvent>();

        InitializeServer();        
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

    void InitializeServer()
    {
        StartCoroutine(HttpMessage.SendHttpChange("192.168.0.141", 60001, "192.168.0.125"));
        serverIP = IPAddress.Parse(Network.player.ipAddress);
        server = new TcpListener(serverIP, serverPort);
        client = default(TcpClient);

        try
        {
            server.Start();
            Debug.Log("Server running on port " + serverPort);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        ThreadStart ts = new ThreadStart(ServerThread);
        thread = new Thread(ts);
        thread.Start();
    }

    public static void SwitchDolphinOn()
    {
        HttpMessage.SendSingleColorAllLeds(CurrentDoplhinColor, ipAddr);
    }

    public static void SwitchDolphinOff()
    {
        //spegni i led
    }

    void ServerThread()
    {
        while (true)
        {
            client = server.AcceptTcpClient();
            byte[] myBuffer = new byte[100000];
            NetworkStream stream = client.GetStream();

            stream.Read(myBuffer, 0, myBuffer.Length);

            string message = Encoding.ASCII.GetString(myBuffer, 0, myBuffer.Length);

            Debug.Log(""+message);

            JsonEvent jsonEvent = JsonEvent.ParseEventJson(message);
            eventStack.Push(jsonEvent);            
        }        
    }

    private void HandleDolphinEvent(JsonEvent jsonEvent)
    {
        //da chiamare quando il giocatore preme la pinna centrale
        if (jsonEvent.typ == "touch" && jsonEvent.act == 1)
        {
            switch (jsonEvent.val)
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
