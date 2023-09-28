// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using netcore_supabase.DbModels;

var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddUserSecrets<Program>(); // 添加UserSecrets
IConfigurationRoot configuration = builder.Build();
string dbConnectionString = configuration.GetConnectionString("DatabaseConnection");

string usersecret_superbaseUrl = configuration["Superbase:Url"];
string usersecret_superbaseKey = configuration["Superbase:Key"];

var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};

var supabase = new Supabase.Client(usersecret_superbaseUrl, usersecret_superbaseKey, options);
await supabase.InitializeAsync();

var result = await supabase.From<Menu>().Get();
var menus = result.Models.OrderBy(x=>x.Id);

foreach (var menu in menus)
{
    Console.WriteLine($"Id: {menu.Id}, Stock: {menu.Stock}, Price: {menu.Price}, Title: {menu.Title}, image: {menu.Image}, categoryId: {menu.Categoryid}");
}



Console.ReadLine();