using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MONO_TEST
{
    /// <summary>
    /// Can't make a 2D array with different variable types, so I had to use a list of objects instead
    /// UserRanking contains all the properties of a player's score on the leaderboards
    /// </summary>
    class UserRanking
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Character { get; set; }
        public int AtkLvl { get; set; }
        public int DexLvl { get; set;}
        public int HpLvl { get; set; }
        public int HealLvl { get; set; }

        /// <summary>
        /// Creates a UserRanking with the player's name, score, character, and items
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        /// <param name="character"></param>
        /// <param name="atkLvl"></param>
        /// <param name="dexLvl"></param>
        /// <param name="hpLvl"></param>
        /// <param name="healLvl"></param>
        public UserRanking(string name, int score, int character, int atkLvl, int dexLvl, int hpLvl, int healLvl)
        {
            Name = name;
            Score = score;
            Character = character;
            AtkLvl = atkLvl;
            DexLvl = dexLvl;
            HpLvl = hpLvl;
            HealLvl = healLvl;
        }
    }
}
