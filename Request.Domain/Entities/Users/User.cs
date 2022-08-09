﻿using Request.Domain.Base;
using Request.Domain.Entities.Requests;
using System.ComponentModel.DataAnnotations;

namespace Request.Domain.Entities.Users
{
    public partial class User : Entity
    {
        public User()
        {
            this.LeaveRequests = new HashSet<LeaveRequest>();
            this.States = new HashSet<State>();
        }
        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        [StringLength(200)]
        public string? Email { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        public virtual ICollection<State> States { get; set; }
    }
}