using GameSOS.Entities;
using GameSOS.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GameSOS
{
    class Entity
    {
        protected PointF[] vertices;
        protected Brush brush;
        private PointF pos;
        private double ang;
        public float Gravity;

        public float velocityWhenStay = 0.5f;

        public float VelocityX { get; set; }
        public float VelocityY { get; set; }

        public PointF Position 
        {
            get { return pos; }
            set 
            {
                pos = value; 
                RecalculateVertices(); 
            } 
        }
        public double Angle 
        { 
            get => ang; 
            set
            {
                ang = value; RecalculateVertices(); 
            } 
        }

        protected virtual void RecalculateVertices() { }
        public virtual void Draw(Graphics graphics)
        {
            graphics.FillPolygon(brush, vertices);
        }
        public List<Segment> GetSegments()
        {
            var list = new List<Segment>();
            for(var i = 0; i < vertices.Length; i++)
            {
                if (i == vertices.Length - 1) list.Add(new Segment(vertices[i], vertices[0]));
                else list.Add(new Segment(vertices[i], vertices[i + 1]));
            }
            return list;
        }

        public void UpdatePosition() // remake
        {
            Position = new PointF(Position.X + (int)VelocityX, Position.Y + (int)VelocityY);
            VelocityY += Gravity;
        }
        public bool IsIntersect(Entity entity)
        {
            foreach(var ownSegment in GetSegments())
            {
                foreach(var entitySegment in entity.GetSegments())
                {
                    if (ownSegment.IsIntersect(entitySegment)) return true;
                }
            }
            return false;
        }
        public IEnumerable<PointF> GetIntersectionPoint(Entity entity)
        {
            foreach (var ownSegment in GetSegments())
            {
                foreach (var entitySegment in entity.GetSegments())
                {
                    if (ownSegment.IsIntersect(entitySegment)) yield return ownSegment.GetIntersectionPoint(entitySegment);
                }
            }
        }
        protected PointF RotatePoint(PointF point, PointF center, double alpha)
        {
            return new PointF(
                    (int)Math.Round(center.X + (point.X - center.X) * Math.Cos(alpha) - (point.Y - center.Y) * Math.Sin(alpha)),
                    (int)Math.Round(center.Y + (point.X - center.X) * Math.Sin(alpha) + (point.Y - center.Y) * Math.Cos(alpha))
                );
        }
    }
}
