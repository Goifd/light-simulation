using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace light_simulation_wpf
{
    class Matrix
    {

        //matrix: [a   b]   -  arr[0,0] arr[0,1] - [1,1] [1,2]
        //        [c   d]   -  arr[1,0] arr[1,1] - [2,1] [2,2]
        private double[,] arr;

        public Matrix()
        {
            arr = new double[2,2];
        }

        //Indexer of the matrix
        public double this[int row, int column]
        {
            get
            {
                if (row == 1)
                {
                    switch (column)
                    {
                        case 1:
                            return arr[0, 0];
                        case 2:
                            return arr[0, 1];
                        default:
                            return -1;
                    }

                }
                else if (row == 2)
                {
                    switch (column)
                    {
                        case 1:
                            return arr[1, 0];
                        case 2:
                            return arr[1, 1];
                        default:
                            return -1;
                    }

                }
                else
                {
                    return -1;
                }

            }

            set
            {
                if (row == 1)
                {
                    switch (column)
                    {
                        case 1:
                            arr[0, 0] = value;
                            break;
                        case 2:
                            arr[0, 1] = value;
                            break;
                    }

                }
                else if (row == 2)
                {
                    switch (column)
                    {
                        case 1:
                            arr[1, 0] = value;
                            break;
                        case 2:
                            arr[1, 1] = value;
                            break;
                    }

                }
            }
        }

    }
}
