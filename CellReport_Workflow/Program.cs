using CellReport_Workflow.Interface;
using CellReport_Workflow.Middleware;
using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

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
builder.Services.AddTransient<ISql, SqlBase>();
builder.Services.AddTransient<ILogService, Log>();
builder.Services.AddTransient<IStampService, StampService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IMergeInstructionPageService, MergeInstructionPageService>();
builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("'https://lis.babybanks.com/")
                .AllowAnyHeader()
                .AllowAnyMethod()
            .SetIsOriginAllowed(origin => true)
                .AllowCredentials();
        });
});

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
app.UseCors();
//app.UseIeCompatibility();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
