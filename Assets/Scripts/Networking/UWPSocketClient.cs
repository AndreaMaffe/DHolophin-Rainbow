using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#if !UNITY_EDITOR
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
    using System.Threading.Tasks;
#endif


public class UWPSocketClient : MonoBehaviour {

    private Text text;

#if !UNITY_EDITOR
        private StreamSocket socket;
        private String ip = "192.168.0.173";
        private String port = "60000";
        private String message = "ciao";
        private StreamWriter writer = null;
        private StreamReader reader = null;

        private bool connected;
#endif

    void Start ()
    {
        text = GameObject.Find("DEBUG_TEXT").GetComponent<Text>();
        Invoke("Connect", 7f);		
	}

    public void Connect()
    {
#if !UNITY_EDITOR
        Task.Run(async () => {
            try
            {
                socket = new StreamSocket();
                await socket.ConnectAsync(new HostName(ip), port);
                writer = new StreamWriter(socket.OutputStream.AsStreamForWrite());
                reader = new StreamReader(socket.InputStream.AsStreamForRead());
                connected = true;
                text.text = "connesso!";
                //await Send("ciao");
                await Receive();
                
            }
            catch(Exception e)
            {
                text.text = e.Message;
            }
        });
#endif
    }

#if !UNITY_EDITOR
        public async Task Send(string message)
        {
            try
            {
                await writer.WriteAsync(message);
                await writer.FlushAsync();
                text.text = "Sent message: " + message;
            }
            catch(Exception e)
            {
               text.text = e.Message;
            }
        }
#endif


#if !UNITY_EDITOR
    private async Task Receive()
    {
            int i=0;

            while (true)
            {
                text.text = "tentativo " + i;
                try
                {
                    char[] buffer = new char[250];
                    int bytes = await reader.ReadAsync(buffer, 0, buffer.Length);

                    if (bytes > 0)
                    {
                        string result = new string(buffer, 0, bytes);
                        message = result;

                        text.text = "Received: " + message;
                    }
                }
                catch (Exception e)
                {
                    text.text = e.Message;
                }

                i++;
            }
    }
#endif

}
