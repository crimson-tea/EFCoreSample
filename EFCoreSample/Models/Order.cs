using EFCoreSample.Models;

namespace EFCoreSample.Models;

public class Order
{
    public int Id { get; set; }
    // 外部キー（Customer との関連付け）
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    // 複雑な集計用：カテゴリ
    public string Category { get; set; }
    public Customer Customer { get; set; }
}
