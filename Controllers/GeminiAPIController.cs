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
using System.Collections.Generic;
using System.Reflection.Metadata;

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

        private static readonly string _apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key="; // use gemini api key here

        [HttpPost("UploadMenuItems")]
        public async Task<List<MenuItem>> Prompt()
        {
            bool isConfirmed = false;
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
                                text = "Analyze the pdf document and return a list of JSON objects like this: MenuItem {Date: \"string\", Caterer:\"EatFresh\",ItemName:\"string\", IsMainMeal: \"bool\",IsSideMeal: \"bool\"} " +
                                " for each of the menu items in the PDF. Format the Date part into this dd-mm-yyyy (2025). Concise answer and plain text only. All items are MainMeals so set isMainMeal to true and isSideMeal to false"
,
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
                    Console.WriteLine("Gemini Request Successful!");

                    //convert to list of entities
                    List<MenuItem> menuItems = JsonSerializer.Deserialize<List<MenuItem>>(ExtractJsonData(geminiResponse.candidates[0].content.parts[0].text));

                    Console.WriteLine("Preview EatFresh");
                    foreach (var item in menuItems)
                    {
                        Console.WriteLine($"{item.ToString()} \n");
                    }
                    Console.WriteLine("Preview EatFresh");


                    //Review and ask if you want it added
                    isConfirmed = false;
                    Console.WriteLine("Would you like to confirm EatFresh upload? (Y/N)");
                    var input = Console.ReadLine();
                    isConfirmed = input.ToUpper() == "Y" ? true : false;
                    if (isConfirmed)
                    {
                        Console.WriteLine("User Confirmation! Upload Initiated...");
                        // Add
                        foreach (var item in menuItems)
                        {
                            item.DayOfWeek = DateTime.ParseExact(item.Date, "dd-MM-yyyy", null, DateTimeStyles.None).ToString("dddd");
                            item.IsMainMeal = true;
                            await _menuRepository.AddMenuItem(item);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Upload cancelled.");
                    }


                    return await _menuRepository.GetAllMenuItems();
                }
                else
                {
                    // Handle the error
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return new List<MenuItem>();
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