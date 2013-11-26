using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Contracts.Services
{
    public interface IConfigurationService
    {
        Task<string> GetFeedUrlAsync();
        Task<string> GetContentXPathAsync();
        Task<string> GetArticleTemplateAsync();
    }
}
