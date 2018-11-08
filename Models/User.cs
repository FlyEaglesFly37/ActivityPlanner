using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Belt.Models
{
    public class User
    {
        public int UserId {get; set;}
        public string first_name {get; set;}
        public string last_name {get; set;}
        public string email {get; set;}
        public string password {get; set;}
        public List<Todo> Todos {get; set;}
        public List<Guest> Guests {get; set;}
        public User(){
            Todos = new List<Todo>();
            Guests = new List<Guest>();
        }
    }
}