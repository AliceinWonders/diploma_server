using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using diploma_server.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;

namespace diploma_server.Controllers
{
    [Route("{controller}")]
    public class ArticleController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly IWebHostEnvironment _hostingEnvironment;
        
        public ArticleController(IElasticClient elasticClient, IWebHostEnvironment hostingEnvironment)
        {
            _elasticClient = elasticClient;
            _hostingEnvironment = hostingEnvironment;
        }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<ElasticsearchMiddleware.ArticleModel> Create(ElasticsearchMiddleware.ArticleModel model)
        // {
        //     try
        //     {
        //         var article = new ElasticsearchMiddleware.ArticleModel()
        //         {
        //             Id = 1,
        //             Title = model.Title,
        //             Link = model.Link,
        //             Author = model.Author,
        //             AuthorLink = model.AuthorLink,
        //             PublishedDate = DateTime.Now
        //         };
        //         await _elasticClient.IndexDocumentAsync(article);
        //         model = new ElasticsearchMiddleware.ArticleModel();
        //     }
        //     catch (Exception ex)
        //     {
        //     }
        //
        //     return model;
        // }
        //
        // public async Task<IActionResult> Update(IActionResult model)
        // {
        //     var article = new ElasticsearchMiddleware.ArticleModel();
        //     await _elasticClient.UpdateAsync<ElasticsearchMiddleware.ArticleModel>(
        //         article.Id,
        //         u => u
        //             .Index("articles")
        //             .Doc(article));
        //     return model;
        //     
        // }
        //
        [HttpGet]
        IEnumerable<ElasticsearchMiddleware.ArticleModel> Index(string keyword)
        {
            var articleList = new List<ElasticsearchMiddleware.ArticleModel>();
            if (!string.IsNullOrEmpty(keyword))
            {
                articleList = GetSearch(keyword).ToList();
            }

            return articleList.AsEnumerable();
        }

        IList<ElasticsearchMiddleware.ArticleModel> GetSearch(string keyword)
        {
            var result = _elasticClient.SearchAsync<ElasticsearchMiddleware.ArticleModel>(s =>
                s.Query(q => q.QueryString(d => d.Query('*' + keyword + '*'))).Size(5000));
            var finalResult = result;
            var finalContent = finalResult.Result.Documents.ToList();
            return finalContent;
        }
    }
}