using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http.Json;
using SlothSecIpCheckerWeb.Models;

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

            // Encode IPv6 (colons break URLs)
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
        catch (Exception)
        {
            ViewBag.Error = "An unexpected error occurred while checking the IP.";
            return View("Index", new AbuseIpReport());
        }
    }
}






