using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordPressReader.Phone.Contracts.Models;

namespace WordPressReader.Phone.Contracts.Repositories
{
    public interface IBlogRepository
    {
        Task<Article[]> GetArticlesAsync(CancellationToken cancellationToken);
        Task<string> GetArticleContentAsync(string articleUrl, CancellationToken cancellationToken);
    }
}
