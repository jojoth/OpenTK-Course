using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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

        // Initialization of the OpenGL context and assets happens here.
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f); // Set the clear color to a very dark gray
        }

        // Triggered when the window resizes.
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // set the viewport to match the new width and height of the window
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        // The rendering for the scene happens here.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit); // clear the OpenGL color buffer

            SwapBuffers(); // Swapping the background and foreground buffers to display our scene
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
