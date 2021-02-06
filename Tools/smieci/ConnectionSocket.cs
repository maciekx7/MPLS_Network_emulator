using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Tools
{
    public class ConnectionSocket
    {
        public ConnectionSocket()
        {
        }

        public static void SocketListener(String ip, int port)
        {
            TcpListener server = null;
            try
            {
                IPAddress serverAddress = IPAddress.Parse(ip);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(serverAddress, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received data: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.

                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent data: {0}", data);

                    }
                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to exit listener...");
            Console.Read();
        }




        public static void SocketSender(String serverIP, int serverPort, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                TcpClient client = new TcpClient(serverIP, serverPort);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent data: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new byte[256];

                // String to store the response ASCII representation.
                String respondData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                int bytes = stream.Read(data, 0, data.Length);
                respondData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received data: {0}", respondData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            //Console.WriteLine("\nHit enter to continue...");
            //Console.Read();
        }
    }
}
