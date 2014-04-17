using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Contracts.Services
{
    public interface IConfigurationService
    {
        string GetFeedUrl();
        string GetContentXPath();
        string GetCategoryFilter();
        Task<string> GetArticleTemplateAsync();
        string ProcessHtml(string html);
    }
}
