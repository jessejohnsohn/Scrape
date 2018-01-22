using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using System.Net.Mail;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace lineup_generation
{
    class Program
    {
        static void Main(string[] args)
        {
            //Player.DownloadNerdsCSV();

            //using (WebClient client = new WebClient())
            //{
            //    string reply = client.DownloadString("https://rotogrinders.com/lineups/nba?site=fanduel");
            //    Regex regex = new Regex("data-away=(.*?) ");
            //    var v = regex.Match(reply);
            //    string s = v.Groups[1].ToString();
            //};

            string fileNamePath = @"C:\Users\jesse\Downloads\DFN NBA FD " + DateTime.Today.ToString("MM_dd") + ".csv";

            List<Player> players = File.ReadAllLines(fileNamePath)
                                           .Skip(1)
                                           .Select(v => Player.FromCsv(v))
                                           .ToList();

            List<string> games = new List<string>();

            foreach (var player in players)
            {
                player.TrueUsage = (player.USG * player.ProjMin) / 48;
                player.TruPer = (player.TrueUsage + player.PER) / 2;
                player.OppDvpDouble = Convert.ToDouble(player.OppDvP.Remove((player.OppDvP.Length - 1))) / 100;
                player.Value = player.Salary / (player.TruPer * (1 + player.OppDvpDouble));

                if (!games.Contains(player.Team)) { games.Add(player.Team); }

                if (player.ProjMin < 10 || player.TrueUsage < 5 || (player.OppDvpDouble < -.15 && player.TruPer < 5))
                {
                    player.Removed = true;
                }
            }

            List<Player> PlayerList = players.FindAll(x => x.Removed != true);

            PlayerList = Player.ScrapeRoto(games, PlayerList);

            var justinList = JustinViewModel.MapPlayersToVM(PlayerList.OrderBy(x => x.Pos).ToList());
            var topDogPG = justinList.Where(x => x.Pos == "PG").OrderByDescending(x => x.FDPoints).First();
            var topDogSG = justinList.Where(x => x.Pos == "SG").OrderByDescending(x => x.FDPoints).First();
            var topDogSF = justinList.Where(x => x.Pos == "SF").OrderByDescending(x => x.FDPoints).First();
            var topDogPF = justinList.Where(x => x.Pos == "PF").OrderByDescending(x => x.FDPoints).First();
            var topDogC = justinList.Where(x => x.Pos == "C").OrderByDescending(x => x.FDPoints).First();

            foreach (var item in justinList)
            {
                if (item.Pos == "PG")
                {
                    item.Autism = item.FDPoints - topDogPG.FDPoints + ((item.Value - topDogPG.Value) * 10);
                }
                if (item.Pos == "SG")
                {
                    item.Autism = item.FDPoints - topDogSG.FDPoints + ((item.Value - topDogSG.Value) * 10);
                }
                if (item.Pos == "SF")
                {
                    item.Autism = item.FDPoints - topDogSF.FDPoints + ((item.Value - topDogSF.Value) * 10);
                }
                if (item.Pos == "PF")
                {
                    item.Autism = item.FDPoints - topDogPF.FDPoints + ((item.Value - topDogPF.Value) * 10);
                }
                if (item.Pos == "C")
                {
                    item.Autism = item.FDPoints - topDogC.FDPoints + ((item.Value - topDogC.Value) * 10);
                }
            }

            justinList.OrderByDescending(x => x.Autism).ToList();


            string justinCSV = CsvSerializer.SerializeToString(justinList);

            File.WriteAllText("C:\\Users\\jesse\\justinDFS.csv", justinCSV);

            Player.SendRevisedCSV("C:\\Users\\jesse\\justinDFS.csv");
        }
    }
}
