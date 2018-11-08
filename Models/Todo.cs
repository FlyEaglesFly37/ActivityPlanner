using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Belt.Models
{
    public class Todo
    {
        [Key]
        public int ActivityId {get; set;}
        [Required]
        [MinLength(2, ErrorMessage="Title must be at least 2 characters long")]
        public string title {get; set;}
        [DataType(DataType.Date)]
        public DateTime date {get; set;}
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan time {get; set;}
        [Required(ErrorMessage="Duration of your event must be set")]
        public int duration {get; set;}
        public string duration_type {get; set;}

        [Required]
        [MinLength(10, ErrorMessage="Description must be greater than 10 characters")]
        public string description {get; set;}
        public int UserId {get; set;}
        public User user {get; set;}
        public List<Guest> Guests {get; set;}
        public Todo(){
            Guests = new List<Guest>();
        }
    }
}