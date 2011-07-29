using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Tutorial04
{
    public class Tutorial : GameWindow
    {
        const string TITLE = "Tutorial #4";
        const int WIDTH = 800;
        const int HEIGHT = 600;

        private float rotation = 0.0f;
        
        private int charTextureId;      // Create a private variable to hold our handle to the texture buffer.
        private float charSize = 64;    // Just a temporary way for us to keep track of how big our caveman is.

        public Tutorial() : base(WIDTH, HEIGHT, GraphicsMode.Default, TITLE) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
            GL.Enable(EnableCap.Texture2D); // Enable Texture2D to tell OpenGL that it should render bound texture.

            // Load up our image using our LoadTexture() static utility method and save its handle id.
            charTextureId = Utilities.LoadTexture(@"Images\caveman.png");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreateOrthographic(ClientRectangle.Width, ClientRectangle.Height, -1f, 1f);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            rotation += (float)e.Time * 360;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.LoadIdentity();
            GL.Rotate(rotation, Vector3.UnitZ);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(BeginMode.Quads);

            GL.BindTexture(TextureTarget.Texture2D, charTextureId); // Bind our texture that we want rendered.

            // Since we are using a texture instead of colors, we can get rid of the Color3() calls and replace
            // them with TexCoord2() calls. We need to describe what part of the image (in X,Y coordinates from
            // 0 to 1) correlates to the point we are rendering. The renderer will work out how the image needs
            // to be drawn for each of the pixels in-between these corner vertices. Additionally, we also
            // changed our Vertex3() calls to draw the points based on the caveman's width/height.
            GL.TexCoord2(0f, 0f); GL.Vertex3(-charSize / 2, charSize / 2, 0);
            GL.TexCoord2(0f, 1f); GL.Vertex3(-charSize / 2, -charSize / 2, 0);
            GL.TexCoord2(1f, 1f); GL.Vertex3(charSize / 2, -charSize / 2, 0);
            GL.TexCoord2(1f, 0f); GL.Vertex3(charSize / 2, charSize / 2, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0); // Unbind texture when we are done with it.

            GL.End();

            SwapBuffers();
        }

        static void Main(string[] args)
        {
            using (Tutorial tutorial = new Tutorial())
            {
                tutorial.Run(60.0f);
            }
        }
    }
}
