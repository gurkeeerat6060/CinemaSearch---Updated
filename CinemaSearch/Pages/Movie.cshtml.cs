using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace CinemaSearch.Pages
{
    public class MovieModel : PageModel
    {
        public string JSONresponse { get; set; }
        public string Message { get; set; }
        public string movieTitle;
        public string movieRating;
        public string moviePosterURL;
        public string movieLanguage;
        public string movieBio;
        public string movieReleaseDate;

        public void OnGet()
        {

            string userEntry = Request.QueryString.Value;
            userEntry = userEntry.Split("=")[1];

            if (userEntry == null)
            {
                Response.Redirect("Index");
            }
            else
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.themoviedb.org/3/search/movie?api_key=a3bdaae66f8cf705750820e17c0e9471&query=" + userEntry);
                try
                {
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                        JSONresponse = reader.ReadToEnd();

                        dynamic parsing = JObject.Parse(JSONresponse);
                        try
                        {

                            movieTitle = parsing.results[0].original_title;
                            movieRating = parsing.results[0].vote_average;
                            moviePosterURL = "https://image.tmdb.org/t/p/w300/" + parsing.results[0].poster_path;
                            movieLanguage = parsing.results[0].original_language;
                            movieBio = parsing.results[0].overview;
                            movieReleaseDate = parsing.results[0].release_date;
                        }
                        catch (Exception e)
                        {
                            Message = e.ToString();
                        }
                    }
                }
                catch (WebException e)
                {
                    Message = e.ToString();
                }
            }
        }
    }
}
