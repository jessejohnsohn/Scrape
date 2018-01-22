using HtmlAgilityPack;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using lineup_generation.RotoGrinders;

namespace lineup_generation
{
    public class Player
    {
        public string PlayerName { get; set; }
        public string Team { get; set; }
        public string Pos { get; set; }
        public int Salary { get; set; }
        public double USG { get; set; }
        public double PER { get; set; }
        public double ProjMin { get; set; }
        public string Pace { get; set; }
        public string OppDvP { get; set; }
        public double OppDvpDouble { get; set; }
        public double TrueUsage { get; set; }
        public double TruPer { get; set; }
        public double Value { get; set; }
        public double FDPPM { get; set; }
        public double DKPPM { get; set; }
        public double PS { get; set; }
        public double Deff { get; set; }
        public int Rest { get; set; }
        public double Ceiling { get; set; }
        public bool Removed { get; set; }

        public static Player FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            var player = new Player();
            player.PlayerName = values[0];
            player.Team = values[5];
            player.Pos = values[3];
            player.Salary = String.IsNullOrEmpty(values[4]) ? 0 : Convert.ToInt32(values[4]);
            player.PS = String.IsNullOrEmpty(values[8]) ? 0 : Convert.ToDouble(values[8]);
            player.Rest = String.IsNullOrEmpty(values[7]) ? 0 : Convert.ToInt32(values[7]);
            player.Deff = String.IsNullOrEmpty(values[12]) ? 0 : Convert.ToDouble(values[12]);
            player.Ceiling = String.IsNullOrEmpty(values[23]) ? 0 : Convert.ToDouble(values[23]);
            player.USG = String.IsNullOrEmpty(values[9]) ? 0 : Convert.ToDouble(values[9]);
            player.PER = String.IsNullOrEmpty(values[10]) ? 0 : Convert.ToDouble(values[10]);
            player.ProjMin = String.IsNullOrEmpty(values[24]) ? 0 : Convert.ToDouble(values[24]);
            player.OppDvP = values[13];
            player.Pace = values[11];

            return player;
        }

        public static List<Player> ScrapeRoto(List<string> games, List<Player> playerList)
        {
            Dictionary<string, int> teamNames = GetRotoTeamNames();
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var game in games)
            {
                var url = "https://rotogrinders.com/courtiq?range=season&on=&off=&opp_on=&opp_off=&team=" + teamNames[game].ToString() + "&opponent=&split=all";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var root = doc.DocumentNode;


                HtmlNodeCollection stats = doc.DocumentNode.SelectNodes("//script");
                string a = "";
                bool breakLoop = false;
                foreach (var i in stats)
                {
                    if (i.InnerHtml.Contains("data = [{"))
                    {
                        int indexStart = i.InnerHtml.IndexOf("data = [{");
                        int indexEnd = i.InnerHtml.IndexOf("}];");
                        a = i.InnerHtml.Substring(indexStart + 7, (indexEnd - indexStart - 5)).Trim();
                        breakLoop = true;
                    }
                    if (breakLoop) break;
                }
                var list = JsonConvert.DeserializeObject<List<RotoGrindersPlayer>>(a);

                foreach (var item in list)
                {
                    try
                    {
                        playerList.First(x => x.PlayerName == item.Player).USG = item.Usg;
                        playerList.First(x => x.PlayerName == item.Player).FDPPM = item.Fpts["43"] / item.Min;
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("Broke on..." + item.Player);
                    }
                }
            }

            return playerList;
        }
        public static Dictionary<string, int> GetRotoTeamNames()
        {
            Dictionary<string, int> teamNames = new Dictionary<string, int>();
            teamNames["ATL"] = 135;
            teamNames["BOS"] = 125;
            teamNames["BKN"] = 126;
            teamNames["CHA"] = 136;
            teamNames["CHI"] = 130;
            teamNames["CLE"] = 131;
            teamNames["DAL"] = 140;
            teamNames["DEN"] = 145;
            teamNames["DET"] = 132;
            teamNames["GSW"] = 150;
            teamNames["HOU"] = 141;
            teamNames["IND"] = 133;
            teamNames["LAC"] = 151;
            teamNames["LAL"] = 152;
            teamNames["MEM"] = 142;
            teamNames["MIA"] = 137;
            teamNames["MIL"] = 134;
            teamNames["MIN"] = 146;
            teamNames["NOR"] = 143;
            teamNames["NYK"] = 127;
            teamNames["OKC"] = 148;
            teamNames["ORL"] = 138;
            teamNames["PHI"] = 128;
            teamNames["POR"] = 147;
            teamNames["PHO"] = 153;
            teamNames["SAC"] = 154;
            teamNames["SAS"] = 144;
            teamNames["TOR"] = 129;
            teamNames["UTA"] = 149;
            teamNames["WAS"] = 139;

            return teamNames;
        }

        public byte[] RotoScrape(int teamId)
        {
            try
            {
                var url = "https://rotogrinders.com/courtiq?range=season&on=&off=&opp_on=&opp_off=&team=" + teamId + "&opponent=&split=all";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                HtmlNodeCollection players = doc.DocumentNode.SelectNodes("//option");

                List<Player> playerz = new List<Player>();

                //    for (int i = 1; i < (golfers.Count / 2); i++)
                //    {
                //        Golfers.Add(new Golfer()
                //        {
                //            Name = golfers[i].NextSibling.InnerText,
                //            PowerRanking = golfers[i].GetAttributeValue("Value", 0)
                //        });
                //    }

                //    string root = "http://www.golfstrat.com/golfstrat-fantasy-edge-player-comparison/";
                //    int lowestRank = Golfers.Max(x => x.PowerRanking);

                //    foreach (var golfer in Golfers)
                //    {
                //        string sURL;
                //        if (golfer.PowerRanking < lowestRank) { sURL = root + "?compare=" + golfer.PowerRanking + "," + lowestRank; }
                //        else { sURL = root + "?compare=" + lowestRank + ",1"; }

                //        string xPath = "/html[1]/body[1]/div[2]/div[2]/div[1]/div[1]/article[1]/div[1]/center[11]/table[1]/tbody[1]/tr";
                //        var sWEB = new HtmlWeb();
                //        var sDOC = sWEB.Load(sURL);

                //        HtmlNodeCollection otherStats1 = sDOC.DocumentNode.SelectNodes(xPath);
                //        string recentPerformance = otherStats1[0].ChildNodes[3].ChildNodes[0].InnerHtml.Trim();
                //        string courseHistory = otherStats1[1].ChildNodes[3].ChildNodes[0].InnerHtml.Trim();
                //        string courseFit = otherStats1[2].ChildNodes[3].ChildNodes[0].InnerHtml.Trim();
                //        string similarCourse = otherStats1[3].ChildNodes[3].ChildNodes[0].InnerHtml.Trim();
                //        golfer.RecentPerformance = (recentPerformance == "NR" || recentPerformance == "") ? 0 : Convert.ToInt32(recentPerformance);
                //        golfer.CourseHistory = (courseHistory == "NR" || courseHistory == "") ? 0 : Convert.ToInt32(courseHistory);
                //        golfer.CourseFit = (courseFit == "NR" || courseFit == "") ? 0 : Convert.ToInt32(courseFit);
                //        golfer.SimilarCourse = (similarCourse == "NR" || similarCourse == "") ? 0 : Convert.ToInt32(similarCourse);
                //    }

                //    string csvFile = CsvSerializer.SerializeToString<List<Golfer>>(Golfers);

                //    byte[] byteArray = Encoding.UTF8.GetBytes(csvFile);

                //    System.IO.File.WriteAllText(@"C:\Users\jesse\file.csv", csvFile);
                //    return byteArray;

                //}
                //catch (Exception ex)
                //{

                //}
            }
            catch(Exception ex)
            {

            }
            string csvFile1 = "";

            byte[] byteArr = Encoding.UTF8.GetBytes(csvFile1);

            return byteArr;
        }

        public static void DownloadNerdsCSV()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 320);

                driver.Navigate().GoToUrl("https://dailyfantasynerd.com/optimizer/fanduel/nba");

                //var btn = driver.FindElement(By.CssSelector("#input-username"));

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                string script = "var username = $('input')[0];" +
                                "var password = $('input')[1];" +
                                "var event = document.createEvent('HTMLEvents');" +
                                "$('#input-username').val('justinjfry@gmail.com');" +
                                "event.initEvent('input', true, true);" +
                                "username.dispatchEvent(event);" +
                                "$('#input-password').val('johnathon');" +
                                "password.dispatchEvent(event);" +
                                "$('#login-form').submit();";

                js.ExecuteScript(script);

                var btn = driver.FindElement(By.ClassName("exportData"));

                btn.Click();
                Thread.Sleep(600);
                driver.Close();
            }
        }

        public static void SendRevisedCSV(string attch)
        {
            var fromAddress = new MailAddress("jesserjohnson@gmail.com", "Jesse Johnson");
            var toAddress = new MailAddress("jesserjohnson@gmail.com", "To Name");
            const string fromPassword = "dhLxTn10";
            const string subject = "Today's CSV";
            const string body = "Here it is.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            Attachment attachment;
            attachment = new System.Net.Mail.Attachment(attch);
            var message = new MailMessage(fromAddress, toAddress);
            message.Subject = subject;
            message.Body = body;
            message.Attachments.Add(attachment);
            smtp.Send(message);
            message.Dispose();
            smtp.Dispose();
        }
    }
}
