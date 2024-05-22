using CellReport_Workflow.Interface;
using CellReport_Workflow.Service;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();  //s
builder.Services.AddSession(op =>
{
    op.IOTimeout = TimeSpan.FromHours(12);
    //op.IOTimeout = TimeSpan.FromSeconds(30);
    op.Cookie.HttpOnly = true;
    op.Cookie.IsEssential = true;
}
 );
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IEncryptService, EncryptService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
