namespace DMS.Model;

public class Dokument
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public Benutzer Ersteller { get; set; }
    public DateTime ErstellDatum { get; set; }
    public byte[] Content { get; set; }
    public bool IsVisibleAllUser { get; set; }
    public List<string> Searchtags { get; set; }
}