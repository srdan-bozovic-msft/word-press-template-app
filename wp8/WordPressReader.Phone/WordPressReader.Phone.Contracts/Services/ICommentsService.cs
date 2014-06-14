using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordPressReader.Phone.Contracts.Models;

namespace WordPressReader.Phone.Contracts.Services
{
    public interface ICommentsService
    {
        Task<Comment[]> GetCommentsAsync(Article article, CancellationToken cancellationToken);
        Task<int?> GetCommentsCountAsync(Article article, CancellationToken cancellationToken); 
        Task<ServiceResult<Comment>> CreateCommentAsync(Article article,  string authorName, string authorEmail, string message, string parent, CancellationToken cancellationToken);
     
    }
}
