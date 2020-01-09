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
        private static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        private readonly Color DEFAULT_SNOWFLAKE_COLOR = Color.PaleTurquoise;
        private const int DEFAULT_TIMER_INTERVAL = 100;

        private List<Snowflake> snowflake = new List<Snowflake>();

        private readonly Random rng = new Random();
               
        public Form1()
        {
            InitializeComponent();
            tmrTick.Interval = DEFAULT_TIMER_INTERVAL;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tmrTick.Start();
        }

        private void TmrTick_Tick(object sender, EventArgs e)
        {
            var desktopPtr = GetDC(IntPtr.Zero);
            using (var g = Graphics.FromHdc(desktopPtr))
            {
                snowflake.Add(new Snowflake(new Point(rng.Next(0, Screen.PrimaryScreen.WorkingArea.Width), 0), rng.Next(5, 20), rng.Next(1, 6)));
                snowflake.ForEach(s => g.FillEllipse(new SolidBrush(Color.Transparent), new Rectangle(s.Location, s.Size)));
                snowflake.ForEach(s =>
                {
                    if (s.Location.Y < Screen.PrimaryScreen.WorkingArea.Height)
                    {
                        s.Fall();
                    }
                });
                snowflake.ForEach(s => g.FillEllipse(new SolidBrush(DEFAULT_SNOWFLAKE_COLOR), new Rectangle(s.Location, s.Size)));
            }
            ReleaseDC(IntPtr.Zero, desktopPtr);
        }
    }
}
