using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TraktTVUpdateClient
{
    class VLCConnection
    {
        public int port { get; set; }
        public TcpClient client { get; set; }        
        public StreamWriter writeStream;

        public VLCConnection(int _port)
        {
            client = new TcpClient();
            client.Connect("localhost", 2150);            
            writeStream = new StreamWriter(client.GetStream());
            read();
        }

        public async Task write(String msg)
        {
            await writeStream.WriteLineAsync(msg);
            await writeStream.FlushAsync();
        }

        public async Task<string> read()
        {
            if (client.Connected)
            {
                NetworkStream stream = client.GetStream();
                {
                    byte[] data = new byte[1024];
                    StringBuilder completeMessage = new StringBuilder();
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = await stream.ReadAsync(data, 0, data.Length);
                        completeMessage.AppendFormat("{0}", Encoding.UTF8.GetString(data, 0, data.Length));
                    } while (stream.DataAvailable);
                    return Regex.Replace(completeMessage.ToString(), @"([\S\s]*)> ([\S\s]*)", "$1");
                }
            }
            return String.Empty;
        }
    }
}
