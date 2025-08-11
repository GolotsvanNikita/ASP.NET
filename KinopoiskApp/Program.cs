using System.Text.Json;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.Map("/", async context =>
{
    var title = context.Request.Query["title"].ToString() ?? "";
    var apiKey = "25b8f844";

    string resultHtml = "";

    if (!string.IsNullOrWhiteSpace(title))
    {
        using var http = new HttpClient();
        var url = $"https://www.omdbapi.com/?t={Uri.EscapeDataString(title)}&apikey={apiKey}&plot=full";
        var json = await http.GetStringAsync(url);

        var movieData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (movieData != null && movieData.TryGetValue("Response", out var resp) && resp.GetString() == "True")
        {
            resultHtml += "<div class='movie-card'>";

            if (movieData.TryGetValue("Poster", out var poster) &&
                poster.ValueKind == JsonValueKind.String && poster.GetString() != "N/A")
            {
                resultHtml += $"<img class='poster' src='{WebUtility.HtmlEncode(poster.GetString())}' alt='Poster'>";
            }

            resultHtml += "<div class='movie-info'>";

            void AddField(string label, string key)
            {
                if (movieData.TryGetValue(key, out var value) && value.ValueKind == JsonValueKind.String)
                {
                    resultHtml += $"<p><strong>{label}:</strong> {WebUtility.HtmlEncode(value.GetString())}</p>";
                }
            }

            AddField("Title", "Title");
            AddField("Year", "Year");
            AddField("Director", "Director");
            AddField("Actors", "Actors");
            AddField("IMDB Rating", "imdbRating");

            if (movieData.TryGetValue("Plot", out var plot) && plot.ValueKind == JsonValueKind.String)
            {
                resultHtml += $"<p class='plot'>{WebUtility.HtmlEncode(plot.GetString())}</p>";
            }

            resultHtml += "</div></div>";
        }
        else
        {
            resultHtml = "<p class='not-found'>Movie not found.</p>";
        }
    }

    var html = $@"
    <html>
    <head>
        <meta charset='utf-8'>
        <title>Movie Search</title>
        <link rel='stylesheet' href='/styles.css'>
    </head>
    <body>
        <div class='container'>
            <h2>Movie Search</h2>
            <form method='get' class='search-form'>
                <input type='text' name='title' placeholder='Enter movie title' required value='{WebUtility.HtmlEncode(title)}' />
                <button type='submit'>Search</button>
            </form>
            {resultHtml}
        </div>
    </body>
    </html>";

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(html);
});

app.Run();
