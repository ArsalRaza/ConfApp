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
    
    public partial class Conference
    {
        public Conference()
        {
            this.Program = new HashSet<Program>();
        }
    
        public int ID { get; set; }
        public string ConferenceName { get; set; }
        public Nullable<System.DateTime> ConferenceDate { get; set; }
        public string Organizer { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string ConferenceDescription { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public Nullable<bool> IsEnabled { get; set; }
        public string FeeStructure { get; set; }
        public string CityInformation { get; set; }
    
        public virtual ICollection<Program> Program { get; set; }
    }
}
