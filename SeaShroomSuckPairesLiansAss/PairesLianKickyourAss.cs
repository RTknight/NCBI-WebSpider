using HtmlAgilityPack;
using SuperNoobHeardSeashroomFencingEveryNightAndAhhhWoKunLe;
using SuperNoobHeardSeashroomFencingEveryNightAndAhhhWoKunLe.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeaShroomSuckPairesLiansAss
{
    class PairesLianKickyourAss
    {
        /// <summary>
        /// NCBI论文的文章地址
        /// </summary>
        /// <param name="Url">关键词</param>
        public static void GetArticles()
        {
            //Task getArticleTask = new Task(() =>
            //{
            if (!Directory.Exists(NCBI.OutputPath)) Directory.CreateDirectory(NCBI.OutputPath);

            string booklistData = HttpHelper.HttpPost(NCBI.NCBIBooksUrl, PostData.BreastCancerFor10Years, true, "https://www.ncbi.nlm.nih.gov/books");
            HtmlDocument bookDoc = new HtmlDocument();
            bookDoc.LoadHtml(booklistData);

            var pageNode = bookDoc.DocumentNode.SelectSingleNode("//div[1]/div[1]/form/div[1]/div[5]/div[1]/div[3]/div[2]/h3[1]");
            pageNode.SelectSingleNode("./label").Remove();
            var pageCount = Convert.ToInt32(pageNode.InnerText.Trim()[3..]);

            Console.WriteLine($"共有{pageCount}页书本/期刊数据，正在获取第1页……");

            var lastQueryKeyNode = bookDoc.DocumentNode.SelectSingleNode("//div/input[@name=\"EntrezSystem2.PEntrez.DbConnector.LastQueryKey\"]");
            var bvLastQueryKeyNode = bookDoc.DocumentNode.SelectSingleNode("//div/input[@name=\"EntrezSystem2.PEntrez.Books.ResultsPanel.ResultsController.BVLastQueryKey\"]");

            string lastQueryKey = lastQueryKeyNode.GetAttributeValue("value", string.Empty);
            string bvLastQueryKey = bvLastQueryKeyNode.GetAttributeValue("value", string.Empty);
            NCBI.GetBookInfo(bookDoc);

            for (int i = 1; i < pageCount; i++)
            {
                Console.WriteLine($"正在获取第{i + 1}/{pageCount}页书本/期刊数据……");
                string requestParam = string.Format(PostData.BreastCancerFor10YearsSearch, i, (i + 1), (20 * (i - 1) + 1), lastQueryKey, bvLastQueryKey);
                string pageBooklistData = HttpHelper.HttpPost(NCBI.NCBIBooksUrl, requestParam, true, "https://www.ncbi.nlm.nih.gov/books");
                HtmlDocument pageBookDoc = new HtmlDocument();
                pageBookDoc.LoadHtml(pageBooklistData);
                NCBI.GetBookInfo(pageBookDoc);

                lastQueryKeyNode = pageBookDoc.DocumentNode.SelectSingleNode("//div/input[@name=\"EntrezSystem2.PEntrez.DbConnector.LastQueryKey\"]");
                bvLastQueryKeyNode = pageBookDoc.DocumentNode.SelectSingleNode("//div/input[@name=\"EntrezSystem2.PEntrez.Books.ResultsPanel.ResultsController.BVLastQueryKey\"]");

                lastQueryKey = lastQueryKeyNode.GetAttributeValue("value", string.Empty);
                bvLastQueryKey = bvLastQueryKeyNode.GetAttributeValue("value", string.Empty);
            }
            Console.WriteLine("所有数据均已获取完成！YOU WIN！！");
            //});
            //getArticleTask.Start();
        }
    }
}
