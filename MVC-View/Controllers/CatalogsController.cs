using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_View.Models;

namespace MVC_View.Controllers
{
    public class CatalogsController : Controller
    {
        private readonly HttpClient _httpClient;

        public CatalogsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
        }

        public async Task<IActionResult> Index()
        {
            var catalogs = await _httpClient.GetFromJsonAsync<List<Catalog>>("Catalogs");
            if (catalogs == null)
            {
                return NotFound();
            }

            return View(catalogs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var catalog = await _httpClient.GetFromJsonAsync<Catalog>($"Catalogs/{id}");
            if (catalog == null)
            {
                return NotFound();
            }
            return View(catalog);
        }


        // GET: Catalogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Catalogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CatalogCode,CatalogName")] Catalog catalog)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Catalogs", catalog);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(catalog);
        }

        // GET: Catalogs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Catalog catalog = await _httpClient.GetFromJsonAsync<Catalog>($"Catalogs/{id}");
            if (catalog == null)
            {
                return NotFound();
            }
            return View(catalog);
        }

        // POST: Catalogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CatalogCode,CatalogName")] Catalog catalog)
        {
            if (id != catalog.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Catalogs/{id}", catalog);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(catalog);
        }

        // GET: Catalogs/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Catalog catalog = await _httpClient.GetFromJsonAsync<Catalog>($"Catalogs/{id}");
            if (catalog == null)
            {
                return NotFound();
            }
            return View(catalog);
        }

        // POST: Catalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"Catalogs/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
