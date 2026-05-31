public class AbuseIpResult
{
    public AbuseIpData Data { get; set; }
}

public class AbuseIpData
{
    public string IpAddress { get; set; }
    public int AbuseConfidenceScore { get; set; }
    public int TotalReports { get; set; }
    public int NumDistinctUsers { get; set; }
    public string CountryCode { get; set; }
    public string Domain { get; set; }
    public bool IsWhitelisted { get; set; }
}
