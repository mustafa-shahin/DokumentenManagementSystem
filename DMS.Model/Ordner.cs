namespace DMS.Model;

public class Ordner
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public List<string> SearchTags { get; set; }
}