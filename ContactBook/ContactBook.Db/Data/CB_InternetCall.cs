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
    
    public partial class CB_InternetCall
    {
        public int InternetCallId { get; set; }
        public string InternetCallNumber { get; set; }
        public long ContactId { get; set; }
    
        public virtual CB_Contact CB_Contacts { get; set; }
    }
}
