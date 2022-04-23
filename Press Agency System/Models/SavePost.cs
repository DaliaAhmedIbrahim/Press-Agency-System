using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Press_Agency_System.Models;

namespace Press_Agency_System.Models
{
    public class SavePost
    {
        public int Id { get; set; }
        public DateTime SaveDate { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

       
    }
}