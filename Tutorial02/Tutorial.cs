using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Tutorial02
{
    public class Tutorial : GameWindow
    {
        const string TITLE = "Tutorial #2";
        const int WIDTH = 800;
        const int HEIGHT = 600;

        public Tutorial() : base(WIDTH, HEIGHT, GraphicsMode.Default, TITLE) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            // We start drawing by telling OpenGL what kind of shapes we will be using with the Begin() method.
            // Since we want to draw a rectangle with 4 points, we will use quads for simplicity.
            GL.Begin(BeginMode.Quads);

            // Each of these 4 lines sets the color and position of a single point. By default, the OpenGL view extends from
            // -1 to 1 both from left to right and bottom to top, regardless of the window size, so by using -0.5 and 0.5, we
            // end up with a rectangle that is exactly 1/4th the size of the screen, and located directly in its center. We
            // don't care about the Z-coordinate since we are creating a rectangle that is perpendicular to our eye, so we
            // use 0.
            GL.Color3(1f, 0f, 0f); GL.Vertex3(-0.5f, 0.5f, 0);      // red, top left
            GL.Color3(0f, 1f, 0f); GL.Vertex3(-0.5f, -0.5f, 0);     // green, bottom left
            GL.Color3(0f, 0f, 1f); GL.Vertex3(0.5f, -0.5f, 0);      // blue, bottom right
            GL.Color3(1f, 0f, 1f); GL.Vertex3(0.5f, 0.5f, 0);       // purple, top right
            
            // We end the drawing operation by calling End().
            GL.End();

            SwapBuffers();
        }

        static void Main(string[] args)
        {
            using (Tutorial tutorial = new Tutorial())
            {
                tutorial.Run();
            }
        }
    }
}
