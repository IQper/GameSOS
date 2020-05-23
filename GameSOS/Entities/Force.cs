using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace GameSOS.Entities
{
    class Force : Entity
    {
        public Vector2 Value;
        public static Color BrushColor = Color.Magenta;

        public Force(PointF pos, float x, float y)
        {
            Value.X = x;
            Value.Y = y;
            Position = pos;
        }

        protected override void RecalculateVertices()
        {
            var angle = Math.Atan2(Value.Y, Value.X);
            vertices = new PointF[]
            {
                new PointF(Position.X, Position.Y),
                new PointF(Position.X + Value.X, Position.Y + Value.Y),
                RotatePoint(new PointF(Position.X + Value.Length() - 5, Position.Y - 5), Position, angle),
                new PointF(Position.X + Value.X, Position.Y + Value.Y),
                RotatePoint(new PointF(Position.X + Value.Length() - 5, Position.Y + 5), Position, angle),
            };
        }

        public override void Draw(Graphics graphics)
        {
            var pen = new Pen(BrushColor);
            for (var i = 0; i + 1 < vertices.Length; i++)
            {
                graphics.DrawLine(pen, vertices[i], vertices[i + 1]);
            }
        }
    }
}
