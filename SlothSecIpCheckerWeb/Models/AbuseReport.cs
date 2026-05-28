namespace SlothSecIpCheckerWeb.Models;

public class AbuseIpReport
{
    public Data data { get; set; } = new();
}

public class Data
{
    public string ipAddress { get; set; } = "";
    public int abuseConfidenceScore { get; set; }
    public string isp { get; set; } = "";
    public string domain { get; set; } = "";
    public string usageType { get; set; } = "";
    public string countryCode { get; set; } = "";
    public List<string> hostnames { get; set; } = new();
    public int totalReports { get; set; }
    public int numDistinctUsers { get; set; }
    public string lastReportedAt { get; set; } = "";
}

