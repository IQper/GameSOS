using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSOS
{
    class Map
    {
        public HashSet<Platform> Platforms;
        public HashSet<Coin> Coins;
        public HashSet<Particle> Particles = new HashSet<Particle>();

        public Map()
        {
            Platforms = new HashSet<Platform>();
            Coins = new HashSet<Coin>();

            Platforms.Add(new Platform(new PointF(600, 400), 500, 20)); // start platform

            var rnd = new Random();
            for (var i = 0; i <= 1000; i++)
            {
                var pos = new PointF(1200 + i * 600, rnd.Next(-100, 100) + 400);
                for (var j = 0; j <= rnd.Next(3); j++)
                {
                    var iteratedPos = new PointF(pos.X + rnd.Next(-40, 40), pos.Y + j * rnd.Next(300, 500) - 500);
                    var coinSize = rnd.Next(10, 40);
                    Coins.Add(new Coin(new PointF(iteratedPos.X, iteratedPos.Y - coinSize * 2 - 10), coinSize, Coins.Count));
                    Platforms.Add(new Platform(iteratedPos, rnd.Next(200, 500), 20));
                }
            }
        }

        public void GenerateParticles(PointF pos, float vel, int count, int maxLifetime, float size, int minAng, int maxAng, Color color, float gravity = 0)
        {
            var rnd = new Random(Particles.Count);
            for (var i = 0; i < count; i++)
            {
                //var v = (rnd.NextDouble() * 0.2) * vel + vel
                Particles.Add(new Particle(pos,
                    size,
                    maxLifetime,
                    rnd.Next(3, 5),
                    rnd.Next(1, 100) / 10,
                    rnd.Next(0, 3),
                    (float)Math.Cos(rnd.Next(minAng, maxAng) * Math.PI / 180) * rnd.Next((int)(vel / 4), (int)vel * 2),
                    (float)Math.Sin(rnd.Next(minAng, maxAng) * Math.PI / 180) * rnd.Next((int)(vel / 4), (int)vel * 2),
                    color, gravity));
            }
        }
    }
}
