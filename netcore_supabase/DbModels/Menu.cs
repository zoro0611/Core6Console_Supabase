using Microsoft.EntityFrameworkCore;
using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;

namespace netcore_supabase.DbModels
{
    public partial class Menu : BaseModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal? Stock { get; set; }
        public decimal? Price { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public int? Categoryid { get; set; }

        public virtual Category? Category { get; set; }
    }
}
