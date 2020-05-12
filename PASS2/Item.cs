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
    /// Class for items purchased from the shop
    /// </summary>
    class Item
    {
        public int Level { get; set; }
        public int Cost { get; set; }
        public Texture2D[] ItemImg { get; set; }
        public Rectangle ShopRec { get; set; }
        public Rectangle HudRec { get; set; }

        public Item()
        {
            Level = 0;
            Cost = 50;
        }

        /// <summary>
        /// Function which changes item properties when purchased
        /// </summary>
        /// <param name="item"></param>
        public static void BuyItem(Item item)
        {
            item.Level++;
            Globals.Gold -= item.Cost;
            item.Cost += 50;
        }
    }
}
