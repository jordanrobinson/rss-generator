using System;
using System.Collections.Generic;
using System.Configuration;

namespace RSSGenerator.Models
{
    public class SourcesModel : ConfigurationSection
    {
        public static Configuration GetConfiguration()
        {
            return ConfigurationManager.GetSection("SourceConfiguration") as Configuration;            
        }


        [ConfigurationProperty("Title")]
        public string Title
        {
            get
            {
                return (string)this["Title"];
            }
        }
        [ConfigurationProperty("Link")]
        public Uri Link { get; set; }
        [ConfigurationProperty("Description")]
        public string Description { get; set; }

    }

    public class Sources : ConfigurationElement
    {
        [ConfigurationProperty("Sources", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(string))]
        public List<string> Value
        {
            get
            {
                var sourceList = (List<string>)base["Sources"];
                return sourceList;
            }
        }
    }
}