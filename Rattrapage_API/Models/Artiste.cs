using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rattrapage_API.Models
{
    public class Artiste
    {
        public string Id { get; set; }
        public string Nom { get; set; }
        public string Genre { get; set; }
    }
}
