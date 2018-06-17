using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Algorithms
{
    public interface ILineDrawingAlgorithm
    {
        void DrawLine(Vector2 begin, Vector2 end, Action<int, int> drawAction);
    }

    public class Bresenham : ILineDrawingAlgorithm
    {
        public void DrawLine(Vector2 begin, Vector2 end, Action<int, int> drawAction)
        {
            var x = (int) begin.X;
            var y = (int) begin.Y;

            var x2 = (int) end.X;
            var y2 = (int) end.Y;

            var dx = Math.Abs(x2 - x);
            var dy = Math.Abs(y2 - y);
            var sx = x < x2 ? 1 : -1;
            var sy = y < y2 ? 1 : -1;
            var err = dx - dy;

            drawAction.Invoke(x, y);

            while (!(x == x2 && y == y2))
            {
                var e2 = err << 1;
                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }

                drawAction.Invoke(x, y);
            }
        }
    }
}
