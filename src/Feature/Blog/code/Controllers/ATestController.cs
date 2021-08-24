
using Sitecore.Data.Items;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Sitecore.Web.UI.WebControls;
using SitecoreAppl.Feature.Blog.Models;


namespace SitecoreAppl.Feature.Blog.Controllers
{
    public class ATestController : Controller
    {
        // GET: ATest
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ArticleBody()
        {
           
            var itemm = Sitecore.Context.Database.GetItem("{E699DF65-6F0B-4AB4-B2D1-1F8B9162FD44}");
            Sitecore.Data.Fields.MultilistField multilistField = itemm.Fields["PreferredArticles"];
            int len = multilistField.GetItems().Length;
            Sitecore.Data.Items.Item[] items = new Item[len];

            var searc = new List<ItemRes>();

            for (int i = 0; i < multilistField.GetItems().Length; i++)
            {
               

                items[i] = multilistField.GetItems()[i];
               
            }



            ViewBag.multi = items;
            return View("~/Views/ATest/ArticleBody.cshtml");
        }

        [HttpPost]
        public ActionResult ArticleBody(FormCollection form)
        {


            string searchText = form["searchInput"];
            
           
            List<string> solr = new List<string>();
            List<string> prefereditems = new List<string>();

            var itemm = Sitecore.Context.Database.GetItem("{E699DF65-6F0B-4AB4-B2D1-1F8B9162FD44}");
            Sitecore.Data.Fields.MultilistField multilistField = itemm.Fields["PreferredArticles"];
            if (multilistField != null)
            {
                foreach (Sitecore.Data.Items.Item city in multilistField.GetItems())
                {
                    prefereditems.Add(Convert.ToString(city.ID));
                }
            }

            var searchIndex = ContentSearchManager.GetIndex("sitecore_web_index"); // Get the search index
            var searchPredicate = GetSearchPredicate(searchText); // Build the search predicate
            using (var searchContext = searchIndex.CreateSearchContext()) // Get a context of the search index
            {
                //Select * from Sitecore_web_index Where Author="searchText" OR Description="searchText" OR Title="searchText"
                //var searchResults = searchContext.GetQueryable<SearchModel>().Where(searchPredicate); // Search the index for items which match the predicate
                var searchResults = searchContext.GetQueryable<SearchModel>()
                    .Where(x => x.Title.Contains(searchText) || x.Description.Contains(searchText));   //LINQ query

                var fullResults = searchResults.GetResults();

                // This is better and will get paged results - page 1 with 10 results per page
                //var pagedResults = searchResults.Page(1, 10).GetResults();
                foreach (var hit in fullResults.Hits)
                {
                   
                    solr.Add(Convert.ToString(hit.Document.ItemId));

                }
                List<string> res = new List<string>();
                //comparing the results from solr search with prefered items and saving in alist
                foreach (string i in prefereditems)
                {
                    if (solr.Contains(i))
                    {

                        res.Add(i);

                    }
                }
                int len = res.Count;
                Sitecore.Data.Items.Item[] items = new Item[len];
                //based on the item id retrieving data
                for (int j = 0; j < res.Count; j++)
                {
                    Sitecore.Data.Items.Item item1 = Sitecore.Context.Database.GetItem(res[j]);
                    items[j] = item1;
                }

               
                ViewBag.multi = items;
                return View();
            }
        }
        public static Expression<Func<SearchModel, bool>> GetSearchPredicate(string searchText)
        {
            var predicate = PredicateBuilder.True<SearchModel>(); // Items which meet the predicate
                                                                  // Search the whole phrase - LIKE
                                                                  // predicate = predicate.Or(x => x.DispalyName.Like(searchText)).Boost(1.2f);
                                                                  // predicate = predicate.Or(x => x.Description.Like(searchText)).Boost(1.2f);
                                                                  // predicate = predicate.Or(x => x.Title.Like(searchText)).Boost(1.2f);
                                                                  // Search the whole phrase - CONTAINS
            predicate = predicate.Or(x => x.Author.Contains(searchText)); // .Boost(2.0f);
            predicate = predicate.Or(x => x.Description.Contains(searchText)); // .Boost(2.0f);
            predicate = predicate.Or(x => x.Title.Contains(searchText)); // .Boost(2.0f);
            return predicate;
        }
         
        public ActionResult Details()
        {
           
            string id = Request.Form["id"];
            var itemm = Sitecore.Context.Database.GetItem(id);
            ViewBag.Detaileddata = itemm;
            return View("~/Views/ATest/Details.cshtml",itemm);
        }
    }
}