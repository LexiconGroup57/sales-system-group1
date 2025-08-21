using System;

namespace SalesSystem.Models;

public enum SaleItemType { Ticket, Snack }

public class SaleRecord
{
    public DateTime Timestamp { get; set; }
    public SaleItemType ItemType { get; set; }
    public string ItemName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int VatPercent { get; set; }
    public decimal Gross => UnitPrice * Quantity;
    public decimal VatAmount => Math.Round(Gross * VatPercent / 100m, 2);
    public decimal Net => Gross - VatAmount;
    public string? Notes { get; set; }
}
