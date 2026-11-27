using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ExamenP2API.Models
{
    public class PokemonModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Nombre { get; set; }

        [Display(Name = "Altura (dm)")]
        [JsonProperty("height")]
        public int Altura { get; set; }

        [Display(Name = "Peso (hg)")]
        [JsonProperty("weight")]
        public int Peso { get; set; }

        public string ImagenUrl { get; set; }

        public string Tipos { get; set; }
    }
}