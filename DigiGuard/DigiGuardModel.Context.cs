﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DigiGuard
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DGGuardEntities : DbContext
    {
        public DGGuardEntities()
            : base("name=DGGuardEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<DimCategory> DimCategories { get; set; }
        public DbSet<DimStatu> DimStatus { get; set; }
        public DbSet<DimUser> DimUsers { get; set; }
        public DbSet<FactReport> FactReports { get; set; }
        public DbSet<Change> Changes { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
