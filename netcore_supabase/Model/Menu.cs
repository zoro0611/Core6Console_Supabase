using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netcore_supabase.Model
{
    public class Menu : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public DateTime Created_At { get; set; }

    }
}
