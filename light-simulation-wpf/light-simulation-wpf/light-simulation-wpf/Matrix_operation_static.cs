using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace light_simulation_wpf
{
    static class Matrix_operation_static
    {
        //multiplication
        public static Vector matrix_times_vector(Matrix m, Vector v)
        {
            Vector result = new Vector();
            result['x'] = m[1, 1] * v['x'] + m[1, 2] * v['y'];
            result['y'] = m[2, 1] * v['x'] + m[2, 2] * v['y'];

            return result;
        }

        //multiplication
        public static double dot_product_2d(Vector a, Vector b)
        {
            return a['x'] * b['x'] + a['y'] * b['y'];
        }

        //square
        public static double vector_squared_2d(Vector a)
        {
            return Math.Pow(a['x'], 2) + Math.Pow(a['y'], 2);
        }

        //sum
        public static Vector sum_of_two_vectors(Vector a,Vector b)
        {
            Vector sum = new Vector();
            sum['x'] = a['x'] + b['x'];
            sum['y'] = a['y'] + b['y'];
            return sum;
        }

        //would be same as addition with a - sign
        //first - second
        public static Vector subs_of_two_vectors(Vector a,Vector b)
        {
            Vector result = new Vector();
            result['x'] = a['x'] - b['x'];
            result['y'] = a['y'] - b['y'];
            return result;
        }


        //num times vector
        public static Vector num_times_vector(Vector a, double num)
        {
            Vector v = new Vector();
            v['x'] = num * a['x'];
            v['y'] = num * a['y'];

            return v;
        }

        //vector from a --> b points
        public static Vector from_point_a_to_b(Point a, Point b)
        {
            Vector retVect = new Vector();
            retVect['x'] = b.X - a.X;
            retVect['Y'] = b.Y - a.Y;
            return retVect;
        }

        //distance of two points
        public static double get_distance_of_points(Point a,Point b)
        {
            return Math.Sqrt(vector_squared_2d(from_point_a_to_b(a,b)));
        }

        public static double get_small_angle_of_vectors(Vector a, Vector b)
        {
            double angle = Math.Acos(dot_product_2d(a, b) / (Math.Sqrt(vector_squared_2d(a)) * Math.Sqrt(vector_squared_2d(b))));
            if (angle >= 3.14 - angle)
                return 3.14 - angle;
            else
                return angle;

        }

        public static double get_angle_of_vectors(Vector a, Vector b)
        {
            double angle = Math.Acos(dot_product_2d(a, b) / (Math.Sqrt(vector_squared_2d(a)) * Math.Sqrt(vector_squared_2d(b))));
            
                return angle;
        }


    }
}
