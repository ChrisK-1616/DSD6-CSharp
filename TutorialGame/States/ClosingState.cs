// Author: Chris Knowles
// Date: Jan 2023
// Copyright: Copperhead Labs, (c)2023
// File: ClosingState.cs
// Version: 1.0.0
// Notes: 

using FSM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TutorialGame.Engine;

namespace TutorialGame.States
{
    public class ClosingState : GameState
    {
        public Texture2D EndScreenTexture { get; protected set; } = null;
        public GameTime StartTime { get; protected set; } = null;

        protected ClosingState(MainGame mainGame, FSM.FSM fsm, string name) :
            base(mainGame, fsm, name) { }

        public static new ClosingState ObtainInstance(FSM.FSM fsm, string name, object mainGame)
        {
            return new ClosingState(mainGame as MainGame, fsm, name);
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
                EndScreenTexture = MainGame.Content.Load<Texture2D>(GameConsts.CONTENT_IMAGES_END_SCREEN_PATH);
            }

            ContentLoaded = true;
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

            var gameTime = data as GameTime;
            StartTime = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime, gameTime.IsRunningSlowly);

            Enable();
            Show();
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                //Console.WriteLine($"Updating game state -> [{Name}] with frame rate -> [{1.0 / gameTime.ElapsedGameTime.TotalSeconds}] fps");

                if ((gameTime.TotalGameTime - StartTime.TotalGameTime).Seconds > 5)
                {
                    MainGame.Exit();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Enabled && Visible)
            {
                MainGame.SpriteBatch.Begin();
                MainGame.SpriteBatch.Draw(EndScreenTexture, Vector2.Zero, Color.White);
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
