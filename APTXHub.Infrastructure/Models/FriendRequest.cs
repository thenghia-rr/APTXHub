﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty; // e.g., "Pending", "Accepted", "Rejected"
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
    }
}
