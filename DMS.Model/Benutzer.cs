using System.ComponentModel.DataAnnotations;

namespace DMS.Model;

public class Benutzer
{
    public int Id { get; set; }
    public string Name { get; set; }
    [MinLength(8, ErrorMessage = "Das Passwort muss mindestens 8 Zeichen lang sein.")]
    public string Passwort { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
}