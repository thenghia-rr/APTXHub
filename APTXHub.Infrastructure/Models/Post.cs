﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int NrOfReports { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DeletedAt { get; set; }


        // Foreign key
        public int UserId { get; set; }

        //Navigation properties
        public User User { get; set; }
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<PostHashtag> PostHashtags { get; set; } = new List<PostHashtag>();
    }
}
