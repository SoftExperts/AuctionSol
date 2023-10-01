//using AuctionSolFront.Controllers.ChatHub;


using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClientHelper>();
builder.Services.AddSingleton<IUserConnectionManager, UserConnectionManager>();

builder.Services.AddSignalR(e => {
    e.MaximumReceiveMessageSize = 102400000;
    e.ClientTimeoutInterval = TimeSpan.FromMinutes(10);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.MapHub<ChatHub>("/chatHub");

app.Run();
