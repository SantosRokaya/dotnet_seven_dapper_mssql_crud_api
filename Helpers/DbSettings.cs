namespace WebApi.Helpers;

public class DbSettings
{
    public string? Server { get; set; }
    public string? Database { get; set; }
    public string? UserId { get; set; }
    public string? Password { get; set; }
    public SiteDataObject? SiteData { get; set; }

}

public class SiteDataObject
{
    public string? Name { get; set; }
    public string? ApiDescription { get; set; }
}
