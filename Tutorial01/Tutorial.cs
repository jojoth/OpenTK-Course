using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;

namespace Tutorial01
{
    // Derive your primary class from GameWindow to use OpenTK's automatic OpenGL window handling.
    public class Tutorial : GameWindow
    {
        // We are going to use this constant in all tutorials for the title of the window.
        const string TITLE = "Tutorial #1";
        
        // The constructor for the class. We tell the GameWindow that we derive from to create a 1024x768 window
        // using the title "Tutorial #1".
        public Tutorial() : base(1024, 768, GraphicsMode.Default, TITLE) { }

        // The rendering for the scene happens here. In this bare bones example, the minimum we need to do is tell
        // OpenTK to swap the backbuffer so we don't hit full CPU usage.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            SwapBuffers();
        }

        // The application's starting point. Here, we just initialize the Tutorial class and tell it to run.
        static void Main(string[] args)
        {
            using (Tutorial tutorial = new Tutorial())
            {
                tutorial.Run();
            }
        }
    }
}
