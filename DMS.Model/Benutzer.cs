namespace DMS.Model;

public class Benutzer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Passwort { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
}