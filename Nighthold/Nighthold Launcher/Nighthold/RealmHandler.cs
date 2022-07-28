using System;
using System.Net.Sockets;
using System.Threading;

namespace Nighthold_Launcher.Nighthold
{
    class RealmHandler
    {
        public static bool GetRealmStatus(string realmaddress, int realmport, int sleepTime)
        {
            // status
            bool status = false;

            try
            {
                DateTime startTime, endTime;

                startTime = DateTime.Now;

                if (TryPingWorldServer(realmaddress, realmport, sleepTime))
                {
                    endTime = DateTime.Now;

                    double elapsedMillisecs = (endTime - startTime).TotalMilliseconds;

                    status = true;
                }
            }
            catch
            {

            }
            finally
            {
                Thread.Sleep(sleepTime);
            }

            return status;
        }

        private static bool TryPingWorldServer(string strIpAddress, int intPort, int nTimeoutMsec)
        {
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);

                IAsyncResult result = socket.BeginConnect(strIpAddress, intPort, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(nTimeoutMsec, true);

                return socket.Connected;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (null != socket)
                    socket.Close();
            }
        }
    }
}
