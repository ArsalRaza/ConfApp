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
    
    public partial class ProgramMemberCategories
    {
        public ProgramMemberCategories()
        {
            this.ProgramPeople = new HashSet<ProgramPeople>();
        }
    
        public int ID { get; set; }
        public string CategoryName { get; set; }
    
        public virtual ICollection<ProgramPeople> ProgramPeople { get; set; }
    }
}
