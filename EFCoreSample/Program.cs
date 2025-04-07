using EFCoreSample.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = "server=localhost;port=3306;database=appdb;user=root;password=my-secret-pw;";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//// DB の初期化＆シード（初回実行時）
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    // データベースがなければ作成（マイグレーションを使う場合は Add-Migration / Update-Database を実施）
//    context.Database.EnsureCreated();

//    // シードデータの追加（必要な場合のみ）
//    if (!context.Customers.Any())
//    {
//        var customerA = new Customer { Name = "Customer A" };
//        var customerB = new Customer { Name = "Customer B" };

//        context.Customers.AddRange(customerA, customerB);

//        context.Orders.AddRange(
//            new Order { Customer = customerA, Amount = 100, Category = "Electronics" },
//            new Order { Customer = customerA, Amount = 200, Category = "Books" },
//            new Order { Customer = customerA, Amount = 150, Category = "Electronics" },
//            new Order { Customer = customerB, Amount = 300, Category = "Books" },
//            new Order { Customer = customerB, Amount = 250, Category = "Electronics" }
//        );
//        context.SaveChanges();
//    }
//}

app.MapControllers();

app.Run();
