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
    private static int port = 0; //Dolphin port

    public delegate void ColorSubmittedEvent();
    public static event ColorSubmittedEvent OnColorSubmitted;

    public static Color CurrentDoplhinColor { get; private set; }

    public static IPAddress serverIP; //server IP address
    public static int serverPort = 60002; //server port

    
    private TcpListener server;
    private TcpClient client;
    private Thread thread;
    


    void Start()
    {
        CurrentDoplhinColor = GameManager.PossibleColors[0];

        //Server initialization
        
        serverIP = IPAddress.Parse(Network.player.ipAddress);
        server = new TcpListener(serverIP, serverPort);
        client = default(TcpClient);
        
        try
        {
            server.Start();
            Debug.Log("Server started");
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        ThreadStart ts = new ThreadStart(ServerThread);
        thread = new Thread(ts);
        thread.Start();
        
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


    //da chiamare quando il giocatore preme la pinna centrale
    // OnColorSubmitted();

    //da usare quando ci si connette al delfino
    public static bool EstablishCommunication()
    {
        return true;
    }

    public static void SwitchDolphinOn()
    {
        //HttpMessage.SendSingleColorAllLeds(CurrentDoplhinColor, ipAddr);
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
            byte[] myBuffer = new byte[1024];
            NetworkStream stream = client.GetStream();

            stream.Read(myBuffer, 0, myBuffer.Length);

            string message = Encoding.ASCII.GetString(myBuffer, 0, myBuffer.Length);

            JsonEvent jsonEvent = parseEventJson(message);
        }        
    }




    
    JsonEvent parseEventJson(string JsonString)
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
}
