//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cidla
    {
        public int Id { get; set; }
        public string mistnost { get; set; }
        public int teplota { get; set; }
        public int spotreba { get; set; }
        public int sviceni { get; set; }
        public System.DateTime datum { get; set; }
        public Nullable<int> TopeniId { get; set; }
    
        public virtual Topeni Topeni { get; set; }
    }
}