using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FinaPart.Models;

namespace FinaPart.Utils
{
    public class FinaContext : DbContext
    {
        public FinaContext() : base("FinaDbContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Companies> Companies { get; set; }

        public DbSet<Products> Products { get; set; }
        public DbSet<Contragents> Contragents { get; set; }
        public DbSet<Units> Units { get; set; }
        public DbSet<ProductPrices> ProductPrices { get; set; }
        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<SubContragent> SubContragents { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ContragentContact> ContragentContacts { get; set; }
        public DbSet<GeneralDocs> GeneralDocs { get; set; }
        public DbSet<Entries> Entries { get; set; }
        public DbSet<ProductOut> ProductOut { get; set; }
        public DbSet<ProductsFlow> ProductsFlow { get; set; }
        public DbSet<ProductShipping> ProductShippings { get; set; }
        public DbSet<ProductCancel> ProductCancels { get; set; }
        public DbSet<ProductBarCode> ProductBarCodes { get; set; }
        public DbSet<GroupProduct> GroupProducts { get; set; }
        public DbSet<Staffs> Staffs { get; set; }
        public DbSet<Params> Params { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<ProductMove> ProductMoves { get; set; }

        public DbSet<Car> Cars { get; set; }

        public DbSet<ProductShippingFlow> ProductShippingFlows { get; set; }
    }
}