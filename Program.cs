using Microsoft.Playwright;
using System.Text.RegularExpressions;

var playwright = await Playwright.CreateAsync();
var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    Headless = false
});

var page = await browser.NewPageAsync();

var mathimata = new Dictionary<string, string>
{
    {"Vaseis_Dedomenwn2", "ICE279"},
    {"Vaseis_Dedomenwn1", "CS161"},
    {"Leitourgika1", "CS121"},
    {"Simata_kai_Sistimata", "CS205"},
    {"Sxediasi_Psifiakwn_Sistimatwn", "ICE284"}
};

string mathima = mathimata["Simata_kai_Sistimata"];

var groups = new List<string>
{
    "ΔΕΥΤΕΡΑ 09:00-11:00 Τμήμα1",
};

string username = "";
string password = "";

if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Username and password are required. Please enter your credentials in the variables.");
    Console.ResetColor();
    return;
}

string baseLink = $"https://eclass.uniwa.gr/courses/{mathima}/";
await page.GotoAsync(baseLink);
await page.FillAsync("#username_id", username);
await page.FillAsync("#password_id", password);
await page.ClickAsync(".login-form-submit");
await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

await page.ClickAsync("a:has-text('Ομάδες Χρηστών')");
await page.ClickAsync("a:has-text('Εμφάνιση σε λίστα')");
await page.WaitForSelectorAsync("table.table-default tbody tr");

bool found = false;
int groupIndex = 0;

while (!found)
{
    var targetGroup = groups[groupIndex];

    while (true)
    {
        var rows = await page.QuerySelectorAllAsync("table.table-default tbody tr");
        bool rowFound = false;

        foreach (var row in rows)
        {
            var firstCell = await row.QuerySelectorAsync("td:nth-child(1)");
            string firstCellText = firstCell != null ? (await firstCell.InnerTextAsync()).Trim() : "";

            if (!firstCellText.Contains(targetGroup, StringComparison.OrdinalIgnoreCase))
                continue;

            rowFound = true;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Found target group: {targetGroup}");
            Console.ResetColor();

            var membersCell = await row.QuerySelectorAsync("td:nth-child(3)");
            string membersText = membersCell != null ? (await membersCell.InnerTextAsync()).Trim() : "";
            membersText = membersText.Replace("Μέλη:", "").Replace("Μέλη", "").Replace("&nbsp;", " ").Trim();

            if (membersText.Contains("-") || membersText.Contains("—"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Class not opened yet. Reloading...");
                Console.ResetColor();
                await Task.Delay(250);
                await page.ReloadAsync();
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                break; 
            }

            var match = Regex.Match(membersText, @"(\d+)\s*/\s*(\d+)");
            if (!match.Success)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Could not parse members info: '{membersText}'");
                Console.ResetColor();
                break;
            }

            int current = int.Parse(match.Groups[1].Value);
            int max = int.Parse(match.Groups[2].Value);

            if (current < max)
            {
                var joinButton = await row.QuerySelectorAsync("a[aria-label='Εγγραφή'], a:has-text('Εγγραφή')");
                if (joinButton != null)
                {
                    await joinButton.ClickAsync();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Successfully joined group {targetGroup}!");
                    Console.ResetColor();
                    found = true;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Group {targetGroup} is full. Trying next group.");
                Console.ResetColor();
            }

            break;
        }

        if (found || !rowFound)
            break;
    }

    if (!found)
    {
        if (groupIndex >= groups.Count - 1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("All groups are full. Exiting...");
            Console.ResetColor();
            break;
        }
        groupIndex++;
        await page.ReloadAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}

Console.WriteLine("Press Enter to exit...");
Console.ReadLine();
await browser.CloseAsync();