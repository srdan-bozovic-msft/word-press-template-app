using MSC.Phone.Shared.Contracts.Repositories;
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
        Task<RepositoryResult<Article[]>> GetArticlesAsync(bool update, CancellationToken cancellationToken);
        Task<RepositoryResult<Article[]>> GetMoreArticlesAsync(CancellationToken cancellationToken);
        Task<RepositoryResult<Comment[]>> GetCommentsAsync(string url, CancellationToken cancellationToken); 
        Task<RepositoryResult<string>> GetArticleContentAsync(string articleUrl, CancellationToken cancellationToken);

    }
}
