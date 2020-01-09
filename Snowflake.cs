using System.Drawing;

namespace SnowOverlay
{
    public class Snowflake
    {
        public Point Location { get; set; }
        public Size Size { get; private set; }
        public int Speed { get; private set; }

        public Snowflake(Point location, int size, int speed = 1)
        {
            Location = location;
            Size = new Size(size, size);
            Speed = speed;
        }

        public void Fall()
        {
            Location = new Point(Location.X, Location.Y + Speed);
        }
    }
}
