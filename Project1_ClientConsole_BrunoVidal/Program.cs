using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Project1_ClientConsole_BrunoVidal
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
            Console.ReadLine();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                // Prepare Client
                client.BaseAddress = new Uri("http://localhost:55450/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Prepare Response Object
                HttpResponseMessage response;
                // Get the Artworks
                try
                {
                    response = await client.GetAsync("api/artworks");
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code

                    List<Artwork> artworks = await response.Content.ReadAsAsync<List<Artwork>>();
                    foreach (Artwork a in artworks)
                    {
                        Console.WriteLine("ID:{0}\t{1}\tDescription:{2}\tEstimated Value:{3}\tType of Art:{4}", a.ID, a.Summary, a.Description, a.Value, a.ArtType.Type);
                    }
                }
                catch (HttpRequestException)
                {
                    throw;
                }
                //Add a artwork, update it and then delete it.
                // HTTP POST
                var artwork = new Artwork() { Name = "Haikyuu!!", Completed = new DateTime(2015, 04, 23),
                    Description = "Hinata Shouyou, a short middle school student, gained a sudden love of volleyball after watching a national championship match on TV.",
                    Value = 15000, ArtTypeID = 6 };
                Console.WriteLine("ID:{0}\t{1}\tDescription:{2}\tEstimated Value:{3}\tType of Art:{4}", artwork.ID, artwork.Summary, artwork.Description, artwork.Value, artwork.ArtTypeID);

                try
                {
                    response = await client.PostAsJsonAsync("api/artworks", artwork);
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code
                    //Get the new ID
                    Uri artworkUrl = response.Headers.Location;
                    int newID = Convert.ToInt32(artworkUrl.ToString().Split('/').Last());
                    Console.WriteLine("SUCCESSFULLY Uploaded:\r\nID:{0}\t{1}\tDescription:{2}\tEstimated Value:{3}\tType of Art:{4}", artwork.ID, artwork.Summary, artwork.Description, artwork.Value, artwork.ArtTypeID);

                    // HTTP PUT
                    //We have to get a copy of the artwork we added in order to have the correct row version
                    response = await client.GetAsync("api/artworks" + "/" + newID);
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code
                    Artwork addedartwork = await response.Content.ReadAsAsync<Artwork>();
                    Console.WriteLine("Record From Database:\r\nID:{0}\t{1}\tDescription:{2}\tEstimated Value:{3}\tType of Art:{4}", addedartwork.ID, addedartwork.Summary, addedartwork.Description, addedartwork.Value, addedartwork.ArtType.Type);

                    addedartwork.Value = 200000;   // Update Value
                    response = await client.PutAsJsonAsync(artworkUrl, addedartwork);
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code
                    Console.WriteLine("Edited:\r\nID:{0}\t{1}\tDescription:{2}\tEstimated Value:{3}\tType of Art:{4}", addedartwork.ID, addedartwork.Summary, addedartwork.Description, addedartwork.Value, addedartwork.ArtType.Type);

                    // HTTP DELETE
                    response = await client.DeleteAsync(artworkUrl);
                    response.EnsureSuccessStatusCode(); // Throw exception if not success code
                    Console.WriteLine("DELETED!\r\n - GET THE LIST AGAIN TO SHOW IT IS GONE\r\n");

                    //GET LIST AND SHOW IT AGAIN
                    response = await client.GetAsync("api/artworks");
                    if (response.IsSuccessStatusCode)
                    {
                        List<Artwork> artworks = await response.Content.ReadAsAsync<List<Artwork>>();
                        foreach (Artwork a in artworks)
                        {
                            Console.WriteLine("ID:{0}\t{1}\tDescription:{2}\tEstimated Value:{3}\tType of Art:{4}", a.ID, a.Summary, a.Description, a.Value, a.ArtType.Type);
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    throw;
                }
            }
        }
    }
}
