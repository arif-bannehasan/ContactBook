﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContactBook.Db.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ContactBookEdmContainer : DbContext
    {
        public ContactBookEdmContainer()
            : base("name=ContactBookEdmContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CB_Contact> CB_Contact { get; set; }
        public DbSet<CB_Number> CB_Number { get; set; }
        public DbSet<CB_Email> CB_Email { get; set; }
        public DbSet<CB_Address> CB_Address { get; set; }
        public DbSet<CB_GroupType> CB_GroupType { get; set; }
        public DbSet<CB_NumberType> CB_NumberType { get; set; }
        public DbSet<CB_ContactByGroup> CB_ContactByGroup { get; set; }
        public DbSet<CB_AddressType> CB_AddressType { get; set; }
        public DbSet<CB_EmailType> CB_EmailType { get; set; }
        public DbSet<CB_IM> CB_IM { get; set; }
        public DbSet<CB_IMType> CB_IMType { get; set; }
        public DbSet<CB_Website> CB_Website { get; set; }
        public DbSet<CB_Relationship> CB_Relationship { get; set; }
        public DbSet<CB_RelationshipType> CB_RelationshipType { get; set; }
        public DbSet<CB_SpecialDates> CB_SpecialDates { get; set; }
        public DbSet<CB_SpecialDateType> CB_SpecialDateType { get; set; }
        public DbSet<CB_InternetCall> CB_InternetCall { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<CB_ContactBook> CB_ContactBook { get; set; }
    }
}
