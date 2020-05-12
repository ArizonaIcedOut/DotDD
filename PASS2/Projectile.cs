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
    /// Janky class for projectiles fired by players or bosses
    /// </summary>
    class Projectile
    {
        /// <summary>
        /// Properties of a projectile. 
        /// ***NOTE*** Horizontal velocity is not needed, as the projectiles will only be moving vertically
        /// </summary>
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int VelocityY { get; set; }
        public Texture2D Img { get; set; }
        
        /// <summary>
        /// If Exists = false, then no projectile is currently on the screen
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// The type of attack the projectile represents
        /// </summary>
        public int Attack { get; set; }

        /// <summary>
        /// 0 = Spawned by player
        /// 1 = Spawned by boss
        /// </summary>
        public int Source { get; set; }

        public Projectile(int x, int y, int width, int height, Texture2D img, int source, int attack)
        {
            Exists = true;
            VelocityY = 8;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Img = img;
            Source = source;
            Attack = attack;
        }

        /// <summary>
        /// Translates the projectile by the Y velocity, in the opposite direction of the entity who shot it
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="source"></param>
        public static void MoveProjectile(Projectile proj, int source)
        {
            if (source == 0) proj.Y -= proj.VelocityY;
            else proj.Y += proj.VelocityY;
        }

        /// <summary>
        /// Checks collision between projectile and rectangle
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="rec"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ProjectileCollision(Projectile proj, Rectangle rec, int source)
        {
            // Conditions depend on whether projectile was spawned by player or boss 
            if (source == 0 && proj.Y <= rec.Y + rec.Height || source == 1 && proj.Y + proj.Height >= rec.Y)
            {
                // If the projectile was spawned by the player
                proj.Exists = false;
                return true;
            }
            else return false;
        }
    }
}
