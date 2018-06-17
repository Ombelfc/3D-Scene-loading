using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Algorithms
{
    public interface IClippingAlgorithm
    {
        void SetBoundingRectangle(Vector2 begin, Vector2 end);
        bool ClipLine(ref Vector2 begin, ref Vector2 end);
    }

    public class LiangBarskyClipping : ClippingAlgorithm, IClippingAlgorithm
    {
        private float _t0;
        private float _t1;

        public bool ClipLine(ref Vector2 begin, ref Vector2 end)
        {
            var delta = end - begin;
            _t0 = 0;
            _t1 = 1;

            if (!Clip(-delta.X, -ClipMin.X + begin.X)) return false;
            if (!Clip(delta.X, ClipMax.X - begin.X)) return false;
            if (!Clip(-delta.Y, -ClipMin.Y + begin.Y)) return false;
            if (!Clip(delta.Y, ClipMax.Y - begin.Y)) return false;

            if(_t1 < 1)
            {
                end = begin + delta * _t1;
            }

            if(_t0 > 0)
            {
                begin += delta * _t0;
            }

            return true;
        }

        private bool Clip(float p, float q)
        {
            if(Math.Abs(p) < float.Epsilon)
            {
                if (q < 0) return false;
            }
            else
            {
                var r = q / p;

                if(p < 0)
                {
                    if (r > _t1) return false;
                    if (r > _t0) _t0 = r;
                }
                else
                {
                    if (r < _t0) return false;
                    if (r < _t1) _t1 = r;
                }
            }

            return true;
        }
    }

    public class ClippingAlgorithm
    {
        protected Vector2 ClipMax;
        protected Vector2 ClipMin;

        public void SetBoundingRectangle(Vector2 begin, Vector2 end)
        {
            ClipMin = begin;
            ClipMax = end;
        }
    }
}
