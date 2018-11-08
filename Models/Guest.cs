using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Belt.Models
{
    public class Guest
    {
        public int GuestId {get; set;}
        public int UserId {get; set;}
        public User user {get; set;}
        public int ActivityId {get; set;}
        public Todo todo {get; set;}
    }
}
