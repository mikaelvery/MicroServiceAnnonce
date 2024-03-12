using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchiveService.Helpers
{
    public class ArchiveParams
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public string PublicationDate { get; set; }
        public string Price { get; set; }
        public string OrderBy { get; set; }
        public string FilterBy { get; set; }
    }
}