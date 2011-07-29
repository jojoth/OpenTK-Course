using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Tutorial05
{
    public class Tutorial : GameWindow
    {
        const string TITLE = "Tutorial #5";
        const int WIDTH = 800;
        const int HEIGHT = 600;

        //private float rotation = 0.0f; [REMOVED]

        private int charTextureId;
        private float charSize = 64;
        private int charAnimIndex = 0; // An index value into the current state of the animation.

        // An array of texture coordinates that match each frame of the animation. I'm only using the first
        // 7 frames of the image here (the eighth is a standing image, so I'm not going to include it yet).
        private Vector2[] charTextureIndexes = new Vector2[] {
            new Vector2(0f, 0f),
            new Vector2(0.25f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0.25f),
            new Vector2(0.5f, 0.25f),
        };

        public Tutorial() : base(WIDTH, HEIGHT, GraphicsMode.Default, TITLE) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
            GL.Enable(EnableCap.Texture2D);
            
            // Add OpenGL state for blending to allow for transparency.
            GL.Enable(EnableCap.Blend);

            // This may seem a bit confusing, but all it really means is that we want to blend the pixels of the
            // source image based on its alpha (1 = fully opaque, 0.5 = half visible, 0 = fully transparent)
            // with the pixels behind it based on the inverse of its alpha (1 = fully covered, 0.5 = half covered,
            // 0 = fully visible). This basically means that transparent pixels in the image are ignored.
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // Changed the texture to load an 8-frame tiled animation of our caveman friend.
            charTextureId = Utilities.LoadTexture(@"Images\caveman-tiled.png");
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

            // When the frame is updated, we want to increase the animation index by one, but clamp it to the values
            // between 0 and 6 using a modulus operator. This means it will rollback to 0 before it reaches 7.
            charAnimIndex = (charAnimIndex + 1) % 7;
            //rotation += (float)e.Time * 360; [REMOVED]
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.LoadIdentity();
            //GL.Rotate(rotation, Vector3.UnitZ); [REMOVED]

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(BeginMode.Quads);

            GL.BindTexture(TextureTarget.Texture2D, charTextureId);

            // Modified the texture coordinates to use the values from the current animation state, and adding an additional 0.25f
            // for the vertices that are on the right or bottom of the frame.
            GL.TexCoord2(charTextureIndexes[charAnimIndex] + new Vector2(0f, 0f)); GL.Vertex3(-charSize / 2, charSize / 2, 0);
            GL.TexCoord2(charTextureIndexes[charAnimIndex] + new Vector2(0f, 0.25f)); GL.Vertex3(-charSize / 2, -charSize / 2, 0);
            GL.TexCoord2(charTextureIndexes[charAnimIndex] + new Vector2(0.25f, 0.25f)); GL.Vertex3(charSize / 2, -charSize / 2, 0);
            GL.TexCoord2(charTextureIndexes[charAnimIndex] + new Vector2(0.25f, 0f)); GL.Vertex3(charSize / 2, charSize / 2, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.End();

            SwapBuffers();
        }

        static void Main(string[] args)
        {
            using (Tutorial tutorial = new Tutorial())
            {
                tutorial.Run(10.0f); // Lower the update calls to only happen 10 times a second.
            }
        }
    }
}
