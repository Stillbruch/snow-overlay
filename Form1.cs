using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private const int SNOW_ON_GROUND_HEIGHT = 10;

        private List<Snowflake> snowflake = new List<Snowflake>();

        private readonly Random rng = new Random();

        private int snowflakeOnGroundCounter = 0;
               
        public Form1()
        {
            InitializeComponent();

            BackColor = transparencyColor;
            TransparencyKey = transparencyColor;
            FormBorderStyle = FormBorderStyle.None;
            TopMost = true;

            tmrTick.Interval = DEFAULT_TIMER_INTERVAL;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // make form click-through
            SetWindowLong(Handle, -20, GetWindowLongPtr(Handle, -20) | 0x80000 | 0x20);
            // other instructions
            tmrTick.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            snowflake.ForEach(s => e.Graphics.FillEllipse(new SolidBrush(DEFAULT_SNOWFLAKE_COLOR), new Rectangle(s.Location, s.Size)));    
            if (snowflakeOnGroundCounter > 20)
            {
                e.Graphics.FillRectangle(new SolidBrush(DEFAULT_SNOWFLAKE_COLOR), new Rectangle(0, Size.Height - SNOW_ON_GROUND_HEIGHT, Size.Width, SNOW_ON_GROUND_HEIGHT));
            }
        }

        private void TmrTick_Tick(object sender, EventArgs e)
        {
            snowflake.Add(new Snowflake(new Point(rng.Next(0, Size.Width), 0), rng.Next(5, 20), rng.Next(1, 6)));
            snowflake.ForEach(s => 
            {
                if (s.Location.Y < Size.Height)
                {
                    s.Fall();
                }
                else
                {
                    snowflakeOnGroundCounter += 1;

                }
            });
            Debug.WriteLine(snowflake.Count);
            Invalidate();
        }
    }
}
