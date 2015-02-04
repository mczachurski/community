using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;

namespace SunLine.Community.Services.Search
{
    public interface ILuceneService
    {
        void AddDocument(Document document);
    }
}
