using Microsoft.AspNetCore.Mvc;
using MyAppClient.Models;
using Newtonsoft.Json;

namespace MyAppClient.Controllers
{
    public class PersonsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyRestApi()
        {
            List<PersonModel> persons = new List<PersonModel>();

            /*var p1 = new PersonModel()
            {
                PersonId = "1",
                Name = "jan",
                Email = "sdfdf"
            };

            var p2 = new PersonModel()
            {
                PersonId = "1",
                Name = "jan",
                Email = "sdfdf"
            };

            persons.Add(p1);
            persons.Add(p2);*/

            var client = new HttpClient();

            await Task.Run(() =>
            {
                var response = client.GetAsync("https://localhost:7163/api/Persons").Result;

                if (response.IsSuccessStatusCode)
                {
                    var dataObject = response.Content.ReadAsStringAsync().Result;

                    var result = JsonConvert.DeserializeObject<List<PersonModel>>(dataObject);

                    foreach(var p in result)
                    {
                        persons.Add(p);
                    }
                }
                else
                {
                    Console.WriteLine("{0} {1}", (int)response.StatusCode, response.ReasonPhrase);
                }

            });

            return View(persons);
        }


        public async Task<IActionResult> Create(PersonModel person)
        {
            person.PersonId = Guid.NewGuid().ToString();

            using var client = new HttpClient();

            string json = JsonConvert.SerializeObject(person);

            StringContent httpContent = new StringContent(json,System.Text.Encoding.UTF8,"application/json");

            var response = await client.PostAsync("https://localhost:7163/api/Persons", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MyRestApi");
            }

            return View();
        }
        public async Task<IActionResult> Details(string id)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7163/api/Persons/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var person = JsonConvert.DeserializeObject<PersonModel>(json);
                return View(person);
            }

            return NotFound();
        }

        public async Task<IActionResult> Edit(string id)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7163/api/Persons/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var person = JsonConvert.DeserializeObject<PersonModel>(json);
                return View(person);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PersonModel person)
        {
            using var client = new HttpClient();
            var json = JsonConvert.SerializeObject(person);
            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"https://localhost:7163/api/Persons/{person.PersonId}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MyRestApi");
            }

            return View(person);
        }

        public async Task<IActionResult> Delete(string id)
        {
            using var client = new HttpClient();
            var response = await client.DeleteAsync($"https://localhost:7163/api/Persons/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MyRestApi");
            }

            return NotFound();
        }

    }
}
