namespace API.Classes
{
    public class GeminiResponseBody
    {
        public List<Candidate> candidates { get; set; }
        public UsageMetadata usageMetadata { get; set; }
        public string modelVersion { get; set; }

        public GeminiResponseBody()
        {
            candidates = new List<Candidate>();
        }
    }

    public class Candidate
    {
        public Content content { get; set; }
        public string finishReason { get; set; }
        public double avgLogprobs { get; set; }
    }

    public class ResponseContent
    {
        public List<Part> parts { get; set; }
        public string role { get; set; }

        public ResponseContent()
        {
            parts = new List<Part>();
        }
    }

    public class ResponsePart
    {
        public string text { get; set; }
    }

    public class UsageMetadata
    {
        public int promptTokenCount { get; set; }
        public int candidatesTokenCount { get; set; }
        public int totalTokenCount { get; set; }
    }
}
