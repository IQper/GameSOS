using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using GameSOS.Structures;
using GameSOS.Entities;

namespace GameSOS
{
    public partial class GameField : Form
    {
        public readonly string WindowTitle = "SexGame";
        private List<PointF> IntersectionPoints = new List<PointF>();

        private int score = 0;
        private int lives = 2;
        private int jumps = 0;

        private Force Force;
        private Map Map = new Map();
        private Platform lastPlatform;
        private Player player = new Player(new PointF(600, 200), 100, 250, new PointF(600, 200));
        private Mouse mouse = new Mouse();
        private Cam Cam = new Cam(0.7f);
        private Timer timer = new Timer();
      
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AutoSizeMode = AutoSizeMode.GrowAndShrink; // resize = false;
            Text = WindowTitle;
            DoubleBuffered = true;
        }
        public GameField()
        {
            InitializeComponent();
            timer.Enabled = true;
            timer.Interval = 25;
            timer.Tick += new EventHandler(Timer_tick);
            MouseDown += new MouseEventHandler(MouseDownEvent);
            MouseUp += new MouseEventHandler(MouseUpEvent);
            MouseMove += new MouseEventHandler(MouseMoveEvent);
            label2.Visible = false;
            button1.Visible = false;
            player.Gravity = 1;
        }

        private void GameField_Paint(object sender, PaintEventArgs e)
        {   
            if(lives >= 0)
            {
                var g = e.Graphics;

                g.ScaleTransform(Cam.X, Cam.Y);
                g.TranslateTransform(-player.Position.X + Width / (2 * Cam.X) - player.VelocityX / 10, -player.Position.Y + Height / (2 * Cam.Y) - player.VelocityY / 1);

                foreach (var x in Map.Platforms) x.Draw(g);
                foreach (var x in Map.Coins) x.Draw(g);
                if(Force != null) new Trajectory(Force.Value.X, Force.Value.Y, Force.Position).Draw(g);
                if(!player.NotDraw) player.Draw(g);
                foreach (var x in Map.Particles) x.Draw(g);
            }
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            Cam.RecalculateCam(new Vector2(player.VelocityX / 100, player.VelocityY / 100).Length());
            player.Angle = Math.Abs(player.VelocityY / 100 * - Math.Sign(player.VelocityX)) <= 0.011f ? 0 : player.VelocityY / 100 * -Math.Sign(player.VelocityX);
            var playerCheck = player.Copy();
            playerCheck.UpdatePosition();

            IntersectionPoints = new List<PointF>();

            var removeList = new List<Particle>();
            foreach (var particle in Map.Particles)
            {
                particle.AnimationStep();
                if (!particle.IsAlive) removeList.Add(particle);
            }
            foreach (var particle in removeList) Map.Particles.Remove(particle);

            foreach (var coin in Map.Coins)
            {
                coin.AnimationStep();
                if (player.IsIntersect(coin))
                {
                    score += (int)coin.Size;
                    Map.Coins.Remove(coin);
                    Map.GenerateParticles(coin.Position, 10, (int)coin.Size, 30, coin.Size / 5, 0, 360, Color.Orange);
                    break;
                }
            }
            foreach (var platform in Map.Platforms)
            {
                if (playerCheck.IsIntersect(platform))
                {
                    foreach (var point in platform.GetIntersectionPoint(playerCheck)) IntersectionPoints.Add(point);
                    if(player.IsJumpReady) lastPlatform = platform;
                }
            }
            if (IntersectionPoints.Count != 0)
            {
                player.VelocityY /= 5f + 1/player.VelocityY;
                player.VelocityX /= 1.3f;
                var averagePoint = new PointF();
                foreach(var point in IntersectionPoints)
                {
                    averagePoint.X += point.X;
                    averagePoint.Y += point.Y;
                }
                averagePoint = new PointF(averagePoint.X / IntersectionPoints.Count, averagePoint.Y / IntersectionPoints.Count);
                Map.GenerateParticles(
                    averagePoint,
                    new Vector2(player.VelocityY, player.VelocityX).Length() / 2,
                    (int)(new Vector2(player.VelocityY, player.VelocityX).Length()),
                    40,
                    4f,
                    (int)(Math.Atan2(player.VelocityY, player.VelocityX) / 0.0175) - 10,
                    (int)(Math.Atan2(player.VelocityY, player.VelocityX) / 0.0175) + 10,
                    Color.Gray, 1);
                var angle = Math.Atan2(averagePoint.Y - playerCheck.Position.Y, averagePoint.X - playerCheck.Position.X);
                if ((angle > -Math.PI * 3 / 4 && angle <= -Math.PI / 4) || (angle > Math.PI / 4 && angle <= Math.PI * 3 / 4))
                {
                    player.VelocityY *= -1;
                    if (Math.Abs(player.VelocityY) <= player.velocityWhenStay) player.VelocityY = 0;
                }
                if ((angle > -Math.PI / 4 && angle <= Math.PI / 4) || (angle > Math.PI * 3 / 4 || angle <= -Math.PI * 3 / 4)) player.VelocityX *= -1;

                player.IsJumpReady = Math.Abs(player.VelocityY) <= player.velocityWhenStay && Math.Abs(player.VelocityX) <= player.velocityWhenStay;
            }
            if(player.Position.Y >= 1800)
            {
                                                           // pos         vel  cnt life size 
                if(!player.NotDraw) Map.GenerateParticles(player.Position, 80, 80, 35, 15f,
                    (int)(Math.Atan2(player.VelocityY, player.VelocityX) / 0.0175) - 20,
                    (int)(Math.Atan2(player.VelocityY, player.VelocityX) / 0.0175) + 20, 
                    Player.BrushColor);
                player.NotDraw = true;
                timer1.Start();
            }
            player.UpdatePosition();

            if (player.IsJumpReady && mouse.isDown)
            {
                Force = new Force(player.Position, 0, 0);
                var rad = player.JumpSpeedLimit;

                var x = -mouse.Location.X + player.Position.X;
                var y = -mouse.Location.Y + player.Position.Y;
                var angle = Math.Atan2(y, x);
                if (x * x + y * y > rad * rad)
                {
                    x = (float)Math.Cos(angle) * rad;
                    y = (float)Math.Sin(angle) * rad;
                }
                Force.Value = new Vector2(x / 6, y / 6);
                Force.Position = player.Position;
            }
            else if (Force != null)
            {
                player.IsJumpReady = false;
                player.VelocityX = Force.Value.X;
                player.VelocityY = Force.Value.Y;
                Force = null;
                jumps += 1;
            }

            label1.Text =
                $"Lives = {lives}\n" +
                $"Score = {score}\n" +
                $"Jumps = {jumps}";

            Refresh();
        }

        private void MouseUpEvent(object sender, MouseEventArgs e)
        {
            mouse.isDown = false;
        }
        private void MouseDownEvent(object sender, MouseEventArgs e)
        {
            mouse.isDown = true;
        }
        private void MouseMoveEvent(object sender, MouseEventArgs e)
        {
            mouse.Location.X = e.Location.X / Cam.X + player.Position.X - Width / (2 * Cam.X);
            mouse.Location.Y = e.Location.Y / Cam.Y + player.Position.Y - Height / (2 * Cam.Y);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label2.Visible = false;
            label1.Visible = true;
            button1.Visible = false;
            lives = 2;
            score = 0;
            jumps = 0;
            Map = new Map();
            player.Position = player.StartPos;
            player.VelocityX = 0; 
            player.VelocityY = 0;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            player.NotDraw = false;
            timer1.Stop();
            player.VelocityY = 0;
            player.VelocityX = 0;
            player.Position = new PointF(lastPlatform.Position.X, lastPlatform.Position.Y - player.Height - 10);
            if (lives > 0) lives -= 1;
            else
            {
                lives -= 1;
                label2.Text =
                    $"Game over\n" +
                    $"Jumps = {jumps}\n" +
                    $"Total Score = {score}";
                label2.Visible = true;
                label1.Visible = false;
                button1.Visible = true;
            }
        }
    }
}
