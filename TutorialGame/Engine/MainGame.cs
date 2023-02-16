// Author: Chris Knowles
// Date: Jan 2023
// Copyright: Copperhead Labs, (c)2023
// File: MainGame.cs
// Version: 1.0.0
// Notes: 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.InteropServices;
using TutorialGame.States;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace TutorialGame.Engine
{
    public partial class MainGame : Game
    {
        public int MainFormFrameWidth { get; protected set; }
        public int MainFormFrameHeight { get; protected set; }
        public int MainFormTitleBarHeight { get; protected set; }
        public string GameName { get; protected set; }
        public ConfigData ConfigData { get; protected set; }
        public GraphicsDeviceManager Graphics { get; protected set; }
        public SpriteBatch SpriteBatch { get; protected set; }
        public FSM.FSM Fsm { get; protected set; }

        public MainGame()
        {
            MainFormFrameWidth = 0;
            MainFormFrameHeight = 0;
            MainFormTitleBarHeight = 0;
            GameName = GameConsts.GAME_NAME;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = GameConsts.CONTENT_ROOT_DIR;
            IsMouseVisible = true;

            // This ensures GraphicsDevice instance has been assigned before trying to change graphics and/or
            // window settings (otherwise it may crash)
            if (GraphicsDevice == null) Graphics.ApplyChanges();

            // Read configuration data into a new ConfigData instance
            ConfigData = new ConfigData(GraphicsDevice);

            // Change to full screen graphics and set resolution
            switch(ConfigData.ScreenMode)
            {
                case GameConsts.ScreenMode.BorderedWindow:
                {
                    Graphics.IsFullScreen = false;
                    Window.IsBorderless = false;
                    break;
                }

                case GameConsts.ScreenMode.BorderlessWindow:
                {
                    Graphics.IsFullScreen = false;
                    Window.IsBorderless = true;
                    break;
                }

                case GameConsts.ScreenMode.FullScreen:
                {
                    Graphics.IsFullScreen = true;
                    Window.IsBorderless = true;
                    break;
                }
            }

            // Set the screen resolution to values from configuration data
            Graphics.PreferredBackBufferWidth = (int)ConfigData.GameWindowScreenSize.X;
            Graphics.PreferredBackBufferHeight = (int)ConfigData.GameWindowScreenSize.Y;

            // Change frame rate using either vertical refresh of the monitor, fixed to 60fps or as fast as
            // it will update
            Graphics.SynchronizeWithVerticalRetrace = ConfigData.VSyncActive;

            if (ConfigData.MaxRefreshRate > 0)
            {
                this.TargetElapsedTime = new TimeSpan((long)((1.0 / ConfigData.MaxRefreshRate) * 10000000));
                IsFixedTimeStep = true;
            }
            else
            {
                IsFixedTimeStep = false;
            }

            Graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Window.Title = GameName;

            Fsm = new FSM.FSM(GameConsts.FSM_MAIN_NAME);
            
            Fsm.AddState<SplashscreenState>(GameConsts.STATE_SPLASH_SCREEN_NAME, this);
            Fsm.AddState<RunningState>(GameConsts.STATE_MAIN_MENU_NAME, this);
            Fsm.AddState<ClosingState>(GameConsts.STATE_CLOSING_NAME, this);
            Fsm.GetState(GameConsts.STATE_SPLASH_SCREEN_NAME).AddTransition(Fsm.GetState(GameConsts.STATE_MAIN_MENU_NAME));
            Fsm.GetState(GameConsts.STATE_SPLASH_SCREEN_NAME).AddTransition(Fsm.GetState(GameConsts.STATE_CLOSING_NAME));
            Fsm.GetState(GameConsts.STATE_MAIN_MENU_NAME).AddTransition(Fsm.GetState(GameConsts.STATE_CLOSING_NAME));

            foreach (var state in Fsm.States.Values)
            {
                (state as GameState).Initialise();
            }

            Fsm.GetState<GameState>(GameConsts.STATE_SPLASH_SCREEN_NAME).Enable();
            //Fsm.GetState<GameState>(GameConsts.STATE_SPLASH_SCREEN_NAME).Disable();

            Fsm.GetState<GameState>(GameConsts.STATE_SPLASH_SCREEN_NAME).Show();
            //Fsm.GetState<GameState>(GameConsts.STATE_SPLASH_SCREEN_NAME).Hide();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var state in Fsm.States.Values)
            {
                (state as GameState).LoadContent();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            (Fsm.ActiveState as GameState).Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Have or have not positioned the main window yet, if not then do this only once during start up - note
            // the repositioning of the main window is only done if the screen mode is set to bordered window setting
            if (MainFormTitleBarHeight == 0)
            {
                GameUtils.WINDOWINFO wInfo = new GameUtils.WINDOWINFO();
                wInfo.cbSize = (uint)Marshal.SizeOf(wInfo);
                GameUtils.GetWindowInfo(GameUtils.GetForegroundWindow(), ref wInfo);
                MainFormFrameWidth = (int)wInfo.cxWindowBorders;
                MainFormFrameHeight = (int)wInfo.cyWindowBorders;
                MainFormTitleBarHeight = wInfo.rcWindow.Height - wInfo.rcClient.Height - MainFormFrameHeight;

                if (ConfigData.ScreenMode == GameConsts.ScreenMode.BorderedWindow)
                {
                    switch (ConfigData.GameWindowPosition)
                    {
                        case GameConsts.WindowPosition.TopLeft:
                        {
                            Window.Position = new Point(0, MainFormTitleBarHeight);
                            break;
                        }

                        case GameConsts.WindowPosition.TopCentre:
                        {
                            Window.Position = new Point((GraphicsDevice.Adapter.CurrentDisplayMode.Width - Graphics.PreferredBackBufferWidth) / 2, MainFormTitleBarHeight);
                            break;
                        }

                        case GameConsts.WindowPosition.TopRight:
                        {
                            Window.Position = new Point(GraphicsDevice.Adapter.CurrentDisplayMode.Width - Graphics.PreferredBackBufferWidth, MainFormTitleBarHeight);
                            break;
                        }

                        case GameConsts.WindowPosition.CentreLeft:
                        {
                            Window.Position = new Point(0, (GraphicsDevice.Adapter.CurrentDisplayMode.Height - Graphics.PreferredBackBufferHeight) / 2);
                            break;
                        }

                        case GameConsts.WindowPosition.Centre:
                        {
                            Window.Position = new Point((GraphicsDevice.Adapter.CurrentDisplayMode.Width - Graphics.PreferredBackBufferWidth) / 2,
                                                        (GraphicsDevice.Adapter.CurrentDisplayMode.Height - Graphics.PreferredBackBufferHeight - MainFormTitleBarHeight) / 2);
                            break;
                        }

                        case GameConsts.WindowPosition.CentreRight:
                        {
                            Window.Position = new Point(GraphicsDevice.Adapter.CurrentDisplayMode.Width - Graphics.PreferredBackBufferWidth,
                                                        (GraphicsDevice.Adapter.CurrentDisplayMode.Height - Graphics.PreferredBackBufferHeight - MainFormTitleBarHeight) / 2);
                            break;
                        }

                        case GameConsts.WindowPosition.BottomLeft:
                        {
                            Window.Position = new Point(0, GraphicsDevice.Adapter.CurrentDisplayMode.Height - Graphics.PreferredBackBufferHeight);
                            break;
                        }

                        case GameConsts.WindowPosition.BottomCentre:
                        {
                            Window.Position = new Point((GraphicsDevice.Adapter.CurrentDisplayMode.Width - Graphics.PreferredBackBufferWidth) / 2,
                                                        GraphicsDevice.Adapter.CurrentDisplayMode.Height - Graphics.PreferredBackBufferHeight);
                            break;
                        }

                        case GameConsts.WindowPosition.BottomRight:
                        {
                            Window.Position = new Point(GraphicsDevice.Adapter.CurrentDisplayMode.Width - Graphics.PreferredBackBufferWidth,
                                                        GraphicsDevice.Adapter.CurrentDisplayMode.Height - Graphics.PreferredBackBufferHeight);
                            break;
                        }

                        case GameConsts.WindowPosition.UserDefined:
                        {
                            Window.Position = new Point((int)ConfigData.GameWindowPositionCoords.X, (int)ConfigData.GameWindowPositionCoords.Y);
                            break;
                        }
                    }
                }
            }

            GraphicsDevice.Clear(Color.CornflowerBlue);

            (Fsm.ActiveState as GameState).Draw(gameTime);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            if (ConfigData.ScreenMode == GameConsts.ScreenMode.BorderedWindow)
            {
                if(ConfigData.GameWindowPosition == GameConsts.WindowPosition.UserDefined)
                {
                    ConfigData.GameWindowPositionCoords = new Vector2(Window.Position.X, Window.Position.Y);
                }
            }

            ConfigData.WriteConfigData();
        }
    }
}
