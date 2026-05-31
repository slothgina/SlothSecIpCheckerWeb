public interface IAbuseIpService
{
    Task<AbuseIpResult?> CheckIpAsync(string ip);
}
