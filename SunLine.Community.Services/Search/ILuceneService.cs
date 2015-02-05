using Lucene.Net.Documents;

namespace SunLine.Community.Services.Search
{
    public interface ILuceneService
    {
        void AddDocument(Document document);
    }
}
