using System.Collections.Generic;

namespace TransactionStore.Api.Models
{
    public class FileRestrictionsSettings
    {
        public int MaxSizeMb { get; set; }
        public List<string> SupportedFormats { get; set; } = new List<string>();
    }
}