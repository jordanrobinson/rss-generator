﻿using System.Collections.Generic;

namespace RSS_Generator.Models
{
    public class ArticleModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ItemModel> Items { get; set; }
    }
}