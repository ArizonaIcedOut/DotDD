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
    /// Class for damage numbers which pop up above player/enemy in gameplay
    /// </summary>
    class DamageNumber
    {
        public int Roll { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public float Opacity { get; set; }
        public int VelocityY { get; set; }
        public int Duration { get; set; }

        /// <summary>
        /// 0 = Successful Attack
        /// 1 = Successful Heal
        /// 2 = Missed Attack
        /// 3 = Failed Heal
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Creates a new DamageNumber given the damage roll, type of move, and X and Y values for the drawn string
        /// </summary>
        /// <param name="roll"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        public DamageNumber(int roll, int x, int y, int type)
        {
            Roll = roll;
            X = x;
            Y = y;
            Opacity = 1f;
            VelocityY = 1;
            Duration = 120;
            Type = type;
        }

        /// <summary>
        /// Increases opacity, translates string, and reduces duration of the damage number
        /// </summary>
        /// <param name="num"></param>
        public static void UpdateDamageNumber(DamageNumber num)
        {
            num.Opacity -= (float)0.005;
            num.Y -= num.VelocityY;
            num.Duration -= 1;
        }
    }
}
