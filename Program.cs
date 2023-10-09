var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

// refer https://khalidabuhakmeh.com/add-headers-to-a-response-in-aspnet-5
app.Use(async (context, next) =>
{
    context.Response.OnStarting((state) =>
    {
        if (state is HttpContext ctx)
        {
            ctx.Response.Headers.Remove("Content-Type");
        }

        return Task.CompletedTask;
    }, context);

    await next();
});

// refer https://stackoverflow.com/questions/70579541/asp-net-minimal-api-how-to-return-download-files-from-url
app.MapGet("/sample.jpg", async () =>
{
    var client = new HttpClient();
    var content = await client.GetByteArrayAsync("https://www.bing.com/th?id=OHR.FremontPetroglyph_EN-CN2259577911_1920x1200.jpg&rf=LaDigue_1920x1200.jpg");
    var mimeType = "image/jpeg";

    return Results.File(content, contentType: mimeType, fileDownloadName: "sample.jpg");
});

app.Run();
