document.getElementById("checkBtn").addEventListener("click", async () => {
    const ip = document.getElementById("ipInput").value.trim();
    const output = document.getElementById("output");
    const resultsBox = document.getElementById("results");

    if (!ip) {
        output.textContent = "Please enter an IP address.";
        resultsBox.style.display = "block";
        return;
    }

    try {
        const response = await fetch(`/Home/CheckIp?ip=${encodeURIComponent(ip)}`);

        if (!response.ok) {
            output.textContent = `Error: ${response.status} ${response.statusText}`;
            resultsBox.style.display = "block";
            return;
        }

        const data = await response.json();

        output.textContent = JSON.stringify(data, null, 2);
        resultsBox.style.display = "block";

    } catch (err) {
        output.textContent = "Request failed: " + err.message;
        resultsBox.style.display = "block";
    }
});
