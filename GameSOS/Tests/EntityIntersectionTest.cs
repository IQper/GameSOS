using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Drawing;
using NUnit.Framework.Internal;

namespace GameSOS
{
    [TestFixture]
    class EntityIntersectionTest
    {
        [Test]
        public void EntitiesIntersects()
        {
            var entity1 = new Platform(new PointF(100, 100), 100, 300);
            var entity2 = new Platform(new PointF(200, 100), 100, 400, Math.PI / 2);
            Assert.AreEqual(true, entity1.IsIntersect(entity2));
        }
        [Test]
        public void EntitiesNotIntersects()
        {
            var entity1 = new Platform(new PointF(100, 100), 200, 300);
            var entity2 = new Platform(new PointF(400, 100), 100, 400);
            Assert.AreEqual(false, entity1.IsIntersect(entity2));
        }
    }
}
