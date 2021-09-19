using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    static class WinnerDeterminingClass
    {
        public static string WinnerDetermining(int PlayerTurn, int GameTurn, int Length)
        {
            // false - player lose
            if (PlayerTurn == GameTurn) return "draw";
            int mark = (PlayerTurn + (Length - 1) / 2) % Length;
            if (mark > PlayerTurn)
            {
                if (GameTurn <= mark && GameTurn > PlayerTurn) return "loose";
                else return "win";
            }
            else
            {
                if (GameTurn > mark && GameTurn < PlayerTurn) return "win";
                else return "loose";
            }

        }
    }
}
