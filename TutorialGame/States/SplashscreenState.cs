// Author: Chris Knowles
// Date: Jan 2023
// Copyright: Copperhead Labs, (c)2023
// File: SplashscreenState.cs
// Version: 1.0.0
// Notes: 

using FSM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;
using TutorialGame.Engine;

namespace TutorialGame.States
{
    public class SplashscreenState : GameState
    {
        public Texture2D SplashscreenTexture { get; protected set; } = null;
        public GameTime StartTime { get; protected set; } = null;

        protected SplashscreenState(MainGame mainGame, FSM.FSM fsm, string name) :
            base(mainGame, fsm, name) { }

        public static new SplashscreenState ObtainInstance(FSM.FSM fsm, string name, object mainGame)
        {
            return new SplashscreenState(mainGame as MainGame, fsm, name);
        }

        public override void Initialise()
        {
            if (!Initialised)
            {
                Console.WriteLine($"GameState -> {this} is initialising");
            }

            Initialised = true;
        }

        public override void LoadContent()
        {
            if (!ContentLoaded)
            {
                Console.WriteLine($"GameState -> {this} is loading content");
                SplashscreenTexture = MainGame.Content.Load<Texture2D>(GameConsts.CONTENT_IMAGES_SPLASH_SCREEN_PATH);
            }

            ContentLoaded= true;
        }

        public override void Enter<T>(FSMState previousState, T data)
        {
            if (data != null)
            {
                Console.WriteLine($"Entered into state \"{Name}\" from state \"{previousState.Name}\" {data}");
            }
            else
            {
                Console.WriteLine($"Entered into state \"{Name}\" from state \"{previousState.Name}\" with no data");
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                //Console.WriteLine($"Updating game state -> [{Name}] with frame rate -> [{1.0 / gameTime.ElapsedGameTime.TotalSeconds}] fps");

                if ((gameTime.TotalGameTime - (StartTime ??= gameTime).TotalGameTime).Seconds > 5)
                {
                    FireTransition<GameTime>(MainGame.Fsm.GetState(GameConsts.STATE_CLOSING_NAME), data : gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Enabled && Visible)
            {
                MainGame.SpriteBatch.Begin();
                MainGame.SpriteBatch.Draw(SplashscreenTexture, Vector2.Zero, Color.White);
                MainGame.SpriteBatch.End();
            }
        }

        public override void Exit<T>(FSMState nextState, T data)
        {
            if (data != null)
            {
                Console.WriteLine($"Exited from state \"{Name}\" into state \"{nextState.Name}\" {data}");
            }
            else
            {
                Console.WriteLine($"Exited from state \"{Name}\" into state \"{nextState.Name}\" with no data");
            }

            Hide();
            Disable();
        }

        public override void Shutdown()
        {
            Console.WriteLine($"GameState -> {this} is shutting down");
        }
    }
}
