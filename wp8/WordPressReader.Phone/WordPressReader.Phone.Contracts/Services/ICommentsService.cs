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
     
    }
}
