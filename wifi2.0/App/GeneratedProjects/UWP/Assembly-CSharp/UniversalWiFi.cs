using Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;


namespace Assets
{
    public class wifis
    {
        public uint Signal { get; set; }
        public string Mac { get; set; }
        public string Ssid { get; set; }
        public double RSSI { get; set; }
        public wifis()
        {
            Mac = string.Empty;
            Ssid = string.Empty;
            Signal = 0u;
            RSSI = 0;

        }
    }
    public class UniversalWiFi : IWiFiAdapter<wifis>
    {
        private bool IsReady { get; set; }
        private uint Signal { get; set; }
        private string Report { get; set; }
        private string Ssid { get; set; }
        private double RSSI { get; set; }
        List<wifis> networks = new List<wifis>();
        public UniversalWiFi()
        {
            IsReady = true;
            Signal = 0u;
            Ssid = string.Empty;
            Report = string.Empty;
            RSSI = 0;
        }
        public uint GetSignal(string ssid)
        {
            Ssid = ssid;
            Scan();
            return Signal;
        }
        public double getRSSI()
        {
            Scan();
            return RSSI;
        }

        private async void Scan()
        {
            if (IsReady)
            {
                IsReady = false;
                uint signal = 0;
                var result = await WiFiAdapter.FindAllAdaptersAsync();
                if (result.Count >= 1)
                {
                    var firstAdapter = result[0];
                    await firstAdapter.ScanAsync();
                    GenerateNetworkReport(firstAdapter.NetworkReport);
                    if (!string.IsNullOrEmpty(Ssid))
                    {
                        signal = GetNetworkSignal(firstAdapter.NetworkReport, Ssid);
                       // RSSI = getRSSI123(firstAdapter.NetworkReport, Ssid);
                    }
                }
                IsReady = true;
                Signal = signal;
            }
        }

        private byte GetNetworkSignal(WiFiNetworkReport report, string ssid)
        {
            var network = report.AvailableNetworks.Where(x => x.Ssid.ToLower() == ssid.ToLower()).FirstOrDefault();
            return network.SignalBars;
        }
        private double getRSSI123(WiFiNetworkReport report, string ssid)
        {
            var network = report.AvailableNetworks.Where(x => x.Ssid.ToLower() == ssid.ToLower()).FirstOrDefault();
            return network.NetworkRssiInDecibelMilliwatts;
        }

        private void GenerateNetworkReport(WiFiNetworkReport report)
        {
            var net = new List<string>();
            foreach (var network in report.AvailableNetworks)
            {
                wifis a = new wifis();
                a.Ssid = network.Ssid;
                a.Signal = network.SignalBars;
                a.RSSI = network.NetworkRssiInDecibelMilliwatts;
                a.Mac = network.Bssid;
                networks.Add(a);
                net.Add(string.Format("SSID: {0} -- SignalBars: {1} -- Db: {2} -- Mac: {3}",
                    network.Ssid, network.SignalBars, network.NetworkRssiInDecibelMilliwatts, network.Bssid));
               
            }
            
            Report = string.Join(Environment.NewLine, net.ToArray());
        }
        public List<wifis> getNetworks()
        {
            Scan();
            return networks;
        }
        public string GetNetworkReport()
        {
            Scan();
            return Report;
        }
    }
}