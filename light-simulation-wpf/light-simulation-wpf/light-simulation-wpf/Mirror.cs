using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace light_simulation_wpf
{
    
    class Mirror
    {
         public Point Start_p { set; get; }
         public Point End_p { set; get; }
        
        public Mirror(Point a, Point b)
        {
            Start_p = a;
            End_p = b;
        }
    }
}
