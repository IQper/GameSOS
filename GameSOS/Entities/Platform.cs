using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSOS   
{
    class Platform : Entity
    {
        public static Color BrushColor = Color.FromArgb(255, 0, 0, 0);
        public readonly int Width;
        public readonly int Height;

        public Platform(PointF position, int width, int height, double angle = 0)
        {
            
            Angle = angle;
            Width = width;
            Height = height;
            Position = position;
            brush = new SolidBrush(BrushColor);
        }

        protected override void RecalculateVertices()
        {
            vertices = new PointF[]
            {
                RotatePoint(new PointF(Position.X - Width/2, Position.Y - Height/2), Position, Angle),
                RotatePoint(new PointF(Position.X + Width/2, Position.Y - Height/2), Position, Angle),
                RotatePoint(new PointF(Position.X + Width/2, Position.Y + Height/2), Position, Angle),
                RotatePoint(new PointF(Position.X - Width/2, Position.Y + Height/2), Position, Angle),
            };
        }
    }
}
