using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace light_simulation_wpf
{
    
    class Light
    {
        public Vector l_vect { set; get; }

        //resize the vector
        public Vector vect_resizer(Vector a)
        {
            Vector resized_vect = new Vector();
            
            if(Math.Abs(a['x']) >= Math.Abs(a['y']))
            {
                if(a['x'] > 0)
                    resized_vect['x'] = 1;
                
                else if(a['x'] != 0)
                    resized_vect['x'] =-1;
                
                else
                    resized_vect['x'] = 0;
                
                
                if(a['y'] > 0)
                    resized_vect['y'] = Math.Abs(a['y']) / Math.Abs(a['x']);
                
                else if(a['y'] != 0)
                    resized_vect['y'] = -1 * Math.Abs(a['y']) / Math.Abs(a['x']);
                
                else
                    resized_vect['y'] = 0; 
            }

            else
            {
                if (a['y'] > 0)
                    resized_vect['y'] = 1;
                
                else if (a['y'] != 0)
                    resized_vect['y'] = -1;
                
                else
                    resized_vect['y'] = 0;
                

                if (a['x'] > 0)
                    resized_vect['x'] = Math.Abs(a['x']) / Math.Abs(a['y']);
                
                else if (a['x'] != 0)
                    resized_vect['x'] = -1 * Math.Abs(a['x']) / Math.Abs(a['y']);
                
                else
                    resized_vect['x'] = 0;  
            }
            return resized_vect;
        }


    }
}
