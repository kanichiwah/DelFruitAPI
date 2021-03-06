﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.Collections.Specialized;
using System.Net.Http;

namespace DelfruitAPI
{
   public class roll
    {
        
        public static StringCollection getgame(StringCollection game)
        {
            Console.WriteLine("Delfruit API - Getting random game!");
            HtmlWeb hweb = new HtmlWeb();
            HtmlDocument doc;
            HtmlDocument doc2;
            string gamename = "";
            string url = "";
            try
            {
                //For pulling a specific string use <name of your stringcollection>[<index_number>]
                /////////////////////
                //  INDEX NUMBERS  //
                //     0 - URL     //
                //  1 - GameName   //
                //   2 - Rating    //
                // 3 - Difficulty  //
                //  4 - Download   //
                /////////////////////
                //load random game webpage
                doc = hweb.Load("http://www.delicious-fruit.com/ratings/game_details.php?random=1");
                //get game url
                url = hweb.ResponseUri.AbsoluteUri.ToString();
                game.Add(url);
                //get game title
                foreach (HtmlNode gametitle in doc.DocumentNode.SelectNodes("//h1"))
                {   
                    if (gametitle.InnerText.ToString().Contains("Delicious Fruit")){
                        
                    }
                    else
                    {
                        gamename = gametitle.InnerText.ToString();
                        game.Add(gamename);
                        
                    }
                   
                }
                //get rating
                foreach (HtmlNode rate in doc.DocumentNode.SelectNodes("//div[@class='rating']/div/span[@id='avgRating']"))
                {
                    game.Add(rate.InnerText.ToString());
                }
                //get difficulty
                foreach (HtmlNode diff in doc.DocumentNode.SelectNodes("//div[@class='rating']/div/span[@id='avgDifficulty']"))
                {
                    game.Add(diff.InnerText.ToString());
                }
                //get direct download URL, this is gonna need some redirects
                foreach(HtmlNode dfdownload in doc.DocumentNode.SelectNodes("//a[@class='standalone']"))
                {
                    doc2 = hweb.Load(dfdownload.Attributes["href"].Value.ToString());
                    foreach(HtmlNode wikidl in doc2.DocumentNode.SelectNodes("//div[@id='wikibody']/div/a"))
                    {
                        game.Add(wikidl.InnerText.ToString());
                    }
                }
                return game;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static StringCollection screenshots(StringCollection screenshots, string url)
        {

            HttpClient client = new HttpClient();
            WebClient webClient = new WebClient();
            var result = client.GetStringAsync(url).Result;
            var doc = new HtmlDocument();
            webClient.DownloadFile(url, "df.txt");
            doc.LoadHtml(System.IO.File.ReadAllText("df.txt"));
            foreach (HtmlNode screenshota in doc.DocumentNode.SelectNodes("//ul[@class='jcarousel-skin-pika']/li/a"))
            {
                //screenshots.Add("http://delicious-fruit.com" + screenshota.Attributes["href"].Value.ToString());
                string suburl = "http://delicious-fruit.com" + screenshota.Attributes["href"].Value.ToString();
                HtmlDocument docHtml = new HtmlWeb().Load(suburl);
                try {
                    foreach (HtmlNode image in docHtml.DocumentNode.SelectNodes("//div[@id='content']/img"))
                    {
                        screenshots.Add("http://delicious-fruit.com" + image.Attributes["src"].Value.ToString());
                    }
                }catch(System.NullReferenceException ex)
                {
                    screenshots.Clear();
                }
                }
            return screenshots;

        }
    }
}
