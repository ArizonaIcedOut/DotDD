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

/// <summary>
/// Author: Eric Pu
/// File Name: Game1.cs
/// Project Name: PASS2
/// Creation Date: March 28th, 2019
/// Modified Date: April 16th, 2019
/// Description: Simple RPG game, featuring leaderboard, shop, different characters and stages. 
/// </summary>

namespace PASS2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        /// <summary>
        /// Player Items
        /// </summary>
        static Item[] items;

        /// <summary>
        /// Player and Boss objects
        /// </summary>
        static Player player;
        static Boss boss;

        /// <summary>
        /// Current projectile on the screen
        /// </summary>
        static Projectile proj;

        /// <summary>
        /// 0 = Menu Screen 
        /// 1 = Gameplay Screen
        /// 2 = Loss Screen
        /// 3 = Victory Screen
        /// 4 = Leaderboard
        /// 5 = Shop 
        /// 6 = Class Select
        /// 7 = Name Selection
        /// </summary>
        static int gamestate = 0;

        /// <summary>
        /// Player name and maximum length of said name
        /// </summary>
        static string name;
        static int maxLength;

        /// <summary>
        /// Stage the player is currently on
        /// </summary>
        static int stage;

        /// <summary>
        /// Floats representing opacity of win and loss screens
        /// </summary>
        static float lossScreenOpacity;
        static float winScreenOpacity;

        /// <summary>
        /// List of DamageNumbers which pop up during gameplay
        /// </summary>
        static List<DamageNumber> damageNumbers;

        /// <summary>
        /// Leaderboard containing UserRankings, holding player stats
        /// </summary>
        List<UserRanking> rankings;

        /// <summary>
        /// Fonts
        /// </summary>
        SpriteFont numsFont;
        SpriteFont hudFont;
        SpriteFont smallFont;
        SpriteFont bigFont;

        /// <summary>
        /// Backgrounds
        /// </summary>
        Texture2D[] backgroundImg;

        /// <summary>
        /// Gameplay Graphics
        /// </summary>
        Texture2D hudImg;
        Texture2D[] bossImg;
        Texture2D[] charImg;
        Texture2D[] charBackImg;
        Texture2D fameImg;

        /// <summary>
        /// Shop Graphics
        /// </summary>
        Texture2D goldImg;
        Texture2D[] shopTooltipImg;

        /// <summary>
        /// Character Selection Assets
        /// </summary>
        Texture2D[] charSelectImg;
        Texture2D[] charTooltipImg;

        /// <summary>
        /// Misc Assets
        /// </summary>
        Texture2D outlineImg;
        Texture2D recImg;
        Texture2D blankRecImg;
        Texture2D healthBarImg;
        Texture2D logoImg;
        Texture2D monsterTooltipImg;

        /// <summary>
        /// UI Buttons
        /// </summary>
        Texture2D nextButtonImg;
        Texture2D menuButtonImg;
        Texture2D shopButtonImg;
        Texture2D startButtonImg;
        Texture2D rankingsButtonImg;
        Texture2D startButton2Img;
        Texture2D rankingsButton2Img;
        Texture2D menuButton2Img;

        /// <summary>
        /// Attack Buttons and Images
        /// </summary>
        Rectangle[] attacksRec;
        Texture2D[] attacksBtnImg;
        Texture2D[,] weaponsImg;
        Texture2D[,] playerProjImg;
        Texture2D[] bossProjImg;

        /// <summary>
        /// Rectangle for boss image
        /// </summary>
        static Rectangle bossRec;

        /// <summary>
        /// Character Select Buttons
        /// </summary>
        Rectangle[] charSelectRec;

        /// <summary>
        /// Menu Buttons
        /// </summary>
        Rectangle menuButtonRec;
        Rectangle startButtonRec;
        Rectangle nextButtonRec;
        Rectangle rankingsButtonRec;
        Rectangle menuButton2Rec;

        /// <summary>
        /// Shop Buttons
        /// </summary>
        Rectangle backgroundRec;

        /// <summary>
        /// Background music (can I get bonus marks if you recognize what this is from?)
        /// </summary>
        Song bgMusic;

        /// <summary>
        /// Various sound effects
        /// </summary>
        SoundEffect nextStageSnd;
        SoundEffect deathSnd;
        SoundEffect[] playerHitSnd;
        SoundEffect[] bossHitSnd;
        SoundEffect buyItemSnd;
        static SoundEffect buttonSnd;
        SoundEffect errorSnd;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            items = new Item[] { new Item(), new Item(), new Item(), new Item() };
            damageNumbers = new List<DamageNumber> { new DamageNumber(0, 0, 0, 0) };

            numsFont = Content.Load<SpriteFont>("Fonts/TestFont");
            hudFont = Content.Load<SpriteFont>("Fonts/hudfont");
            smallFont = Content.Load<SpriteFont>("Fonts/smallFont");
            bigFont = Content.Load<SpriteFont>("Fonts/bigfont");

            recImg = Content.Load<Texture2D>("Images/Misc/grayrec");
            blankRecImg = Content.Load<Texture2D>("Images/Misc/blankrec");
            logoImg = Content.Load<Texture2D>("Images/Misc/logo");
            hudImg = Content.Load<Texture2D>("Images/Misc/ui");
            outlineImg = Content.Load<Texture2D>("Images/Misc/outline");
            healthBarImg = Content.Load<Texture2D>("Images/Misc/hpbar");

            nextButtonImg = Content.Load<Texture2D>("Images/Buttons/nextstage");
            startButtonImg = Content.Load<Texture2D>("Images/Buttons/startgame");
            startButton2Img = Content.Load<Texture2D>("Images/Buttons/startbutton2");
            shopButtonImg = Content.Load<Texture2D>("Images/Buttons/shop");
            menuButtonImg = Content.Load<Texture2D>("Images/Buttons/mainmenu");
            menuButton2Img = Content.Load<Texture2D>("Images/Buttons/mainmenu2");
            rankingsButtonImg = Content.Load<Texture2D>("Images/Buttons/leaderboard");
            rankingsButton2Img = Content.Load<Texture2D>("Images/Buttons/leaderboard2");

            monsterTooltipImg = Content.Load<Texture2D>("Images/Misc/monstertooltip");

            bossImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Sprites/imp"),
                Content.Load<Texture2D>("Images/Sprites/beholder"),
                Content.Load<Texture2D>("Images/Sprites/oryx"),
                Content.Load<Texture2D>("Images/Sprites/archmage") };

            charSelectImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Misc/archer"),
                Content.Load<Texture2D>("Images/Misc/wizard"),
                Content.Load<Texture2D>("Images/Misc/priest"),
                Content.Load<Texture2D>("Images/Misc/knight") };

            charTooltipImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Misc/archertooltip"),
                Content.Load<Texture2D>("Images/Misc/wizardtooltip"),
                Content.Load<Texture2D>("Images/Misc/priesttooltip"),
                Content.Load<Texture2D>("Images/Misc/knighttooltip") };

            charImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Sprites/archer"),
                Content.Load<Texture2D>("Images/Sprites/wizard"),
                Content.Load<Texture2D>("Images/Sprites/priest"),
                Content.Load<Texture2D>("Images/Sprites/knight") };

            charBackImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Sprites/archerback"),
                Content.Load<Texture2D>("Images/Sprites/wizardback"),
                Content.Load<Texture2D>("Images/Sprites/priestback"),
                Content.Load<Texture2D>("images/Sprites/knightback") };

            goldImg = Content.Load<Texture2D>("Images/Sprites/gold");
            fameImg = Content.Load<Texture2D>("Images/Sprites/fame");

            items[0].ItemImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Sprites/attackcandy"),
                Content.Load<Texture2D>("Images/Sprites/attack"),
                Content.Load<Texture2D>("Images/Sprites/attackbigpot") };

            items[1].ItemImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Sprites/dexcandy"),
                Content.Load<Texture2D>("Images/Sprites/dex"),
                Content.Load<Texture2D>("Images/Sprites/dexbigpot") };

            items[2].ItemImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Sprites/lifecandy"),
                Content.Load<Texture2D>("Images/Sprites/life"),
                Content.Load<Texture2D>("Images/Sprites/lifebigpot") };

            items[3].ItemImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Sprites/hpminor"),
                Content.Load<Texture2D>("Images/Sprites/hppot"),
                Content.Load<Texture2D>("Images/Sprites/hpelixir"),
                Content.Load<Texture2D>("Images/Sprites/startingpotion") };

            shopTooltipImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Misc/attacktooltip"),
                Content.Load<Texture2D>("Images/Misc/dextooltip"),
                Content.Load<Texture2D>("Images/Misc/lifetooltip"),
                Content.Load<Texture2D>("Images/Misc/healtooltip") };

            backgroundImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Backgrounds/ocean"),
                Content.Load<Texture2D>("Images/Backgrounds/beach"),
                Content.Load<Texture2D>("Images/Backgrounds/fields"),
                Content.Load<Texture2D>("Images/Backgrounds/glands2"),
                Content.Load<Texture2D>("Images/Backgrounds/fields2"),
                Content.Load<Texture2D>("Images/Backgrounds/forest"),
                Content.Load<Texture2D>("Images/Backgrounds/glands") };

            attacksBtnImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Buttons/attack1"),
                Content.Load<Texture2D>("Images/Buttons/attack2"),
                Content.Load<Texture2D>("Images/Buttons/attack3"),
                Content.Load<Texture2D>("Images/Buttons/heal") };

            weaponsImg = new Texture2D[4, 3] {
                // Archer
                { Content.Load<Texture2D>("Images/Sprites/dbow"),
                Content.Load<Texture2D>("Images/Sprites/coralbow"),
                Content.Load<Texture2D>("Images/Sprites/quiver") } ,
                // Wizard
                { Content.Load<Texture2D>("Images/Sprites/epstaff"),
                Content.Load<Texture2D>("Images/Sprites/esbenstaff"),
                Content.Load<Texture2D>("Images/Sprites/spell")} ,
                // Priest
                { Content.Load<Texture2D>("Images/Sprites/cwand"),
                Content.Load<Texture2D>("Images/Sprites/bulwark"),
                Content.Load<Texture2D>("Images/Sprites/tome")} ,
                // Knight
                { Content.Load<Texture2D>("Images/Sprites/cutlass"),
                Content.Load<Texture2D>("Images/Sprites/illuminatisword"),
                Content.Load<Texture2D>("Images/Sprites/shield") } };

            playerProjImg = new Texture2D[4, 3] {
                // Archer
                { Content.Load<Texture2D>("Images/Sprites/dbowproj"),
                Content.Load<Texture2D>("Images/Sprites/cbowproj"),
                Content.Load<Texture2D>("Images/Sprites/quiverproj") } ,
                // Wizard
                { Content.Load<Texture2D>("Images/Sprites/epstaffproj"),
                Content.Load<Texture2D>("Images/Sprites/esbenstaffproj"),
                Content.Load<Texture2D>("Images/Sprites/spellproj")} ,
                // Priest
                { Content.Load<Texture2D>("Images/Sprites/cwandproj"),
                Content.Load<Texture2D>("Images/Sprites/bulwarkproj"),
                Content.Load<Texture2D>("Images/Sprites/tomeproj")} ,
                // Knight
                { Content.Load<Texture2D>("Images/Sprites/cutlassproj"),
                Content.Load<Texture2D>("Images/Sprites/illuminatiswordproj"),
                Content.Load<Texture2D>("Images/Sprites/shield") } };

            bossProjImg = new Texture2D[] {
                Content.Load<Texture2D>("Images/Sprites/impproj"),
                Content.Load<Texture2D>("Images/Sprites/beholderproj"),
                Content.Load<Texture2D>("Images/Sprites/oryxproj"),
                Content.Load<Texture2D>("Images/Sprites/archshot") };

            bgMusic = Content.Load<Song>("Audio/ramranch");
            deathSnd = Content.Load<SoundEffect>("Audio/losssound");
            buyItemSnd = Content.Load<SoundEffect>("Audio/buyitem");
            nextStageSnd = Content.Load<SoundEffect>("Audio/nextstage");
            errorSnd = Content.Load<SoundEffect>("Audio/error");
            buttonSnd = Content.Load<SoundEffect>("Audio/buttonclick");

            playerHitSnd = new SoundEffect[] {
                Content.Load<SoundEffect>("Audio/archerhit"),
                Content.Load<SoundEffect>("Audio/wizardhit"), 
                Content.Load<SoundEffect>("Audio/priesthit"),
                Content.Load<SoundEffect>("Audio/knighthit") };

            bossHitSnd = new SoundEffect[] {
                Content.Load<SoundEffect>("Audio/imphit"),
                Content.Load<SoundEffect>("Audio/beholderhit"),
                Content.Load<SoundEffect>("Audio/oryxhit"),
                Content.Load<SoundEffect>("Audio/archmagehit") };

            startButtonRec = new Rectangle(310, 520, 180, 80);
            menuButtonRec = new Rectangle(300, 200, 200, 100);
            nextButtonRec = new Rectangle(200, 400, 400, 100);
            rankingsButtonRec = new Rectangle(50, 520, 180, 80);
            menuButton2Rec = new Rectangle(310, 520, 180, 80); 

            attacksRec = new Rectangle[] {
                new Rectangle(575, 280, 205, 50),
                new Rectangle(575, 360, 205, 50),
                new Rectangle(575, 430, 205, 50),
                new Rectangle(575, 500, 205, 50) };

            bossRec = new Rectangle(250, 50, 50, 50);

            charSelectRec = new Rectangle[] {
                new Rectangle(25, 100, 150, 150),
                new Rectangle(225, 100, 150, 150),
                new Rectangle(425, 100, 150, 150),
                new Rectangle(625, 100, 150, 150) };

            // Rectangles for items must be defined here for code to work
            for (int i = 0; i < items.Length; i++)
            {
                items[i].ShopRec = new Rectangle(125 + (i * 150), 50, 100, 100);
                items[i].HudRec = new Rectangle(570 + (55 * i), 175, 50, 50);
            }

            backgroundRec = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            maxLength = 8;

            // Rankings are hardcoded in and reset whenever the game is booted, didn't have time to figure out the file I/O :<
            rankings = new List<UserRanking>() {
                new UserRanking("DAVID", 159, 3, 3, 3, 3, 3),
                new UserRanking("LEON", 84, 2, 3, 2, 2, 3),
                new UserRanking("SEAN", 69, 1, 3, 2, 3, 2),
                new UserRanking("SHON", 42, 0, 3, 2, 2, 0),
                new UserRanking("SHON", 37, 3, 3, 1, 0, 0),
                new UserRanking("ROY", 33, 0, 2, 0, 0, 0),
                new UserRanking("BULAT", 14, 2, 1, 0, 0, 1),
                new UserRanking("YONI", 3, 3, 0, 0, 0, 1) };

            // Creates an empty projectile so the code compiles 
            proj = new Projectile(0, 0, 0, 0, recImg, 0, 0);
            proj.Exists = false;

            MediaPlayer.Play(bgMusic);
            MediaPlayer.Volume = .1f;
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);

            Globals.RefreshInputs();

            // Update for each screen of the game
            switch (gamestate)
            {
                case 0: // Menu
                    // Buttons
                    if (CheckButton(startButtonRec))
                    {
                        // Start button
                        ResetGame();
                        gamestate = 6;
                        buttonSnd.CreateInstance().Play();
                    }
                    if (CheckButton(rankingsButtonRec))
                    {
                        // Rankings button
                        gamestate = 4;
                        buttonSnd.CreateInstance().Play();
                    }
                    break;
                case 1: // Gameplay
                    // If a projectile does not exist, combat progresses
                    if (!proj.Exists)
                    {
                        if (player.IsPlayerTurn)
                        {
                            // Loops through each of the attacks, besides heal
                            for (int i = 0; i < attacksRec.Length - 1; i++)
                            {
                                // Creates a projectile according to the attack used
                                if (CheckButton(attacksRec[i])) proj = new Projectile(player.Rec.X + (player.Rec.Width / 2), player.Rec.Y,
                                                                                      playerProjImg[player.Character, i].Width, playerProjImg[player.Character, i].Height,
                                                                                      playerProjImg[player.Character, i], 0, i);
                            }
                            // If player used heal
                            if (CheckButton(attacksRec[3]))
                            {
                                // Progresses combat, with the heal
                                // Seperate code needed since heal does not create a projectile
                                PlayerTurn(3);

                                buyItemSnd.CreateInstance().Play();

                                // If the boss is not within healing range, the boss will create a projectile and then attack
                                if (boss.Hp > (double)boss.MaxHp * .25) proj = new Projectile(bossRec.X + (bossRec.Width / 2), bossRec.Y,
                                                                                      bossProjImg[stage].Width, bossProjImg[stage].Height,
                                                                                      bossProjImg[stage], 1, 0);
                                // Otherwise, BossTurn will force boss to use heal
                                else
                                {
                                    BossTurn();
                                    buyItemSnd.CreateInstance().Play();
                                }
                            }
                        }
                    }
                    // If the projectile does exist
                    else
                    {
                        // If the projectile is spawned by the player, and it intersects with the boss rectangle
                        if (proj.Source == 0 && Projectile.ProjectileCollision(proj, bossRec, 0))
                        {
                            // Progresses the combat using the attack used by the player
                            PlayerTurn(proj.Attack);

                            // Plays hit sound
                            bossHitSnd[stage].CreateInstance().Play();

                            // Creates a new projectile for the boss, progressing combat
                            proj = new Projectile(bossRec.X + (bossRec.Width / 2), 
                                                  bossRec.Y + bossRec.Height,
                                                  bossProjImg[stage].Width, 
                                                  bossProjImg[stage].Height,
                                                  bossProjImg[stage], 1, 0);

                        }
                        // If the projectile was spawned by the boss, and it intersects with the player
                        if (proj.Source == 1 && Projectile.ProjectileCollision(proj, player.Rec, 1))
                        {
                            // Progresses combat
                            BossTurn();

                            // Makes proj.Exists false, allowing player to make their turn next
                            proj.Exists = false;

                            // Plays player hit sound
                            playerHitSnd[player.Character].CreateInstance().Play();
                        }
                        // Otherwise, projectile is updated
                        else Projectile.MoveProjectile(proj, proj.Source);
                    }

                    // Goes through all the damage numbers and if their durations have ended, they are removed. Otherwise, they are updated.
                    for (int i = 0; i < damageNumbers.Count; i++)
                    {
                        if (damageNumbers[i].Duration < 0)
                        {
                            damageNumbers.RemoveAt(i);
                            i -= 1;
                        }
                        else DamageNumber.UpdateDamageNumber(damageNumbers[i]);
                    }

                    // Loss condition
                    if (player.Hp <= 0)
                    {
                        gamestate = 2;
                        deathSnd.CreateInstance().Play();
                    }
                    // Win Condition
                    else if (boss.Hp <= 0)
                    {
                        gamestate = 3;
                        nextStageSnd.CreateInstance().Play();
                    }
                    break;
                case 2: // Loss Screen

                    // Menu button
                    if (CheckButton(menuButtonRec))
                    {
                        gamestate = 4;
                        lossScreenOpacity = 0;
                        buttonSnd.CreateInstance().Play();
                    }

                    // Increases loss screen opacity
                    if (lossScreenOpacity < .5f) lossScreenOpacity += 0.015f;
                    break;
                case 3: // Win Screen

                    // Next stage button
                    if (CheckButton(menuButtonRec))
                    {
                        // If it was the last stage
                        if (stage == 3)
                        {
                            // Changes game to leaderboard
                            gamestate = 4;

                            // Loops through each of the player rankings from the top
                            for (int i = 0; i < rankings.Count; i++)
                            {
                                // If the player's score is lower than your own, the new ranking is placed in that position
                                if (rankings[i].Score < player.Score)
                                {
                                    rankings.Insert(i, new UserRanking(name, player.Score, player.Character, items[0].Level, items[1].Level, items[2].Level, items[3].Level));
                                    rankings.RemoveAt(rankings.Count - 1);
                                    break;
                                }
                            }
                        }
                        // Otherwise, progresses to shop
                        else gamestate = 5;

                        // Resets win screen opacity
                        winScreenOpacity = 0;

                        buttonSnd.CreateInstance().Play();
                    }

                    // Increases opacity of win screen
                    if (winScreenOpacity < .5f) winScreenOpacity += 0.015f;
                    break;
                case 4: // Leaderboard

                    // Menu button
                    if (CheckButton(menuButton2Rec))
                    {
                        gamestate = 0;
                        buttonSnd.CreateInstance().Play();
                    }
                    break;
                case 5: // Shop

                    // Next stage button
                    if (CheckButton(nextButtonRec))
                    {
                        buttonSnd.CreateInstance().Play();
                        NextStage();
                    }

                    // Buying potions
                    if (CheckButton(items[0].ShopRec) && Globals.Gold >= items[0].Cost && items[0].Level < 3)
                    {
                        // Buying Attack Potion
                        Item.BuyItem(items[0]);
                        for (int i = 0; i < player.DamageRange.GetLength(0); i++)
                        {
                            player.DamageRange[i, 0] += 15;
                            player.DamageRange[i, 1] += 15;
                            buyItemSnd.CreateInstance().Play();
                        }
                    }
                    else if (CheckButton(items[1].ShopRec) && Globals.Gold >= items[1].Cost && items[1].Level < 3)
                    {
                        // Buying Dexterity Potion
                        Item.BuyItem(items[1]);
                        for (int i = 0; i < player.Accuracy.Count(); i++)
                        {
                            player.Accuracy[i] += 15;
                            if (player.Accuracy[i] > 100) player.Accuracy[i] = 100;
                            buyItemSnd.CreateInstance().Play();
                        }
                    }
                    else if (CheckButton(items[2].ShopRec) && Globals.Gold >= items[2].Cost && items[2].Level < 3)
                    {
                        // Buying Health Potion
                        Item.BuyItem(items[2]);
                        player.MaxHp += 50;
                        buyItemSnd.CreateInstance().Play();
                    }
                    else if (CheckButton(items[3].ShopRec) && Globals.Gold >= items[3].Cost && items[3].Level < 3)
                    {
                        // Buying Healing Potion
                        Item.BuyItem(items[3]);
                        player.HealRange[0] += 20;
                        player.HealRange[1] += 20;
                        player.HealAccuracy += 15;

                        if (player.HealAccuracy > 100) player.HealAccuracy = 100;
                        buyItemSnd.CreateInstance().Play();
                    }
                    break;
                case 6: // Character Selection

                    // Loops through each of the 4 character select buttons
                    for (int i = 0; i < charSelectRec.Length; i++)
                    {
                        // Selects the character and starts the game if the player clicks on the button
                        if (CheckButton(charSelectRec[i]))
                        {
                            player.Character = i;
                            gamestate = 7;
                            buttonSnd.CreateInstance().Play();
                            break;
                        }
                    }
                    break;
                case 7: // Name Selection

                    name = KeyboardInput(name, maxLength);

                    // If player tries to start game
                    if (CheckButton(startButtonRec))
                    {
                        // If the player enters their name, game starts
                        if (name.Length > 0)
                        {
                            gamestate = 1;
                            buttonSnd.CreateInstance().Play();
                        }
                        // Otherwise, it does nothing and plays an error sound
                        else errorSnd.CreateInstance().Play();
                    }
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            // Draws the various screens of the game
            switch (gamestate)
            {
                case 0: // Menu Screen
                    spriteBatch.Draw(backgroundImg[3], backgroundRec, Color.White);
                    spriteBatch.Draw(logoImg, new Rectangle(100, 40, 600, 120), Color.White);
                    spriteBatch.Draw(recImg, new Rectangle(0, 520, 800, 80), Color.White);

                    // Buttons
                    if (CheckMouseInside(startButtonRec)) spriteBatch.Draw(startButton2Img, startButtonRec, Color.Yellow);
                    else spriteBatch.Draw(startButton2Img, startButtonRec, Color.White);

                    if (CheckMouseInside(rankingsButtonRec)) spriteBatch.Draw(rankingsButton2Img, rankingsButtonRec, Color.Yellow);
                    else spriteBatch.Draw(rankingsButton2Img, rankingsButtonRec, Color.White);
                    break;
                case 1: // Gameplay Menu
                    spriteBatch.Draw(backgroundImg[stage], backgroundRec, Color.White);

                    // UI Elements
                    spriteBatch.Draw(hudImg, new Rectangle(550, 0, 250, 600), Color.White);

                    spriteBatch.Draw(charImg[player.Character], new Rectangle(570, 10, 40, 40), Color.White);
                    spriteBatch.DrawString(hudFont, name, new Vector2(630, 15), Color.White);

                    spriteBatch.DrawString(hudFont, "DAMAGE TAKEN: " + Convert.ToString(player.DamageTaken), new Vector2(10, 560), Color.Gold);
                    spriteBatch.DrawString(hudFont, "DAMAGE TAKEN: " + Convert.ToString(boss.DamageTaken), new Vector2(10, 20), Color.Gold);

                    if (!player.IsPlayerTurn) spriteBatch.DrawString(hudFont, "ENEMY TURN", new Vector2(370, 560), Color.Gold);
                    else spriteBatch.DrawString(hudFont, "PLAYER TURN", new Vector2(370, 560), Color.Gold);

                    spriteBatch.Draw(fameImg, new Rectangle(570, 60, 30, 30), Color.White);
                    spriteBatch.DrawString(hudFont, Convert.ToString(player.Score), new Vector2(610, 60), Color.White);

                    // Player HP Bar
                    spriteBatch.Draw(blankRecImg, new Rectangle(570, 105, Convert.ToInt16(220.0 * ((double)player.Hp / player.MaxHp)), 30), new Color(244, 66, 66));
                    spriteBatch.DrawString(hudFont, "HP", new Vector2(580, 105), Color.White);
                    spriteBatch.DrawString(hudFont, Convert.ToString(player.Hp) + " / " + player.MaxHp, new Vector2(640, 105), Color.Yellow);

                    // Boss HP Bar
                    spriteBatch.Draw(blankRecImg, new Rectangle(bossRec.X, bossRec.Y + bossRec.Height + 25, Convert.ToInt16(bossRec.Width * ((double)boss.Hp/boss.MaxHp)), 25), Color.Red);
                    spriteBatch.Draw(healthBarImg, new Rectangle(bossRec.X, bossRec.Y + bossRec.Height + 25, bossRec.Width, 25), Color.White);
                    if (boss.Hp > 0) spriteBatch.DrawString(hudFont, Convert.ToString(boss.Hp) + " / " + boss.MaxHp, new Vector2(bossRec.X, bossRec.Y + bossRec.Height + 55), Color.Gold);
                    else spriteBatch.DrawString(hudFont, "0 / " + boss.MaxHp, new Vector2(bossRec.X, bossRec.Y + bossRec.Height + 55), Color.Gold);

                    // Attack buttons and outline over selected button
                    for (int i = 0; i < attacksRec.Length; i++)
                    {
                        // Attack buttons
                        spriteBatch.Draw(attacksBtnImg[i], attacksRec[i], Color.White);

                        // Outline over buttons when hovered
                        if (CheckMouseInside(attacksRec[i])) spriteBatch.Draw(outlineImg, attacksRec[i], Color.Red);

                        // Weapon icons, special case needed for level 0 healing potion
                        if (i != 3) spriteBatch.Draw(weaponsImg[player.Character, i], new Rectangle(attacksRec[i].X + 10, attacksRec[i].Y + 5, 40, 40), Color.White);
                        else if (items[3].Level == 0) spriteBatch.Draw(items[3].ItemImg[3], new Rectangle(attacksRec[i].X + 10, attacksRec[i].Y + 5, 40, 40), Color.White);
                        else spriteBatch.Draw(items[3].ItemImg[items[3].Level - 1], new Rectangle(attacksRec[i].X + 10, attacksRec[i].Y + 5, 40, 40), Color.White);

                        // Damage ranges and accuracy
                        if (i != 3) spriteBatch.DrawString(numsFont, Convert.ToString(player.DamageRange[i, 0]) + "-" + Convert.ToString(player.DamageRange[i, 1]) + " | " + Convert.ToString(player.Accuracy[i] + "%"),
                            new Vector2(attacksRec[i].X + 50, attacksRec[i].Y + 10), Color.White);
                        else spriteBatch.DrawString(numsFont, Convert.ToString(player.HealRange[0]) + "-" + Convert.ToString(player.HealRange[1]) + " | " + Convert.ToString(player.HealAccuracy) + "%",
                            new Vector2(attacksRec[i].X + 50, attacksRec[i].Y + 10), Color.White);
                    }

                    // Projectile (only one can exist at a time)
                    if (proj.Exists) spriteBatch.Draw(proj.Img, new Rectangle(proj.X, proj.Y, proj.Width, proj.Height), Color.White);

                    // Items 
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].Level > 0) spriteBatch.Draw(items[i].ItemImg[items[i].Level - 1], items[i].HudRec, Color.White);
                    }

                    // Character and Boss image
                    spriteBatch.Draw(bossImg[stage], bossRec, Color.White);
                    spriteBatch.Draw(charBackImg[player.Character], player.Rec, Color.White);

                    for (int i = 0; i < damageNumbers.Count; i++)
                    {
                        switch (damageNumbers[i].Type)
                        {
                            case 0:
                                // Successful Attack
                                spriteBatch.DrawString(smallFont, "-" + Convert.ToString(damageNumbers[i].Roll), new Vector2(damageNumbers[i].X + 10, damageNumbers[i].Y), Color.Red * damageNumbers[i].Opacity);
                                break;
                            case 1:
                                // Successful Heal
                                spriteBatch.DrawString(smallFont, "+" + Convert.ToString(damageNumbers[i].Roll), new Vector2(damageNumbers[i].X + 10, damageNumbers[i].Y), Color.Green * damageNumbers[i].Opacity);
                                break;
                            case 2:
                                // Missed Attack
                                spriteBatch.DrawString(smallFont, "*MISS*", new Vector2(damageNumbers[i].X - 15, damageNumbers[i].Y), Color.Black * damageNumbers[i].Opacity);
                                break;
                            case 3:
                                // Failed Heal
                                spriteBatch.DrawString(smallFont, "*FAIL*", new Vector2(damageNumbers[i].X - 15, damageNumbers[i].Y), Color.Black * damageNumbers[i].Opacity);
                                break;
                        }
                    }

                    // Monster tooltip
                    if (CheckMouseInside(bossRec)) spriteBatch.Draw(monsterTooltipImg, new Rectangle(Globals.MouseCurrent.X, Globals.MouseCurrent.Y, 225, 300), Color.White);

                    break;
                case 2: // Loss Screen (Identical to Gameplay but with a fading in black screen)
                    spriteBatch.Draw(backgroundImg[stage], backgroundRec, Color.White);

                    // UI Elements
                    spriteBatch.Draw(hudImg, new Rectangle(550, 0, 250, 600), Color.White);

                    spriteBatch.Draw(charImg[player.Character], new Rectangle(570, 10, 40, 40), Color.White);
                    spriteBatch.DrawString(hudFont, name, new Vector2(630, 15), Color.White);

                    spriteBatch.DrawString(hudFont, "DAMAGE TAKEN: " + Convert.ToString(player.DamageTaken), new Vector2(10, 560), Color.Gold);
                    spriteBatch.DrawString(hudFont, "DAMAGE TAKEN: " + Convert.ToString(boss.DamageTaken), new Vector2(10, 20), Color.Gold);

                    if (!player.IsPlayerTurn) spriteBatch.DrawString(hudFont, "ENEMY TURN", new Vector2(370, 560), Color.Gold);
                    else spriteBatch.DrawString(hudFont, "PLAYER TURN", new Vector2(370, 560), Color.Gold);

                    spriteBatch.Draw(fameImg, new Rectangle(570, 60, 30, 30), Color.White);
                    spriteBatch.DrawString(hudFont, Convert.ToString(player.Score), new Vector2(610, 60), Color.White);

                    // Player HP Bar
                    spriteBatch.Draw(blankRecImg, new Rectangle(570, 105, Convert.ToInt16(220.0 * ((double)player.Hp / player.MaxHp)), 30), new Color(244, 66, 66));
                    spriteBatch.DrawString(hudFont, "HP", new Vector2(580, 105), Color.White);
                    spriteBatch.DrawString(hudFont, Convert.ToString(player.Hp) + " / " + player.MaxHp, new Vector2(640, 105), Color.Yellow);

                    // Boss HP Bar
                    spriteBatch.Draw(blankRecImg, new Rectangle(bossRec.X, bossRec.Y + bossRec.Height + 25, Convert.ToInt16(bossRec.Width * ((double)boss.Hp / boss.MaxHp)), 25), Color.Red);
                    spriteBatch.Draw(healthBarImg, new Rectangle(bossRec.X, bossRec.Y + bossRec.Height + 25, bossRec.Width, 25), Color.White);
                    if (boss.Hp > 0) spriteBatch.DrawString(hudFont, Convert.ToString(boss.Hp) + " / " + boss.MaxHp, new Vector2(bossRec.X, bossRec.Y + bossRec.Height + 55), Color.Gold);
                    else spriteBatch.DrawString(hudFont, "0 / " + boss.MaxHp, new Vector2(bossRec.X, bossRec.Y + bossRec.Height + 55), Color.Gold);

                    // Attack buttons and outline over selected button
                    for (int i = 0; i < attacksRec.Length; i++)
                    {
                        // Attack buttons
                        spriteBatch.Draw(attacksBtnImg[i], attacksRec[i], Color.White);

                        // Outline over buttons when hovered
                        if (CheckMouseInside(attacksRec[i])) spriteBatch.Draw(outlineImg, attacksRec[i], Color.Red);

                        // Weapon icons, special case needed for level 0 healing potion
                        if (i != 3) spriteBatch.Draw(weaponsImg[player.Character, i], new Rectangle(attacksRec[i].X + 10, attacksRec[i].Y + 5, 40, 40), Color.White);
                        else if (items[3].Level == 0) spriteBatch.Draw(items[3].ItemImg[3], new Rectangle(attacksRec[i].X + 10, attacksRec[i].Y + 5, 40, 40), Color.White);
                        else spriteBatch.Draw(items[3].ItemImg[items[3].Level - 1], new Rectangle(attacksRec[i].X + 10, attacksRec[i].Y + 5, 40, 40), Color.White);

                        // Damage ranges and accuracy
                        if (i != 3) spriteBatch.DrawString(numsFont, Convert.ToString(player.DamageRange[i, 0]) + "-" + Convert.ToString(player.DamageRange[i, 1]) + " | " + Convert.ToString(player.Accuracy[i] + "%"),
                            new Vector2(attacksRec[i].X + 50, attacksRec[i].Y + 10), Color.White);
                        else spriteBatch.DrawString(numsFont, Convert.ToString(player.HealRange[0]) + "-" + Convert.ToString(player.HealRange[1]) + " | " + Convert.ToString(player.HealAccuracy) + "%",
                            new Vector2(attacksRec[i].X + 50, attacksRec[i].Y + 10), Color.White);
                    }

                    // Projectile (only one can exist at a time)
                    if (proj.Exists) spriteBatch.Draw(proj.Img, new Rectangle(proj.X, proj.Y, proj.Width, proj.Height), Color.White);

                    // Items 
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].Level > 0) spriteBatch.Draw(items[i].ItemImg[items[i].Level - 1], items[i].HudRec, Color.White);
                    }

                    // Character and Boss image
                    spriteBatch.Draw(bossImg[stage], bossRec, Color.White);
                    spriteBatch.Draw(charBackImg[player.Character], player.Rec, Color.White);

                    for (int i = 0; i < damageNumbers.Count; i++)
                    {
                        switch (damageNumbers[i].Type)
                        {
                            case 0:
                                // Successful Attack
                                spriteBatch.DrawString(smallFont, "-" + Convert.ToString(damageNumbers[i].Roll), new Vector2(damageNumbers[i].X + 10, damageNumbers[i].Y), Color.Red * damageNumbers[i].Opacity);
                                break;
                            case 1:
                                // Successful Heal
                                spriteBatch.DrawString(smallFont, "+" + Convert.ToString(damageNumbers[i].Roll), new Vector2(damageNumbers[i].X + 10, damageNumbers[i].Y), Color.Green * damageNumbers[i].Opacity);
                                break;
                            case 2:
                                // Missed Attack
                                spriteBatch.DrawString(smallFont, "*MISS*", new Vector2(damageNumbers[i].X - 15, damageNumbers[i].Y), Color.Black * damageNumbers[i].Opacity);
                                break;
                            case 3:
                                // Failed Heal
                                spriteBatch.DrawString(smallFont, "*FAIL*", new Vector2(damageNumbers[i].X - 15, damageNumbers[i].Y), Color.Black * damageNumbers[i].Opacity);
                                break;
                        }
                    }

                    // Monster tooltip
                    if (CheckMouseInside(bossRec)) spriteBatch.Draw(monsterTooltipImg, new Rectangle(Globals.MouseCurrent.X, Globals.MouseCurrent.Y, 225, 300), Color.White);

                    // Fading in loss screen graphics
                    if (lossScreenOpacity < .7f) lossScreenOpacity += 0.015f;
                    spriteBatch.Draw(recImg, backgroundRec, Color.Black * lossScreenOpacity);

                    spriteBatch.Draw(fameImg, new Rectangle(340, 120, 30, 30), Color.White);
                    spriteBatch.DrawString(bigFont, Convert.ToString(player.Score), new Vector2(375, 120), Color.Gold);

                    spriteBatch.DrawString(bigFont, "YOU LOSE.", new Vector2(300, 50), Color.White * (lossScreenOpacity + .5f));
                    spriteBatch.Draw(menuButtonImg, menuButtonRec, Color.White * (lossScreenOpacity + .5f));
                    break;
                case 3: // Win Screen (similar to loss screen)
                    spriteBatch.Draw(backgroundImg[stage], backgroundRec, Color.White);

                    // UI Elements
                    spriteBatch.Draw(hudImg, new Rectangle(550, 0, 250, 600), Color.White);

                    spriteBatch.Draw(charImg[player.Character], new Rectangle(570, 10, 40, 40), Color.White);
                    spriteBatch.DrawString(hudFont, name, new Vector2(630, 15), Color.White);

                    spriteBatch.DrawString(hudFont, "DAMAGE TAKEN: " + Convert.ToString(player.DamageTaken), new Vector2(10, 560), Color.Gold);
                    spriteBatch.DrawString(hudFont, "DAMAGE TAKEN: " + Convert.ToString(boss.DamageTaken), new Vector2(10, 20), Color.Gold);

                    if (!player.IsPlayerTurn) spriteBatch.DrawString(hudFont, "ENEMY TURN", new Vector2(370, 560), Color.Gold);
                    else spriteBatch.DrawString(hudFont, "PLAYER TURN", new Vector2(370, 560), Color.Gold);

                    spriteBatch.Draw(fameImg, new Rectangle(570, 60, 30, 30), Color.White);
                    spriteBatch.DrawString(hudFont, Convert.ToString(player.Score), new Vector2(610, 60), Color.White);

                    // Player HP Bar
                    spriteBatch.Draw(blankRecImg, new Rectangle(570, 105, Convert.ToInt16(220.0 * ((double)player.Hp / player.MaxHp)), 30), new Color(244, 66, 66));
                    spriteBatch.DrawString(hudFont, "HP", new Vector2(580, 105), Color.White);
                    spriteBatch.DrawString(hudFont, Convert.ToString(player.Hp) + " / " + player.MaxHp, new Vector2(640, 105), Color.Yellow);

                    // Boss HP Bar
                    spriteBatch.Draw(blankRecImg, new Rectangle(bossRec.X, bossRec.Y + bossRec.Height + 25, Convert.ToInt16(bossRec.Width * ((double)boss.Hp / boss.MaxHp)), 25), Color.Red);
                    spriteBatch.Draw(healthBarImg, new Rectangle(bossRec.X, bossRec.Y + bossRec.Height + 25, bossRec.Width, 25), Color.White);
                    if (boss.Hp > 0) spriteBatch.DrawString(hudFont, Convert.ToString(boss.Hp) + " / " + boss.MaxHp, new Vector2(bossRec.X, bossRec.Y + bossRec.Height + 55), Color.Gold);
                    else spriteBatch.DrawString(hudFont, "0 / " + boss.MaxHp, new Vector2(bossRec.X, bossRec.Y + bossRec.Height + 55), Color.Gold);

                    // Attack buttons and outline over selected button
                    for (int i = 0; i < attacksRec.Length; i++)
                    {
                        // Attack buttons
                        spriteBatch.Draw(attacksBtnImg[i], attacksRec[i], Color.White);

                        // Outline over buttons when hovered
                        if (CheckMouseInside(attacksRec[i])) spriteBatch.Draw(outlineImg, attacksRec[i], Color.Red);

                        // Weapon icons, special case needed for level 0 healing potion
                        if (i != 3) spriteBatch.Draw(weaponsImg[player.Character, i], new Rectangle(attacksRec[i].X + 10, attacksRec[i].Y + 5, 40, 40), Color.White);
                        else if (items[3].Level == 0) spriteBatch.Draw(items[3].ItemImg[3], new Rectangle(attacksRec[i].X + 10, attacksRec[i].Y + 5, 40, 40), Color.White);
                        else spriteBatch.Draw(items[3].ItemImg[items[3].Level - 1], new Rectangle(attacksRec[i].X + 10, attacksRec[i].Y + 5, 40, 40), Color.White);

                        // Damage ranges and accuracy
                        if (i != 3) spriteBatch.DrawString(numsFont, Convert.ToString(player.DamageRange[i, 0]) + "-" + Convert.ToString(player.DamageRange[i, 1]) + " | " + Convert.ToString(player.Accuracy[i] + "%"),
                            new Vector2(attacksRec[i].X + 50, attacksRec[i].Y + 10), Color.White);
                        else spriteBatch.DrawString(numsFont, Convert.ToString(player.HealRange[0]) + "-" + Convert.ToString(player.HealRange[1]) + " | " + Convert.ToString(player.HealAccuracy) + "%",
                            new Vector2(attacksRec[i].X + 50, attacksRec[i].Y + 10), Color.White);
                    }

                    // Projectile (only one can exist at a time)
                    if (proj.Exists) spriteBatch.Draw(proj.Img, new Rectangle(proj.X, proj.Y, proj.Width, proj.Height), Color.White);

                    // Items 
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].Level > 0) spriteBatch.Draw(items[i].ItemImg[items[i].Level - 1], items[i].HudRec, Color.White);
                    }

                    // Character and Boss image
                    spriteBatch.Draw(bossImg[stage], bossRec, Color.White);
                    spriteBatch.Draw(charBackImg[player.Character], player.Rec, Color.White);

                    for (int i = 0; i < damageNumbers.Count; i++)
                    {
                        switch (damageNumbers[i].Type)
                        {
                            case 0:
                                // Successful Attack
                                spriteBatch.DrawString(smallFont, "-" + Convert.ToString(damageNumbers[i].Roll), new Vector2(damageNumbers[i].X + 10, damageNumbers[i].Y), Color.Red * damageNumbers[i].Opacity);
                                break;
                            case 1:
                                // Successful Heal
                                spriteBatch.DrawString(smallFont, "+" + Convert.ToString(damageNumbers[i].Roll), new Vector2(damageNumbers[i].X + 10, damageNumbers[i].Y), Color.Green * damageNumbers[i].Opacity);
                                break;
                            case 2:
                                // Missed Attack
                                spriteBatch.DrawString(smallFont, "*MISS*", new Vector2(damageNumbers[i].X - 15, damageNumbers[i].Y), Color.Black * damageNumbers[i].Opacity);
                                break;
                            case 3:
                                // Failed Heal
                                spriteBatch.DrawString(smallFont, "*FAIL*", new Vector2(damageNumbers[i].X - 15, damageNumbers[i].Y), Color.Black * damageNumbers[i].Opacity);
                                break;
                        }
                    }

                    // Monster tooltip
                    if (CheckMouseInside(bossRec)) spriteBatch.Draw(monsterTooltipImg, new Rectangle(Globals.MouseCurrent.X, Globals.MouseCurrent.Y, 225, 300), Color.White);

                    // Fading in win screen graphics
                    if (winScreenOpacity < .7f) winScreenOpacity += 0.015f;
                    spriteBatch.Draw(recImg, backgroundRec, Color.Black * winScreenOpacity);
                    spriteBatch.DrawString(bigFont, "YOU WIN!", new Vector2(305, 50), Color.White * (winScreenOpacity + .5f));

                    spriteBatch.Draw(fameImg, new Rectangle(340, 120, 30, 30), Color.White);
                    spriteBatch.DrawString(bigFont, Convert.ToString(player.Score), new Vector2(375, 120), Color.Gold);

                    if (stage != 3) spriteBatch.Draw(shopButtonImg, menuButtonRec, Color.White * (winScreenOpacity + .5f));
                    else spriteBatch.Draw(rankingsButtonImg, menuButtonRec, Color.White * (winScreenOpacity + .5f));
                    break;
                case 4: // Leaderboard
                    spriteBatch.Draw(backgroundImg[3], backgroundRec, Color.White);

                    // Displays information of the player
                    for (int i = 0; i < rankings.Count; i++)
                    {
                        // Rank, character image, and name
                        spriteBatch.DrawString(hudFont, Convert.ToString(i + 1) + ". ", new Vector2(50, 20 + (60 * i)), Color.White);
                        spriteBatch.Draw(charImg[rankings[i].Character], new Rectangle(100, 20 + (60 * i), 40, 40), Color.White);
                        spriteBatch.DrawString(hudFont, rankings[i].Name, new Vector2(155, 20 + (60 * i)), Color.White);

                        // Draws each of the player's items
                        if (rankings[i].AtkLvl > 0) spriteBatch.Draw(items[0].ItemImg[rankings[i].AtkLvl - 1], new Rectangle(400, 20 + (60 * i), 40, 40), Color.White);
                        if (rankings[i].DexLvl > 0) spriteBatch.Draw(items[1].ItemImg[rankings[i].DexLvl - 1], new Rectangle(450, 20 + (60 * i), 40, 40), Color.White);
                        if (rankings[i].HpLvl > 0) spriteBatch.Draw(items[2].ItemImg[rankings[i].HpLvl - 1], new Rectangle(500, 20 + (60 * i), 40, 40), Color.White);
                        if (rankings[i].HealLvl > 0) spriteBatch.Draw(items[3].ItemImg[rankings[i].HealLvl - 1], new Rectangle(550, 20 + (60 * i), 40, 40), Color.White);

                        // Score
                        spriteBatch.Draw(fameImg, new Rectangle(650, 20 + (60 * i), 40, 40), Color.White);
                        spriteBatch.DrawString(hudFont, Convert.ToString(rankings[i].Score), new Vector2(700, 20 + (60 * i)), Color.White);
                    }

                    // Main menu button
                    spriteBatch.Draw(recImg, new Rectangle(0, 520, 800, 80), Color.White);
                    if (CheckMouseInside(menuButton2Rec)) spriteBatch.Draw(menuButton2Img, menuButton2Rec, Color.Yellow);
                    else spriteBatch.Draw(menuButton2Img, menuButton2Rec, Color.White);
                    break;
                case 5: // Shop Screen
                    spriteBatch.Draw(backgroundImg[3], backgroundRec, Color.White);

                    spriteBatch.Draw(nextButtonImg, nextButtonRec, Color.White);

                    // Gold
                    spriteBatch.Draw(goldImg, new Rectangle(40, 40, 50, 50), Color.White);
                    spriteBatch.DrawString(hudFont, Convert.ToString(Globals.Gold), new Vector2(43, 100), Color.Gold);

                    // Items and item costs
                    for (int i = 0; i < items.Length; i++)
                    {
                        // Draws items if they are not maxed out
                        if (items[i].Level < 3)
                        {
                            // Item image
                            spriteBatch.Draw(items[i].ItemImg[items[i].Level], items[i].ShopRec, Color.White);
                            
                            // Item cost, red if you cannot afford it
                            if (Globals.Gold >= items[i].Cost) spriteBatch.DrawString(hudFont, "$" + items[i].Cost, new Vector2(items[i].ShopRec.X + 20, items[i].ShopRec.Y + 120), Color.Green);
                            else spriteBatch.DrawString(hudFont, "$" + items[i].Cost, new Vector2(items[i].ShopRec.X + 20, items[i].ShopRec.Y + 120), Color.Red);
                        }
                    }

                    // Tooltips, seperate loop needed so tooltips go over items
                    for (int i = 0; i < items.Length; i++)
                    {
                        // Tooltip when hovered over
                        if (CheckMouseInside(items[i].ShopRec) && items[i].Level < 3) spriteBatch.Draw(shopTooltipImg[i], new Rectangle(Globals.MouseCurrent.X - 150, Globals.MouseCurrent.Y + 100, 300, 350), Color.White);
                    }
                    break;
                case 6: // Class Select
                    spriteBatch.Draw(backgroundImg[3], backgroundRec, Color.White);
                    spriteBatch.DrawString(bigFont, "CHOOSE YOUR CLASS", new Vector2(200, 20), Color.White);
  
                    // Character buttons
                    for (int i = 0; i < charSelectImg.Length; i++)
                    {
                        // Draws each of the character buttons
                        spriteBatch.Draw(charSelectImg[i], charSelectRec[i], Color.White);
                    }

                    // Tooltips for characters, seperate loop needed so tooltips go over other character buttons
                    for (int i = 0; i < charSelectImg.Length; i++)
                    {
                        // Draws tooltips for characters when hovered over, tooltips that would go off the screen have their X values reduced by 300 
                        if (CheckMouseInside(charSelectRec[i]))
                        {
                            if (i < 2) spriteBatch.Draw(charTooltipImg[i], new Rectangle(Globals.MouseCurrent.X, Globals.MouseCurrent.Y, 300, 350), Color.White);
                            else spriteBatch.Draw(charTooltipImg[i], new Rectangle(Globals.MouseCurrent.X - 300, Globals.MouseCurrent.Y, 300, 350), Color.White);
                        }
                    }
                    break;
                case 7: // Name Selection
                    spriteBatch.Draw(backgroundImg[3], backgroundRec, Color.White);

                    // Displays name
                    spriteBatch.DrawString(bigFont, "ENTER YOUR NAME", new Vector2(230, 20), Color.White);

                    // Text box
                    spriteBatch.Draw(blankRecImg, new Rectangle(275, 200, 250, 50), Color.White);
                    spriteBatch.DrawString(bigFont, name, new Vector2(290, 205), Color.Black);

                    // Main menu button
                    spriteBatch.Draw(recImg, new Rectangle(0, 520, 800, 80), Color.White);
                    if (CheckMouseInside(startButtonRec)) spriteBatch.Draw(startButton2Img, startButtonRec, Color.Yellow);
                    else spriteBatch.Draw(startButton2Img, startButtonRec, Color.White);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Resets the game's stats
        /// </summary>
        public static void ResetGame()
        { 
            Globals.Gold = 0;

            stage = 0;
            name = "";

            player = new Player();
            boss = new Boss();

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Cost = 200;
                items[i].Level = 0;
            }

            bossRec = new Rectangle(250, 50, 50, 50);
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Resets stage-specific stats
        /// </summary>
        public static void NextStage()
        {      
            gamestate = 1;

            player.Hp = player.MaxHp;
            boss.MaxHp += 50;
            boss.Hp = boss.MaxHp;

            player.DamageRoll = 0;
            boss.DamageRoll = 0;

            player.DamageTaken = 0;
            boss.DamageTaken = 0;

            if (Globals.Rng.Next(0, 1) == 0) player.IsPlayerTurn = true;
            else player.IsPlayerTurn = false;

            stage++;

            bossRec.Height += 40;
            bossRec.Width += 40;
            bossRec.X -= 20;
            bossRec.Y -= 20;

            gamestate = 1;
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Progresses combat if it is the player's turn
        /// </summary>
        /// <param name="playerChoice"></param>
        public static void PlayerTurn(int playerChoice)
        {
            // If the player used heal
            if (playerChoice == 3)
            {
                // Checks accuracy on the heal
                if (Globals.Rng.Next(1, 100) < player.HealAccuracy)
                {
                    // Rolls healing
                    player.DamageRoll = Globals.Rng.Next(player.HealRange[0], player.HealRange[1]);

                    // Rolls crit chance on the heal
                    if (Globals.Rng.Next(1, 100) < player.CritChance) player.DamageRoll *= 2;

                    // If the player's HP surpasses their maximum, this brings it back down to the max
                    if (player.Hp + player.DamageRoll > player.MaxHp)
                    {
                        player.Score += player.MaxHp - player.Hp;
                        Globals.Gold += player.MaxHp - player.Hp;
                        player.Hp = player.MaxHp;
                        damageNumbers.Add(new DamageNumber(player.DamageRoll, player.Rec.X, player.Rec.Y + (player.Rec.Height / 2), 1));
                    }
                    else
                    {
                        player.Hp += player.DamageRoll;
                        player.Score += player.DamageRoll;
                        Globals.Gold += player.DamageRoll * 5;
                        damageNumbers.Add(new DamageNumber(player.DamageRoll, player.Rec.X, player.Rec.Y + (player.Rec.Height / 2), 1));
                    }
                }
                else damageNumbers.Add(new DamageNumber(player.DamageRoll, player.Rec.X, player.Rec.Y + (player.Rec.Height / 2), 3));
            }
            else
            {   
                // Rolls accuracy
                if (Globals.Rng.Next(1, 100) < player.Accuracy[playerChoice])
                {
                    // Rolls damage
                    player.DamageRoll = Globals.Rng.Next(player.DamageRange[playerChoice, 0], player.DamageRange[playerChoice, 1]);

                    // Rolls crit
                    if (Globals.Rng.Next(1, 100) < player.CritChance) player.DamageRoll *= 2;

                    boss.Hp -= player.DamageRoll;
                    boss.DamageTaken += player.DamageRoll;
                    player.Score += player.DamageRoll * 2;
                    Globals.Gold += player.DamageRoll * 5;

                    damageNumbers.Add(new DamageNumber(player.DamageRoll, bossRec.X, bossRec.Y + (bossRec.Height / 2), 0));
                }
                else damageNumbers.Add(new DamageNumber(player.DamageRoll, bossRec.X, bossRec.Y + (bossRec.Height / 2), 2));
            }
            player.IsPlayerTurn = false;
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Similar to PlayerTurn, but for the boss
        /// </summary>
        public static void BossTurn()
        {
            // If the boss is within this range, it will use its healing ability
            if (boss.Hp < boss.MaxHp * .25)
            { 
                // Rolls accuracy for heal
                if (Globals.Rng.Next(1, 100) < player.HealAccuracy)
                {
                    // Rolls heal
                    boss.DamageRoll = Globals.Rng.Next(boss.HealRange[0], boss.HealRange[1]);
                    if (Globals.Rng.Next(1, 100) < boss.CritChance) boss.DamageRoll *= 2;

                    // If the heal results in the boss' health surpassing the maximum HP, it brings it back to the max
                    if (boss.Hp + boss.DamageRoll > boss.MaxHp)
                    {
                        damageNumbers.Add(new DamageNumber(boss.MaxHp - boss.Hp, bossRec.X, bossRec.Y + (bossRec.Height / 2), 1));
                        boss.Hp = boss.MaxHp;
                    }
                    else
                    {
                        boss.Hp += boss.DamageRoll;
                        damageNumbers.Add(new DamageNumber(boss.DamageRoll, bossRec.X, bossRec.Y + (bossRec.Height / 2), 1));
                    }
                }
            }
            else
            {
                // Determines a random attack for the boss
                boss.Choice = Globals.Rng.Next(0, 2);

                // Rolls accuracy
                if (Globals.Rng.Next(1, 100) < boss.Accuracy[boss.Choice])
                {
                    // Rolls damage and crit
                    boss.DamageRoll = Globals.Rng.Next(boss.DamageRange[boss.Choice, 0], boss.DamageRange[boss.Choice, 1]);
                    if (Globals.Rng.Next(1, 100) < boss.CritChance) boss.DamageRoll *= 2;

                    player.Hp -= boss.DamageRoll;
                    player.DamageTaken += boss.DamageRoll;
                    player.Score -= boss.DamageRoll;

                    damageNumbers.Add(new DamageNumber(boss.DamageRoll, player.Rec.X, player.Rec.Y, 0));
                }
                else damageNumbers.Add(new DamageNumber(boss.DamageRoll, player.Rec.X, player.Rec.Y, 2));
            }

            player.IsPlayerTurn = true;
        }
       
        /// <summary>
        /// Pre: button is the rectangle currently being checked
        /// Post: Returns whether the mouse is within the rectangle or not
        /// Description: Function that checks if mouse is within a button using CheckMouseInside
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool CheckButton(Rectangle button)
        {
            if (Globals.MouseCurrent.LeftButton == ButtonState.Pressed && Globals.MouseLast.LeftButton != ButtonState.Pressed && CheckMouseInside(button)) return true;
            else return false;
        }

        /// <summary>
        /// Pre: rec is the rectangle currently being checked
        /// Post: Returns whether the mouse is on the button or not
        /// Description: Used in CheckButton, checks if mouse is inside of button
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public static bool CheckMouseInside(Rectangle rec)
        {
            if (Globals.MouseCurrent.X >= rec.X && Globals.MouseCurrent.X <= rec.X + rec.Width &&
                 Globals.MouseCurrent.Y >= rec.Y && Globals.MouseCurrent.Y <= rec.Y + rec.Height)
                return true;
            else return false;
        }

        /// <summary>
        /// Pre: text is the string user wishes to check input for, maxLength is maximum length of said string
        /// Post: Returns string after keyboard input
        /// Description: The worst keyboard input function ever made, complete with hardcoding for each of the 26 letters, spaces, and backspaces!
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string KeyboardInput(string text, int maxLength)
        {
            // Checks if the string is less than the maximum length
            if (text.Length < maxLength)
            {
                // Checks every single letter, KbLast prevents holding down a key 
                if (Globals.KbCurrent.IsKeyDown(Keys.A) && !(Globals.KbLast.IsKeyDown(Keys.A))) text += "A";
                if (Globals.KbCurrent.IsKeyDown(Keys.B) && !(Globals.KbLast.IsKeyDown(Keys.B))) text += "B";
                if (Globals.KbCurrent.IsKeyDown(Keys.C) && !(Globals.KbLast.IsKeyDown(Keys.C))) text += "C";
                if (Globals.KbCurrent.IsKeyDown(Keys.D) && !(Globals.KbLast.IsKeyDown(Keys.D))) text += "D";
                if (Globals.KbCurrent.IsKeyDown(Keys.E) && !(Globals.KbLast.IsKeyDown(Keys.E))) text += "E";
                if (Globals.KbCurrent.IsKeyDown(Keys.F) && !(Globals.KbLast.IsKeyDown(Keys.F))) text += "F";
                if (Globals.KbCurrent.IsKeyDown(Keys.G) && !(Globals.KbLast.IsKeyDown(Keys.G))) text += "G";
                if (Globals.KbCurrent.IsKeyDown(Keys.H) && !(Globals.KbLast.IsKeyDown(Keys.H))) text += "H";
                if (Globals.KbCurrent.IsKeyDown(Keys.I) && !(Globals.KbLast.IsKeyDown(Keys.I))) text += "I";
                if (Globals.KbCurrent.IsKeyDown(Keys.J) && !(Globals.KbLast.IsKeyDown(Keys.J))) text += "J";
                if (Globals.KbCurrent.IsKeyDown(Keys.K) && !(Globals.KbLast.IsKeyDown(Keys.K))) text += "K";
                if (Globals.KbCurrent.IsKeyDown(Keys.L) && !(Globals.KbLast.IsKeyDown(Keys.L))) text += "L";
                if (Globals.KbCurrent.IsKeyDown(Keys.M) && !(Globals.KbLast.IsKeyDown(Keys.M))) text += "M";
                if (Globals.KbCurrent.IsKeyDown(Keys.N) && !(Globals.KbLast.IsKeyDown(Keys.N))) text += "N";
                if (Globals.KbCurrent.IsKeyDown(Keys.O) && !(Globals.KbLast.IsKeyDown(Keys.O))) text += "O";
                if (Globals.KbCurrent.IsKeyDown(Keys.P) && !(Globals.KbLast.IsKeyDown(Keys.P))) text += "P";
                if (Globals.KbCurrent.IsKeyDown(Keys.Q) && !(Globals.KbLast.IsKeyDown(Keys.Q))) text += "Q";
                if (Globals.KbCurrent.IsKeyDown(Keys.R) && !(Globals.KbLast.IsKeyDown(Keys.R))) text += "R";
                if (Globals.KbCurrent.IsKeyDown(Keys.S) && !(Globals.KbLast.IsKeyDown(Keys.S))) text += "S";
                if (Globals.KbCurrent.IsKeyDown(Keys.T) && !(Globals.KbLast.IsKeyDown(Keys.T))) text += "T";
                if (Globals.KbCurrent.IsKeyDown(Keys.U) && !(Globals.KbLast.IsKeyDown(Keys.U))) text += "U";
                if (Globals.KbCurrent.IsKeyDown(Keys.V) && !(Globals.KbLast.IsKeyDown(Keys.V))) text += "V";
                if (Globals.KbCurrent.IsKeyDown(Keys.W) && !(Globals.KbLast.IsKeyDown(Keys.W))) text += "W";
                if (Globals.KbCurrent.IsKeyDown(Keys.X) && !(Globals.KbLast.IsKeyDown(Keys.X))) text += "X";
                if (Globals.KbCurrent.IsKeyDown(Keys.Y) && !(Globals.KbLast.IsKeyDown(Keys.Y))) text += "Y";
                if (Globals.KbCurrent.IsKeyDown(Keys.Z) && !(Globals.KbLast.IsKeyDown(Keys.Z))) text += "Z";
                if (Globals.KbCurrent.IsKeyDown(Keys.Space) && !(Globals.KbLast.IsKeyDown(Keys.Space))) text += " ";
            }
            // Backspace
            if (Globals.KbCurrent.IsKeyDown(Keys.Back) && !(Globals.KbLast.IsKeyDown(Keys.Back)) && text.Length > 0) text = text.Substring(0, text.Length - 1);
            return text;
        }
    }
}
