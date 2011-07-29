using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Tutorial04
{
    // Utilities will be our static class where we can keep our helper methods.
    static public class Utilities
    {
        // Loading textures takes quite a bit of code, so we throw it in a helper method.
        static public int LoadTexture(string filename)
        {
            // If a filename isn't provided, throw an error.
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            // Generate an OpenGL texture handle and bind to it.
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            // Load up the bitmap image from a file on the computer.
            Bitmap bmp = new Bitmap(filename);

            // Lock the image's data into system memory so it can be used by OpenGL. We are forcing a
            // 32-bit alpha/red/green/blue pixel format.
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // With the image locked, we can tell OpenGL to store the bitmap's scan data into our texture buffer.
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            
            // Last, we unlock the bitmap. At this point, the Bitmap is no longer needed and can be discarded.
            bmp.UnlockBits(bmpData);

            // We also need to tell OpenGL how to render the texture when it is either smaller or larger than the
            // original bitmap size. Here, we are using nearest as we want to see the pixels without any filtering.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

            // Finally, we return the texture buffer handle back to the application for later use.
            return id;
        }
    }
}
