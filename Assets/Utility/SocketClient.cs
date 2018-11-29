// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

/*
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace HoloToolkit.Unity.InputModule.Tests
{
    public class SocketClient : MonoBehaviour, IInputClickHandler
    { 
        private const String PUBLIC_IP_SONDRIO = "93.41.244.249";
        private const String PUBLIC_IP_MILANO = "93.70.3.224";
        private const String LOCAL_IP_1 = "127.0.0.1";
        private const String LOCAL_IP_2 = "192.168.1.5";
        private const int SERVER_PORT = 60000;

        private TcpClient client;
        private NetworkStream stream;
        private Byte[] bytes;

        private StreamWriter writer;

        void Start()
        {
            client = new TcpClient(LOCAL_IP_2, SERVER_PORT);
            stream = client.GetStream();
            writer = new StreamWriter(stream) { AutoFlush = true };
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            // Increase the scale of the object just as a response.
            gameObject.transform.localScale += 0.05f * gameObject.transform.localScale;

            eventData.Use(); // Mark the event as used, so it doesn't fall through to other handlers.

            writer.WriteLine("Current scale of the object: " + gameObject.transform.localScale);
        }

        void OnDestroy()
        {
            stream.Close();
            client.Close();
            writer.Close();

            stream = null;
        }
    }
}*/