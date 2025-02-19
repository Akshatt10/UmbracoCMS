namespace UmbracoProject.Models
{
    public class ProcedureRequest
{
    public string? ProcedureId { get; set; }  // Nullable Id
    public decimal Version { get; set; }
    public string? Icon { get; set; }  // Nullable Icon
    public string? Description { get; set; }  // Nullable Description
    public List<ChapterRequest> Chapters { get; set; } = new List<ChapterRequest>();
}

public class ChapterRequest
{
    public string? ChapterId { get; set; }  // Nullable Id
    public string? Description { get; set; }  // Nullable Description
    public string? Content { get; set; }  // Nullable Content
    public List<string> Images { get; set; } = new List<string>();
}

}
