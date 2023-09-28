using System;
using System.Collections.Generic;

namespace netcore_supabase.DbModels
{
    public partial class Category
    {
        public Category()
        {
            Menus = new HashSet<Menu>();
        }

        public long CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Categoryname { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
