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

/// <summary>
/// Global variables
/// </summary>
public static class Globals
{
    public static Random Rng = new Random();
    public static MouseState MouseCurrent;
    public static MouseState MouseLast;
    public static KeyboardState KbCurrent;
    public static KeyboardState KbLast;

    public static int Gold = 0;

    public static void RefreshInputs()
    {
        MouseLast = MouseCurrent;
        MouseCurrent = Mouse.GetState();
        KbLast = KbCurrent;
        KbCurrent = Keyboard.GetState();
    }
}
