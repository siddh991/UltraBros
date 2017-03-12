// Author: Siddharth Surana
// File Name: UltraBros.cs
// Project Name: UltraBros
// Creation Date: Dec. 16, 2015
// Modified Date: Jan. 20, 2016
// Description: A recreation of the retro version of Super Mario Bros using C# and XNA. 
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace UltraBros
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class UltraBros : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // create variables to store the main background themes
        Song mainThemeSong;
        bool isMainThemeSongPlayed = false;
        Song mainThemeSongFast;
        bool isMainThemeSongFastPlayed = false;

        // create variables to store the sound effects 
        SoundEffect coinCollect;
        SoundEffectInstance coinCollectInstance;
        SoundEffect goombaKill;
        SoundEffectInstance goombaKillInstance;
        SoundEffect failSound;
        SoundEffectInstance failSoundInstance;
        bool isFailSoundPlayed;
        SoundEffect stageClear;
        SoundEffectInstance stageClearInstance;
        bool isStageClearPlayed;

        // define the font used in the game
        SpriteFont font;

        // define the variables that store the location and amount of the score and time 
        Vector2 scorePosition;
        Vector2 timeLeftPosition;
        Vector2 stagePosition;
        int timeLeft = 500;
        int almostTimeUp = 100;
        int scoreIncrease = 100;
        int timeIncrease = 50;

        // define the variables to store the background graphical data
        Texture2D brickBackground;
        Rectangle brickBackgroundBounds;
        Rectangle brickBackgroundBounds2;

        // define varaible to store the title box graphical data
        Texture2D titleSprite;
        Rectangle titleSpriteBounds;

        // define the variables to store the mario image graphical data
        Texture2D marioSprite;
        Rectangle marioSpriteBounds;
        Rectangle marioSrcBounds;

        // define the variables to store the graphical data of the items and collectables
        Texture2D redMushroom;
        Rectangle redMushroomBounds;
        Texture2D greenMushroom;
        Rectangle greenMushroomBounds;
        Texture2D yellowMushroom;
        Rectangle yellowMushroomBounds;
        Texture2D star;
        Rectangle starBounds;
        Texture2D coin;
        Rectangle[] coinBounds = new Rectangle[12];

        // define the variables to store the graphical data of enemies
        Texture2D goombaSprite;
        Rectangle[] goombaSpriteBounds = new Rectangle[5];
        Rectangle goombaSrcBounds;

        // define the variables to store the graphical data of objects in the game
        Texture2D castle;
        Rectangle castleBounds;
        Texture2D stoneBrick;
        Rectangle[] stoneBrickBounds = new Rectangle[178];
        Texture2D pipe;
        Rectangle[] pipeBounds = new Rectangle[4];
        Texture2D tallPipe;
        Rectangle tallPipeBounds;
        Texture2D noGroundBlue;
        Rectangle[] noGroundBlueBounds = new Rectangle[15];
        Texture2D squareBrick;
        Rectangle[] squareBrickBounds = new Rectangle[32];
        Texture2D itemBox;
        Texture2D cannon;
        Rectangle[] cannonBounds = new Rectangle[3];
        Texture2D brownBlock;
        Rectangle[] brownBlockBounds = new Rectangle[92];
        Texture2D flag;
        Rectangle flagBounds;

        // define variable to hold the graphical data of the two items in storage 
        Texture2D item1;
        Rectangle item1Bounds;
        Texture2D item2;
        Rectangle item2Bounds;

        // define variables to store data of mario animation
        byte NumOfMarioImgs = 8;
        int marioY = 370;
        int marioX = 200;
        int marioYOriginal;
        int frameNum = 0;
        byte numFrames = 3;
        string marioDirection = "right";
        string marioDirectionJump;
        byte moveSpeed = 4;
        byte jumpSpeed = 10;
        byte jumpSpeedSlow = 8;
        const byte ORIGINAL_JUMP_SPEED = 10;
        int jumpHeight = 160;
        int originalJumpHeight = 160;

        // define variables to store background x values
        int brickBackgroundX = 0;
        int brickBackgroundX2;

        // define variables to store the titles location and movement
        int titleY = 50;
        byte titleSpeed = 4;

        // define variables to store the values of the various platform heights
        int platform1Height = 300;
        int platform2Height = 160;

        // define the variables for goomba animation
        int[] goombaX = new[] {1520, 1680, 880, 3200};
        int[] goombaY = new[] {400, 400, 260, 400};
        byte NumOfGoombaImgs = 3;
        int goombaFrameNum = 0;
        byte goombaNumFrames = 2;

        // define variables to store the various bricks x values
        int squareBricks1X = 800;
        int squareBricks2X = 920;
        int squareBricks3X = 1040;
        int squareBricks4X = 2800;
        int squareBricks5X = 2920;
        int squareBricks6X = 3040;
        int squareBricks7X = 6000;
        int squareBricks8X = 6200;
        int squareBricks9X = 7440;

        // define variables to store the various stone bricks x values
        int stoneBricks1X = 10000;
        int stoneBricks2X = 9240;
        int stoneBricks3X = 12600;

        // define variables to store the green pipe locatoin data
        int pipe1X = 1400;
        int pipe2X = 8240;
        int tallPipeX = 6640;
        int twoPipeDistance = 360;

        // define a variable to hold the reference points for various objects in the game
        int cannonX = 7240;
        int distanceToCannon2 = 10;
        int distanceToCannon3 = 20;
        int castleX = 9200;
        int castleXShift = 500;
        int stoneWallX = 9200;
        byte distanceToCastleDoor = 80;
        byte distanceToFlag = 200;
        int ground = 370;
        int stairLength3 = 3;
        int stairLength7 = 7;

        // define variables to hold locations for the staircase
        int brownStairDistance = 240;
        int brownBlockX = 2200;
        int brownBlock2X = 4000;
        int brownBlock3X = 8520;
        int greyStairX = 11840;
        int greyRectangleX = 12080;

        // define variables to store the time values used for in game events
        int timePassed = 0;
        const byte SMOOTHNESS = 8;
        const byte TIME_INTERVAL = 10;

        // define variables to store the characters' vertical movement 
        bool isJumpOn = false;
        bool onGround = true;
        bool enemy1OnGround = true;
        bool enemy2OnGround = true;
        bool enemy3OnGround = true;
        bool enemy4OnGround = true;

        // variable to store the movement data of enemies
        int enemyMoveDir = 1;
        int enemy2MoveDir = 1;
        int enemy3MoveDir = -1;
        int enemy4MoveDir = -1;
        byte enemyMoveSpeed = 2;

        // variable to store the x location of the no ground area
        int noGroundBlueX = 3398;

        // variables to store the x and y location of each coin
        int[] coinX = new int[12] { 1620, 2380, 2380, 6600, 6600, 6600, 6600, 11440, 11360, 11280, 11200, 0 };
        int[] coinY = new int[12] { 300, 300, 380, 380, 300, 220, 140, 120, 120, 120, 120, -40 };

        // variable to store the number of coins collected
        int coinCount = 0;

        // store the x and y values for each item and collectables in the game
        int redMushroomX = 0;
        int redMushroomY = -40;
        int greenMushroomX = 0;
        int greenMushroomY = -40;
        int yellowMushroomX = 0;
        int yellowMushroomY = -40;
        int starX = 0;
        int starY = -40;

        // create variable to store the temporary X and Y object values
        int objXTemp = -1;
        int objYTemp = -1;
        int objHeightTemp = -1;
        int objWidthTemp = -1;

        // define variable to store the distance to another level
        int distanceToNextWorld = 800;

        // define variable to store if the game is started or ended
        bool gameStarted = false;
        bool gameEnded = false;

        // create variable to store inventory data 
        string[] inventoryItems = new string[2];
        bool[] areItemBoxesActive = new bool[7] { true, true, true, true, true, true, true };

        // create variable to store data for item usage
        bool isCharacterImmune = false;
        int lowGravityTime = 100;
        int lowGravityTimer = 0;
        int highJumpTime = 50;
        int highJumpTimer = 0;

        // create variable for scoreboard location and data
        Vector2 scoreBoardPosition;
        int scoreBoardStartY = 70;
        int scoreBoardPositionY = 30;
        int scoreBoardX = 300;
        int[] scoreBoard = new int[11];
        bool scoreBoardAdjusted = false; 

        // define variables that store the location of the files to externally save the game to
        string highScoreFile = "highScoresSave.txt";
        string saveFile = "marioGameSave.txt";

        // define the variables to read and write to an external file
        StreamReader inFile;
        StreamWriter outFile;

        // define a variable to store keyboard input
        KeyboardState currentKB;
        KeyboardState oldKB;

        public UltraBros()
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

            // load the sprite font and define its location
            font = Content.Load<SpriteFont>("Images/Fonts/marioFont");
            scorePosition = new Vector2(0, 42);
            timeLeftPosition = new Vector2(800 - 120, 42);
            stagePosition = new Vector2(390, 42);
            scoreBoardPosition = new Vector2(scoreBoardX, scoreBoardStartY);

            // load the background and define its locations
            brickBackground = Content.Load<Texture2D>(@"Images\Backgrounds\BrickBackground");
            brickBackgroundBounds = new Rectangle(brickBackgroundX, 0, brickBackground.Width, brickBackground.Height);
            brickBackgroundX2 = brickBackgroundX + brickBackground.Width;

            // load the title board and define its location
            titleSprite = Content.Load<Texture2D>(@"Images\Sprites\title");
            titleSpriteBounds = new Rectangle((brickBackground.Width - titleSprite.Width) / 2, titleY, titleSprite.Width, titleSprite.Height);

            // load the image for the various blocks
            squareBrick = Content.Load<Texture2D>(@"Images\Sprites\squareBrick");
            brownBlock = Content.Load<Texture2D>(@"Images\Sprites\brownBlock");
            stoneBrick = Content.Load<Texture2D>(@"Images\Sprites\stoneBrick");

            // load the images used for items and collectables
            itemBox = Content.Load<Texture2D>(@"Images\Sprites\itemBox");
            redMushroom = Content.Load<Texture2D>(@"Images\Sprites\redMushroom");
            greenMushroom = Content.Load<Texture2D>(@"Images\Sprites\greenMushroom");
            yellowMushroom = Content.Load<Texture2D>(@"Images\Sprites\yellowMushroom");
            star = Content.Load<Texture2D>(@"Images\Sprites\star");
            coin = Content.Load<Texture2D>(@"Images\Sprites\newCoin");

            // load image for objects in the game
            pipe = Content.Load<Texture2D>(@"Images\Sprites\pipe");
            tallPipe = Content.Load<Texture2D>(@"Images\Sprites\tallPipe");
            cannon = Content.Load<Texture2D>(@"Images\Sprites\cannon");
            castle = Content.Load<Texture2D>(@"Images\Sprites\castle");
            noGroundBlue = Content.Load<Texture2D>(@"Images\Sprites\noGroundBlue");
            flag = Content.Load<Texture2D>(@"Images\Sprites\flag");

            // load the images for the various characters and define marios location
            marioSprite = Content.Load<Texture2D>(@"Images\Sprites\marioSpriteFinal");
            marioSpriteBounds = new Rectangle(marioX, marioY, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
            goombaSprite = Content.Load<Texture2D>(@"Images\Sprites\goombaSprite1");

            // load the main theme songs 
            mainThemeSong = Content.Load<Song>(@"Audio\Music\mainThemeOverworld");
            mainThemeSongFast = Content.Load<Song>(@"Audio\Music\fastBackgroundMusic");

            // load the sound effects used in the game
            coinCollect = Content.Load<SoundEffect>(@"Audio\SoundEffects\coinSoundEffect");
            coinCollectInstance = coinCollect.CreateInstance();
            failSound = Content.Load<SoundEffect>(@"Audio\SoundEffects\loseGameSound");
            failSoundInstance = failSound.CreateInstance();
            stageClear = Content.Load<SoundEffect>(@"Audio\SoundEffects\stageClear");
            stageClearInstance = stageClear.CreateInstance();
            goombaKill = Content.Load<SoundEffect>(@"Audio\SoundEffects\hoohoo");
            goombaKillInstance = goombaKill.CreateInstance();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // store the old keyboard state as the previous keyboard state and get the new keyboard state
            oldKB = currentKB;
            currentKB = Keyboard.GetState();

            // increase the amount of time passed
            timePassed++;

            // define the first character that collision is tested with
            string characterType = "mario";
            int characterX = marioX;
            int characterY = marioY;

            // if is theme song is not playing, start playing it
            if (isMainThemeSongPlayed == false)
            {
                // reduce main theme volume
                MediaPlayer.Volume = 0.4f;

                //play the main theme and repeat it
                MediaPlayer.Play(mainThemeSong);
                MediaPlayer.IsRepeating = true;

                // tell the game that the song is currently playing
                isMainThemeSongPlayed = true;
            }

            // is the time is almost up, play the fast music
            if (almostTimeUp > timeLeft)
            {
                // if the fast song is not playing, enter the if statement
                if (isMainThemeSongFastPlayed == false)
                {
                    // play the fast music and play it on repeat
                    MediaPlayer.Play(mainThemeSongFast);
                    MediaPlayer.IsRepeating = true;

                    // tell the game the fast song is already playing
                    isMainThemeSongFastPlayed = true;
                }
            }

            // is the game is started enter the code
            if (gameStarted == true && gameEnded == false)
            {
                // move the title screen up until it is off the screen
                if (titleY > 0 - titleSprite.Height)
                {
                    titleY = titleY - titleSpeed;
                    titleSpriteBounds = new Rectangle((brickBackground.Width - titleSprite.Width) / 2, titleY, titleSprite.Width, titleSprite.Height);
                }

                // subtract from the amount of time left if the time passed is an interval of 10
                if (timePassed % TIME_INTERVAL == 0)
                {
                    timeLeft--;
                }

                // if time left is 0 or less, end the game and music
                if (timeLeft <= 0)
                {
                    gameEnded = true;

                    MediaPlayer.Stop();
                    failSoundInstance.Play();
                }
            }

            // if the low gravity timer is a multiple of 10, reduce the amount of time alloted by 1
            if (lowGravityTimer > 0)
            {
                if (timePassed % TIME_INTERVAL == 0)
                {
                    lowGravityTimer--;
                }
            }
            // if the gravity timer is not active, use default jump speed of 10
            else
            {
                jumpSpeed = ORIGINAL_JUMP_SPEED;
            }

            // if the high jump timer is active and the amount of time passed is a multiple of 10, reduce high jump timer by 1
            if (highJumpTimer > 0)
            {
                if (timePassed % TIME_INTERVAL == 0)
                {
                    highJumpTimer--;
                }
            }
            // if the high jump timer is not active, use default jump speed of 160
            else
            {
                jumpHeight = originalJumpHeight; 
            }

            // is the game is ended, dont allow player to move, enemies to move, and items to be used
            if (gameEnded == false)
            {
                CharacterMovement();

                EnemyMovement();

                ItemUsage();
            }
            // if the game is over, adjust the scoreboard one time 
            else
            {
                // if the scoreboard has not been adjusted, enter the if statement
                if (scoreBoardAdjusted == false)
                {
                    // send the players score and the high score text file to a subprogram for the scoreboard to be adjusted
                    AdjustScoreBoard(coinCount + timeLeft, highScoreFile);
                    scoreBoardAdjusted = true;
                }
            }
            
            // if the user clicked n, save the game by calling a subprogram
            if (currentKB.IsKeyDown(Keys.N))
            {
                SaveGame(saveFile);
            }
            // if the user clicked m, load the previous saved game by calling a subprogram
            else if (currentKB.IsKeyDown(Keys.M))
            {
                LoadGame(saveFile);
            }

            // if the final stone barrier is passed to the left a certain point, the game is complete
            if (stoneBricks3X == 0)
            {
                // end the game
                gameEnded = true;

                // stop playing the background music and play victory music
                MediaPlayer.Stop();
                stageClearInstance.Play();
            }

            // alternate between each character to be checked for collision against
            for (byte q = 0; q <= 4; q++)
            {
                // if q is 0, set variables to check for collision with to mario data
                if (q == 0)
                {
                    characterType = "mario";
                    characterX = marioX;
                    characterY = marioY;
                }
                // if q is 1, set variables to check for collision with to the first goombas data
                else if(q == 1)
                {
                    characterType = "enemy1";
                    characterX = goombaX[0];
                    characterY = goombaY[0];
                }
                // if q is 2, set variables to check for collision with to the second goombas data
                else if (q == 2)
                {
                    characterType = "enemy2";
                    characterX = goombaX[1];
                    characterY = goombaY[1];
                }
                // if q is 3, set variables to check for collision with to the third goombas data
                else if (q == 3)
                {
                    characterType = "enemy3";
                    characterX = goombaX[2];
                    characterY = goombaY[2];
                }
                // if q is 4, set variables to check for collision with to the fourth goombas data
                else if (q == 4)
                {
                    characterType = "enemy4";
                    characterX = goombaX[3];
                    characterY = goombaY[3];
                }

                // check collision against all the square brick platforms. These blocks are checked for collision so players and enemies can intercat with them in game
                CheckForCollision(squareBricks1X, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks1X + squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks1X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks2X, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks2X + squareBrick.Width, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks2X + squareBrick.Width * 2, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks3X, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks3X + squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks3X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks4X, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks4X + squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks4X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks5X, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks5X + squareBrick.Width, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks5X + squareBrick.Width * 2, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks6X, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks6X + squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks6X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks7X, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks7X + +squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks7X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks7X + squareBrick.Width * 3, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks7X + squareBrick.Width * 4, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks8X, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks8X + squareBrick.Width, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks8X + squareBrick.Width * 2, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks8X + squareBrick.Width * 3, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks8X + squareBrick.Width * 4, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks8X + squareBrick.Width * 5, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks8X + squareBrick.Width * 6, platform2Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks9X, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);
                CheckForCollision(squareBricks9X + squareBrick.Width * 10, platform1Height, squareBrick.Width, squareBrick.Height, characterType, characterX, characterY);

                // check collision against all the stone bricks in the game to create barriers and platforms for the player
                CheckForCollision(stoneBricks1X, brickBackground.Height - squareBrick.Height - stoneBrick.Height, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 2, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 2, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 2, brickBackground.Height - squareBrick.Height - stoneBrick.Height, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 4, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 3, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 6, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 7, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 8, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 9, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 10, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 11, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 12, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 13, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 14, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 15, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 16, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 17, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 18, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 19, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 20, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 20, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 5, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks1X + stoneBrick.Width * 20, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 6, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks2X + stoneBrick.Width * 56, platform2Height - stoneBrick.Height * 1, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks2X + stoneBrick.Width * 56, platform2Height - stoneBrick.Height * 2, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks2X + stoneBrick.Width * 56, platform2Height - stoneBrick.Height * 3, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 1, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 2, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 3, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                CheckForCollision(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 5, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);

                // check collision against all the items that can be collected in the game to check if an item was collected
                CheckForCollision(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height, characterType, characterX, characterY);
                CheckForCollision(greenMushroomX, greenMushroomY, greenMushroom.Width, greenMushroom.Height, characterType, characterX, characterY);
                CheckForCollision(yellowMushroomX, yellowMushroomY, yellowMushroom.Width, yellowMushroom.Height, characterType, characterX, characterY);
                CheckForCollision(starX, starY, star.Width, star.Height, characterType, characterX, characterY);

                // check collision against a stone brick staircase to allow players to climb and interect with it
                byte blockSpace = 0;
                for (byte x = 32; x <= 59; x++)
                {
                    CheckForCollision(stoneBricks2X + stoneBrick.Width * blockSpace, platform2Height, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                    blockSpace++;
                }

                // check collision against a second stone brick staircase to allow players to climb and interect with it
                blockSpace = 31;
                for (byte x = 60; x <= 85; x++)
                {
                    CheckForCollision(stoneBricks2X + stoneBrick.Width * blockSpace, platform2Height, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                    blockSpace++;
                }

                // check collision against the pipes arround the map to allow players to jump on and interect with them
                CheckForCollision(pipe1X, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width, pipe.Height, characterType, characterX, characterY);
                CheckForCollision(pipe1X + twoPipeDistance, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width, pipe.Height, characterType, characterX, characterY);
                CheckForCollision(pipe1X + twoPipeDistance + squareBrick.Width, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width / 2, pipe.Height, characterType, characterX, characterY);
                CheckForCollision(pipe2X, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width, pipe.Height, characterType, characterX, characterY);
                CheckForCollision(pipe2X + squareBrick.Width, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width / 2, pipe.Height, characterType, characterX, characterY);
                CheckForCollision(pipe2X + twoPipeDistance, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width, pipe.Height, characterType, characterX, characterY);
                CheckForCollision(pipe2X + twoPipeDistance + squareBrick.Width, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width / 2, pipe.Height, characterType, characterX, characterY);
                CheckForCollision(tallPipeX, brickBackground.Height - squareBrick.Height - tallPipe.Height, tallPipe.Width, tallPipe.Height, characterType, characterX, characterY);
                CheckForCollision(tallPipeX + squareBrick.Width, brickBackground.Height - squareBrick.Height - tallPipe.Height, tallPipe.Width / 2, tallPipe.Height, characterType, characterX, characterY);

                // check collision against the cannons so players can jump o and interact with them
                CheckForCollision(cannonX, brickBackground.Height - squareBrick.Height - cannon.Height, cannon.Width, cannon.Height, characterType, characterX, characterY);
                CheckForCollision(cannonX + squareBrick.Width * 10, brickBackground.Height - squareBrick.Height - cannon.Height, cannon.Width, cannon.Height, characterType, characterX, characterY);
                CheckForCollision(cannonX + squareBrick.Width * 20, brickBackground.Height - squareBrick.Height - cannon.Height, cannon.Width, cannon.Height, characterType, characterX, characterY);

                // check collision against a first brown block staircase
                int colStartDisp = -1;
                for (byte blockRow = 0; blockRow <= 3; blockRow++)
                {
                    colStartDisp++;
                    for (int blockCol = colStartDisp; blockCol <= 3; blockCol++)
                    {
                        CheckForCollision(brownBlockX + brownBlock.Width * blockCol, brickBackground.Height - squareBrick.Height - brownBlock.Height - brownBlock.Height * blockRow, brownBlock.Width, brownBlock.Height, characterType, characterX, characterY);
                    }
                }

                // check collision against a second brown block staircase
                int colEndDisp = 4;
                for (byte blockRow = 0; blockRow <= 3; blockRow++)
                {
                    colEndDisp--;
                    for (int blockCol = 0; blockCol <= colEndDisp; blockCol++)
                    {
                        CheckForCollision(brownBlockX + brownStairDistance + brownBlock.Width * blockCol, brickBackground.Height - squareBrick.Height - brownBlock.Height - brownBlock.Height * blockRow, brownBlock.Width, brownBlock.Height, characterType, characterX, characterY);
                    }
                }

                // check collision against a third brown block staircase
                colStartDisp = -1;
                for (byte blockRow = 0; blockRow <= 7; blockRow++)
                {
                    colStartDisp++;
                    for (int blockCol = colStartDisp; blockCol <= 7; blockCol++)
                    {
                        CheckForCollision(brownBlock2X + brownBlock.Width * blockCol, brickBackground.Height - squareBrick.Height - brownBlock.Height - brownBlock.Height * blockRow, brownBlock.Width, brownBlock.Height, characterType, characterX, characterY);
                    }
                }

                // check collision against a fourth brown block staircase
                colStartDisp = -1;
                for (byte blockRow = 0; blockRow <= 7; blockRow++)
                {
                    colStartDisp++;
                    for (int blockCol = colStartDisp; blockCol <= 7; blockCol++)
                    {
                        CheckForCollision(brownBlock3X + brownBlock.Width * blockCol, brickBackground.Height - squareBrick.Height - brownBlock.Height - brownBlock.Height * blockRow, brownBlock.Width, brownBlock.Height, characterType, characterX, characterY);
                    }
                }

                // check collision against a stone brick staircase
                colStartDisp = -1;
                for (byte blockRow = 0; blockRow <= 3; blockRow++)
                {
                    colStartDisp++;
                    for (int blockCol = colStartDisp; blockCol <= 3; blockCol++)
                    {
                        CheckForCollision(greyStairX + stoneBrick.Width * blockCol, brickBackground.Height - squareBrick.Height - stoneBrick.Height - stoneBrick.Height * blockRow, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                    }
                }

                // check collision against a rectangle of stone brick staircase
                for (byte blockRow = 2; blockRow <= 7; blockRow++)
                {
                    for (int blockCol = 0; blockCol <= 10; blockCol++)
                    {
                        CheckForCollision(greyRectangleX + stoneBrick.Width * blockCol, brickBackground.Height - squareBrick.Height - stoneBrick.Height - stoneBrick.Height * blockRow, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                    }
                }

                // check collision against a wall barrier that stops the player from going out of the castle
                if (castleX + distanceToCastleDoor - marioX <= 0)
                {
                    for (byte y = 0; y <= 10; y++)
                    {
                        CheckForCollision(stoneWallX, brickBackground.Height - squareBrick.Height - stoneBrick.Height - stoneBrick.Height * y, stoneBrick.Width, stoneBrick.Height, characterType, characterX, characterY);
                    }
                }

                // check collision against all goombas to see if the player must die or a goomba must die
                CheckForCollision(goombaX[0], goombaY[0], goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height, characterType, characterX, characterY);
                CheckForCollision(goombaX[1], goombaY[1], goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height, characterType, characterX, characterY);
                CheckForCollision(goombaX[2], goombaY[2], goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height, characterType, characterX, characterY);
                CheckForCollision(goombaX[3], goombaY[3], goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height, characterType, characterX, characterY);

                // check collision against all the coins to check if a player collected a coin
                for (byte c = 0; c < coinBounds.Length; c++)
                {
                    CheckForCollision(coinX[c], coinY[c], coin.Width, coin.Height, characterType, characterX, characterY);
                }
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // start the sprite batch
            spriteBatch.Begin();

            // draw the two backgrounds 
            spriteBatch.Draw(brickBackground, brickBackgroundBounds, Color.White);
            spriteBatch.Draw(brickBackground, brickBackgroundBounds2, Color.White);

            // draw the title box
            spriteBatch.Draw(titleSprite, titleSpriteBounds, Color.White);

            // draw all the square brick platforms and item boxes
            spriteBatch.Draw(squareBrick, squareBrickBounds[0], Color.White);
            spriteBatch.Draw(itemBox, squareBrickBounds[1], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[2], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[3], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[4], Color.White);
            spriteBatch.Draw(itemBox, squareBrickBounds[5], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[6], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[7], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[8], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[9], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[10], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[11], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[12], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[13], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[14], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[15], Color.White);
            spriteBatch.Draw(itemBox, squareBrickBounds[16], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[17], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[18], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[19], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[20], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[21], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[22], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[21], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[22], Color.White);
            spriteBatch.Draw(itemBox, squareBrickBounds[23], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[24], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[25], Color.White);
            spriteBatch.Draw(itemBox, squareBrickBounds[26], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[27], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[28], Color.White);
            spriteBatch.Draw(squareBrick, squareBrickBounds[29], Color.White);
            spriteBatch.Draw(itemBox, squareBrickBounds[30], Color.White);
            spriteBatch.Draw(itemBox, squareBrickBounds[31], Color.White);

            // draw the pipes 
            spriteBatch.Draw(pipe, pipeBounds[0], Color.White);
            spriteBatch.Draw(pipe, pipeBounds[1], Color.White);
            spriteBatch.Draw(pipe, pipeBounds[2], Color.White);
            spriteBatch.Draw(pipe, pipeBounds[3], Color.White);
            spriteBatch.Draw(tallPipe, tallPipeBounds, Color.White);

            // draw the cannons 
            spriteBatch.Draw(cannon, cannonBounds[0], Color.White);
            spriteBatch.Draw(cannon, cannonBounds[1], Color.White);
            spriteBatch.Draw(cannon, cannonBounds[2], Color.White);

            // draw the castle
            spriteBatch.Draw(castle, castleBounds, Color.White);

            // draw the flag
            spriteBatch.Draw(flag, flagBounds, Color.White);

            // draw the first two stair cases 
            for (byte displayBrownBlock = 0; displayBrownBlock < brownBlockBounds.Length; displayBrownBlock++)
            {
                spriteBatch.Draw(brownBlock, brownBlockBounds[displayBrownBlock], Color.White);
            }

            // draw the stone wall
            for (byte y = 0; y < stoneBrickBounds.Length; y++)
            {
                spriteBatch.Draw(stoneBrick, stoneBrickBounds[y], Color.White);
            }
            
            // draw the non-ground blue block
            spriteBatch.Draw(noGroundBlue, noGroundBlueBounds[0], Color.White);
            spriteBatch.Draw(noGroundBlue, noGroundBlueBounds[1], Color.White);
            spriteBatch.Draw(noGroundBlue, noGroundBlueBounds[2], Color.White);
            spriteBatch.Draw(noGroundBlue, noGroundBlueBounds[3], Color.White);
            spriteBatch.Draw(noGroundBlue, noGroundBlueBounds[4], Color.White);
            spriteBatch.Draw(noGroundBlue, noGroundBlueBounds[5], Color.White);
            spriteBatch.Draw(noGroundBlue, noGroundBlueBounds[6], Color.White);
            spriteBatch.Draw(noGroundBlue, noGroundBlueBounds[7], Color.White);

            // draw mario on screen
            spriteBatch.Draw(marioSprite, marioSpriteBounds, marioSrcBounds, Color.White);

            // draw all the goombas
            spriteBatch.Draw(goombaSprite, goombaSpriteBounds[0], goombaSrcBounds, Color.White);
            spriteBatch.Draw(goombaSprite, goombaSpriteBounds[1], goombaSrcBounds, Color.White);
            spriteBatch.Draw(goombaSprite, goombaSpriteBounds[2], goombaSrcBounds, Color.White);
            spriteBatch.Draw(goombaSprite, goombaSpriteBounds[3], goombaSrcBounds, Color.White);

            // draw the coins arround the screen
            for (byte i = 0; i < coinBounds.Length; i++)
            {
                spriteBatch.Draw(coin, coinBounds[i], Color.White);
            }

            // draw the items and collectables on screen
            spriteBatch.Draw(redMushroom, redMushroomBounds, Color.White);
            spriteBatch.Draw(greenMushroom, greenMushroomBounds, Color.White);
            spriteBatch.Draw(yellowMushroom, yellowMushroomBounds, Color.White);
            spriteBatch.Draw(star, starBounds, Color.White);

            // if the first item spot is full, draw the item that it is storing
            if (item1 != null)
            {
                item1Bounds = new Rectangle(brickBackground.Width - greenMushroom.Width * 2, 0, greenMushroom.Width, greenMushroom.Height);
                spriteBatch.Draw(item1, item1Bounds, Color.White);
            }
            // if the second item spot is full, draw the item that it is storing
            if (item2 != null)
            {
                item2Bounds = new Rectangle(brickBackground.Width - star.Width, 0, star.Width, star.Height);
                spriteBatch.Draw(item2, item2Bounds, Color.White);
            }

            // draw the coin count title and coin counter
            spriteBatch.DrawString(font, "Coin Count: " + coinCount, scorePosition, Color.White);

            // if the player is in the castle, display stage 3
            if (castleX  + distanceToCastleDoor - marioX <= 0)
            {
                spriteBatch.DrawString(font, "Stage: 3", stagePosition, Color.White);
            }
            // if the player is in the desert, display stage 2
            else if (brownBlock2X + distanceToNextWorld <= 0)
            {
                spriteBatch.DrawString(font, "Stage: 2", stagePosition, Color.White);
            }
            // if the player is anywhere else, display stage 1
            else
            {
                spriteBatch.DrawString(font, "Stage: 1", stagePosition, Color.White);
            }
            

            // draw the time left title and the time left counter
            spriteBatch.DrawString(font, "Time Left: " + timeLeft, timeLeftPosition, Color.White);

            // if the game is over, display the high score board
            if (gameEnded == true)
            {
                // display the title of the scoreboard on the screen
                spriteBatch.DrawString(font, "High Score Board (coin count + time left): ", scoreBoardPosition, Color.White);
                // display every score stored in the array to the screen with a vertical downward shit after each score
                for (int x = 0; x < scoreBoard.Length - 1; x++)
                {
                    scoreBoardPosition = new Vector2(scoreBoardX, scoreBoardStartY + scoreBoardPositionY * (x + 1));
                    spriteBatch.DrawString(font, "" + (x + 1) + "." + scoreBoard[x], scoreBoardPosition, Color.White); 
                }

                scoreBoardPosition = new Vector2(scoreBoardX, scoreBoardStartY);
            }

            // end the sprite batch
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Pre: N/A
        //Post: N/A
        //Description: change the location of the backgrounds and objects or the character when the character is moved
        private void CharacterMovement()
        {
            // if the user is past a certain point after the large staircase, switch background
            if (brownBlock2X + distanceToNextWorld <= 0)
            {
                brickBackground = Content.Load<Texture2D>(@"Images\Backgrounds\desertbackdrop");
            }

            // if mario is at the castle door, switch background to castle backgorund and remove the castle from the screen
            if (castleX + distanceToCastleDoor - marioX <= 0)
            {
                brickBackground = Content.Load<Texture2D>(@"Images\Backgrounds\castleBackground");
                castleX = castleX - castleXShift;
                castleBounds = new Rectangle(castleX, brickBackground.Height - squareBrick.Height - castle.Height, castle.Width, castle.Height);
            }

            // if the player clicks the d key, enter the if statement. D is pressed to make mario move to the right
            if (currentKB.IsKeyDown(Keys.D))
            {
                // start the game
                gameStarted = true;

                // set marios direction to right
                marioDirection = "right";

                // move the backgrounds to the left
                brickBackgroundX = brickBackgroundX - moveSpeed;
                brickBackgroundX2 = brickBackgroundX2 - moveSpeed;

                // move all the brick platforms to the left
                squareBricks1X = squareBricks1X - moveSpeed;
                squareBricks2X = squareBricks2X - moveSpeed;
                squareBricks3X = squareBricks3X - moveSpeed;
                squareBricks4X = squareBricks4X - moveSpeed;
                squareBricks5X = squareBricks5X - moveSpeed;
                squareBricks6X = squareBricks6X - moveSpeed;
                squareBricks7X = squareBricks7X - moveSpeed;
                squareBricks8X = squareBricks8X - moveSpeed;
                squareBricks9X = squareBricks9X - moveSpeed;

                // move the collectable items to the left
                redMushroomX = redMushroomX - moveSpeed;
                greenMushroomX = greenMushroomX - moveSpeed;
                yellowMushroomX = yellowMushroomX - moveSpeed;
                starX = starX - moveSpeed;

                // move the stone bricks to the left
                stoneBricks1X = stoneBricks1X - moveSpeed;
                stoneBricks2X = stoneBricks2X - moveSpeed;
                stoneBricks3X = stoneBricks3X - moveSpeed;
                stoneWallX = stoneWallX - moveSpeed;
                greyStairX = greyStairX - moveSpeed;
                greyRectangleX = greyRectangleX - moveSpeed;

                // move the pipes to the left 
                pipe1X = pipe1X - moveSpeed;
                pipe2X = pipe2X - moveSpeed;
                tallPipeX = tallPipeX - moveSpeed;

                // move the cannons reference point to the left
                cannonX = cannonX - moveSpeed;

                // move the stair case reference points to the left
                brownBlockX = brownBlockX - moveSpeed;
                brownBlock2X = brownBlock2X - moveSpeed;
                brownBlock3X = brownBlock3X - moveSpeed;

                // move the groundless blue blocks reference points to the left
                noGroundBlueX = noGroundBlueX - moveSpeed;

                // move the goomba location to the left
                for (int i = 0; i < goombaX.Length; i++)
                {
                    goombaX[i] = goombaX[i] - moveSpeed;
                }

                // move castle object to the left
                castleX = castleX - moveSpeed;

                // move all coin x values to the left
                for (byte c = 0; c < coinX.Length; c++)
                {
                    coinX[c] = coinX[c] - moveSpeed;
                }

                // move the temporary object reference that is colided with to the left
                objXTemp = objXTemp - moveSpeed;

                // call subprogram which updates all location bounds for objects in the game
                UpdateLocations();

                // if 8 frames have passed, increase the frame number by 1 until it has reached the last frame and then restart the cycle of frames
                if (timePassed % SMOOTHNESS == 0)
                {
                    frameNum = (frameNum + 1) % numFrames;
                }

                // if frameNum is equal to 0, display the first right running frame
                if (frameNum == 0)
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 7, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }
                // if frameNum is equal to 1, display the second right running frame
                else if (frameNum == 1)
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 6, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }
                // if frameNum is equal to 2, display the third right running frame
                else if (frameNum == 2)
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 5, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }

                // if the second background moves off the screen to the left, assign the first background to the immediate right of the second background's end
                if (brickBackgroundX2 <= 0)
                {
                    brickBackgroundX = brickBackgroundX2 + brickBackground.Width;
                    brickBackgroundBounds = new Rectangle(brickBackgroundX, 0, brickBackground.Width, brickBackground.Height);
                }
                // if the first background moves off the screen to the left, assign the second background to the immediate right of the first background's end
                if (brickBackgroundX <= 0)
                {
                    brickBackgroundX2 = brickBackgroundX + brickBackground.Width;
                    brickBackgroundBounds2 = new Rectangle(brickBackgroundX2, 0, brickBackground.Width, brickBackground.Height);
                }
            }
            // if the player clicks the a key, enter the if statement. A is pressed to make mario move to the left
            else if (currentKB.IsKeyDown(Keys.A))
            {
                // set the game to started
                gameStarted = true;

                // set the main characters direction to left
                marioDirection = "left";

                // move the backgrounds to the right
                brickBackgroundX = brickBackgroundX + moveSpeed;
                brickBackgroundX2 = brickBackgroundX2 + moveSpeed;

                // move the square bricks to the right 
                squareBricks1X = squareBricks1X + moveSpeed;
                squareBricks2X = squareBricks2X + moveSpeed;
                squareBricks3X = squareBricks3X + moveSpeed;
                squareBricks4X = squareBricks4X + moveSpeed;
                squareBricks5X = squareBricks5X + moveSpeed;
                squareBricks6X = squareBricks6X + moveSpeed;
                squareBricks7X = squareBricks7X + moveSpeed;
                squareBricks8X = squareBricks8X + moveSpeed;
                squareBricks9X = squareBricks9X + moveSpeed;

                // move the collectable items to the right
                redMushroomX = redMushroomX + moveSpeed;
                greenMushroomX = greenMushroomX + moveSpeed;
                yellowMushroomX = yellowMushroomX + moveSpeed;
                starX = starX + moveSpeed;

                //move the stone blocks to the right
                stoneBricks1X = stoneBricks1X + moveSpeed;
                stoneBricks2X = stoneBricks2X + moveSpeed;
                stoneBricks3X = stoneBricks3X + moveSpeed;
                stoneWallX = stoneWallX + moveSpeed;
                greyStairX = greyStairX + moveSpeed;
                greyRectangleX = greyRectangleX + moveSpeed;

                // move the pipes x reference point to the right
                pipe1X = pipe1X + moveSpeed;
                pipe2X = pipe2X + moveSpeed;
                tallPipeX = tallPipeX + moveSpeed;

                // move the cannons reference points to the right
                cannonX = cannonX + moveSpeed;

                // move the brown blocks reference point to the right
                brownBlockX = brownBlockX + moveSpeed;
                brownBlock2X = brownBlock2X + moveSpeed;
                brownBlock3X = brownBlock3X + moveSpeed;

                // move the non-ground blue blocks reference point to the right
                noGroundBlueX = noGroundBlueX + moveSpeed;

                // move all the goombas to the right
                for (int i = 0; i < goombaX.Length; i++)
                {
                    goombaX[i] = goombaX[i] + moveSpeed;
                }

                // move the castle to the right
                castleX = castleX + moveSpeed;
                
                // move the coin location to the right
                for (byte c = 0; c < coinX.Length; c++)
                {
                    coinX[c] = coinX[c] + moveSpeed;
                }

                // move the temporary object reference that was last colided with to the right
                objXTemp = objXTemp + moveSpeed;

                // call subprogram which updates all location bounds for objects in the game
                UpdateLocations();

                // if 8 frames have passed, increase the frame number by 1 until it has reached the last frame and then restart the cycle of frames
                if (timePassed % SMOOTHNESS == 0)
                {
                    frameNum = (frameNum + 1) % numFrames;
                }

                // if frameNum is equal to 0, display the first left running frame
                if (frameNum == 0)
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 0, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }
                // if frameNum is equal to 1, display the second left running frame
                else if (frameNum == 1)
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 1, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }
                // if frameNum is equal to 2, display the third left running frame
                else if (frameNum == 2)
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 2, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }

                // if the first background moves off the screen to the right, assign the second background to the immediate left of the first background's start
                if (brickBackgroundX >= 0)
                {
                    brickBackgroundX2 = brickBackgroundX - brickBackground.Width;
                    brickBackgroundBounds2 = new Rectangle(brickBackgroundX2, 0, brickBackground.Width, brickBackground.Height);
                }
                // if the second background moves off the screen to the right, assign the first background to the immediate left of the second background's start
                if (brickBackgroundX2 >= 0)
                {
                    brickBackgroundX = brickBackgroundX2 - brickBackground.Width;
                    brickBackgroundBounds = new Rectangle(brickBackgroundX, 0, brickBackground.Width, brickBackground.Height);
                }
            }
            // if neither the a or d key is pressed, enter this code
            else
            {
                // if the previous direction of mario was left, show the standing image or mario looking left
                if (marioDirection == "left")
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 3, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }
                // if the previous direction of mario was not left, show the standing image or mario looking right
                else
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 4, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }
            }

            // if the character pressed the w key and was previously not pressing this key, enter the if statement. W is pressed for mario to jump
            if (currentKB.IsKeyDown(Keys.W) && oldKB.IsKeyUp(Keys.W))
            {
                // set the game to started
                gameStarted = true;

                // if the character is on the ground enter the if statement
                if (onGround == true)
                {
                    // set the just direction to the direction mario is looking
                    marioDirectionJump = marioDirection;

                    // set the jump to true and set state the character is not on the ground
                    isJumpOn = true;
                    onGround = false;

                    // save the origional y value of mario when he started jumping
                    marioYOriginal = marioY;
                }
            }

            // if mario is jumping, enter the if statement
            if (isJumpOn == true)
            {
                // continue to make mario move up in a jump
                marioY = marioY - jumpSpeed;

                // define the new mario sprite bounds
                marioSpriteBounds = new Rectangle(marioX, marioY, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);

                // if the jump direction is left, set the mario image to him jumping facing left
                if (marioDirectionJump == "left")
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 2, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }
                // in any other case, show mario jumping facing the right
                else
                {
                    marioSrcBounds = new Rectangle(marioSprite.Width / NumOfMarioImgs * 5, 0, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
                }

                // if mario reaches the top if his jump height, set jump to false
                if (marioY < marioYOriginal - jumpHeight)
                {
                    isJumpOn = false;
                }
            }
            // if mario is not on the ground and not jumping, enter the if statement 
            else if (onGround == false && isJumpOn == false)
            {
                // add to marios y location to make him move towards the ground
                marioY = marioY + jumpSpeed;

                // update marios location to account for the change is height due to the drop
                marioSpriteBounds = new Rectangle(marioX, marioY, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
            }
        }

        // Pre: N/A
        // Post: N/A
        // Description: update all the bounds for all the objects so when they are drawn the new image will be in accordance to any changes made to its location
        private void UpdateLocations()
        {
            // reset all counter variables to start at their starting values
            int colStart = -1;
            byte blockNum = 0;
            byte greyBrickStairIndex = 91;
            byte greyBrickRectangleIndex = 107;
            int colEnd = 4;

            // update the bounds for the blocks in the first starcase
            for (byte blockRow = 0; blockRow <= stairLength3; blockRow++)
            {
                colStart++;
                for (int blockCol = colStart; blockCol <= 3; blockCol++)
                {
                    brownBlockBounds[blockNum] = new Rectangle(brownBlockX + brownBlock.Width * blockCol, brickBackground.Height - squareBrick.Height - brownBlock.Height - brownBlock.Height * blockRow, brownBlock.Width, brownBlock.Height);
                    blockNum++;
                }
            }

            // update the bounds for the blocks in the second staircase
            for (byte blockRow = 0; blockRow <= stairLength3; blockRow++)
            {
                colEnd--;
                for (int blockCol = 0; blockCol <= colEnd; blockCol++)
                {
                    brownBlockBounds[blockNum] = new Rectangle(brownBlockX + brownStairDistance + brownBlock.Width * blockCol, brickBackground.Height - squareBrick.Height - brownBlock.Height - brownBlock.Height * blockRow, brownBlock.Width, brownBlock.Height);
                    blockNum++;
                }
            }

            // set the colStart to its starting value
            colStart = -1;
            // update the bounds for the blocks in the third staircase
            for (byte blockRow = 0; blockRow <= stairLength7; blockRow++)
            {
                colStart++;
                for (int blockCol = colStart; blockCol <= stairLength7; blockCol++)
                {
                    brownBlockBounds[blockNum] = new Rectangle(brownBlock2X + brownBlock.Width * blockCol, brickBackground.Height - squareBrick.Height - brownBlock.Height - brownBlock.Height * blockRow, brownBlock.Width, brownBlock.Height);
                    blockNum++;
                }
            }

            // set the colStart to its starting value
            colStart = -1;
            // update the bounds for the blocks in the third staircase
            for (byte blockRow = 0; blockRow <= stairLength7; blockRow++)
            {
                colStart++;
                for (int blockCol = colStart; blockCol <= stairLength7; blockCol++)
                {
                    brownBlockBounds[blockNum] = new Rectangle(brownBlock3X + brownBlock.Width * blockCol, brickBackground.Height - squareBrick.Height - brownBlock.Height - brownBlock.Height * blockRow, brownBlock.Width, brownBlock.Height);
                    blockNum++;
                }
            }

            // if mario reached the castle door, enter the if statement
            if (castleX + distanceToCastleDoor - marioX <= 0)
            {
                //update the location for the stone brick wall at the left of the castle level
                for (byte y = 0; y <= 10; y++)
                {
                    stoneBrickBounds[y] = new Rectangle(stoneWallX, brickBackground.Height - squareBrick.Height - stoneBrick.Height - stoneBrick.Height * y, stoneBrick.Width, stoneBrick.Height);
                }

                // set the blockSpace to its starting value for the counter
                byte blockSpace = 0;
                // update the bounds for the blocks in the stone staircase
                for (byte x = 32; x <= 59; x++)
                {
                    stoneBrickBounds[x] = new Rectangle(stoneBricks2X + stoneBrick.Width * blockSpace, platform2Height, stoneBrick.Width, stoneBrick.Height);
                    blockSpace++;
                }

                // set blockSpace to its staring value for the counter
                blockSpace = 31;
                // update the bounds for the blocks in the stone rectangle
                for (byte x = 60; x <= 85; x++)
                {
                    stoneBrickBounds[x] = new Rectangle(stoneBricks2X + stoneBrick.Width * blockSpace, platform2Height, stoneBrick.Width, stoneBrick.Height);
                    blockSpace++;
                }

                // update the bounds for all the stone brick platforms and walkways. These blocks are used to define the level and create the platforms and areas for players to explore 
                stoneBrickBounds[11] = new Rectangle(stoneBricks1X, brickBackground.Height - squareBrick.Height - stoneBrick.Height, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[12] = new Rectangle(stoneBricks1X + stoneBrick.Width * 2, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 2, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[13] = new Rectangle(stoneBricks1X + stoneBrick.Width * 2, brickBackground.Height - squareBrick.Height - stoneBrick.Height, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[14] = new Rectangle(stoneBricks1X + stoneBrick.Width * 4, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 3, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[15] = new Rectangle(stoneBricks1X + stoneBrick.Width * 6, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[16] = new Rectangle(stoneBricks1X + stoneBrick.Width * 7, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[17] = new Rectangle(stoneBricks1X + stoneBrick.Width * 8, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[18] = new Rectangle(stoneBricks1X + stoneBrick.Width * 9, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[19] = new Rectangle(stoneBricks1X + stoneBrick.Width * 10, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[20] = new Rectangle(stoneBricks1X + stoneBrick.Width * 11, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[21] = new Rectangle(stoneBricks1X + stoneBrick.Width * 12, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[22] = new Rectangle(stoneBricks1X + stoneBrick.Width * 13, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[23] = new Rectangle(stoneBricks1X + stoneBrick.Width * 14, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[26] = new Rectangle(stoneBricks1X + stoneBrick.Width * 15, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[27] = new Rectangle(stoneBricks1X + stoneBrick.Width * 16, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[28] = new Rectangle(stoneBricks1X + stoneBrick.Width * 17, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[29] = new Rectangle(stoneBricks1X + stoneBrick.Width * 18, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[30] = new Rectangle(stoneBricks1X + stoneBrick.Width * 19, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[31] = new Rectangle(stoneBricks1X + stoneBrick.Width * 20, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[86] = new Rectangle(stoneBricks1X + stoneBrick.Width * 20, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 5, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[87] = new Rectangle(stoneBricks1X + stoneBrick.Width * 20, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 6, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[88] = new Rectangle(stoneBricks2X + stoneBrick.Width * 56, platform2Height - stoneBrick.Height * 1, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[89] = new Rectangle(stoneBricks2X + stoneBrick.Width * 56, platform2Height - stoneBrick.Height * 2, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[90] = new Rectangle(stoneBricks2X + stoneBrick.Width * 56, platform2Height - stoneBrick.Height * 3, stoneBrick.Width, stoneBrick.Height);

                // set colStart to its starting value
                colStart = -1;
                // update the bounds for the blocks in the stone final barrier
                for (byte blockRow = 0; blockRow <= stairLength3; blockRow++)
                {
                    colStart++;
                    for (int blockCol = colStart; blockCol <= stairLength3; blockCol++)
                    {
                        stoneBrickBounds[greyBrickStairIndex] = new Rectangle(greyStairX + stoneBrick.Width * blockCol, brickBackground.Height - squareBrick.Height - stoneBrick.Height - stoneBrick.Height * blockRow, stoneBrick.Width, stoneBrick.Height);
                        greyBrickStairIndex++;
                    }
                }

                // update the bounds for the blocks in the stone large rectagle platform
                for (byte blockRow = 2; blockRow <= 7; blockRow++)
                {
                    for (int blockCol = 0; blockCol <= 10; blockCol++)
                    {
                        stoneBrickBounds[greyBrickRectangleIndex] = new Rectangle(greyRectangleX + stoneBrick.Width * blockCol, brickBackground.Height - squareBrick.Height - stoneBrick.Height - stoneBrick.Height * blockRow, stoneBrick.Width, stoneBrick.Height);
                        greyBrickRectangleIndex++;
                    }
                }

                //update the stone blocks in the wall at the end of the deadend tunnel
                stoneBrickBounds[173] = new Rectangle(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 1, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[174] = new Rectangle(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 2, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[175] = new Rectangle(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 3, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[176] = new Rectangle(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 4, stoneBrick.Width, stoneBrick.Height);
                stoneBrickBounds[177] = new Rectangle(stoneBricks3X, brickBackground.Height - squareBrick.Height - stoneBrick.Height * 5, stoneBrick.Width, stoneBrick.Height);
            }

            // update the first patch of blue blocks that make the first drop
            noGroundBlueBounds[0] = new Rectangle(noGroundBlueX, brickBackground.Height - squareBrick.Height, noGroundBlue.Width, noGroundBlue.Height);
            noGroundBlueBounds[1] = new Rectangle(noGroundBlueX + noGroundBlue.Width, brickBackground.Height - squareBrick.Height, noGroundBlue.Width, noGroundBlue.Height);
            noGroundBlueBounds[2] = new Rectangle(noGroundBlueX + noGroundBlue.Width * 2, brickBackground.Height - squareBrick.Height, noGroundBlue.Width, noGroundBlue.Height);
            noGroundBlueBounds[3] = new Rectangle(noGroundBlueX + noGroundBlue.Width * 3, brickBackground.Height - squareBrick.Height, noGroundBlue.Width, noGroundBlue.Height);

            // update the second patch of blue blocks that make the second drop
            noGroundBlueBounds[4] = new Rectangle(noGroundBlueX + noGroundBlue.Width * 5, brickBackground.Height - squareBrick.Height, noGroundBlue.Width, noGroundBlue.Height);
            noGroundBlueBounds[5] = new Rectangle(noGroundBlueX + noGroundBlue.Width * 6, brickBackground.Height - squareBrick.Height, noGroundBlue.Width, noGroundBlue.Height);
            noGroundBlueBounds[6] = new Rectangle(noGroundBlueX + noGroundBlue.Width * 7, brickBackground.Height - squareBrick.Height, noGroundBlue.Width, noGroundBlue.Height);
            noGroundBlueBounds[7] = new Rectangle(noGroundBlueX + noGroundBlue.Width * 8, brickBackground.Height - squareBrick.Height, noGroundBlue.Width, noGroundBlue.Height);

            // update the location of all the coins
            for (byte c = 0; c < coinBounds.Length; c++)
            {
                coinBounds[c] = new Rectangle(coinX[c], coinY[c], coin.Width, coin.Height);
            }

            // update the location bounds of all the pipes in the game
            pipeBounds[0] = new Rectangle(pipe1X, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width, pipe.Height);
            pipeBounds[1] = new Rectangle(pipe1X + twoPipeDistance, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width, pipe.Height);
            pipeBounds[2] = new Rectangle(pipe2X, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width, pipe.Height);
            pipeBounds[3] = new Rectangle(pipe2X + twoPipeDistance, brickBackground.Height - squareBrick.Height - pipe.Height, pipe.Width, pipe.Height);
            tallPipeBounds = new Rectangle(tallPipeX, brickBackground.Height - squareBrick.Height - tallPipe.Height, tallPipe.Width, tallPipe.Height);

            // update the bound for the castle to adjust its location
            castleBounds = new Rectangle(castleX, brickBackground.Height - squareBrick.Height - castle.Height, castle.Width, castle.Height);

            // update the bound for the flag to adjust its location
            flagBounds = new Rectangle(stoneBricks3X + distanceToFlag, brickBackground.Height - squareBrick.Height - flag.Height, flag.Width, flag.Height);

            // update the bound of the cannons to adjust their locations
            cannonBounds[0] = new Rectangle(cannonX, brickBackground.Height - squareBrick.Height - cannon.Height, cannon.Width, cannon.Height);
            cannonBounds[1] = new Rectangle(cannonX + squareBrick.Width * distanceToCannon2, brickBackground.Height - squareBrick.Height - cannon.Height, cannon.Width, cannon.Height);
            cannonBounds[2] = new Rectangle(cannonX + squareBrick.Width * distanceToCannon3, brickBackground.Height - squareBrick.Height - cannon.Height, cannon.Width, cannon.Height);

            // update the bounds for the two background images
            brickBackgroundBounds = new Rectangle(brickBackgroundX, 0, brickBackground.Width, brickBackground.Height);
            brickBackgroundBounds2 = new Rectangle(brickBackgroundX2, 0, brickBackground.Width, brickBackground.Height);

            //update the bounds for all the randomly located brick platforms
            squareBrickBounds[0] = new Rectangle(squareBricks1X, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[1] = new Rectangle(squareBricks1X + squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[2] = new Rectangle(squareBricks1X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[3] = new Rectangle(squareBricks2X, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[4] = new Rectangle(squareBricks2X + squareBrick.Width, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[5] = new Rectangle(squareBricks2X + squareBrick.Width * 2, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[6] = new Rectangle(squareBricks3X, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[7] = new Rectangle(squareBricks3X + squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[8] = new Rectangle(squareBricks3X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[9] = new Rectangle(squareBricks4X, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[10] = new Rectangle(squareBricks4X + squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[11] = new Rectangle(squareBricks4X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[12] = new Rectangle(squareBricks5X, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[13] = new Rectangle(squareBricks5X + squareBrick.Width, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[14] = new Rectangle(squareBricks5X + squareBrick.Width * 2, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[15] = new Rectangle(squareBricks6X, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[16] = new Rectangle(squareBricks6X + squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[17] = new Rectangle(squareBricks6X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[18] = new Rectangle(squareBricks7X, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[19] = new Rectangle(squareBricks7X + +squareBrick.Width, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[20] = new Rectangle(squareBricks7X + squareBrick.Width * 2, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[21] = new Rectangle(squareBricks7X + squareBrick.Width * 3, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[22] = new Rectangle(squareBricks7X + squareBrick.Width * 4, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[23] = new Rectangle(squareBricks8X, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[24] = new Rectangle(squareBricks8X + squareBrick.Width, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[25] = new Rectangle(squareBricks8X + squareBrick.Width * 2, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[26] = new Rectangle(squareBricks8X + squareBrick.Width * 3, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[27] = new Rectangle(squareBricks8X + squareBrick.Width * 4, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[28] = new Rectangle(squareBricks8X + squareBrick.Width * 5, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[29] = new Rectangle(squareBricks8X + squareBrick.Width * 6, platform2Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[30] = new Rectangle(squareBricks9X, platform1Height, squareBrick.Width, squareBrick.Height);
            squareBrickBounds[31] = new Rectangle(squareBricks9X + squareBrick.Width * 10, platform1Height, squareBrick.Width, squareBrick.Height);

            // update the bounds for all teh collectable items
            redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);
            greenMushroomBounds = new Rectangle(greenMushroomX, greenMushroomY, greenMushroom.Width, greenMushroom.Height);
            yellowMushroomBounds = new Rectangle(yellowMushroomX, yellowMushroomY, yellowMushroom.Width, yellowMushroom.Height);
            starBounds = new Rectangle(starX, starY, star.Width, star.Height);

            // update the bounds for all the goombas
            goombaSpriteBounds[0] = new Rectangle(goombaX[0], goombaY[0], goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height);
            goombaSpriteBounds[1] = new Rectangle(goombaX[1], goombaY[1], goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height);
            goombaSpriteBounds[2] = new Rectangle(goombaX[2], goombaY[2], goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height);
            goombaSpriteBounds[3] = new Rectangle(goombaX[3], goombaY[3], goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height);

            // update the bounds for mario
            marioSpriteBounds = new Rectangle(marioX, marioY, marioSprite.Width / NumOfMarioImgs, marioSprite.Height);
        }

        // Pre: N/A
        // Post: N/A
        // Description: update the enemys location values as time passes to make them seem alive
        private void EnemyMovement()
        {
            // if 8 frames have passed, add one to the goomba frame count until its reaches the final goomba frame and then start from the first frame in a cycle
            if (timePassed % SMOOTHNESS == 0)
            {
                goombaFrameNum = (goombaFrameNum + 1) % goombaNumFrames;
            }
    
            // if the goomba frame counter is 0, display the first goomba frame
            if (goombaFrameNum == 0)
            {
                goombaSrcBounds = new Rectangle(goombaSprite.Width / NumOfGoombaImgs * 0, 0, goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height);
            }
            // if the goomba frame count is 1, display the second goomba frame
            else if (goombaFrameNum == 1)
            {
                goombaSrcBounds = new Rectangle(goombaSprite.Width / NumOfGoombaImgs * 1, 0, goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height);
            }

            // is the game has started, enter the if statement
            if (gameStarted == true)
            {
                // move all the goombas at 2 pixels per frame in the direction they are set to move in
                goombaX[0] = goombaX[0] + enemyMoveSpeed * enemyMoveDir;
                goombaX[1] = goombaX[1] + enemyMoveSpeed * enemy2MoveDir;
                goombaX[2] = goombaX[2] + enemyMoveSpeed * enemy3MoveDir;

                // if mario hasnt reached the staircase, tell the final goomba to stop moving to prevent it from falling off the edge before mario encounters it
                if (marioX >= brownBlockX)
                {
                    goombaX[3] = goombaX[3] + enemyMoveSpeed * enemy4MoveDir;
                }
            }

            //if the goomba on the platfrom is on the ground, set on ground to true for that goomba
            if (goombaY[2] >= brickBackground.Height - goombaSprite.Height*2)
            {
                enemy3OnGround = true;
            }

            // if the third or fourth goomba are not on the ground, add to their y value to make them fall
            if (enemy3OnGround == false)
            {
                goombaY[2] = goombaY[2] + jumpSpeed;
            }
            if (enemy4OnGround == false)
            {
                goombaY[3] = goombaY[3] + jumpSpeed;
            }

            // update the bounds of all the goombas in the game
            for (int x = 0; x < goombaX.Length; x++)
            {
                goombaSpriteBounds[x] = new Rectangle(goombaX[x], goombaY[x], goombaSprite.Width / NumOfGoombaImgs, goombaSprite.Height);
            }
        }

        // Pre: an object that is subprogram is checking for colision against's x value, y value, width, height, the type of character the object may
        //      be coliding with, and that characters x and y values
        // Post: N/A
        // Description: Check for a collision between characters or objects in the game and adjust the game accordingly
        private void CheckForCollision(int objX, int objY, int objWidth, int objHeight, string characterType, int characterX, int  characterY)
        {
            // tell the game there is no coin above as default
            bool isCoinAbove = false;
            int eightOffSet = 8;
            int fourOffSet = 4;
            int blockSize = 40;

            // if that character is above ground and it is over the first groundless drop block, enter the if statement
            if ((characterY >= ground && characterX >= noGroundBlueX && characterX <= noGroundBlueX + noGroundBlue.Width) ||
                (characterY >= ground && characterX >= noGroundBlueX + noGroundBlue.Width && characterX <= noGroundBlueX + noGroundBlue.Width + noGroundBlue.Width) ||
                (characterY >= ground && characterX >= noGroundBlueX + noGroundBlue.Width * 2 && characterX <= noGroundBlueX + noGroundBlue.Width * 2 + noGroundBlue.Width) ||
                (characterY >= ground && characterX >= noGroundBlueX + noGroundBlue.Width * 5 && characterX <= noGroundBlueX + noGroundBlue.Width * 5 + noGroundBlue.Width) ||
                (characterY >= ground && characterX >= noGroundBlueX + noGroundBlue.Width * 6 && characterX <= noGroundBlueX + noGroundBlue.Width * 6 + noGroundBlue.Width) ||
                (characterY >= ground && characterX >= noGroundBlueX + noGroundBlue.Width * 7 && characterX <= noGroundBlueX + noGroundBlue.Width * 7 + noGroundBlue.Width))
            {
                if (characterType == "mario")
                {
                    // set on ground to false
                    onGround = false;

                    // end the game
                    gameEnded = true;

                    // stop playing the game theme
                    MediaPlayer.Stop();

                    // play the fail sound effect only once
                    if (isFailSoundPlayed == false)
                    {
                        failSoundInstance.Play();

                        isFailSoundPlayed = true;
                    }
                }
                // if the fourth goomba is on the blue drop, set its on ground variable to false
                else if (characterType == "enemy4")
                {
                    enemy4OnGround = false;
                }
            }
            // id mario is on below the ground, set his location to on the ground
            else if (marioY >= ground)
            {
                marioY = ground;
                onGround = true;
            }

            // if mario is colising with the top of an object, enter the code
            if (objX <= marioX + marioSprite.Width / NumOfMarioImgs - fourOffSet &&
                objX >= marioX - objWidth + fourOffSet &&
                objY <= characterY + marioSprite.Height &&
                objY >= characterY + marioSprite.Height - eightOffSet)
            {
                // if the character type is mario, enter the code
                if (characterType == "mario")
                {
                    // set mario on ground to true
                    onGround = true;

                    // run through the code inside for every coin in the game
                    for (byte c = 0; c < coinX.Length; c++)
                    {
                        // if the object mario landed on top of was a coin, enter the if statement
                        if (objX == coinX[c] && objY == coinY[c])
                        {
                            // play the coin collection music
                            coinCollectInstance.Play();

                            // move the collected off the screen
                            coinY[c] = 0 - objHeight * 10;
                            coinBounds[c] = new Rectangle(coinX[c], coinY[c], coin.Width, coin.Height);

                            // increase the coin count by 100
                            coinCount = coinCount + scoreIncrease;

                            // set on ground to false
                            onGround = false;
                        }
                    }

                    // if the object being collided with is a red mushroom
                    if (objX == redMushroomX && objY == redMushroomY)
                    {
                        // the character is on the ground
                        onGround = false;

                        // if the first inventory slot is empty, enter the code
                        if (inventoryItems[0] == null)
                        {
                            // move the red mushrooom off the screen
                            redMushroomX = 0;
                            redMushroomY = 0 - redMushroom.Height;

                            // update the red mushrooms bounds
                            redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);

                            // save red mushroom to the first inventory
                            inventoryItems[0] = "redMushroom";
                            item1 = redMushroom;

                            // update the inventory 1 image bounds
                            item1Bounds = new Rectangle(brickBackground.Width - redMushroom.Width * 2, 0, redMushroom.Width, redMushroom.Height);
                        }
                        // if the the second inventory is empty, enter code
                        else if(inventoryItems[1] == null)
                        {
                            // move the red mushrooom off the screen
                            redMushroomX = 0;
                            redMushroomY = 0 - redMushroom.Height;

                            // update the red mushrooms bounds
                            redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);

                            // save red mushroom to the second inventory
                            inventoryItems[1] = "redMushroom";
                            item2 = redMushroom;

                            // update the inventory 2 image bounds
                            item2Bounds = new Rectangle(brickBackground.Width - redMushroom.Width, 0, redMushroom.Width, redMushroom.Height);
                        }
                    }

                    // if the object being collided with a green mushroom
                    if (objX == greenMushroomX && objY == greenMushroomY)
                    {
                        // set on ground to false
                        onGround = false;

                        // if the first inventory slot is empty, enter the code
                        if (inventoryItems[0] == null)
                        {
                            // move the green mushrooom off the screen
                            greenMushroomX = 0;
                            greenMushroomY = 0 - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height;

                            // update the green mushrooms bounds
                            greenMushroomBounds = new Rectangle(greenMushroomX, greenMushroomY, greenMushroom.Width, greenMushroom.Height);

                            // save green mushroom to the first inventory
                            inventoryItems[0] = "greenMushroom";
                            item1 = greenMushroom;

                            // update the inventory 1 image bounds
                            item1Bounds = new Rectangle(brickBackground.Width - greenMushroom.Width * 2, 0, greenMushroom.Width, greenMushroom.Height);
                        }
                        // if the the second inventory is empty, enter code
                        else if (inventoryItems[1] == null)
                        {
                            // move the green mushroom off the screen 
                            greenMushroomX = 0;
                            greenMushroomY = 0 - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height;

                            // update the green mushrooms bounds
                            greenMushroomBounds = new Rectangle(greenMushroomX, greenMushroomY, greenMushroom.Width, greenMushroom.Height);

                            // save green mushroom to the second inventory
                            inventoryItems[1] = "greenMushroom";
                            item2 = greenMushroom;

                            // update the inventory 2 image bounds
                            item2Bounds = new Rectangle(brickBackground.Width - greenMushroom.Width, 0, greenMushroom.Width, greenMushroom.Height);
                        }
                    }

                    // if the collided object is a yellow mushroom, enter if statement
                    if (objX == yellowMushroomX && objY == yellowMushroomY)
                    {
                        // set on ground to false
                        onGround = false;

                        // if the first inventory is empty, enter code
                        if (inventoryItems[0] == null)
                        {
                            // move the yellow mushroom off screen
                            yellowMushroomX = 0;
                            yellowMushroomY = 0 - yellowMushroom.Height;

                            // update the mushroom bounds
                            yellowMushroomBounds = new Rectangle(yellowMushroomX, yellowMushroomY, yellowMushroom.Width, yellowMushroom.Height);

                            // save the yellow mushroom to the first inventory 
                            inventoryItems[0] = "yellowMushroom";
                            item1 = yellowMushroom;

                            // update the item 1 bounds 
                            item1Bounds = new Rectangle(brickBackground.Width - yellowMushroom.Width * 2, 0, yellowMushroom.Width, yellowMushroom.Height);
                        }
                        // if the second inventory is empty, enter the is statement
                        else if (inventoryItems[1] == null)
                        {
                            // move the yellow mushroom off the screen
                            yellowMushroomX = 0;
                            yellowMushroomY = 0 - yellowMushroom.Height;

                            // update the yellow mushroom bounds
                            yellowMushroomBounds = new Rectangle(yellowMushroomX, yellowMushroomY, yellowMushroom.Width, yellowMushroom.Height);

                            // save the yellow mushroom to the second inventory
                            inventoryItems[1] = "yellowMushroom";
                            item2 = yellowMushroom;

                            // update the item 2 bounds
                            item2Bounds = new Rectangle(brickBackground.Width - yellowMushroom.Width, 0, yellowMushroom.Width, yellowMushroom.Height);
                        }
                    }

                    // if the colided object is a star, enter if statement
                    if (objX == starX && objY == starY)
                    {
                        // drop the main character to the ground
                        onGround = false;

                        // if the first inventory is empty, enter the is statement
                        if (inventoryItems[0] == null)
                        {
                            // move the star off the screen
                            starX = 0;
                            starY = 0 - star.Height - star.Height - star.Height - star.Height;

                            // update the star bounds
                            starBounds = new Rectangle(starX, starY, star.Width, star.Height);

                            // store the star in the first inventory slot
                            inventoryItems[0] = "star";
                            item1 = star;

                            // update the first item inventory
                            item1Bounds = new Rectangle(brickBackground.Width - star.Width * 2, 0, star.Width, star.Height);
                        }
                        // if the second inventory is empty, enter the is statement 
                        else if (inventoryItems[1] == null)
                        {
                            // move the star off the screen
                            starX = 0;
                            starY = 0 - star.Height - star.Height - star.Height - star.Height;

                            // update the star bounds
                            starBounds = new Rectangle(starX, starY, star.Width, star.Height);

                            // store the star in the second inventory slot
                            inventoryItems[1] = "star";
                            item2 = star;

                            // update the second inventory bounds
                            item2Bounds = new Rectangle(brickBackground.Width - star.Width, 0, star.Width, star.Height);
                        }
                    }
                    // if the collision is with the first goomba, move it off screen
                    if (objX == goombaX[0] && objY == goombaY[0])
                    {
                        goombaY[0] = 0 - objHeight - objHeight - objHeight;
                        onGround = false;

                        // play the cheer sound effect of mario when he kills a goomba
                        goombaKillInstance.Play();
                    }
                    // if the collision is whtih the second goomba, move it off screen
                    if (objX == goombaX[1] && objY == goombaY[1])
                    {
                        goombaY[1] = 0 - objHeight - objHeight - objHeight;
                        onGround = false;

                        // play the cheer sound effect of mario when he kills a goomba
                        goombaKillInstance.Play();
                    }
                    // if the collision is whtih the third goomba, move it off screen
                    if (objX == goombaX[2] && objY == goombaY[2])
                    {
                        goombaY[2] = 0 - objHeight - objHeight - objHeight;
                        onGround = false;

                        // play the cheer sound effect of mario when he kills a goomba
                        goombaKillInstance.Play();
                    }
                    // if the collision is whtih the fourth goomba, move it off screen
                    if (objX == goombaX[3] && objY == goombaY[3])
                    {
                        goombaY[3] = 0 - objHeight - objHeight - objHeight;
                        onGround = false;

                        // play the cheer sound effect of mario when he kills a goomba
                        goombaKillInstance.Play();
                    }

                }

                // create temporary variables to store the dimensions and x and y of the block the the character is currently colliding with
                objXTemp = objX;
                objYTemp = objY;
                objHeightTemp = objHeight;
                objWidthTemp = objWidth;
            }
            
            // if the character is off the block that he was previously standing on, proceed to fall
            if (objYTemp <= characterY + marioSprite.Height &&
                objYTemp >= characterY + marioSprite.Height - eightOffSet &&
                (objXTemp >= characterX + marioSprite.Width / NumOfMarioImgs - 1 ||
                objXTemp <= characterX - objWidth - 1))
            {
                if (characterType == "mario")
                {
                    onGround = false;
                }
            }

            // if the enemy is off an object it was previously standing on, enter code
            if (objX <= characterX + blockSize - fourOffSet &&
                objX >= characterX - objWidth + fourOffSet &&
                objY <= characterY + blockSize &&
                objY >= characterY + blockSize - eightOffSet)
            {
                // save the temporary dimensions in a variable that is used to compare if the player is off the platform later on
                objXTemp = objX;
                objYTemp = objY;
                objHeightTemp = objHeight;
                objWidthTemp = objWidth;
            }

            // if the character has falling off the block it wsa previously on, enter code
            else if (objYTemp <= characterY + blockSize &&
                    objYTemp >= characterY + blockSize - eightOffSet &&
                    (objXTemp >= characterX + blockSize - 1 ||
                    objXTemp <= characterX - objWidth + 1))
            {
                // if the character that is off the platform is enemy 1, fall off the platform
                if (characterType == "enemy1")
                {
                    enemy1OnGround = false;
                }
                // if the character that is off the platform is enemy 2, fall off the platform
                if (characterType == "enemy2")
                {
                    enemy2OnGround = false;
                }
                // if the character that is off the platform is enemy 3, fall off the platform
                if (characterType == "enemy3")
                {
                    enemy3OnGround = false;
                }
            }

            // if the character made contact with a block on the left of the object, enter the if statement 
            if (objX <= characterX + marioSprite.Width / NumOfMarioImgs &&
                objX >= characterX + marioSprite.Width / NumOfMarioImgs - moveSpeed &&
                objY >= characterY - objHeight &&
                objY <= characterY + marioSprite.Height - eightOffSet)
            {
                // if the character type that is colliding is mario, enter the if statement
                if (characterType == "mario")
                {
                    // for each coin run through the code insied the loop
                    for (byte c = 0; c < coinX.Length; c++)
                    {
                        // if the object that is collided with is a coin, enter the code
                        if (objX == coinX[c] && objY == coinY[c])
                        {
                            // play the coin colleciton sound
                            coinCollectInstance.Play();

                            // move the coin off the screen
                            coinY[c] = 0 - objHeight * 10;
                            coinBounds[c] = new Rectangle(coinX[c], coinY[c], coin.Width, coin.Height);

                            // increase the coin score by 100
                            coinCount = coinCount + scoreIncrease;
                        }
                    }
                    
                    // if the character colided with a goomba, enter code 
                    if ((objX == goombaX[0] || objX == goombaX[1] || objX == goombaX[2] || objX == goombaX[3]) &&
                        (objY == goombaY[0] || objY == goombaY[1] || objY == goombaY[2] || objY == goombaY[3]))
                    {
                        // if the character is not immune, enter the if statement
                        if (isCharacterImmune == false)
                        {
                            // end the game
                            gameEnded = true;

                            // stop playing backgound music and play the fail sound
                            MediaPlayer.Stop();
                            failSoundInstance.Play();
                        }
                        // if the player is immune, enter the code
                        else
                        {
                            // play the cheer sound effect of mario when he kills a goomba
                            goombaKillInstance.Play();

                            // turn off the characters immunity
                            isCharacterImmune = false;

                            // if the character collided with the first goomba, move that goomba off screen
                            if (objX == goombaX[0])
                            {
                                goombaY[0] = 0 - objHeight;
                            }
                            // if the character collided with the second goomba, move that goomba off screen
                            if (objX == goombaX[1])
                            {
                                goombaY[1] = 0 - objHeight;
                            }
                            // if the character collided with the third goomba, move that goomba off screen
                            if (objX == goombaX[2])
                            {
                                goombaY[2] = 0 - objHeight;
                            }
                            // if the character collided with the fourth goomba, move that goomba off screen
                            if (objX == goombaX[3])
                            {
                                goombaY[3] = 0 - objHeight;
                            }
                        }
                    }

                    // if the collision was with a red mushroom, enter the if statement
                    if (objX == redMushroomX && objY == redMushroomY)
                    {
                        // if the first inventory slot is empty, enter the if statement
                        if (inventoryItems[0] == null)
                        {
                            // move the red mushroom of the screen
                            redMushroomX = 0;
                            redMushroomY = 0 - redMushroom.Height;

                            // update the red mushroom bounds so when it is drawn it will be off the page
                            redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);

                            // store red mushroom in the first inventory
                            inventoryItems[0] = "redMushroom";
                            item1 = redMushroom;

                            // update the bounds of the first item slot 
                            item1Bounds = new Rectangle(brickBackground.Width - redMushroom.Width * 2, 0, redMushroom.Width, redMushroom.Height);
                        }
                        // if the second item slot is empty, enter the if statement 
                        else if (inventoryItems[1] == null)
                        {
                            // move the red mushroom off the screen
                            redMushroomX = 0;
                            redMushroomY = 0 - redMushroom.Height;

                            // update the red mushroom bounds so when it is drawn it will be off screen
                            redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);

                            // store the red mushroom in the second inventory slot
                            inventoryItems[1] = "redMushroom";
                            item2 = redMushroom;

                            // update the second inventory slot for usage in drawing the inventory
                            item2Bounds = new Rectangle(brickBackground.Width - redMushroom.Width, 0, redMushroom.Width, redMushroom.Height);
                        }
                    }
                    // if the object that was collided with was a green mushroom, enter the if statement
                    else if (objX == greenMushroomX && objY == greenMushroomY)
                    {
                        // if the first inventory slot is empty, enter if statement 
                        if (inventoryItems[0] == null)
                        {
                            // move the green mushroom off the screen
                            greenMushroomX = 0;
                            greenMushroomY = 0 - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height;

                            //  update the green mushroom bounds
                            greenMushroomBounds = new Rectangle(greenMushroomX, greenMushroomY, greenMushroom.Width, greenMushroom.Height);

                            // store the green mushroom in the first inventory slot
                            inventoryItems[0] = "greenMushroom";
                            item1 = greenMushroom;

                            // update the inventory one bounds so when drawn the program will know the new bounds
                            item1Bounds = new Rectangle(brickBackground.Width - greenMushroom.Width * 2, 0, greenMushroom.Width, greenMushroom.Height);
                        }
                        // if the second inventory is empty, enter the if statement 
                        else if (inventoryItems[1] == null)
                        {
                            // move the green mushroom off the screen
                            greenMushroomX = 0;
                            greenMushroomY = 0 - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height;

                            // update the green mushroom bounds so it is drawn off screen
                            greenMushroomBounds = new Rectangle(greenMushroomX, greenMushroomY, greenMushroom.Width, greenMushroom.Height);

                            // save the green mushroom in the second inventory slot
                            inventoryItems[1] = "greenMushroom";
                            item2 = greenMushroom;

                            // update the second inventory bounds so when drawn the program willl know the new bounds
                            item2Bounds = new Rectangle(brickBackground.Width - greenMushroom.Width, 0, greenMushroom.Width, greenMushroom.Height);
                        }
                    }
                    // if the object collised with was a yellow mushroom, enter the if statement 
                    else if (objX == yellowMushroomX && objY == yellowMushroomY)
                    {
                        // if the first inventory slot is empty, enter the if statement 
                        if (inventoryItems[0] == null)
                        {
                            // move the yellow mushroom off the screen
                            yellowMushroomX = 0;
                            yellowMushroomY = 0 - yellowMushroom.Height;

                            //update the yellow mushroom bounds so it will be drawn off screen
                            yellowMushroomBounds = new Rectangle(yellowMushroomX, yellowMushroomY, yellowMushroom.Width, yellowMushroom.Height);

                            // save the yellow mushroom in the first inventory slot
                            inventoryItems[0] = "yellowMushroom";
                            item1 = yellowMushroom;

                            // update the first inventory bounds so when drawn it will adjust for the new bounds
                            item1Bounds = new Rectangle(brickBackground.Width - yellowMushroom.Width * 2, 0, yellowMushroom.Width, yellowMushroom.Height);
                        }
                        // if the second inventory slot it empty, enter the if statement 
                        else if (inventoryItems[1] == null)
                        {
                            // move the yellow mushroom off the screen
                            yellowMushroomX = 0;
                            yellowMushroomY = 0 - yellowMushroom.Height;

                            // update the yellow mushroom bounds so it will be drawn off screen
                            yellowMushroomBounds = new Rectangle(yellowMushroomX, yellowMushroomY, yellowMushroom.Width, yellowMushroom.Height);

                            //save the yellow mushroom in the second inventory slot
                            inventoryItems[1] = "yellowMushroom";
                            item2 = yellowMushroom;

                            // update the second inventory bounds so when drawn it will adjust for the new bounds
                            item2Bounds = new Rectangle(brickBackground.Width - yellowMushroom.Width, 0, yellowMushroom.Width, yellowMushroom.Height);
                        }
                    }
                    // if the object being collided with is a star, enter the if statement 
                    else if (objX == starX && objY == starY)
                    {
                        // if the first inventory slot is empty, enter the if statement 
                        if (inventoryItems[0] == null)
                        {
                            // move the star off the screen
                            starX = 0;
                            starY = 0 - star.Height - star.Height - star.Height - star.Height;

                            // update the star bounds so it will appear off the screen when drawn
                            starBounds = new Rectangle(starX, starY, star.Width, star.Height);

                            // save the star in the first inventory slot
                            inventoryItems[0] = "star";
                            item1 = star;

                            // update the first inventory bounds so it will be drawn according to the new item stored in the first inventory slot
                            item1Bounds = new Rectangle(brickBackground.Width - star.Width * 2, 0, star.Width, star.Height);
                        }
                        // if the second inventory is empty, enter the if statement 
                        else if (inventoryItems[1] == null)
                        {
                            // move the star off the screen
                            starX = 0;
                            starY = 0 - star.Height - star.Height - star.Height - star.Height;

                            // update the star bounds so it will be redrawn off screen
                            starBounds = new Rectangle(starX, starY, star.Width, star.Height);

                            // save the star in the second inventory slot
                            inventoryItems[1] = "star";
                            item2 = star;

                            // update the second inventory bounds so when drawn it will adjust for the new bounds
                            item2Bounds = new Rectangle(brickBackground.Width - star.Width, 0, star.Width, star.Height);
                        }
                    }
                    // if the collision is not with an item, enter the else
                    else
                    {
                        // move the background images to the right 
                        brickBackgroundX = brickBackgroundX + moveSpeed;
                        brickBackgroundX2 = brickBackgroundX2 + moveSpeed;

                        //move the square bricks to the right
                        squareBricks1X = squareBricks1X + moveSpeed;
                        squareBricks2X = squareBricks2X + moveSpeed;
                        squareBricks3X = squareBricks3X + moveSpeed;
                        squareBricks4X = squareBricks4X + moveSpeed;
                        squareBricks5X = squareBricks5X + moveSpeed;
                        squareBricks6X = squareBricks6X + moveSpeed;
                        squareBricks7X = squareBricks7X + moveSpeed;
                        squareBricks8X = squareBricks8X + moveSpeed;
                        squareBricks9X = squareBricks9X + moveSpeed;

                        // move the items and collectables to the right 
                        redMushroomX = redMushroomX + moveSpeed;
                        greenMushroomX = greenMushroomX + moveSpeed;
                        yellowMushroomX = yellowMushroomX + moveSpeed;
                        starX = starX + moveSpeed;

                        // move the stone block platforms to the right 
                        stoneBricks1X = stoneBricks1X + moveSpeed;
                        stoneBricks2X = stoneBricks2X + moveSpeed;
                        stoneBricks3X = stoneBricks3X + moveSpeed;

                        // move the pipes to the right 
                        pipe1X = pipe1X + moveSpeed;
                        pipe2X = pipe2X + moveSpeed;
                        tallPipeX = tallPipeX + moveSpeed;

                        // move the cannon to the right
                        cannonX = cannonX + moveSpeed;

                        // move the brown blocks to the right
                        brownBlockX = brownBlockX + moveSpeed;
                        brownBlock2X = brownBlock2X + moveSpeed;
                        brownBlock3X = brownBlock3X + moveSpeed;

                        // move the non ground blue blocks that represent a drop to the right 
                        noGroundBlueX = noGroundBlueX + moveSpeed;

                        // move the goomba location to the right
                        for (int i = 0; i < goombaX.Length; i++)
                        {
                            goombaX[i] = goombaX[i] + moveSpeed;
                        }

                        // move the castle to the right 
                        castleX = castleX + moveSpeed;

                        // move the stone walls and barriers to the right
                        stoneWallX = stoneWallX + moveSpeed;
                        greyStairX = greyStairX + moveSpeed;
                        greyRectangleX = greyRectangleX + moveSpeed;

                        // move the coins to the right
                        for (byte c = 0; c < coinX.Length; c++)
                        {
                            coinX[c] = coinX[c] + moveSpeed;
                        }
                    }
                }

                // mif the character type is the first enemy, switch its direction
                if (characterType == "enemy1")
                {
                    enemyMoveDir = enemyMoveDir * -1;
                }
                // mif the character type is the second enemy, switch its direction
                if (characterType == "enemy2")
                {
                    enemy2MoveDir = enemy2MoveDir * -1;
                }

            }
            
            // if the character makes a collision on the bottom of a block, enter this if statement 
            if (objX <= characterX + marioSprite.Width / NumOfMarioImgs &&
               objX >= characterX - objWidth &&
               objY >= characterY - objHeight &&
               objY <= characterY - objHeight + eightOffSet)

            {
                // if the character type is mario, enter the if statement
                if (characterType == "mario")
                {
                    // for each coin that exists in the game, run the code inside the loop
                    for (byte c = 0; c < coinX.Length; c++)
                    {
                        // if the object that is being collided with is a coin, enter the if statement 
                        if (objX == coinX[c] && objY == coinY[c])
                        {
                            // play the coin collecting sound effect
                            coinCollectInstance.Play();

                            // move the collected coin off the screen and update its bounds
                            coinY[c] = 0 - objHeight * 10;
                            coinBounds[c] = new Rectangle(coinX[c], coinY[c], coin.Width, coin.Height);

                            // tell the game that the object collided with is a coin
                            isCoinAbove = true;

                            // increase the coin count by 100
                            coinCount = coinCount + scoreIncrease;
                        }
                    }

                    // if the object collided with is a coin, do not stop the jump
                    if (isCoinAbove == false)    
                    {
                        isJumpOn = false;
                    }

                    // if the collision is with the first item box and it is active, enter the if statement
                    if (objX == squareBricks1X + squareBrick.Width && objY == platform1Height && areItemBoxesActive[0] == true)
                    {
                        // make the used item box no longer active
                        areItemBoxesActive[0] = false;

                        // move the red mushroom above the block and update the red mushroom bounds
                        redMushroomX = squareBricks1X + squareBrick.Width;
                        redMushroomY = platform1Height - redMushroom.Height;
                        redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);
                    }
                    // if the collision is with the second item box and it is active, enter the if statement
                    else if (objX == squareBricks2X + squareBrick.Width * 2 && objY == platform2Height && areItemBoxesActive[1] == true)
                    {
                        // make the used item box no longer active
                        areItemBoxesActive[1] = false;

                        // move the star above the block and update the star bounds
                        starX = squareBricks2X + squareBrick.Width * 2;
                        starY = platform2Height - star.Height;
                        starBounds = new Rectangle(starX, starY, star.Width, star.Height);
                    }
                    // if the collision is with the third item box and it is active, enter the if statement
                    else if (objX == squareBricks6X + squareBrick.Width && objY == platform1Height && areItemBoxesActive[2] == true)
                    {
                        // make the used item box no longer active
                        areItemBoxesActive[2] = false;

                        // move the yellow mushroom above the block and update the yellow mushroom bounds
                        yellowMushroomX = squareBricks6X + squareBrick.Width;
                        yellowMushroomY = platform1Height - greenMushroom.Height;
                        yellowMushroomBounds = new Rectangle(yellowMushroomX, yellowMushroomY, yellowMushroom.Width, yellowMushroom.Height);
                    }
                    // if the collision is with the fourth item box and it is active, enter the if statement
                    else if (objX == squareBricks8X && objY == platform2Height && areItemBoxesActive[3] == true)
                    {
                        // make the used item box no longer active
                        areItemBoxesActive[3] = false;

                        // move the red mushroom above the block and update the red mushroom bounds
                        redMushroomX = squareBricks8X;
                        redMushroomY = platform2Height - redMushroom.Height;
                        redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);
                    }
                    // if the collision is with the fifth item box and it is active, enter the if statement
                    else if (objX == squareBricks8X + squareBrick.Width * 3 && objY == platform2Height && areItemBoxesActive[4] == true)
                    {
                        // make the used item box no longer active
                        areItemBoxesActive[4] = false;

                        // move the green mushroom above the block and update the green mushroom bounds
                        greenMushroomX = squareBricks8X + squareBrick.Width * 3;
                        greenMushroomY = platform2Height - greenMushroom.Height;
                        greenMushroomBounds = new Rectangle(greenMushroomX, greenMushroomY, greenMushroom.Width, greenMushroom.Height);
                    }
                    // if the collision is with the sixth item box and it is active, enter the if statement
                    else if (objX == squareBricks9X && objY == platform1Height && areItemBoxesActive[5] == true)
                    {
                        // make the used item box no longer active
                        areItemBoxesActive[5] = false;

                        // move the coin above the block and update the coin bounds 
                        coinX[11] = squareBricks9X;
                        coinY[11] = platform1Height - coin.Height;
                        coinBounds[11] = new Rectangle(coinX[11], coinY[11], coin.Width, coin.Height);
                    }
                    // if the collision is with the seventh item box and it is active, enter the if statement
                    else if (objX == squareBricks9X + squareBrick.Width * 10 && objY == platform1Height && areItemBoxesActive[6] == true)
                    {
                        // make the used item box no longer active 
                        areItemBoxesActive[6] = false;

                        // move the red mushroom above the block and update the red mushroom bounds 
                        redMushroomX = squareBricks9X + squareBrick.Width * 10;
                        redMushroomY = platform1Height - star.Height;
                        redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);
                    } 
                }
            }

            // if the character made contact with a block on the right of the block, enter the if statement 
            if (objX >= characterX - objWidth &&
                objX <= characterX - objWidth + moveSpeed &&
                objY >= characterY - objHeight &&
                objY <= characterY + marioSprite.Height - eightOffSet)
            {
                // if the character type is mario, enter the if statement 
                if (characterType == "mario")
                {
                    // for each coin in the game, run the loop
                    for (byte c = 0; c < coinX.Length; c++)
                    {
                        // if the object collided with is a coin, enter the if statement
                        if (objX == coinX[c] && objY == coinY[c])
                        {
                            // play the coin collecting sounf effect
                            coinCollectInstance.Play();

                            // move the coin off the screen and update its bounds so when it is drawn it will be off screen
                            coinY[c] = 0 - objHeight * 10;
                            coinBounds[c] = new Rectangle(coinX[c], coinY[c], coin.Width, coin.Height);

                            // add 100 to the coin count
                            coinCount = coinCount + scoreIncrease;
                        }
                    }
                    
                    // if the collision is with a goomba, enter the if statement 
                    if ((objX == goombaX[0] || objX == goombaX[1] || objX == goombaX[2] || objX == goombaX[3]) &&
                        (objY == goombaY[0] || objY == goombaY[1] || objY == goombaY[2] || objY == goombaY[3]))
                    {
                        // if the character does not have immunity on, enter the if statement 
                        if (isCharacterImmune == false)
                        {
                            // end the game 
                            gameEnded = true;

                            // stop playing backgound music and play the fail sound
                            MediaPlayer.Stop();
                            failSoundInstance.Play();
                        }
                        // if the character has immunity on, enter the if statement
                        else
                        {
                            // play the cheer sound effect of mario when he kills a goomba
                            goombaKillInstance.Play();

                            // turn the characters immunity off
                            isCharacterImmune = false;

                            // if the character made contact with the first goomba, move this goomba off the screen
                            if (objX == goombaX[0])
                            {
                                goombaY[0] = 0 - objHeight;
                            }
                            // if the character made contact with the second goomba, move this goomba off the screen
                            if (objX == goombaX[1])
                            {
                                goombaY[1] = 0 - objHeight;
                            }
                            // if the character made contact with the third goomba, move this goomba off the screen
                            if (objX == goombaX[2])
                            {
                                goombaY[2] = 0 - objHeight;
                            }
                            // if the character made contact with the fourth goomba, move this goomba off the screen
                            if (objX == goombaX[3])
                            {
                                goombaY[3] = 0 - objHeight;
                            }
                        }
                    }

                    // if the object collided with was a red mushroom, enter the code
                    if (objX == redMushroomX && objY == redMushroomY)
                    {
                        // if the first inventory slot is empty, enter the if statement
                        if (inventoryItems[0] == null)
                        {
                            // move the red mushroom off the screen
                            redMushroomX = 0;
                            redMushroomY = 0 - redMushroom.Height;

                            // update the red mushroom bounds so when redrawn it will appear off screen
                            redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);

                            // save the red mushroom in the first inventory slot
                            inventoryItems[0] = "redMushroom";
                            item1 = redMushroom;

                            // update the first item inventory bounds according to the new object it is used to draw
                            item1Bounds = new Rectangle(brickBackground.Width - redMushroom.Width * 2, 0, redMushroom.Width, redMushroom.Height);
                        }
                        // if the second inventory slot is empty, enter the if statement
                        else if (inventoryItems[1] == null)
                        {
                            // move the red mushroom off the screen
                            redMushroomX = 0;
                            redMushroomY = 0 - redMushroom.Height;

                            // update the red mushroom bounds so when redrawn it will appear off screen
                            redMushroomBounds = new Rectangle(redMushroomX, redMushroomY, redMushroom.Width, redMushroom.Height);

                            // store the red mushroom in the second inventory slot
                            inventoryItems[1] = "redMushroom";
                            item2 = redMushroom;

                            // update the second item inventory bounds according to the new object it is used to draw
                            item2Bounds = new Rectangle(brickBackground.Width - redMushroom.Width, 0, redMushroom.Width, redMushroom.Height);
                        }
                    }
                    // if the object collided with was a green mushroom, enter the code
                    else if (objX == greenMushroomX && objY == greenMushroomY)
                    {
                        // if the first inventory slot is empty, enter the if statement
                        if (inventoryItems[0] == null)
                        {
                            // move the green mushroom off the screen
                            greenMushroomX = 0;
                            greenMushroomY = 0 - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height;

                            // update the green mushroom bounds so it will be redrawn off the screen
                            greenMushroomBounds = new Rectangle(greenMushroomX, greenMushroomY, greenMushroom.Width, greenMushroom.Height);

                            // add the green mushroom to the first inventory slot
                            inventoryItems[0] = "greenMushroom";
                            item1 = greenMushroom;

                            // update the first item inventory bounds according to the new object it is used to draw
                            item1Bounds = new Rectangle(brickBackground.Width - greenMushroom.Width * 2, 0, greenMushroom.Width, greenMushroom.Height);
                        }
                        // if the second inventory slot is empty, enter the if statement
                        else if (inventoryItems[1] == null)
                        {
                            // move the green mushroom off the screen
                            greenMushroomX = 0;
                            greenMushroomY = 0 - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height - greenMushroom.Height;

                            // update the green mushroom bounds so it will be redrawn off the screen
                            greenMushroomBounds = new Rectangle(greenMushroomX, greenMushroomY, greenMushroom.Width, greenMushroom.Height);

                            // add the green mushroom to the second inventory slot
                            inventoryItems[1] = "greenMushroom";
                            item2 = greenMushroom;

                            // update the second item inventory bounds according to the new object it is used to draw
                            item2Bounds = new Rectangle(brickBackground.Width - greenMushroom.Width, 0, greenMushroom.Width, greenMushroom.Height);
                        }
                    }
                    // if the object collided with was a yellow mushroom, enter the code
                    else if (objX == yellowMushroomX && objY == yellowMushroomY)
                    {
                        // if the first inventory slot is empty, enter the if statement
                        if (inventoryItems[0] == null)
                        {
                            // move the yellow mushroom off the screen
                            yellowMushroomX = 0;
                            yellowMushroomY = 0 - yellowMushroom.Height;

                            // update the yellow mushroom bounds so it will be redrawn off the screen
                            yellowMushroomBounds = new Rectangle(yellowMushroomX, yellowMushroomY, yellowMushroom.Width, yellowMushroom.Height);

                            // add the yellow mushroon to the first inventory slot
                            inventoryItems[0] = "yellowMushroom";
                            item1 = yellowMushroom;

                            // update the first item inventory bounds according to the new object it is used to draw
                            item1Bounds = new Rectangle(brickBackground.Width - yellowMushroom.Width * 2, 0, yellowMushroom.Width, yellowMushroom.Height);
                        }
                        // if the second inventory slot is empty, enter the if statement
                        else if (inventoryItems[1] == null)
                        {
                            // move the yellow mushroom off the screen
                            yellowMushroomX = 0;
                            yellowMushroomY = 0 - yellowMushroom.Height;

                            // update the yellow mushroom bounds so it will be redrawn off the screen
                            yellowMushroomBounds = new Rectangle(yellowMushroomX, yellowMushroomY, yellowMushroom.Width, yellowMushroom.Height);

                            // add the yellow mushroom to the second inventory slot
                            inventoryItems[1] = "yellowMushroom";
                            item2 = yellowMushroom;

                            // update the second item inventory bounds according to the new object it is used to draw
                            item2Bounds = new Rectangle(brickBackground.Width - yellowMushroom.Width, 0, yellowMushroom.Width, yellowMushroom.Height);
                        }
                    }
                    // if the object collided with was a star, enter the code
                    else if (objX == starX && objY == starY)
                    {
                        // if the first inventory slot is empty, enter the if statement
                        if (inventoryItems[0] == null)
                        {
                            // move the star off the screen
                            starX = 0;
                            starY = 0 - star.Height - star.Height - star.Height - star.Height;

                            // update the star bounds so it will be redrawn off the screen
                            starBounds = new Rectangle(starX, starY, star.Width, star.Height);

                            // add the star to the first slot of the inventory
                            inventoryItems[0] = "star";
                            item1 = star;

                            // update the first item inventory bounds according to the new object it is used to draw
                            item1Bounds = new Rectangle(brickBackground.Width - star.Width * 2, 0, star.Width, star.Height);
                        }
                        // if the second inventory slot is empty, enter the if statement
                        else if (inventoryItems[1] == null)
                        {
                            // move the star off the screen
                            starX = 0;
                            starY = 0 - star.Height - star.Height - star.Height - star.Height;

                            // update the star bounds so it will be redrawn off the screen
                            starBounds = new Rectangle(starX, starY, star.Width, star.Height);

                            // add the star to the second slot of the inventory
                            inventoryItems[1] = "star";
                            item2 = star;

                            // update the second item inventory bounds according to the new object it is used to draw
                            item2Bounds = new Rectangle(brickBackground.Width - star.Width, 0, star.Width, star.Height);
                        }
                    }
                    else
                    {
                        // move the backgorund to the left
                        brickBackgroundX = brickBackgroundX - moveSpeed;
                        brickBackgroundX2 = brickBackgroundX2 - moveSpeed;

                        // move the square brick blocks to the left
                        squareBricks1X = squareBricks1X - moveSpeed;
                        squareBricks2X = squareBricks2X - moveSpeed;
                        squareBricks3X = squareBricks3X - moveSpeed;
                        squareBricks4X = squareBricks4X - moveSpeed;
                        squareBricks5X = squareBricks5X - moveSpeed;
                        squareBricks6X = squareBricks6X - moveSpeed;
                        squareBricks7X = squareBricks7X - moveSpeed;
                        squareBricks8X = squareBricks8X - moveSpeed;
                        squareBricks9X = squareBricks9X - moveSpeed;

                        // move the items and collectables to the left
                        redMushroomX = redMushroomX - moveSpeed;
                        greenMushroomX = greenMushroomX - moveSpeed;
                        yellowMushroomX = yellowMushroomX - moveSpeed;
                        starX = starX - moveSpeed;

                        // move the stone bricks of platforms to the left 
                        stoneBricks1X = stoneBricks1X - moveSpeed;
                        stoneBricks2X = stoneBricks2X - moveSpeed;
                        stoneBricks3X = stoneBricks3X - moveSpeed;

                        // move the pipes to the left
                        pipe1X = pipe1X - moveSpeed;
                        pipe2X = pipe2X - moveSpeed;
                        tallPipeX = tallPipeX - moveSpeed;

                        // move the reference point of the cannons to the left
                        cannonX = cannonX - moveSpeed;

                        // move the brown blocks on the left
                        brownBlockX = brownBlockX - moveSpeed;
                        brownBlock2X = brownBlock2X - moveSpeed;
                        brownBlock3X = brownBlock3X - moveSpeed;

                        // move the blue blocks that represent a drop to the left
                        noGroundBlueX = noGroundBlueX - moveSpeed;

                        // move the goomba location to the left
                        for (int i = 0; i < goombaX.Length; i++)
                        {
                            goombaX[i] = goombaX[i] - moveSpeed;
                        }

                        // move the castle to the left
                        castleX = castleX - moveSpeed;

                        // move the stone structures to the left
                        stoneWallX = stoneWallX - moveSpeed;
                        greyStairX = greyStairX - moveSpeed;
                        greyRectangleX = greyRectangleX - moveSpeed;

                        // move the coins to the left
                        for (byte c = 0; c < coinX.Length; c++)
                        {
                            coinX[c] = coinX[c] - moveSpeed;
                        }
                    }
                }

                // if the character type is the first enemy, switch its direction
                if (characterType == "enemy1")
                {
                    enemyMoveDir = enemyMoveDir * -1;
                }
                // if the character type is the second enemy, switch its direction
                if (characterType == "enemy2")
                {
                    enemy2MoveDir = enemy2MoveDir * -1;
                }
                // if the character type is the fourth enemy, switch its direction
                if (characterType == "enemy4")
                {
                    enemy4MoveDir = enemy4MoveDir * -1;
                }
            }

        }

        // Pre: N/A
        // Post: N/A
        // Description: Change a stat based on which item is used from marios inventory
        private void ItemUsage()
        {
            // if the first inventory item is clicked and it is not empty, enter the if statement
            if (currentKB.IsKeyDown(Keys.Q) && inventoryItems[0] != null)
            {
                // if the item is red mushroom, add 50 to the timer
                if (inventoryItems[0] == "redMushroom")
                {
                    timeLeft = timeLeft + timeIncrease;
                }
                // if the item is a green mushroom, double the jump height for a limited time
                if (inventoryItems[0] == "greenMushroom")
                {
                    jumpHeight = jumpHeight + jumpHeight;

                    highJumpTimer = highJumpTime;
                }
                // if the item is a yellow mushroom for a limited amount of time
                if (inventoryItems[0] == "yellowMushroom")
                {
                    jumpSpeed = jumpSpeedSlow;

                    lowGravityTimer = lowGravityTime; 
                }
                // if the item is a star, set character immunity to true
                if (inventoryItems[0] == "star")
                {
                    isCharacterImmune = true;
                }

                // delete whatever is on the first inventory
                inventoryItems[0] = null;
                item1 = null;
            }
            // if the second inventory item is clicked and it is not empty, enter the if statement
            if (currentKB.IsKeyDown(Keys.E) && inventoryItems[1] != null)
            {
                // if the item is red mushroom, add 50 to the timer
                if (inventoryItems[1] == "redMushroom")
                {
                    timeLeft = timeLeft + timeIncrease;
                }
                // if the item is a green mushroom, double the jump height for a limited time
                if (inventoryItems[1] == "greenMushroom")
                {
                    jumpHeight = jumpHeight + jumpHeight;

                    highJumpTimer = highJumpTime;
                }
                // if the item is a yellow mushroom for a limited amount of time
                if (inventoryItems[1] == "yellowMushroom")
                {
                    jumpSpeed = jumpSpeedSlow;

                    lowGravityTimer = lowGravityTime; 
                }
                // if the item is a star, set character immunity to true
                if (inventoryItems[1] == "star")
                {
                    isCharacterImmune = true;
                }

                // delete whatever is on the second inventory
                inventoryItems[1] = null;
                item2 = null;
            }
        }

        // Pre: the location where the data will be saved
        // Post: N/A
        // Description: save the game at its current state to an external file
        private void SaveGame(string saveFile)
        {
            // state the file name where the data will be saved 
            outFile = File.CreateText(saveFile);

            // save the backgrounds' x loctions to the external file
            outFile.WriteLine(brickBackgroundX);
            outFile.WriteLine(brickBackgroundX2);

            // save the square block x loctions to the external file
            outFile.WriteLine(squareBricks1X);
            outFile.WriteLine(squareBricks2X);
            outFile.WriteLine(squareBricks3X);
            outFile.WriteLine(squareBricks4X);
            outFile.WriteLine(squareBricks5X);
            outFile.WriteLine(squareBricks6X);
            outFile.WriteLine(squareBricks7X);
            outFile.WriteLine(squareBricks8X);
            outFile.WriteLine(squareBricks9X);

            // save the collectables and items loctions to the external file
            outFile.WriteLine(redMushroomX);
            outFile.WriteLine(greenMushroomX);
            outFile.WriteLine(yellowMushroomX);
            outFile.WriteLine(starX);
            outFile.WriteLine(redMushroomY);
            outFile.WriteLine(greenMushroomY);
            outFile.WriteLine(yellowMushroomY);
            outFile.WriteLine(starY);

            // save the stone block x loctions to the external file
            outFile.WriteLine(stoneBricks1X);
            outFile.WriteLine(stoneBricks2X);
            outFile.WriteLine(stoneBricks3X);

            // save the pipes x loctions to the external file
            outFile.WriteLine(pipe1X);
            outFile.WriteLine(pipe2X);
            outFile.WriteLine(tallPipeX);

            // save the cannons x loctions to the external file
            outFile.WriteLine(cannonX);

            // save the brown block x loctions to the external file
            outFile.WriteLine(brownBlockX);
            outFile.WriteLine(brownBlock2X);
            outFile.WriteLine(brownBlock3X);

            // save the x location of the drop blocks reference point
            outFile.WriteLine(noGroundBlueX);

            // save all the x values of the goombas 
            for (int i = 0; i < goombaX.Length; i++)
            {
                outFile.WriteLine(goombaX[i]);
            }
            // save all the y values of the goombas 
            for (int i = 0; i < goombaY.Length; i++)
            {
                outFile.WriteLine(goombaY[i]);
            }

            // save the castle x loction to the external file
            outFile.WriteLine(castleX);

            // save the stone wall x loction to the external file
            outFile.WriteLine(stoneWallX);

            // save the stone staircase x loction referece point to the external file
            outFile.WriteLine(greyStairX);

            // save the stone rectangle x loction to the external file
            outFile.WriteLine(greyRectangleX);

            //// save all the coins' x and y loction to the external file
            for (byte c = 0; c < coinX.Length; c++)
            {
                outFile.WriteLine(coinX[c]);
                outFile.WriteLine(coinY[c]);
            }

            // save the temporay platform block x loction to the external file
            outFile.WriteLine(objXTemp);

            // save the time left to the external file
            outFile.WriteLine(timeLeft);

            // save the coin count to the external file
            outFile.WriteLine(coinCount);

            // save marios y loction to the external file
            outFile.WriteLine(marioY);

            // save which item boxes are active to the external file
            for (byte i = 0; i < areItemBoxesActive.Length; i++)
            {
                outFile.WriteLine(areItemBoxesActive[i]);
            }

            // save the inventory to the external file
            outFile.WriteLine(inventoryItems[0]);
            outFile.WriteLine(inventoryItems[1]);

            // if the item 1 is a red mushroom, save this to an external file
            if (item1 == redMushroom)
            {
                outFile.WriteLine("redMushroom");
            }
            // if the item 1 is a green mushroom, save this to an external file
            else if (item1 == greenMushroom)
            {
                outFile.WriteLine("greenMushroom");
            }
            // if the item 1 is a yellow mushroom, save this to an external file
            else if (item1 == yellowMushroom)
            {
                outFile.WriteLine("yellowMushroom");
            }
            // if the item 1 is a star, save this to an external file
            else if (item1 == star)
            {
                outFile.WriteLine("star");
            }

            // if the item 2 is a red mushroom, save this to an external file
            if (item2 == redMushroom)
            {
                outFile.WriteLine("redMushroom");
            }
            // if the item 2 is a green mushroom, save this to an external file
            else if (item2 == greenMushroom)
            {
                outFile.WriteLine("greenMushroom");
            }
            // if the item 2 is a yellow mushroom, save this to an external file
            else if (item2 == yellowMushroom)
            {
                outFile.WriteLine("yellowMushroom");
            }
            // if the item 2 is a star, save this to an external file
            else if (item2 == star)
            {
                outFile.WriteLine("star");
            }

            // close the file that was written to
            outFile.Close();
        }

        // Pre: the location where the data will be retrieved from
        // Post: N/A
        // Description: load the game at a previously saved state from an external file
        private void LoadGame(string saveFile)
        {
            // variable to hold which item is loaded into the game
            string itemName;

            // store the name of the file from which to load the data from
            inFile = File.OpenText(saveFile);

            //load the backgrounds x location
            brickBackgroundX = Convert.ToInt32(inFile.ReadLine());
            brickBackgroundX2 = Convert.ToInt32(inFile.ReadLine());

            //load the square bricks x location
            squareBricks1X = Convert.ToInt32(inFile.ReadLine());
            squareBricks2X = Convert.ToInt32(inFile.ReadLine());
            squareBricks3X = Convert.ToInt32(inFile.ReadLine());
            squareBricks4X = Convert.ToInt32(inFile.ReadLine());
            squareBricks5X = Convert.ToInt32(inFile.ReadLine());
            squareBricks6X = Convert.ToInt32(inFile.ReadLine());
            squareBricks7X = Convert.ToInt32(inFile.ReadLine());
            squareBricks8X = Convert.ToInt32(inFile.ReadLine());
            squareBricks9X = Convert.ToInt32(inFile.ReadLine());

            //load the items and collectables x and y location
            redMushroomX = Convert.ToInt32(inFile.ReadLine());
            greenMushroomX = Convert.ToInt32(inFile.ReadLine());
            yellowMushroomX = Convert.ToInt32(inFile.ReadLine());
            starX = Convert.ToInt32(inFile.ReadLine());
            redMushroomY = Convert.ToInt32(inFile.ReadLine());
            greenMushroomY = Convert.ToInt32(inFile.ReadLine());
            yellowMushroomY = Convert.ToInt32(inFile.ReadLine());
            starY = Convert.ToInt32(inFile.ReadLine());

            //load the stone brick x location into the game
            stoneBricks1X = Convert.ToInt32(inFile.ReadLine());
            stoneBricks2X = Convert.ToInt32(inFile.ReadLine());
            stoneBricks3X = Convert.ToInt32(inFile.ReadLine());

            //load all the pipes x location into the game
            pipe1X = Convert.ToInt32(inFile.ReadLine());
            pipe2X = Convert.ToInt32(inFile.ReadLine());
            tallPipeX = Convert.ToInt32(inFile.ReadLine());

            //load the cannon x location into the game
            cannonX = Convert.ToInt32(inFile.ReadLine());

            //load the brown blocks x location into the game
            brownBlockX = Convert.ToInt32(inFile.ReadLine());
            brownBlock2X = Convert.ToInt32(inFile.ReadLine());
            brownBlock3X = Convert.ToInt32(inFile.ReadLine());

            //load the x location of the drop blocks reference location
            noGroundBlueX = Convert.ToInt32(inFile.ReadLine());

            //load all the goombas x location into the game
            for (int i = 0; i < goombaX.Length; i++)
            {
                goombaX[i] = Convert.ToInt32(inFile.ReadLine());
            }

            //load all the goombas y location into the game
            for (int i = 0; i < goombaY.Length; i++)
            {
                goombaY[i] = Convert.ToInt32(inFile.ReadLine());
            }

            //outValue = castleX;
            castleX = Convert.ToInt32(inFile.ReadLine());

            //outValue = stoneWallX;
            stoneWallX = Convert.ToInt32(inFile.ReadLine());

            //outValue = greyStairX;
            greyStairX = Convert.ToInt32(inFile.ReadLine());

            //outValue = greyRectangleX;
            greyRectangleX = Convert.ToInt32(inFile.ReadLine());

            //load the coins x and y location into the game
            for (byte c = 0; c < coinX.Length; c++)
            {
                coinX[c] = Convert.ToInt32(inFile.ReadLine());
                coinY[c] = Convert.ToInt32(inFile.ReadLine());
            }

            // load the temporary x location of the block the character was on into the game
            objXTemp = Convert.ToInt32(inFile.ReadLine());

            // load the time left in the game when it was saved
            timeLeft = Convert.ToInt32(inFile.ReadLine());

            // load the number of coins the user had from the previous saved game
            coinCount = Convert.ToInt32(inFile.ReadLine());

            // load marios y value from the previous saved game
            marioY = Convert.ToInt32(inFile.ReadLine());

            // load which item boxes are active from the previous 
            for (byte i = 0; i < areItemBoxesActive.Length; i++)
            {
                areItemBoxesActive[i] = Convert.ToBoolean(inFile.ReadLine());
            }

            // load the itmes stored in the inventories 
            inventoryItems[0] = inFile.ReadLine();
            inventoryItems[1] = inFile.ReadLine();

            // if the first inventory slot is empty, set it to null
            if (inventoryItems[0] == "")
            {
                inventoryItems[0] = null;
            }
            // if the second inventory slot is empty, set it to null
            if (inventoryItems[1] == "")
            {
                inventoryItems[1] = null;
            }

            // read the item type that is stored in the first item slot
            itemName = inFile.ReadLine();

            // if the item type is a red mushroom, store that in the item 1 slot
            if (itemName == "redMushroom")
            {
                item1 = redMushroom;
            }
            // if the item type is a green mushroom, store that in the item 1 slot
            else if (itemName == "greenMushroom")
            {
                item1 = greenMushroom;
            }
            // if the item type is a yellow mushroom, store that in the item 1 slot
            else if (itemName == "yellowMushroom")
            {
                item1 = yellowMushroom;
            }
            // if the item type is a star mushroom, store that in the item 1 slot
            else if (itemName == "star")
            {
                item1 = star;
            }

            // read the item type that is stored in the second item slot
            itemName = inFile.ReadLine();

            // if the item type is a red mushroom, store that in the item 2 slot
            if (itemName == "redMushroom")
            {
                item2 = redMushroom;
            }
            // if the item type is a green mushroom, store that in the item 2 slot
            else if (itemName == "greenMushroom")
            {
                item2 = greenMushroom;
            }
            // if the item type is a yellow mushroom, store that in the item 2 slot
            else if (itemName == "yellowMushroom")
            {
                item2 = yellowMushroom;
            }
            // if the item type is a star mushroom, store that in the item 2 slot
            else if (itemName == "star")
            {
                item2 = star;
            }   

            // close the file that is being read from
            inFile.Close();

            // call subprogram to update all the bounds of the objects in the game
            UpdateLocations();
        }

        // Pre: the current score, the name of the file that the high scores are stored in
        // Post: N/A
        // Description: Adjust the score board 
        private void AdjustScoreBoard(int currentScore, string highScoreFile)
        {
            // open the file to read into
            inFile = File.OpenText(highScoreFile);

            // read in the entire scoreboard to the game
            for (int x = 0; x < scoreBoard.Length; x++)
            {
                scoreBoard[x] = Convert.ToInt32(inFile.ReadLine());
            }

            // close the file that is being read
            inFile.Close();

            // go through every score on the scoreboard from bottom to top
            for (int x = scoreBoard.Length - 2; x >= 0; x--)
            {
                // if the players score is greater than the current score beng checked, replace it and move the current score down
                if (currentScore > scoreBoard[x])
                {
                    scoreBoard[x + 1] = scoreBoard[x];
                    scoreBoard[x] = currentScore;
                }
            }

            // define the file that the scores are being read from
            outFile = File.CreateText(highScoreFile);

            // read in all the scores from the list on the external file and save it to the scoreBoard array
            for (int x = 0; x < scoreBoard.Length; x++)
            {
                outFile.WriteLine(scoreBoard[x]);
            }

            // close the file that is being read from
            outFile.Close();
        }
    }
}
