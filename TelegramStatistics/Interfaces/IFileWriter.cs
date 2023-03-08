using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramStatistics.Models;

namespace TelegramStatistics.Interfaces
{
    public interface IFileWriter
    {
        public void WriteFile(Chat? chat, IEnumerable<WordCount> result, Dictionary<string, int> numberOfMessagesByUsers);
    }
}
