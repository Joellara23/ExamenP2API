using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ExamenP2API.Models;
using Newtonsoft.Json.Linq;

namespace ExamenP2API.Controllers
{
    public class PokemonModelsController : Controller
    {
        private PokemonContext db = new PokemonContext();
        private static readonly HttpClient _httpClient = new HttpClient();

        // GET: PokemonModels?nombre=pikachu
        public async Task<ActionResult> Index(string nombre = "pikachu")
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                nombre = "pikachu";
            }

            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                string url = $"https://pokeapi.co/api/v2/pokemon/{nombre.ToLower().Trim()}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = $"No se encontró el Pokémon '{nombre}'.";
                    return View((PokemonModel)null);
                }

                string json = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(json);

                string imagen = (string)obj["sprites"]?["other"]?["official-artwork"]?["front_default"]
                                ?? (string)obj["sprites"]?["front_default"];

                string tipos = obj["types"] != null
                    ? string.Join(", ", obj["types"].Select(t => (string)t["type"]?["name"]))
                    : "N/A";

                PokemonModel pokemon = new PokemonModel
                {
                    Id = (int)obj["id"],
                    Nombre = (string)obj["name"],
                    Altura = (int)obj["height"],
                    Peso = (int)obj["weight"],
                    ImagenUrl = imagen,
                    Tipos = tipos
                };

                return View(pokemon);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al consultar la API: " + ex.Message;
                return View((PokemonModel)null);
            }
        }

        // GET: PokemonModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PokemonModel pokemonModel = db.PokemonModels.Find(id);
            if (pokemonModel == null)
            {
                return HttpNotFound();
            }
            return View(pokemonModel);
        }

        // GET: PokemonModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PokemonModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Altura,Peso,ImagenUrl,Tipos")] PokemonModel pokemonModel)
        {
            if (ModelState.IsValid)
            {
                db.PokemonModels.Add(pokemonModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pokemonModel);
        }

        // GET: PokemonModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PokemonModel pokemonModel = db.PokemonModels.Find(id);
            if (pokemonModel == null)
            {
                return HttpNotFound();
            }
            return View(pokemonModel);
        }

        // POST: PokemonModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Altura,Peso,ImagenUrl,Tipos")] PokemonModel pokemonModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pokemonModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pokemonModel);
        }

        // GET: PokemonModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PokemonModel pokemonModel = db.PokemonModels.Find(id);
            if (pokemonModel == null)
            {
                return HttpNotFound();
            }
            return View(pokemonModel);
        }

        // POST: PokemonModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PokemonModel pokemonModel = db.PokemonModels.Find(id);
            db.PokemonModels.Remove(pokemonModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}