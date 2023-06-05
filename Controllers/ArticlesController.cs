using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using diploma_server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace diploma_server.Controllers
{
    [Route("{controller}")]
    public class ArticlesController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ArticlesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("searchByContent/{q}")]
        public async Task<IActionResult> SearchByContent(string q)
        {
            var client = new HttpClient();
            try
            {
                using HttpResponseMessage responseMessage =
                    await client.GetAsync($"http://localhost:9200/_search/?q=content:{q}");
                responseMessage.EnsureSuccessStatusCode();
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                return Ok(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message: " + e.Message);
            }

            return Ok();
        }

        [HttpGet]
        [EnableCors]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var client = new HttpClient();
            try
            {
                using HttpResponseMessage responseMessage =
                    await client.GetAsync("http://localhost:9200/diploma/_search?pretty=true&q=*:*");
                responseMessage.EnsureSuccessStatusCode();
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                return Ok(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message: " + e.Message);
            }

            return Ok();
        }
        
        [HttpPost]
        [Route("Create/{i}")]
        public async Task<IActionResult> Create(string i)
        {
            var client = new HttpClient();
            try
            {
                using HttpResponseMessage responseMessage =
                    await client.GetAsync($"http://localhost:9200/diploma/_doc/{i}");
                responseMessage.EnsureSuccessStatusCode();
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                return Ok(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message: " + e.Message);
            }

            return Ok();
        }
        
        [HttpPost]
        [Route("Update/{i}")]
        public async Task<IActionResult> Update(string i)
        {
            var client = new HttpClient();
            try
            {
                using HttpResponseMessage responseMessage =
                    await client.GetAsync($"http://localhost:9200/diploma/_doc/{i}");
                responseMessage.EnsureSuccessStatusCode();
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                return Ok(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message: " + e.Message);
            }

            return Ok();
        }
        
        [HttpPost("addNewTravelPhotoById")]
        public async Task<ActionResult> AddNewConferencePhoto(IFormFile uploadedFile )
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = $"{_configuration["PathTravelPhoto"]}\\{uploadedFile.FileName}";
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                return Ok();
            }

            return BadRequest();
        }
    }
}