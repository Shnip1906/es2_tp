using System.ComponentModel.DataAnnotations;

namespace Backend.Models

{
    public class LoginModel
    {
        [EmailAddress]
        public string username { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        
        [MinLength(6)]
        public  int tipo { get; set; }
    }
}