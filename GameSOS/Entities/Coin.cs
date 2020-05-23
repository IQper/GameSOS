using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSOS
{
    class Coin : Entity
    {
        public static Color BrushColor = Color.FromArgb(255, 250, 200, 0);
        public float Size;
        private double iteration;
        
        public void AnimationStep()
        {
            iteration += 1;
            Angle = Math.Pow(Math.Sin(iteration / 100), 2) * 20;
        }
        
        public Coin(PointF position, int size, int seed)
        {
            iteration = new Random(seed).NextDouble() * 2000;
            Size = size;
            Position = position;
            brush = new SolidBrush(BrushColor);
        }

        protected override void RecalculateVertices()
        {
            vertices = new PointF[]
            {
                RotatePoint(new PointF(Position.X + Size, Position.Y + Size), Position, Angle),
                RotatePoint(new PointF(Position.X + Size, Position.Y + Size), Position, Angle + Math.PI * 2 / 3),
                RotatePoint(new PointF(Position.X + Size, Position.Y + Size), Position, Angle - Math.PI * 2 / 3)
            };
        }
    }
}
