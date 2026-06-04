namespace SlothSecIpCheckerWeb.Models
{
    public class ReportLogEntry
    {
        public string Ip { get; set; } = "";
        public int Category { get; set; }
        public string Comment { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
}
