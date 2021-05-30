using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace light_simulation_wpf
{
    class Lense
    {
        public Point S_1 { set; get; }
        public Point S_2 { set; get; }
        public Point E_1 { set; get; }
        public Point E_2 { set; get; }
        public double R_1 { set; get; }
        public double R_2 { set; get; }

        public int Lense_hit { set; get; }

        public Lense(Point s1,Point s2,Point e1,Point e2, double r1, double r2)
        {
            S_1 = s1;
            S_2 = s2;
            E_1 = e1;
            E_2 = e2;
            R_1 = r1;
            R_2 = r2;
            Lense_hit = 0;
        }
            

    }
}
