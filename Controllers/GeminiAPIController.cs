using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.IO;
using API.Entities;
using System.Text.RegularExpressions;
using API.Classes;
using API.Data;
using Microsoft.AspNetCore.Identity;
using API.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace API.Controllers
{
    [ApiController]
    [Route("GeminiAPI")]
    public class GeminiAPIController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public GeminiAPIController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        private static readonly string _apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=AIzaSyA3Hcj5JgE70M3NXQOkx8ScZ65-QDUWn04";

        [HttpPost("ExtractMenuItems")]
        public async Task<string> Prompt()
        {
            string pdfFilePath = "C:\\Users\\JordonPillay\\Source\\Repos\\lunch-app-api\\Files\\13-17 January menu.pdf";
            string base64EncodedPdf = ConvertPdfToBase64(pdfFilePath);

            GeminiRequestBody request = new GeminiRequestBody
            {
                contents = new List<Content>
                {
                    new Content
                    {
                        parts = new List<Part>
                        {
                            new Part
                            {
                                inline_data = new InlineData
                                {
                                    mime_type = "application/pdf",
                                    data = base64EncodedPdf
                                }
                            },
                            new Part
                            {
                                text = "Analyze the pdf document and return a list of JSON objects like this: MenuItem {Date: \"string\", Caterer:\"EatFresh\",ItemName:\"string\", MainMeal: \"bool\"} " +
                                " for each of the menu items in the PDF. Format the Date part into this dd-mm-yyyy (2025). Concise answer and plain text only."
                            }
                        }
                    }
                }
            };

            using (HttpClient client = new HttpClient())
            {

                string jsonData = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_apiEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    GeminiResponseBody geminiResponse = JsonSerializer.Deserialize<GeminiResponseBody>(responseContent);
                    Console.WriteLine("POST request successful!");

                    //convert to list of entities
                    List<MenuItem> menuItems = JsonSerializer.Deserialize<List<MenuItem>>(ExtractJsonData(geminiResponse.candidates[0].content.parts[0].text));

                    foreach (var item in menuItems)
                    {
                        item.DayOfWeek = DateTime.ParseExact(item.Date, "dd-MM-yyyy", null, DateTimeStyles.None).ToString("dddd");
                        item.MainMeal = true;
                        _menuRepository.AddMenuItem(item);
                    }
                    return "All menu items added!";
                }
                else
                {
                    // Handle the error
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public static string ConvertPdfToBase64(string pdfFilePath)
        {
            try
            {
                byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);
                string base64String = Convert.ToBase64String(pdfBytes);
                return base64String;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting PDF to Base64: {ex.Message}");
                return string.Empty;
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
