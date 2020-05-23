using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameSOS
{
    class Cam
    {
        public float DefaultZoom;
        public float X = 1;
        public float Y = 1;
        private float target;
        private float speed;

        public Cam(float defaultZoom) => DefaultZoom = defaultZoom;
        public void RecalculateCam(float velocity)
        {
            target = velocity > 0.05 ? 1 / (velocity * 2 + 1) * DefaultZoom : DefaultZoom;

            speed = Math.Abs(X - target) / 5;

            if (Math.Abs(X - target) <= speed) X = target;
            else if (X < target) X += speed;
            else if (X > target) X -= speed;

            Y = X;
        }
    }
}
