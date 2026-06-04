using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using SlothSecIpCheckerWeb.Models;
using SlothSecIpCheckerWeb.Helpers;

public class HomeController : Controller
{
    private readonly HttpClient _http;

    public HomeController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ipchecker");
    }

    public IActionResult Index()
    {
        return View(new AbuseIpReport());
    }

    [HttpPost]
    public async Task<IActionResult> CheckIp(string ip)
    {
        try
        {
            // Validate IPv4 + IPv6
            if (!IPAddress.TryParse(ip, out _))
            {
                ViewBag.Error = "Invalid IPv4 or IPv6 address.";
                return View("Index", new AbuseIpReport());
            }

            // Parse + classify BEFORE encoding
            var ipObj = IPAddress.Parse(ip);
            ViewBag.IpType = IpClassifier.ClassifyIp(ipObj);

            // Encode IPv6
            var encodedIp = Uri.EscapeDataString(ip);

            // Call AbuseIPDB
            var report = await _http.GetFromJsonAsync<AbuseIpReport>(
                $"check?ipAddress={encodedIp}&maxAgeInDays=90"
            );

            if (report?.data == null)
            {
                ViewBag.Error = "The API returned no data. Check the IP address or your API key.";
                return View("Index", new AbuseIpReport());
            }

            int score = report.data.abuseConfidenceScore;

            // Risk classification
            if (score <= 5)
            {
                ViewBag.RiskLabel = "Low";
                ViewBag.RiskColor = "#2ecc71";
            }
            else if (score <= 30)
            {
                ViewBag.RiskLabel = "Medium";
                ViewBag.RiskColor = "#f1c40f";
            }
            else
            {
                ViewBag.RiskLabel = "High";
                ViewBag.RiskColor = "#e74c3c";
            }

            return View("Index", report);
        }
        catch
        {
            ViewBag.Error = "An unexpected error occurred while checking the IP.";
            return View("Index", new AbuseIpReport());
        }
    }

    [HttpPost]
    public async Task<IActionResult> ReportIp(string ip, int category, string comment)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            ViewBag.ReportError = "No IP address provided.";
            return RedirectToAction("Index");
        }

        var payload = new Dictionary<string, string>
        {
            { "ip", ip },
            { "categories", category.ToString() },
            { "comment", comment ?? "" }
        };

        var content = new FormUrlEncodedContent(payload);

        try
        {
            var response = await _http.PostAsync("report", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["ReportSuccess"] = $"Successfully reported {ip} to AbuseIPDB.";
            }
            else
            {
                TempData["ReportError"] = $"Failed to report {ip}. Status: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            TempData["ReportError"] = $"Error reporting IP: {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}








