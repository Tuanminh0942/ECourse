//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public long ID { get; set; }
        public long IDCourse { get; set; }
        public long IDUser { get; set; }
        public string Description { get; set; }
        public Nullable<byte> Rate { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
