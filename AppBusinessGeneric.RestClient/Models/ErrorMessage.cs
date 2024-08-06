namespace AppBusinessGeneric.RestClient.Models;
public partial class ErrorMessage
{
    public string? Code { get; set; }

    public string? Description { get; set; }

    public string? Details { get; set; }

    public string? ErrorPath { get; set; }

    public override string ToString()
    {
        var msgParts = new List<string>();
        if (!string.IsNullOrWhiteSpace(Code))
        {
            msgParts.Add(Code);
        }
        if (!string.IsNullOrWhiteSpace(ErrorPath))
        {
            msgParts.Add(ErrorPath);
        }
        if (!string.IsNullOrWhiteSpace(Description))
        {
            msgParts.Add(Description);
        }
        return string.Join("; ", msgParts);
    }
}