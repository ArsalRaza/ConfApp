//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConferenceWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProgramPeople
    {
        public int ID { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> UserId { get; set; }
        public int CategoryId { get; set; }
    
        public virtual Program Program { get; set; }
        public virtual ProgramMemberCategories ProgramMemberCategories { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
