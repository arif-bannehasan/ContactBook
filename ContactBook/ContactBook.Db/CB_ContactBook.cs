//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContactBook.Db
{
    using System;
    using System.Collections.Generic;
    
    public partial class CB_ContactBook
    {
        public CB_ContactBook()
        {
            this.CB_Numbers = new HashSet<CB_Number>();
            this.CB_Emails = new HashSet<CB_Email>();
            this.CB_Book_GroupTypes = new HashSet<CB_Book_GroupTypes>();
            this.CB_Addresses = new HashSet<CB_Address>();
            this.CB_IMs = new HashSet<CB_IM>();
            this.CB_Websites = new HashSet<CB_Website>();
            this.CB_Relationships = new HashSet<CB_Relationship>();
            this.CB_SpecialDates = new HashSet<CB_SpecialDates>();
            this.CB_InternetCalls = new HashSet<CB_InternetCall>();
        }
    
        public long BookId { get; set; }
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public Nullable<int> SuffixId { get; set; }
        public string PhFirstname { get; set; }
        public string PhMiddlename { get; set; }
        public string PhSurname { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string Notes { get; set; }
        public string NickName { get; set; }
    
        public virtual ICollection<CB_Number> CB_Numbers { get; set; }
        public virtual ICollection<CB_Email> CB_Emails { get; set; }
        public virtual ICollection<CB_Book_GroupTypes> CB_Book_GroupTypes { get; set; }
        public virtual ICollection<CB_Address> CB_Addresses { get; set; }
        public virtual CB_Suffix CB_Suffix { get; set; }
        public virtual ICollection<CB_IM> CB_IMs { get; set; }
        public virtual ICollection<CB_Website> CB_Websites { get; set; }
        public virtual ICollection<CB_Relationship> CB_Relationships { get; set; }
        public virtual ICollection<CB_SpecialDates> CB_SpecialDates { get; set; }
        public virtual ICollection<CB_InternetCall> CB_InternetCalls { get; set; }
    }
}