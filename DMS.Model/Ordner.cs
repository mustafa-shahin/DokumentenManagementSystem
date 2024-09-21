namespace DMS.Model;

public class Ordner
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public List<string> SearchTags { get; set; }
    public int BenutzerId { get; set; }
    public Benutzer Benutzer { get; set; }
}