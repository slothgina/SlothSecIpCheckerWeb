using System.Net;

namespace SlothSecIpCheckerWeb.Helpers;

public static class IpClassifier
{
    public static string ClassifyIp(IPAddress ip)
    {
        if (ip == null)
            return "Unknown";

        // IPv4 classification
        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            byte[] bytes = ip.GetAddressBytes();

            // Loopback 127.0.0.0/8
            if (bytes[0] == 127)
                return "Loopback (IPv4)";

            // Private ranges
            if (bytes[0] == 10)
                return "Private (10.0.0.0/8)";
            if (bytes[0] == 172 && (bytes[1] >= 16 && bytes[1] <= 31))
                return "Private (172.16.0.0/12)";
            if (bytes[0] == 192 && bytes[1] == 168)
                return "Private (192.168.0.0/16)";

            return "Public IPv4";
        }

        // IPv6 classification
        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
        {
            byte[] bytes = ip.GetAddressBytes();

            // Loopback ::1
            if (ip.Equals(IPAddress.IPv6Loopback))
                return "Loopback IPv6 (::1)";

            // Link-local fe80::/10
            if ((bytes[0] == 0xFE) && ((bytes[1] & 0xC0) == 0x80))
                return "Link-Local IPv6 (fe80::/10)";

            // Multicast ff00::/8
            if (bytes[0] == 0xFF)
                return "Multicast IPv6 (ff00::/8)";

            // Unique Local Address (ULA) fc00::/7
            if ((bytes[0] & 0xFE) == 0xFC)
                return "Unique Local IPv6 (fc00::/7)";

            return "Global Unicast IPv6";
        }

        return "Unknown Address Type";
    }
}


