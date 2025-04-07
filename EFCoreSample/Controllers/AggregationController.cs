using EFCoreSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AggregationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AggregationController(ApplicationDbContext context)
    {
        _context = context;
    }

    // シンプルな集計：各顧客ごとの注文金額合計を返す
    // GET api/aggregation/simple
    [HttpGet("simple")]
    public async Task<SimpleAggregationResult[]> GetSimpleAggregation()
    {
        var result = await _context.Orders
            .Include(o => o.Customer)
            .GroupBy(o => o.Customer.Name)
            .Select(g => new
            {
                Customer = g.Key,
                TotalAmount = g.Sum(o => o.Amount)
            })
            .ToListAsync();

        return result.Select(x => new SimpleAggregationResult(x.Customer, x.TotalAmount)).ToArray();
    }

    public record SimpleAggregationResult(string Customer, decimal TotalAmount);

    // 複雑な集計：顧客ごとに、さらに注文のカテゴリごとに合計金額を計算する
    // GET api/aggregation/complex
    [HttpGet("complex")]
    public async Task<ComplexAggregationResult> GetComplexAggregation()
    {
        var result = await _context.Orders
            .Include(o => o.Customer)
            .GroupBy(o => new { o.CustomerId, o.Customer.Name })
            .Select(g => new
            {
                g.Key.CustomerId,
                Customer = g.Key.Name,
                Categories = g.GroupBy(o => o.Category)
                              .Select(cg => new
                              {
                                  Category = cg.Key,
                                  TotalAmount = cg.Sum(o => o.Amount)
                              })
                              .ToList()
            })
            .ToListAsync();

        return new ComplexAggregationResult(
            result.Select(x =>
                new CustomerAmount(
                    x.CustomerId,
                    x.Customer,
                    x.Categories.Select(x => new Amount(x.Category, x.TotalAmount)).ToArray()
                )
            )
            .ToArray());
    }

    public record ComplexAggregationResult(CustomerAmount[] CustomerAmounts);
    public record CustomerAmount(int CustomerId, string Customer, Amount[] TotalAmounts);
    public record Amount(string Category, decimal TotalAmount);
}
