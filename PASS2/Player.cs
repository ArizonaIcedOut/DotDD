using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;
using MONO_TEST;

namespace MONO_TEST
{
    /// <summary>
    /// Holds all the information for the player, such as their stats and attacks
    /// </summary>
    class Player
    {
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int DamageRoll { get; set; }
        public int[,] DamageRange { get; set; }
        public int[] HealRange { get; set; }
        public int[] Accuracy { get; set; }
        public int HealAccuracy { get; set; }
        public bool IsPlayerTurn { get; set; }
        public int CritChance { get; set; }
        public int Score { get; set; }
        public Rectangle Rec { get; set; }
        public int DamageTaken { get; set; }

        /// <summary>
        /// 0 = Archer
        /// 1 = Wizard
        /// 2 = Priest
        /// 3 = Knight
        /// </summary>
        public int Character { get; set; }

        /// <summary>
        /// Creates a Player with default values
        /// </summary>
        public Player()
        {
            MaxHp = 150;
            Hp = MaxHp;
            CritChance = 10;
            Score = 0;

            Rec = new Rectangle(250, 520, 50, 50);
            if (Globals.Rng.Next(0, 1) == 0) IsPlayerTurn = true;
            else IsPlayerTurn = false;

            DamageRange = new int[3, 2] { { 15, 30 }, { 25, 40 }, { 25, 60 } };
            Accuracy = new int[] { 80, 65, 30 };
            HealRange = new int[] { 20, 80 };
            HealAccuracy = 75;

            DamageRoll = 0;
            DamageTaken = 0;
        }
    }
}
