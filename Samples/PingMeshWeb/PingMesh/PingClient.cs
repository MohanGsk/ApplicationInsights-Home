using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Timers;

namespace PingMesh
{
    public class PingClient : IDisposable
    {
        internal static HashSet<PingTarget> PingTargets = new HashSet<PingTarget>();

        public void SubmitEndpointToTargetList(string endpoint)
        {

            try
            {
                PingTarget target = new PingTarget();
                Uri _uri = new Uri(endpoint);
                target.Uri = _uri.Host;
                target.Port = _uri.Port;

                if (PingTargets.Contains(target))
                { return; }

                lock (PingTargets)
                {
                    PingTargets.Add(target);
                }
            }
            catch (Exception exception)
            {
                LogException(exception);
            }
        }

        public PingClient()
        {
            try
            {
                //TODO: hard coded timer to 30 seconds. Move this to config in AI.config for extension. 
                m_timer = new Timer(Convert.ToInt32(30 * 1000));  
                m_timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
                m_timer.Enabled = true;
            }
            catch (Exception exception)
            {
                LogException(exception);
            }
        }

        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                StartPingMeshTest();
            }
            catch (Exception exception)
            {
                LogException(exception);
                m_timer.Enabled = false;
            }
        }

        private void StartPingMeshTest()
        {
            try
            {
                if (true)
                {
                    var pingTasks = new List<Task>();

                    foreach (PingTarget pingTarget in PingTargets)
                    {
                        pingTasks.Add(Task.Factory.StartNew(() 
                            => RunPingTarget(pingTarget.Uri, pingTarget.Port)));
                    }

                    Task.WaitAll(pingTasks.ToArray());
                }
            }
            catch (Exception exception)
            {
                LogException(exception);
                throw;
            }
        }

        private void RunPingTarget(string DestinationHost, int port)
        {

            Stopwatch timeKeeper = new Stopwatch();
            IPAddress lastKnownAddress = null;
            IPAddress resolvedAddress = null;
            bool isDestinationAnIpAddress = false;

            DateTime TimeStamp = DateTime.UtcNow;
            int dnsResult = 0;
            long dnsDuration = 0;
            int tcpResult = 0;
            long tcpDuration = 0;

            isDestinationAnIpAddress = IPAddress.TryParse(DestinationHost, out lastKnownAddress);

            // DNS lookup
            if (isDestinationAnIpAddress)
            {
                dnsResult = 1;
                dnsDuration = 0;
            }
            else
            {
                TimeStamp = DateTime.UtcNow;
                timeKeeper.Start();
                resolvedAddress = null;

                try
                {
                    foreach (IPAddress Address in Dns.GetHostEntry(DestinationHost).AddressList)
                    {
                        // Check if it is v4
                        if (Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            resolvedAddress = Address;
                        }
                    }

                    if (resolvedAddress != null)
                    {
                        lastKnownAddress = resolvedAddress;
                    }
                    dnsResult = 1;
                }
                catch
                {
                    dnsResult = 0;
                    dnsDuration = 0;
                }
                timeKeeper.Stop();
                dnsDuration = timeKeeper.ElapsedMilliseconds;
                timeKeeper.Reset();
            }

            // TCP Lookup
            timeKeeper.Start();
            try
            {
                using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    // Connect to configured TCP port
                    var asyncResult = s.BeginConnect(new IPEndPoint(lastKnownAddress, port), (result) => { return; }, null);
                    try
                    {
                        asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
                        if (!s.Connected)
                        {
                            tcpResult = 0;
                            tcpDuration = 0;
                        }
                        else
                        {
                            tcpResult = 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                        tcpResult = 0;
                        tcpDuration = 0;
                    }
                    finally
                    {
                        if (s.Connected)
                        {
                            s.Disconnect(true);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogException(exception);
                tcpResult = 0;
                tcpDuration = 0;
            }

            timeKeeper.Stop();
            tcpDuration = timeKeeper.ElapsedMilliseconds;
            timeKeeper.Reset();

            /* Populate DNSLookup Result */
            PingResult pDnsResult = new PingResult();
            pDnsResult.EndPoint = DestinationHost + "_" + port.ToString();
            pDnsResult.IsSuccess = dnsResult;
            pDnsResult.Value = dnsDuration;

            /* Populate TCPLookup Result */
            PingResult pTcpResult = new PingResult();
            pTcpResult.EndPoint = DestinationHost + "_" + port.ToString();
            pTcpResult.IsSuccess = tcpResult;
            pTcpResult.Value = tcpDuration;

            /* Push data to Application Insights */
            LogPingResult(pDnsResult, pTcpResult);
        }

        private static bool LogException(Exception message)
        {
            var telemetry = new TelemetryClient();
            bool result = true;

            // Write AI exception for debugging 
            try
            {
                telemetry.TrackException(message);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        private static void LogPingResult(PingResult dnsResult, PingResult tcpResult)
        {
            //Submit metrics to AppInsights for outcome (success=1 / failure=0) and response time for Dns lookup and Tcp port ping
            var telemetry = new TelemetryClient();
            telemetry.TrackMetric("NetDnsHealth_" + dnsResult.EndPoint.Replace(":", "_") ,dnsResult.IsSuccess);
            telemetry.TrackMetric("NetDnsTime_"   + dnsResult.EndPoint.Replace(":", "_") ,dnsResult.Value);
            telemetry.TrackMetric("NetTcpHealth_" + tcpResult.EndPoint.Replace(":", " ") ,tcpResult.IsSuccess);
            telemetry.TrackMetric("NetTcpTime_"   + tcpResult.EndPoint.Replace(":", "_") ,tcpResult.Value);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (m_timer != null)
                    {
                        m_timer.Dispose();
                        m_timer = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        internal bool PingMeshEnabled { get; private set; }
        internal Timer m_timer;

    }
}
