using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramStatistics.Models
{
    public class UserWordCount
    {
        public string? UserName { get; set; } 
        public List<WordCount> UserWordCounts { get; set; } = new List<WordCount>();
    }
}
