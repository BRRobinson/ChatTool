namespace ChatTool.API.Models.Settings;

public class AppSettings
{
    public required ConnectionStrings ConnectionStrings { get; set; }
    public required JWTSettings JWT { get; set; }
}
