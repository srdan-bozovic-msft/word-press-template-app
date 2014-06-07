﻿using MSC.Phone.Shared.Contracts.Repositories;
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
        Task<RepositoryResult<Article[]>> GetArticlesAsync(string category, bool update, CancellationToken cancellationToken);
        Task<RepositoryResult<Article[]>> GetMoreArticlesAsync(string category, CancellationToken cancellationToken);
        Task<RepositoryResult<Comment[]>> GetCommentsAsync(Article article, CancellationToken cancellationToken); 
        Task<RepositoryResult<string>> GetArticleContentAsync(string articleUrl, CancellationToken cancellationToken);
        Task<RepositoryResult<int?>> GetCommentsCountAsync(string articleUrl, CancellationToken cancellationToken);

    }
}
