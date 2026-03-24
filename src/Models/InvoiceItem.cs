using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CariHesap.Models
{
    public class InvoiceItem : BaseEntity
    {
        [Required(ErrorMessage = "Fatura seçimi zorunludur.")]
        public int InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public Invoice? Invoice { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Miktar zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Birim fiyat zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Birim fiyat 0'dan büyük olmalıdır.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "KDV oranı zorunludur.")]
        [Range(0, 100, ErrorMessage = "KDV oranı 0-100 arasında olmalıdır.")]
        public decimal VATRate { get; set; }

        [Required(ErrorMessage = "Toplam tutar zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Toplam tutar 0'dan büyük olmalıdır.")]
        public decimal Total { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }
    }
} 