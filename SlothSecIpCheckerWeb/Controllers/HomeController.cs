using Microsoft.AspNetCore.Mvc;
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
        var report = await _http.GetFromJsonAsync<AbuseIpReport>($"check?ip={ip}");

        int score = report!.data!.abuseConfidenceScore;

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
}

