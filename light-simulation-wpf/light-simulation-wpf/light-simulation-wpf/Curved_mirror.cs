using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace light_simulation_wpf
{
    class Curved_mirror : Mirror
    {
        public double Radius { get; set; }
        public Curved_mirror(Point a, Point b, double r):base(a,b)
        {
            Radius = r;
        }
    }
}
