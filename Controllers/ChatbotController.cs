using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using API.Classes;
using API.Interfaces;
using System.Text.RegularExpressions;
using API.Entities;
using System.Globalization;

namespace API.Controllers
{
    [ApiController]
    [Route("chatbot")]
    public class ChatbotController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public ChatbotController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        private static readonly string _apiEndpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={Environment.GetEnvironmentVariable("APIKey")}"; 

        [HttpPost("save-menu")]
        public async Task<IActionResult> SaveMenu([FromBody] TextRequest request)
        {
            if (string.IsNullOrEmpty(request.Text))
            {
                return BadRequest(new { status = "error", message = "No text provided" });
            }

            try
            {
                GeminiRequestBody geminiRequest = new GeminiRequestBody
                {
                    contents = new List<Content>
                    {
                        new Content
                        {
                            parts = new List<Part>
                            {
                                new Part
                                {
                                    text = $"Today is {DateTime.Now}." + "Using this menu " + request.Text + ". Return a list of JSON objects like this: MenuItem {Date: \"string\", Caterer:\"string\",ItemName:\"string\", IsMainMeal: \"bool\",IsSideMeal: \"bool\"} " +
                                           ".Format the Date part into this yyyy-MM-dd. Every meal is a main meal so set IsMainMeal to true and IsSideMeal to false. The Caterer name is EatFresh. " + $"The Date value should always start from the next week from now {DateTime.Now}." +
                                           "Concise answer and plain text only."
                                }
                            }
                        }
                    }
                };

                using (HttpClient client = new HttpClient())
                {
                    string jsonData = JsonSerializer.Serialize(geminiRequest);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(_apiEndpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        GeminiResponseBody geminiResponse = JsonSerializer.Deserialize<GeminiResponseBody>(responseContent);

                        string responseText = geminiResponse.candidates[0].content.parts[0].text;

                        List<MenuItem> menuItems = JsonSerializer.Deserialize<List<MenuItem>>(ExtractJsonData(geminiResponse.candidates[0].content.parts[0].text));

                        foreach (var item in menuItems)
                        {
                            item.DayOfWeek = DateTime.ParseExact(item.Date, "yyyy-MM-dd", null, DateTimeStyles.None).ToString("dddd");
                            item.IsMainMeal = true;
                            item.IsSideMeal = false;
                            item.Caterer = "EatFresh";
                            await _menuRepository.AddMenuItem(item);
                        }

                        return Ok(new { status = "success", data = responseText });
                    }
                    else
                    {
                        // Handle the error
                        string errorContent = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, new { status = "error", message = errorContent });
                    }
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        //user prompts to correct the menu or if it looks good then save it
        [HttpPost("chat-message")]
        public async Task<IActionResult> ChatMessage([FromBody] TextRequest request)
        {
            if (string.IsNullOrEmpty(request.Text))
            {
                return BadRequest(new { status = "error", message = "No text provided" });
            }

            try
            {
                // Create the Gemini request body with the specified prompt
                GeminiRequestBody geminiRequest = new GeminiRequestBody
                {
                    contents = new List<Content>
            {
                new Content
                {
                    parts = new List<Part>
                    {
                        new Part
                        {
                            text = "I'm providing you with a menu. " +
                            "Pull out the menu items for monday. " +
                            "Then prompt me to click the save button to save menu if mondays menu items look correct. If theres an issue say that whats submitted doesnt look correct" +
                            "Here is the menu: \n" + request.Text
                        }
                    }
                }
            }
                };

                using (HttpClient client = new HttpClient())
                {
                    string jsonData = JsonSerializer.Serialize(geminiRequest);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(_apiEndpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        GeminiResponseBody geminiResponse = JsonSerializer.Deserialize<GeminiResponseBody>(responseContent);

                        // Extract the response text from Gemini
                        string responseText = geminiResponse.candidates[0].content.parts[0].text;

                        // Return the response text
                        return Ok(new { status = "success", data = responseText });
                    }
                    else
                    {
                        // Handle the error
                        string errorContent = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, new { status = "error", message = errorContent });
                    }
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }

        }

        //user uploads a menu
        [HttpPost("upload-menu")]
        public async Task<IActionResult> UploadMenu([FromBody] TextRequest request)
        {
            if (string.IsNullOrEmpty(request.Text))
            {
                return BadRequest(new { status = "error", message = "No text provided" });
            }

            try
            {
                // Create the Gemini request body with the specified prompt
                GeminiRequestBody geminiRequest = new GeminiRequestBody
                {
                    contents = new List<Content>
            {
                new Content
                {
                    parts = new List<Part>
                    {
                        new Part
                        {
                            text = "I'm providing you with a menu. " +
                            "Pull out the menu items for monday. " +
                            "Then prompt me to click the save button to save menu if mondays menu items look correct. If theres an issue say that whats submitted doesnt look correct" +
                            "Here is the menu: \n" + request.Text
                        }
                    }
                }
            }
                };

                using (HttpClient client = new HttpClient())
                {
                    string jsonData = JsonSerializer.Serialize(geminiRequest);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(_apiEndpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        GeminiResponseBody geminiResponse = JsonSerializer.Deserialize<GeminiResponseBody>(responseContent);

                        // Extract the response text from Gemini
                        string responseText = geminiResponse.candidates[0].content.parts[0].text;

                        // Return the response text
                        return Ok(new { status = "success", data = responseText });
                    }
                    else
                    {
                        // Handle the error
                        string errorContent = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, new { status = "error", message = errorContent });
                    }
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        public static string ExtractJsonData(string input)
        {
            string pattern = @"^```json\n(.*)```\n$";
            Match match = Regex.Match(input, pattern, RegexOptions.Singleline);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return string.Empty; // Or throw an exception if no match is found
            }
        }
    }
}

public class TextRequest
{
    public string Text { get; set; }
}

//public static string ConvertPdfToBase64(string pdfFilePath)
//{
//    try
//    {
//        byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);
//        string base64String = Convert.ToBase64String(pdfBytes);
//        return base64String;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error converting PDF to Base64: {ex.Message}");
//        return string.Empty;
//    }
//}
