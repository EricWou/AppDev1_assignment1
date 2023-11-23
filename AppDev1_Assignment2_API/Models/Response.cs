using System.Collections;

namespace AppDev1_Assignment2_API.Models
{
    public class Response
    {
        public int status_code { get; set; }
        public string status_message { get; set; }
        public Market product { get; set; }
        public List<Market> products { get; set; }
        public ArrayList purchase { get; set; }
    }
}
