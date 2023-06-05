using System;

namespace diploma_server.Models
{
    public class ElasticsearchMiddleware
    {
        public class ArticleModel {
            public ArticleModel(int id, string author, DateTime publishedDate, string title, string content, string photo, string map, string link, string authorLink)
            {
                Id = id;
                Author = author;
                PublishedDate = publishedDate;
                Title = title;
                Content = content;
                Photo = photo;
                Map = map;
                Link = link;
                AuthorLink = AuthorLink;
            }

            public ArticleModel()
            {
                throw new NotImplementedException();
            }

            public int Id {get;set; }
            public string Author { get; set; }
            public DateTime PublishedDate {get;set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Photo { get; set; } // path to photo
            public string Map { get; set; }
            public string Link { get; set; }
            public string AuthorLink { get; set; }
            
        }
    }
}