using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lineup_generation.RotoGrinders
{
    public class RotoGrindersPlayer
    {
        public int Id { get; set; }
        public string Player { get; set; }
        public double Min { get; set; }
        public int FGM { get; set; }
        public int FGA { get; set; }
        public int FTM { get; set; }
        public int FTA { get; set; }
        public int Reb { get; set; }
        public int Ast { get; set; }
        public int Stl { get; set; }
        public int Blk { get; set; }
        public int To { get; set; }
        public int Pts { get; set; }
        public dynamic Fpts { get; set; }
        public double Usg { get; set; }
        public int Poss { get; set; }
        public int Foul { get; set; }
    }
}
