using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameSOS
{
    class Player : Entity
    {
        public static Color BrushColor = Color.FromArgb(255, 0, 90, 100);
        public readonly int Width;
        public readonly int Height;
        public readonly PointF StartPos;
        public bool IsJumpReady;
        public readonly float JumpSpeedLimit;
        public bool NotDraw;

        public Player(PointF position, int size, float jumpSpeedLimit, PointF startPos)
        {
            NotDraw = false;
            StartPos = startPos;
            JumpSpeedLimit = jumpSpeedLimit;
            Angle = 0;
            Width = size;
            Height = size;
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

        public Player Copy()
        {
            var player = new Player(Position, (Width + Height) / 2, JumpSpeedLimit, StartPos);
            player.Gravity = Gravity;
            player.VelocityX = VelocityX;
            player.VelocityY = VelocityY;
            return player;
        }
    }
}
