using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Tutorial03
{
    public class Tutorial : GameWindow
    {
        const string TITLE = "Tutorial #3";
        const int WIDTH = 800;
        const int HEIGHT = 600;

        private float rotation = 0.0f; // Create a variable to hold our rotation value.

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

            // Create the projection matrix that will align our screen's width/height with OpenGL to remove distortion.
            Matrix4 projection = Matrix4.CreateOrthographic(ClientRectangle.Width, ClientRectangle.Height, -1f, 1f);

            // Set OpenGL's state to projection mode and load in our projection matrix.
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            // Set OpenGL's state back to modelview mode which will allow us to handle our 2D tranformations without
            // affecting the projection matrix we set above.
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            // Modify the rotation angle based on time since last update. Multiplying the time by 360 will cause
            // the rectangle to spin 360 degrees per second regardless of framerate.
            rotation += (float)e.Time * 360;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.LoadIdentity(); // Clear the current modelview matrix to the identity matrix so we can start fresh.
            GL.Rotate(rotation, Vector3.UnitZ); // Rotate the modelview matrix around the Z-axis.

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(BeginMode.Quads);

            // Modified the hardcoded decimal values to use the screen's original WIDTH and HEIGHT values.
            // Since the viewport width/height is adjusted when the screen resizes, we should no longer see any
            // distortion caused by resizing.
            GL.Color3(1f, 0f, 0f); GL.Vertex3(-WIDTH / 4, HEIGHT / 4, 0);
            GL.Color3(0f, 1f, 0f); GL.Vertex3(-WIDTH / 4, -HEIGHT / 4, 0);
            GL.Color3(0f, 0f, 1f); GL.Vertex3(WIDTH / 4, -HEIGHT / 4, 0);
            GL.Color3(1f, 0f, 1f); GL.Vertex3(WIDTH / 4, HEIGHT / 4, 0);

            GL.End();

            SwapBuffers();
        }

        static void Main(string[] args)
        {
            using (Tutorial tutorial = new Tutorial())
            {
                tutorial.Run(60.0f); // Set the number of update calls to 30 / second.
            }
        }
    }
}
