using Microsoft.VisualBasic;

namespace API.Classes
{
    public class GeminiRequestBody
    {
        public List<Content> contents { get; set; }

        public GeminiRequestBody()
        {
            contents = new List<Content>();
        }
    }

    public class Content
    {
        public List<Part> parts { get; set; }

        public Content()
        {
            parts = new List<Part>();
        }
    }

    public class Part
    {
        public InlineData inline_data { get; set; }
        public string text { get; set; }
    }

    public class InlineData
    {
        public string mime_type { get; set; }
        public string data { get; set; }
    }
}
