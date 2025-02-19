public class QuestionRequest
{
    public string? QuestionId { get; set; }
    public string? QuestionText { get; set; }
    public string? QuizzType { get; set; }
    public string? Image { get; set; }
    public bool ShowToggle { get; set; }
    public bool Essential { get; set; }
    public string? Description { get; set; }
    public string? Answers { get; set; }
}