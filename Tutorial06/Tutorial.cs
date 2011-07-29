using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Tutorial06
{
    public class Tutorial : GameWindow
    {
        const string TITLE = "Tutorial #6";
        const int WIDTH = 800;
        const int HEIGHT = 600;

        private int charTextureId;
        private float charSize = 64;
        private int charAnimIndex = 7; // Changed to 7 to use the standing frame at start time.
        private bool isMoving = false; // Added to indicate whether our character is moving or not.
        private int direction = 1; // Specifies the direction of the player. -1 for left, 1 for right.

        private Vector2[] charTextureIndexes = new Vector2[] {
            new Vector2(0f, 0f),
            new Vector2(0.25f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0.25f),
            new Vector2(0.5f, 0.25f),
            new Vector2(0.75f, 0.25f), // Added one more texture coordinate location for standing frame.
        };

        public Tutorial() : base(WIDTH, HEIGHT, GraphicsMode.Default, TITLE) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

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

            // If the player is pressing left, we want to set the direction to left and enable movement animation.
            if (Keyboard[Key.Left])
            {
                direction = -1;
                isMoving = true;
            }
            // If the player is pressing right, we want to set the direction to right and enable movement animation.
            else if (Keyboard[Key.Right])
            {
                direction = 1;
                isMoving = true;
            }
            // If the player is not pressing left or right, we want to stop the movement animation.
            else
                isMoving = false;

            if (isMoving)
                charAnimIndex = (charAnimIndex + 1) % 7;    // If moving, switch between frames 0-6.
            else
                charAnimIndex = 7;                          // If not moving, lock animation on frame 7.
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.LoadIdentity();

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(BeginMode.Quads);

            GL.BindTexture(TextureTarget.Texture2D, charTextureId);

            // Multiplying the X coordinate of our vertices by the direction variable to simulate the player
            // facing left (-1) or right (1). This is easier than trying to switch the X coordinate of the
            // texture coordinates, but it only works because we are not culling our quad's back face. When
            // we reorganize our code into reusable classes, we will do this properly.
            GL.TexCoord2(charTextureIndexes[charAnimIndex] + new Vector2(0f, 0f)); GL.Vertex3(direction * -charSize / 2, charSize / 2, 0);
            GL.TexCoord2(charTextureIndexes[charAnimIndex] + new Vector2(0f, 0.25f)); GL.Vertex3(direction * -charSize / 2, -charSize / 2, 0);
            GL.TexCoord2(charTextureIndexes[charAnimIndex] + new Vector2(0.25f, 0.25f)); GL.Vertex3(direction * charSize / 2, -charSize / 2, 0);
            GL.TexCoord2(charTextureIndexes[charAnimIndex] + new Vector2(0.25f, 0f)); GL.Vertex3(direction * charSize / 2, charSize / 2, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.End();

            SwapBuffers();
        }

        static void Main(string[] args)
        {
            using (Tutorial tutorial = new Tutorial())
            {
                tutorial.Run(10.0f);
            }
        }
    }
}
