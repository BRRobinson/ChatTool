namespace ChatTool.Models.Settings;

public class JWTSettings
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Key { get; set; }
}
