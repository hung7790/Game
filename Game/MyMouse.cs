using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;	

namespace Game
{
    class MyMouse
    {
        MouseState previousState;
        MouseState newState;
        int movedX;     
        int movedY;   

        public MyMouse()
        {
            previousState = newState = Mouse.GetState();
        }

        public void Update()
        {
            previousState = newState;
            newState = Mouse.GetState();
            movedX = newState.X - previousState.X;
            movedY = newState.Y - previousState.Y;
        }

        public Vector2 MouseMoved()
        {
            Vector2 v = new Vector2((float)movedX, (float)movedY);
            return v;
        }

        public Vector2 MousePosition()
        {
            Vector2 v = new Vector2((float)newState.X, (float)newState.Y);
            return v;
        }

        public bool IsRightButtonzPressed()
        {
            return (newState.RightButton == ButtonState.Pressed);
        }

        public bool IsRightButtonReleased()
        {
            return (newState.RightButton == ButtonState.Released &&
                    previousState.RightButton == ButtonState.Pressed);
        }
    
    }
}
