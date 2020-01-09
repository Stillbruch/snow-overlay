using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SnowOverlay
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLongPtr(IntPtr hWnd, int nIndex);

        private readonly Color transparencyColor = Color.Linen;
        private readonly Size DEFAULT_SIZE = new Size(20, 20);
        private readonly Color DEFAULT_SNOWFLAKE_COLOR = Color.PaleTurquoise;
        private const int DEFAULT_TIMER_INTERVAL = 50;
        private const int MAX_NUMBER_OF_SNOWFLAKES = 150;
        private List<Snowflake> snowflakes = new List<Snowflake>();

        private readonly Random rng = new Random();

        // new unfinished feature
        private int snowflakeOnGroundCounter = 0;
        private int SnowflakeOnGroundCounter
        {
            get
            {
                return snowflakeOnGroundCounter;
            }
            set
            {
                snowflakeOnGroundCounter = value;
                if (AddSnowOnGround != null)
                {
                    OnPropertyChange(value);
                }
            }
        }
        private void OnPropertyChange(int e) => AddSnowOnGround?.Invoke(this, e);
        private event EventHandler<int> AddSnowOnGround;
        private const int SNOW_ON_GROUND_HEIGHT = 10;

        public Form1()
        {
            InitializeComponent();

            Text = "Snow Overlay";
            BackColor = transparencyColor;
            TransparencyKey = transparencyColor;
            FormBorderStyle = FormBorderStyle.None;
            TopMost = true;
            Location = new Point(0, 0);
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            tmrTick.Interval = DEFAULT_TIMER_INTERVAL;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            // make form click-through
            SetWindowLong(Handle, -20, GetWindowLongPtr(Handle, -20) | 0x80000 | 0x20);
            // other instructions
            tmrTick.Start();
            Debug.WriteLine($"{Size.Width} {Size.Height}");
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            snowflakes.ForEach(s => e.Graphics.FillEllipse(new SolidBrush(DEFAULT_SNOWFLAKE_COLOR), new Rectangle(s.Location, s.Size)));    
        }

        private void TmrTick_Tick(object sender, EventArgs e)
        {
            if (snowflakes.Count < MAX_NUMBER_OF_SNOWFLAKES)
            {
                snowflakes.Add(new Snowflake(new Point(rng.Next(0, Size.Width), 0), rng.Next(5, 20), rng.Next(1, 6)));
            }
            snowflakes.ForEach(s => s.Fall());
            snowflakes.RemoveAll(s => s.Location.Y >= Size.Height);
            Invalidate();
        }
    }
}
