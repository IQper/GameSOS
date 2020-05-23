using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSOS.Structures
{
    class Segment : Entity
    {
        public PointF Start;
        public PointF End;

        public Segment(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }
        public Segment(int startX, int startY, int endX, int endY)
        {
            Start = new PointF(startX, startY);
            End = new PointF(endX, endY);
        }

        public bool IsIntersect(Segment segment)
        {
            var start1 = Start;
            var end1 = End;
            var start2 = segment.Start;
            var end2 = segment.End;
            var dir1 = new PointF(end1.X - start1.X, end1.Y - start1.Y);
            var dir2 = new PointF(end2.X - start2.X, end2.Y - start2.Y);

            //считаем уравнения прямых проходящих через отрезки
            float a1 = -dir1.Y;
            float b1 = +dir1.X;
            float d1 = -(a1 * start1.X + b1 * start1.Y);

            float a2 = -dir2.Y;
            float b2 = +dir2.X;
            float d2 = -(a2 * start2.X + b2 * start2.Y);

            //подставляем концы отрезков, для выяснения в каких полуплоскотях они
            float seg1_line2_start = a2 * start1.X + b2 * start1.Y + d2;
            float seg1_line2_end = a2 * end1.X + b2 * end1.Y + d2;

            float seg2_line1_start = a1 * start2.X + b1 * start2.Y + d1;
            float seg2_line1_end = a1 * end2.X + b1 * end2.Y + d1;

            //если концы одного отрезка имеют один знак, значит он в одной полуплоскости и пересечения нет.
            if (seg1_line2_start * seg1_line2_end >= 0 || seg2_line1_start * seg2_line1_end >= 0)
                return false;
                return true;
        }
        public override void Draw(Graphics graphics)
        {
            graphics.DrawLine(new Pen(Color.Red), Start, End);
        }
        public PointF GetIntersectionPoint(Segment segment)
        {
            var start1 = Start;
            var end1 = End;
            var start2 = segment.Start;
            var end2 = segment.End;
            var dir1 = new PointF(end1.X - start1.X, end1.Y - start1.Y);
            var dir2 = new PointF(end2.X - start2.X, end2.Y - start2.Y);

            //считаем уравнения прямых проходящих через отрезки
            float a1 = -dir1.Y;
            float b1 = +dir1.X;
            float d1 = -(a1 * start1.X + b1 * start1.Y);

            float a2 = -dir2.Y;
            float b2 = +dir2.X;
            float d2 = -(a2 * start2.X + b2 * start2.Y);

            //подставляем концы отрезков, для выяснения в каких полуплоскотях они
            float seg1_line2_start = a2 * start1.X + b2 * start1.Y + d2;
            float seg1_line2_end = a2 * end1.X + b2 * end1.Y + d2;

            float seg2_line1_start = a1 * start2.X + b1 * start2.Y + d1;
            float seg2_line1_end = a1 * end2.X + b1 * end2.Y + d1;

            //если концы одного отрезка имеют один знак, значит он в одной полуплоскости и пересечения нет.
            if (seg1_line2_start * seg1_line2_end >= 0 || seg2_line1_start * seg2_line1_end >= 0)
                throw new Exception();

            float u = seg1_line2_start / (seg1_line2_start - seg1_line2_end);
            return new PointF(start1.X + dir1.X * u, start1.Y + dir1.Y * u);
        }
    }
}
