using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Tutorial07
{
    public class Tutorial : GameWindow
    {
        const string TITLE = "Tutorial #7";
        const int WIDTH = 800;
        const int HEIGHT = 600;

        private int charTextureId;
        private float charSize = 64;
        private int charAnimIndex = 7;
        private bool isMoving = false;
        private int direction = 1;
        private float xPos = 0f; // Add a variable for storing our X position on the screen.
        private float moveSpeed = 100f; // Add a variable for movement speed.

        private Vector2[] charTextureIndexes = new Vector2[] {
            new Vector2(0f, 0f),
            new Vector2(0.25f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0.75f, 0f),
            new Vector2(0f, 0.25f),
            new Vector2(0.25f, 0.25f),
            new Vector2(0.5f, 0.25f),
            new Vector2(0.75f, 0.25f),
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

            if (Keyboard[Key.Left])
            {
                direction = -1;
                xPos -= (float)e.Time * moveSpeed; // Move the player left 60 pixels per second.
                isMoving = true;
            }
            else if (Keyboard[Key.Right])
            {
                direction = 1;
                xPos += (float)e.Time * moveSpeed; // Move the player right 60 pixels per second.
                isMoving = true;
            }
            else
                isMoving = false;

            if (isMoving)
                charAnimIndex = (charAnimIndex + 1) % 7;
            else
                charAnimIndex = 7;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.LoadIdentity();

            // Translate the modelview by player's xPos before drawing our player.
            GL.Translate(new Vector3(xPos, 0f, 0f));

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(BeginMode.Quads);

            GL.BindTexture(TextureTarget.Texture2D, charTextureId);

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
                tutorial.Run(10f);
            }
        }
    }
}
