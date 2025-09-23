using Microsoft.EntityFrameworkCore;
using Poc.EF.Context;

using Poc.Implementation.Repositories.CustomerRepositories;
using Poc.Implementation.Repositories.RequirementRepository;
using Poc.Implementation.Services.CustomerServices;
using Poc.Implementation.Services.RequirementService;
using Poc.Infrastructure.Interfaces.IRepositories.Customer;
using Poc.Infrastructure.Interfaces.IRepositories.IRequirementRepositories;
using Poc.Infrastructure.Interfaces.IServices.Customer;
using Poc.Infrastructure.Interfaces.IServices.IRequirementServices;
using YourWebProject.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalSessionAuthorizeAttribute>();
});
var  provider=builder.Services.BuildServiceProvider();
var config= provider.GetRequiredService<IConfiguration>();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DBms")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRequirementsRepository, RequirementRepository>();
builder.Services.AddScoped<IRequirementService, RequirementService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
;    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
