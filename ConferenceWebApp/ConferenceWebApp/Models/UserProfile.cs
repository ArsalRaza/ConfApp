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
    
    public partial class UserProfile
    {
        public UserProfile()
        {
            this.ProgramPeople = new HashSet<ProgramPeople>();
            this.Conversation_reply = new HashSet<Conversation_reply>();
            this.MyAgenda = new HashSet<MyAgenda>();
            this.Conversation = new HashSet<Conversation>();
            this.Conversation1 = new HashSet<Conversation>();
        }
    
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Photo { get; set; }
        public string Profile { get; set; }
        public string Designation { get; set; }
        public string Organization { get; set; }
    
        public virtual ICollection<ProgramPeople> ProgramPeople { get; set; }
        public virtual ICollection<Conversation_reply> Conversation_reply { get; set; }
        public virtual ICollection<MyAgenda> MyAgenda { get; set; }
        public virtual ICollection<Conversation> Conversation { get; set; }
        public virtual ICollection<Conversation> Conversation1 { get; set; }
    }
}
