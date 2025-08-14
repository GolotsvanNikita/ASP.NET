using RequestProcessingPipeline;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
var app = builder.Build();

app.UseSession();

app.UseFromTenThousandToHundredThousand();
app.UseFromThousandToTenthousand();
app.UseFromHundredToThousand();
app.UseFromTwentyToHundred();
app.UseFromElevenToNineteen();
app.UseFromOneToTen();

app.Run();