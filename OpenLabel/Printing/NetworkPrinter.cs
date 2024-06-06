using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace OpenLabel.Printing
{
    internal class NetworkPrinter
    {
        public async Task PrintLabelAsync(string printerPath, int labelCount, string zplString)
        {
            try
            {
                string printerIpAddress = GetPrinterIpAddress(printerPath);

                using TcpClient client = new TcpClient(printerIpAddress, 9100);
                using StreamWriter writer = new StreamWriter(client.GetStream());

                for (int i = 0; i < labelCount; i++)
                {
                    await writer.WriteAsync(zplString);
                    await writer.FlushAsync();
                }
            }
            catch (SocketException ex)
            {
                throw new ApplicationException($"Error printing label to printer '{printerPath}': Network error occurred.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error printing label to printer '{printerPath}': {ex.Message}", ex);
            }
        }

        private string GetPrinterIpAddress(string printerPath)
        {
            string printerName = printerPath.Split('\\')[3];
            IPAddress[] addresses = Dns.GetHostAddresses(printerName);
            if (addresses.Length == 0)
            {
                throw new ApplicationException($"Printer '{printerName}' IP address not found.");
            }

            string ipAddress = addresses[0].ToString();

            if (!IsPrinterReachable(ipAddress))
            {
                throw new ApplicationException($"Printer '{printerName}' at IP address '{ipAddress}' is not reachable.");
            }

            return ipAddress;
        }

        private bool IsPrinterReachable(string ipAddress)
        {
            try
            {
                using Ping ping = new Ping();
                PingReply reply = ping.Send(ipAddress);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
