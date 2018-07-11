using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestTask.Models
{
    public class Sentence
    {
        public int Id { get;set; }
        public string Text { get; set; }
        public int Count { get; set; }
        public DateTime DateTime { get; set; }
    }
}