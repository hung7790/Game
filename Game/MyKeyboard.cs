using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Input;

namespace Game
{
   public class MyKeyboard
    {
        KeyboardState previousState;
        KeyboardState newState;

        Collection<Keys> previousPressedKeys;
        Collection<Keys> hittedKeys;

        public MyKeyboard()
        {
            previousState = newState = Keyboard.GetState();
            hittedKeys = new Collection<Keys>();
            previousPressedKeys = new Collection<Keys>();
        }

        public void Update()
        {
            previousState = newState;
            newState = Keyboard.GetState();

            hittedKeys.Clear();

            foreach (Keys key in previousPressedKeys)
            {
                if (newState.IsKeyUp(key))
                    hittedKeys.Add(key);
            }

            previousPressedKeys.Clear();

            foreach (Keys key in newState.GetPressedKeys())
                previousPressedKeys.Add(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return newState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return newState.IsKeyUp(key);
        }

        public bool IsKeyHit(Keys key)
        {
            return hittedKeys.Contains(key);
        }

    }

}
