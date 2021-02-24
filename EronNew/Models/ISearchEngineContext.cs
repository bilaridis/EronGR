using Microsoft.EntityFrameworkCore;

namespace EronNew.Models
{
    public interface ISearchEngineContext
    {
        DbSet<Idxentries> Idxentries { get; set; }
        DbSet<SearchRawData> SearchRawData { get; set; }
    }
}