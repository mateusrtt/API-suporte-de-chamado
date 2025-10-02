namespace Infrastructure.OpenIA.Response
{
    public class ChatCompletionResponse
    {
        public List<Choice> Choices { get; set; } = new();
    }
    public class Choice
    {
        public Message Message { get; set; } = new();
    }

    public class Message
    {
        public string Content { get; set; } = string.Empty;
    }
}