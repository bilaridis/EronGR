using System.Collections.Generic;
using System.Threading.Tasks;
using Watson.ORM.Core;

namespace EronNew.Models
{
    public interface IMyIndexEngine
    {
        int CurrentIndexingThreads { get; }
        List<string> IgnoreWords { get; set; }
        int MaxIndexingThreads { get; set; }
        IEnumerable<string> RawDatasIndexing { get; }
        char[] TermDelimiters { get; set; }
        int TermMinimumLength { get; set; }

        void Add(Document RawData);
        void Add(Document RawData, List<string> tags);
        Task AddAsync(Document RawData);
        Task AddAsync(Document RawData, List<string> tags);
        void DeleteRawDataByGuid(string guid);
        void DeleteRawDataByHandle(string handle);
        void Dispose();
        Document GetRawDataByGuid(string guid);
        Document GetRawDataByHandle(string handle);
        List<string> GetRawDataGuidsByTerms(List<string> terms);
        List<string> GetRawDataGuidsByTerms(List<string> terms, int? indexStart, int? maxResults, DbExpression filter);
        long GetTermReferenceCount(string term);
        bool IsGuidIndexed(string guid);
        bool IsHandleIndexed(string handle);
        List<Document> Search(List<string> terms);
        List<Document> Search(List<string> terms, int? indexStart, int? maxResults);
        List<Document> Search(List<string> terms, int? indexStart, int? maxResults, DbExpression filter);
    }
}