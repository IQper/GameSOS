using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameSOS.Structures;
using NUnit.Framework;

namespace GameSOS
{
    [TestFixture]
    class SegmentTest
    {
        [Test]
        public void SegmentsNotIntersecting5()
        {
            var segment1 = new Segment(1, 0, 2, 1);
            var segment2 = new Segment(1, 0, 2, 0);
            Assert.AreEqual(false, segment1.IsIntersect(segment2));
        }

        [Test]
        public void SegmentsIntersecting2()
        {
            var segment1 = new Segment(-1, 1, 2, -2);
            var segment2 = new Segment(0, -2, 1, 3);
            Assert.AreEqual(true, segment1.IsIntersect(segment2));
        }

        [Test]
        public void SegmentsNotIntersecting4()
        {
            var segment1 = new Segment(1, 1, 2, 2);
            var segment2 = new Segment(2, 2, 1, 1);
            Assert.AreEqual(false, segment1.IsIntersect(segment2));
        }

        [Test]
        public void SegmentsNotIntersecting1()
        {
            var segment1 = new Segment(-1, 1, -1, 2);
            var segment2 = new Segment(1, 1, 2, 2);
            Assert.AreEqual(false, segment1.IsIntersect(segment2));
        }
        [Test]
        public void SegmentsNotIntersecting2()
        {
            var segment1 = new Segment(-2, -1, -2, 3);
            var segment2 = new Segment(0, -1, 0, 3);
            Assert.AreEqual(false, segment1.IsIntersect(segment2));
        }
        [Test]
        public void SegmentsNotIntersecting3()
        {
            var segment1 = new Segment(-2, 0, 0, 2);
            var segment2 = new Segment(0, -2, 1, 1);
            Assert.AreEqual(false, segment1.IsIntersect(segment2));
        }
    }
}
