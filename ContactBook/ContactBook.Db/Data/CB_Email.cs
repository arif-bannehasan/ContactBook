//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class CB_Email
    {
        public int EmailId { get; set; }
        public long ContactId { get; set; }
        public string Email { get; set; }
        public Nullable<int> EmailTypeId { get; set; }
    
        public virtual CB_Contacts CB_Contacts { get; set; }
        public virtual CB_EmailType CB_EmailType { get; set; }
    }
}
