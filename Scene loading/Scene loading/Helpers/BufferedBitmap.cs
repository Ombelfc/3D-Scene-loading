using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Scene_loading.Helpers
{
    // A class that implements cached bitmap access.
    public class BufferedBitmap : IBufferedBitmap
    {
        // Buffer holding the pixels, which will be displayed in the next frame of the animation.
        private readonly byte[] _backBuffer;

        // The actual bitmap that is displayed to the user.
        public WriteableBitmap BitmapSource { get; }

        // The ratio of the width and height of the bitmap.
        public float AspectRatio { get; }

        // The width of the bitmap.
        public int PixelWidth { get; }

        // The height of the bitmap.
        public int PixelHeight { get; }

        // Creates a cached bitmap with a given width and height in pixels.
        public BufferedBitmap(int pixelWidth, int pixelHeight)
        {
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;
            AspectRatio = PixelWidth / (float) PixelHeight;
            BitmapSource = new WriteableBitmap(pixelWidth, pixelHeight, 96, 96, PixelFormats.Bgra32, null);

            // The buffer size is equal to the number of pixels drawn.
            // Each pixel has a size of 4 bytes, 1 byte per channel (BGRA).
            _backBuffer = new byte[PixelWidth * PixelHeight * 4];
        }

        // Fills the whole buffer with a solid color.
        public void Clear(Color32 color)
        {
            for (var offset = 0; offset < _backBuffer.Length; offset += 4)
            {
                _backBuffer[offset] = color.B;
                _backBuffer[offset + 1] = color.G;
                _backBuffer[offset + 2] = color.R;
                _backBuffer[offset + 3] = color.A;
            }
        }

        // Draws a pixel in a buffered frame, first checking if it is within limits.
        public void DrawPoint(int x, int y, Color32 color)
        {
            if (x < 0 || y < 0 || x >= PixelWidth || y >= PixelHeight) return;

            var offset = (x + y * PixelWidth) * 4;
            _backBuffer[offset] = color.B;
            _backBuffer[offset + 1] = color.G;
            _backBuffer[offset + 2] = color.R;
            _backBuffer[offset + 3] = color.A;
        }

        // When the buffer is ready, it will be pushed into the bitmap.
        // So that the frame of the animation could be displayed on the screen.
        public void Present()
        {
            var rect = new Int32Rect(0, 0, PixelWidth, PixelHeight);
            BitmapSource.WritePixels(rect, _backBuffer, BitmapSource.BackBufferStride, 0);
        }
    }
}
