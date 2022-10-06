using Microsoft.EntityFrameworkCore;
using PartyProduct_Exercise_03.Data;
using PartyProduct_Exercise_03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyProduct_Exercise_03.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly PartyProductMVCContext _context = null;

        public InvoiceRepository(PartyProductMVCContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceModel>> GetAllInvoice()
        {
            return await _context.Invoice
                  .Select(invoice => new InvoiceModel()
                  {
                      Id = invoice.Id,
                      PartyId = invoice.PartyId,
                      PartyName = invoice.PartyName,
                      ProductId = invoice.ProductId,
                      ProductName = invoice.ProductName,
                      CurrentRate = invoice.CurrentRate,
                      Quantity = invoice.Quantity,
                      Total = invoice.Total
                  }).ToListAsync();
        }

        public async Task<int> InvoiceAdd(InvoiceModel invoiceModel)
        {
            var y = _context.Invoice
                    .Where(x => x.PartyId == invoiceModel.PartyId && x.ProductId == invoiceModel.ProductId && x.CurrentRate == invoiceModel.CurrentRate).FirstOrDefault();

            if (y == null)
            {
                var newInvoice = new Invoice()
                {
                    PartyId = invoiceModel.PartyId,
                    PartyName = _context.Party.Where(x => x.Id == invoiceModel.PartyId).FirstOrDefault().PartyName,
                    ProductId = invoiceModel.ProductId,
                    ProductName = _context.Product.Where(x => x.Id == invoiceModel.ProductId).FirstOrDefault().ProductName,
                    CurrentRate = invoiceModel.CurrentRate,
                    Quantity = invoiceModel.Quantity,
                    Total = invoiceModel.CurrentRate * invoiceModel.Quantity
                };
                await _context.Invoice.AddAsync(newInvoice);
            }
            else
            {
                y.PartyId = invoiceModel.PartyId;
                y.PartyName = _context.Party.Where(x => x.Id == invoiceModel.PartyId).FirstOrDefault().PartyName;
                y.ProductId = invoiceModel.ProductId;
                y.ProductName = _context.Product.Where(x => x.Id == invoiceModel.ProductId).FirstOrDefault().ProductName;
                y.CurrentRate = invoiceModel.CurrentRate;
                y.Quantity = y.Quantity + invoiceModel.Quantity;
                y.Total = invoiceModel.CurrentRate * y.Quantity;
            }

            await _context.SaveChangesAsync();

            int grandTotal = (from i in _context.Invoice select i.Total).Sum();

            return grandTotal;
        }

        public async Task<List<AssignPartyModel>> BindProduct(int PartyId)
        {
            return await _context.AssignParty.Where(x => x.PartyId == PartyId)
                .Select(product => new AssignPartyModel()
                {
                    ProductId = product.ProductId,
                    productName = product.Product.ProductName
                }).Distinct().ToListAsync();
        }

        public async Task<int> BindRate(int ProductId)
        {
            return await _context.ProductRate.Where(x => x.ProductId == ProductId)
                .Select(x => x.Rate).FirstOrDefaultAsync();
        }

        public async Task ClrearInvoice()
        {
            foreach (var item in _context.Invoice)
            {
                _context.Invoice.Remove(item);
            }
            _context.SaveChanges();
        }
    }
}
