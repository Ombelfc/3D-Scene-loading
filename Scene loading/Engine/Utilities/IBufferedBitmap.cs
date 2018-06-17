using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utilities
{
    public interface IBufferedBitmap
    {
        // The ratio of the width and height of the bitmap.
        float AspectRatio { get; }

        // The width of the bitmap.
        int PixelWidth { get; }

        // The height of the bitmap.
        int PixelHeight { get; }

        // Draws a pixel in a buffered frame, first checking if it is within limits.
        void DrawPoint(int x, int y, Color32 color);

        // Fill the whole buffer with a solid color.
        void Clear(Color32 color);

        // The method is called when the animation frame is fully rendered.
        void Present();
    }
}
