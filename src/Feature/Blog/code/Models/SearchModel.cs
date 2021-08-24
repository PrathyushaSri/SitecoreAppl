using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace SitecoreAppl.Feature.Blog.Models
{
    

        public class SearchModel : SearchResultItem
        {
            [IndexField("_name")]
            public virtual string ItemName { get; set; }

            [IndexField("author_t")]
            public virtual string Author { get; set; }

            [IndexField("description_t")]
            public virtual string Description { get; set; } // Custom field on my template

            [IndexField("title_t")]
            public virtual string Title { get; set; } // Custom field on my template
         
        
        [IndexField("Id_t")]
        public virtual string ID { get; set; }
        //public SearchModel() { }
        //public SearchModel(Item item)
        //{
        //    this.Title = FieldRenderer.Render(item, "Title");
        //    this.ItemName = FieldRenderer.Render(item, "ItemName");
        //}
    }
  

    public class SearchResult
        {
            public string ItemName { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Description { get; set; }
       
        public string ItemId { get; set; }
        
    }

        /// <summary>
        /// Custom search result model for binding to front end
        /// </summary>
        public class SearchResults
        {
            public List<SearchResult> Results { get; set; }
        }

    public class ItemRes
    {
        public Sitecore.Data.Items.Item itemtitle { get; set; }
    }
    }

