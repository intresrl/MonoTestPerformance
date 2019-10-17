using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace TestPerformance
{
    public partial class Form1 : Form
    {
        private readonly int _forms;
        TestForm tf = null;
        int i = 0;
        System.Diagnostics.Stopwatch watch = null;
        public Form1(int forms)
        {
            _forms = forms;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            showForm();
            watch = System.Diagnostics.Stopwatch.StartNew();
        }

        private void Tf_Shown(object sender, EventArgs e)
        {
            tf.Close();
            Timer t = new Timer();
            t.Interval = 1;
            t.Tick += (o, args) =>
            {
                t.Stop();
                showForm();
            }; 
            t.Start();
        }
        private void showForm()
        {
            if (i == _forms)
            {
                watch.Stop();
                Console.WriteLine("Elapsed: " + watch.ElapsedMilliseconds);
                Console.Read();
                this.Close();
            }
            else
            {
                tf = new TestForm();
                tf.Shown += Tf_Shown;
                tf.Show();
                Console.WriteLine("w: " + i);
                i++;
            }
        }
    }
}
