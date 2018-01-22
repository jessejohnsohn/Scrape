using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lineup_generation
{
    public class JustinViewModel
    {
        public double Autism { get; set; }
        public double Rating { get; set; }
        public string PlayerName { get; set; }
        public string Pos { get; set; }
        public double USG { get; set; }
        public double ProjMin { get; set; }
        public double? FDPPM { get; set; }
        public double FDPoints { get { return ProjMin * Convert.ToDouble(FDPPM); } }
        public int FDSalary { get; set; }
        public int Pace { get; set; }
        public double OppDvp { get; set; }
        public double PS { get; set; }
        public double Deff { get; set; }
        public int Rest { get; set; }
        public double Ceiling { get; set; }
        public double TrueUsage { get; set; }
        public double Value { get { return FDPoints / FDSalary; } }
        public static List<JustinViewModel> MapPlayersToVM(List<Player> players)
        {
            var list = new List<JustinViewModel>();

            foreach (var player in players)
            {
                var vm = new JustinViewModel();
                vm.Rating = Math.Round(((player.USG * player.ProjMin * player.FDPPM) / player.Salary) * 1000, 0);
                vm.PlayerName = player.PlayerName;
                vm.Pos = player.Pos;
                vm.Pace = Convert.ToInt32(player.Pace);
                vm.Deff = Convert.ToDouble(player.Deff);
                vm.Ceiling = player.Ceiling;
                vm.PS = player.PS;
                vm.Rest = player.Rest;
                vm.USG = player.USG;
                vm.ProjMin = player.ProjMin;
                vm.FDPPM = Math.Round(player.FDPPM, 2);
                vm.FDSalary = player.Salary;
                vm.OppDvp = Math.Round(player.OppDvpDouble, 2);
                vm.TrueUsage = Math.Round(player.TrueUsage, 2);
                list.Add(vm);
            }
            return list;
        }
    }
}
