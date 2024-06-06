using OpenLabel.Shared;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace OpenLabel.Printing
{
    /// <summary>
    /// Handles sending ZPL commands to a network-connected Zebra printer.
    /// </summary>
    public class NetworkPrinter
    {
        /// <summary>
        /// Sends ZPL data to a Zebra network printer asynchronously.
        /// </summary>
        /// <param name="printerPath">The UNC path or hostname of the printer.</param>
        /// <param name="labelCount">The number of labels to print.</param>
        /// <param name="zplString">The ZPL code to send to the printer.</param>
        /// <returns>
        /// A <see cref="Result{T, E}"/> where:
        /// - <c>Ok(true)</c> indicates successful printing.
        /// - <c>Err(string)</c> contains an error message if printing fails.
        /// </returns>
        public async Task<Result<bool, string>> PrintLabelAsync(string printerPath, int labelCount, string zplString)
        {
            var ipResult = GetPrinterIpAddress(printerPath);
            if (ipResult.IsFailure)
            {
                return Result<bool, string>.Err(ipResult.Error);
            }

            using TcpClient client = new TcpClient();
            var connectTask = client.ConnectAsync(ipResult.Value, 9100);

            if (!connectTask.Wait(TimeSpan.FromSeconds(5)))
            {
                return Result<bool, string>.Err($"Timeout: Unable to connect to printer '{printerPath}'.");
            }

            using StreamWriter writer = new StreamWriter(client.GetStream());

            for (int i = 0; i < labelCount; i++)
            {
                await writer.WriteAsync(zplString);
                await writer.FlushAsync();
            }

            return Result<bool, string>.Ok(true);
        }

        /// <summary>
        /// Resolves the printer's hostname to an IP address.
        /// </summary>
        /// <param name="printerPath">The UNC path or hostname of the printer.</param>
        /// <returns>
        /// A <see cref="Result{T, E}"/> where:
        /// - <c>Ok(string)</c> contains the printer's IP address.
        /// - <c>Err(string)</c> contains an error message if resolution fails.
        /// </returns>
        private Result<string, string> GetPrinterIpAddress(string printerPath)
        {
            string printerName = printerPath.Split('\\')[3];
            IPAddress[] addresses = Dns.GetHostAddresses(printerName);

            if (addresses.Length == 0)
            {
                return Result<string, string>.Err($"Printer '{printerName}' IP address not found.");
            }

            string ipAddress = addresses[0].ToString();
            Result<bool, string> reachabilityResult = IsPrinterReachable(ipAddress);

            if (reachabilityResult.IsFailure)
            {
                return Result<string, string>.Err($"Printer '{printerName}' at IP address '{ipAddress}' is not reachable.");
            }

            return Result<string, string>.Ok(ipAddress);
        }

        /// <summary>
        /// Checks whether the given printer IP address is reachable using a ping.
        /// </summary>
        /// <param name="ipAddress">The IP address of the printer.</param>
        /// <returns>
        /// A <see cref="Result{T, E}"/> where:
        /// - <c>Ok(true)</c> indicates the printer is reachable.
        /// - <c>Err(string)</c> contains an error message if the printer is unreachable.
        /// </returns>
        private Result<bool, string> IsPrinterReachable(string ipAddress)
        {
            using Ping ping = new Ping();
            PingReply reply = ping.Send(ipAddress);

            return reply.Status == IPStatus.Success
                ? Result<bool, string>.Ok(true)
                : Result<bool, string>.Err($"Printer at IP '{ipAddress}' is unreachable (Status: {reply.Status}).");
        }
    }
}
