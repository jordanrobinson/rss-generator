using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSS_Generator.Models
{
    public class ItemModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Link { get; set; }
    }
}