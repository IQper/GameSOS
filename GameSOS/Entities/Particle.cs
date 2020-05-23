using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSOS
{
    class Particle : Entity
    {
        public int Lifetime;
        public float Size;
        public double AngleSpeed;
        public bool IsAlive;
        public float SizeDecrease;
        public int verticesCount;


        public Particle(PointF pos, float size, int lifetime, int vertCount, double angleSpeed, double angle, float velX, float velY, Color color, float gravity = 0)
        {
            Gravity = gravity;
            IsAlive = true;
            Lifetime = lifetime;
            verticesCount = vertCount;
            VelocityX = velX;
            VelocityY = velY;
            brush = new SolidBrush(color);
            Angle = angle;
            AngleSpeed = angleSpeed;
            Size = size;
            Position = pos;
            SizeDecrease = Size / Lifetime;
        }

        public void AnimationStep()
        {
            if (Lifetime < 0 && IsAlive) IsAlive = false;
            else
            {
                Lifetime -= 1;
                Angle += AngleSpeed;
                Size -= SizeDecrease;
                UpdatePosition();
            }
        }

        protected override void RecalculateVertices()
        {
            var vert = new List<PointF>();
            for(var i = 0; i <= verticesCount; i++)
            {
                var ang = Math.PI * 2 / verticesCount;
                vert.Add(RotatePoint(new PointF(Position.X + Size, Position.Y + Size), Position, ang * i));
            }
            vertices = vert.ToArray();
        }
    }
}
