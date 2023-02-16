// Author: Chris Knowles
// Date: Jan 2023
// Copyright: Copperhead Labs, (c)2023
// File: GameState.cs
// Version: 1.0.0
// Notes: 

using FSM;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TutorialGame.Engine;
using TutorialGame.UI;

namespace TutorialGame.States
{
    /// <summary>
    /// Class <c>GameState</c> inherits from <see cref="FSMState"/> and represents a state within
    /// the game FSM, this is an abstract class and should be the base class for the concrete
    /// classes for the various actual game states
    /// </summary>
    public abstract class GameState : FSMState
    {
        public MainGame MainGame { get; protected set; }
        public bool Initialised { get; protected set; }
        public bool ContentLoaded { get; protected set; }
        public bool Configured { get { return Initialised && ContentLoaded; } } 
        public bool Enabled { get; protected set; }
        public bool Visible { get; protected set; }
        public List<GameObject> GameObjects { get; protected set; }
        public List<UIObject> UIObjects { get; protected set; }

        protected GameState(MainGame mainGame, FSM.FSM fsm, string name) :
            base(fsm, name)
        {
            MainGame = mainGame;
            Initialised = ContentLoaded = Enabled = Visible = false;
            GameObjects = new List<GameObject>();
            UIObjects = new List<UIObject>();
        }

        public bool Enable()
        {
            var prev = Enabled;
            Enabled = true;
            return prev;
        }

        public bool Disable()
        {
            var prev = Enabled;
            Enabled = false;
            return prev;
        }

        public bool ToggleEnabled()
        {
            var prev = Enabled;
            Enabled = !Enabled;
            return prev;
        }

        public bool Show()
        {
            var prev = Visible;
            Visible = true;
            return prev;
        }

        public bool Hide()
        {
            var prev = Visible;
            Visible = false;
            return prev;
        }

        public bool ToggleVisible()
        {
            var prev = Visible;
            Visible = !Visible;
            return prev;
        }

        public abstract void Initialise();
        
        public abstract void LoadContent();
        
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void Shutdown();
    }
}
