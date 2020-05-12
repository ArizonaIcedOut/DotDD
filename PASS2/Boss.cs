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
    /// Contains all the properties of the current boss
    /// </summary>
    class Boss
    {
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int DamageRoll { get; set; }
        public int Choice { get; set; }
        public int[,] DamageRange { get; set; }
        public int[] Accuracy { get; set; }
        public int[] HealRange { get; set; }
        public int HealAccuracy { get; set; }
        public int CritChance { get; set; }
        public int DamageTaken { get; set; }

        /// <summary>
        /// Restores all boss properties to their default values
        /// </summary>
        public Boss()
        {
            MaxHp = 50;
            Hp = MaxHp;

            CritChance = 10;
            DamageRange = new int[3, 2] { { 20, 35 }, { 30, 50 }, { 40, 80 } };
            Accuracy = new int[] { 90, 50, 25 };
            HealRange = new int[] { 30, 60 };
            HealAccuracy = 40;

            DamageRoll = 0;
            DamageTaken = 0;
        }
    }
}
