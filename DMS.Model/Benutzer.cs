using System.ComponentModel.DataAnnotations;

namespace DMS.Model
{
    public class Benutzer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Der Name darf nicht leer sein.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Das Passwort darf nicht leer sein.")]
        [MinLength(8, ErrorMessage = "Das Passwort muss mindestens 8 Zeichen lang sein.")]
        public string Passwort { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
