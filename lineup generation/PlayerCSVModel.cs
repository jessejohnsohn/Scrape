using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lineup_generation
{
    public class PlayerCSVModel
    {
        public string PG { get; set; }
        public int PGRating { get; set; }
        public int PGPrice { get; set; }
        public int PGValue { get { return PGPrice / PGRating; }}
        public string SG { get; set; }
        public int SGRating { get; set; }
        public int SGPrice { get; set; }
        public int SGValue { get { return SGPrice / SGRating; } }
        public string SF { get; set; }
        public int SFRating { get; set; }
        public int SFPrice { get; set; }
        public int SFValue { get { return SFPrice / SFRating; } }
        public string PF { get; set; }
        public int PFRating { get; set; }
        public int PFPrice { get; set; }
        public int PFValue { get { return PFPrice / PFRating; } }
        public string C { get; set; }
        public int CRating { get; set; }
        public int CPrice { get; set; }
        public int CValue { get { return CPrice / CRating; } }

        //public static PlayerCSVModel FromCsv(string csvLine)
        //{
        //    string[] values = csvLine.Split(',');
        //    var player = new Player();
        //    player.PlayerName = values[0];
        //    player.Pos = values[3];
        //    player.Salary = String.IsNullOrEmpty(values[4]) ? 0 : Convert.ToInt32(values[4]);
        //    player.USG = String.IsNullOrEmpty(values[9]) ? 0 : Convert.ToDouble(values[9]);
        //    player.PER = String.IsNullOrEmpty(values[10]) ? 0 : Convert.ToDouble(values[10]);
        //    player.ProjMin = String.IsNullOrEmpty(values[24]) ? 0 : Convert.ToDouble(values[24]);
        //    player.OppDvP = values[13];

        //    return player;
        //}
    }
}
