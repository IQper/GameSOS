using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSOS
{
    class Trajectory : Entity
    {
        public Trajectory(float velX, float velY, PointF pos)
        {
            var verticesList = new List<PointF>() { pos };
            
            for(var i = 0; i < 100; i ++)
            {
                velY += 1;
                pos.X += velX;
                pos.Y += velY;
                verticesList.Add(new PointF(pos.X, pos.Y));
            }
            vertices = verticesList.ToArray();
        }

        public override void Draw(Graphics graphics) => graphics.DrawCurve(new Pen(Color.Magenta), vertices);
    }
}
