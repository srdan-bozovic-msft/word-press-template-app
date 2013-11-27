using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Windows.Storage;
using WordPressReader.Phone.Contracts.Services;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using System.IO;

namespace WordPressReader.Phone.Services
{
    public class ConfigurationService : IConfigurationService
    {

        public async Task<string> GetFeedUrlAsync()
        {
            return "http://www.vitkigurman.com/feed/";
            //return "http://www.vecnidecak.com/feed/";
        }

        public async Task<string> GetContentXPathAsync()
        {
            return "//div[@class='pf-content']";
            //return "//div[@class='entry fix']";
        }

        public async Task<string> GetArticleTemplateAsync()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Templates/ArticleTemplate.html"));
            using (var s = await file.OpenReadAsync())
            {
                using(var reader = new StreamReader(s.AsStreamForRead()))
                {
                    return (await reader.ReadToEndAsync())
                        .Replace("{","{{").Replace("[[","{")
                        .Replace("}","}}").Replace("]]","}");
                }
            }
        }
    }
}
