using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace SunLine.Community.Services.Search
{
    public class LuceneRamService : ILuceneService
    {
        public void AddDocument(Document document)
        {
            using (var indexWriter = CreateIndexWriter())
            {
                indexWriter.AddDocument(document);
            }
        }

        private IndexWriter CreateIndexWriter()
        {
            Directory directory = new RAMDirectory();

            var indexWriter = new IndexWriter(directory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), true,
                new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH));

            return indexWriter;
        }
    }
}