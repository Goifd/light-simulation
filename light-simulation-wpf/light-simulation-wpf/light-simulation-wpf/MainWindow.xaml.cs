using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace light_simulation_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ellipse light_ellipse = new Ellipse();
        Light light = new Light();

        bool mirror_create;
        bool light_create;
        bool curved_mirror_create;
        bool lense_create;

        double index_of_refraction = 1;
        double index_of_refraction_lense = 1.6;

        Point s_Point_1;
        Point e_Point_1;
        Point s_Point_2;
        Point e_Point_2;
        double radius_1;
        double radius_2;

        int no_double_hit = 0;

        List<Line> lines = new List<Line>();
        List<Mirror> mirrors = new List<Mirror>();

        List<Curved_mirror> curved_mirrors = new List<Curved_mirror>();
        List<Path> curv_path = new List<Path>();

        List<Lense> lenses = new List<Lense>();
        List<Path> lense_path = new List<Path>();

        DispatcherTimer timer = new DispatcherTimer();
        
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 3);
            timer.Tick += timer1_Tick;
        }

        //Create button action
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if(cmbSelectItem.Text == "Mirror")
                mirror_create = true;

            if (cmbSelectItem.Text == "Light beme" && light.l_vect == null)
                light_create = true;

            if (cmbSelectItem.Text == "Curved Mirror")
                curved_mirror_create = true;

            if (cmbSelectItem.Text == "Lense")
                lense_create = true;


        }

        //Create by data
        private void btn_byValue_Click(object sender, RoutedEventArgs e)
        {
            if (mirror_create || light_create || curved_mirror_create || lense_create)
            {
                if (mirror_create)
                {
                    set_data(ref s_Point_1, txtSP_1.Text);
                    set_data(ref e_Point_1, txtEP_1.Text);
                    create_mirror();
                }
                else if (light_create)
                {
                    set_data(ref s_Point_1, txtSP_1.Text);
                    set_data(ref e_Point_1, txtEP_1.Text);

                    light_ellipse.Height = 15;
                    light_ellipse.Width = 15;
                    light_ellipse.Fill = Brushes.PaleGoldenrod;
                    light_ellipse.Stroke = Brushes.Goldenrod;
                    light_ellipse.StrokeThickness = 4;
                    Canvas.SetLeft(light_ellipse, s_Point_1.X);
                    Canvas.SetTop(light_ellipse, s_Point_1.Y);

                    cnv_display.Children.Add(light_ellipse);
                    
                    Vector v = new Vector();
                    v['x'] = e_Point_1.X;
                    v['y'] = e_Point_1.Y;

                    light.l_vect = light.vect_resizer(v);
                    timer.Start();
                }
                else if (curved_mirror_create)
                {
                    set_data(ref s_Point_1, txtSP_1.Text);
                    set_data(ref e_Point_1, txtEP_1.Text);
                    set_data(ref radius_1, txtR_1.Text);
                    create_curved_mirror();
                }

                else if(lense_create)
                {
                    set_data(ref s_Point_1, txtSP_1.Text);
                    set_data(ref e_Point_1, txtEP_1.Text);
                    set_data(ref s_Point_2, txtSP_2.Text);
                    set_data(ref e_Point_2, txtEP_2.Text);
                    set_data(ref radius_1, txtR_1.Text);
                    set_data(ref radius_2, txtR_2.Text);
                    create_lense();
                }
            }
        }
        
        //Create with mouse
        private void cnv_display_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mirror_create || light_create || curved_mirror_create)
                s_Point_1 = Mouse.GetPosition(cnv_display);
        }

        private void cnv_display_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mirror_create || light_create || curved_mirror_create)
            {
                e_Point_1 = Mouse.GetPosition(cnv_display);

                if(mirror_create)
                    create_mirror();


                else if(light_create)
                {
                    //set the ellipse's properties
                    light_ellipse.Height = 15;
                    light_ellipse.Width = 15;
                    light_ellipse.Fill = Brushes.PaleGoldenrod;
                    light_ellipse.Stroke = Brushes.Goldenrod;
                    light_ellipse.StrokeThickness = 4;
                    Canvas.SetLeft(light_ellipse, s_Point_1.X - 7);
                    Canvas.SetTop(light_ellipse, s_Point_1.Y - 7);

                    //display the ellipse/light and start timer1
                    cnv_display.Children.Add(light_ellipse);
                    Vector v = new Vector();
                    v['x'] = e_Point_1.X - s_Point_1.X;
                    v['y'] = e_Point_1.Y - s_Point_1.Y;

                    light.l_vect = light.vect_resizer(v);
                    light_create = false;
                    timer.Start();
                }

                else if(curved_mirror_create)
                {
                    set_data(ref radius_1, txtR_1.Text);
                    create_curved_mirror();  
                }
                    
                
                
            }
        }

        //timer1's tick event
        private void timer1_Tick(object sender, EventArgs e)
        {
            

            //check for hit
            Point pt = new Point();
            pt.X = Canvas.GetLeft(light_ellipse) + 7;
            pt.Y = Canvas.GetTop(light_ellipse) + 7;

            Ellipse new_e = new Ellipse();
            new_e.Width = 5;
            new_e.Height = 5;
            Canvas.SetLeft(new_e, pt.X  - 2);
            Canvas.SetTop(new_e, pt.Y  - 2);
            new_e.Fill = Brushes.Yellow;
            cnv_display.Children.Add(new_e);
            

            redraw_light();
               
            if(no_double_hit < 10 && no_double_hit > 0)
                no_double_hit++;
            else
            {
                VisualTreeHelper.HitTest(cnv_display, null, new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(pt));
            }
            
        }

        //On hit action
        public HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {
            timer.Stop();

            if (no_double_hit   == 10)
                no_double_hit = 0;

            if (result.VisualHit.GetType() != typeof(Ellipse) && result.VisualHit.GetType() != typeof(Canvas) && no_double_hit == 0) 
            {

                //check if it is a line
                if(result.VisualHit.GetType() == typeof(Line))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                         if(result.VisualHit.Equals(lines[i]))
                         {
                            light.l_vect = light.vect_resizer(mirror_vect_to_line(light.l_vect, mirrors[i].Start_p, mirrors[i].End_p));
                            break;
                         }
                    }
                }

                bool isThereBreak = false;
                //check if it is a path
                if (result.VisualHit.GetType() == typeof(Path))
                {
                    //check if it is a curved mirror
                    for (int i = 0; i < curv_path.Count; i++)
                    {
                        if (result.VisualHit.Equals(curv_path[i]))
                        {
                            Point hitPoint = new Point();
                            hitPoint.X = Canvas.GetLeft(light_ellipse) + 7;
                            hitPoint.Y = Canvas.GetTop(light_ellipse) + 7;

                            Vector tangent = get_tangent_of_curvedM(curved_mirrors[i].End_p,curved_mirrors[i].Start_p, curved_mirrors[i].Radius, hitPoint);
                            light.l_vect = light.vect_resizer(mirror_vect_to_line(light.l_vect, tangent));
                            isThereBreak = true;
                            break;
                            
                        }
                    }
                    //check if it is a lense
                    if(!isThereBreak)
                    {
                        for (int i = 0; i < lense_path.Count; i++)
                        {
                            if (result.VisualHit.Equals(lense_path[i]))
                            {
                            Point hitPoint = new Point();
                            hitPoint.X = Canvas.GetLeft(light_ellipse) + 7;
                            hitPoint.Y = Canvas.GetTop(light_ellipse) + 7;

                                Point s1 = lenses[i / 4].S_1;
                                Point s2 = lenses[i / 4].S_2;
                                Point e1 = lenses[i / 4].E_1;
                                Point e2 = lenses[i / 4].E_2;
                                double r1 = lenses[i / 4].R_1;
                                double r2 = lenses[i / 4].R_2;

                                

                                if(i % 4 == 0)
                                {
                                    Console.WriteLine("line 1 was hit");
                                    if(lenses[i / 4].Lense_hit % 2 == 0)
                                        light.l_vect = light.vect_resizer(snell_law(light.l_vect, new Vector { ['x'] = s2.X - s1.X, ['y'] = s2.Y - s1.Y },index_of_refraction,index_of_refraction_lense));
                                    else
                                        light.l_vect = light.vect_resizer(snell_law(light.l_vect, new Vector { ['x'] = s2.X - s1.X, ['y'] = s2.Y - s1.Y }, index_of_refraction_lense, index_of_refraction));

                                }
                                if (i % 4 == 1)
                                {
                                    if (lenses[i / 4].Lense_hit % 2 == 0)
                                        light.l_vect = light.vect_resizer(snell_law(light.l_vect, new Vector { ['x'] = e1.X - e2.X, ['y'] = e1.Y - e2.Y }, index_of_refraction, index_of_refraction_lense));
                                    else
                                    {
                                        light.l_vect = light.vect_resizer(snell_law(light.l_vect, new Vector { ['x'] = e1.X - e2.X, ['y'] = e1.Y - e2.Y }, index_of_refraction_lense, index_of_refraction));
                                    }
                                        
                                    Console.WriteLine("line 2 was hit");
                                }
                                if (i % 4 == 2)
                                {
                                    if (lenses[i / 4].Lense_hit % 2 == 0)
                                    {
                                        light.l_vect = light.vect_resizer(snell_law_for_curved(light.l_vect,get_tangent_of_curvedM(s1, e1, r1, hitPoint), index_of_refraction, index_of_refraction_lense));
                                    }
                                        
                                    else
                                        light.l_vect = light.vect_resizer(snell_law_for_curved(light.l_vect, get_tangent_of_curvedM(e1, s1, r1, hitPoint), index_of_refraction, index_of_refraction_lense ));
                                }
                                if (i % 4 == 3)
                                {
                                    if (lenses[i / 4].Lense_hit % 2 == 0)
                                        light.l_vect = light.vect_resizer(snell_law(light.l_vect, get_tangent_of_curvedM(e2, s2, r2, hitPoint), index_of_refraction, index_of_refraction_lense));
                                    else
                                    {
                                        light.l_vect = light.vect_resizer(snell_law(light.l_vect, get_tangent_of_curvedM(e2, s2, r2, hitPoint), index_of_refraction_lense, index_of_refraction));
                                    }
                                }

                                lenses[i / 4].Lense_hit++;

                                break;
                            }
                        }

                    }
                }

                no_double_hit++;  
            }
            
            timer.Start();
            return HitTestResultBehavior.Continue;
        }

        //the vector mirrored operator owerloaling
        Vector mirror_vect_to_line(Vector vect, Point s_point, Point e_point)
        {

            //reflected vector to be returned
            Vector to_return = new Vector();

            Vector v = new Vector();
            v['x'] = s_point.X - e_point.X;
            v['y'] = s_point.Y - e_point.Y;

            double num = 2 * Matrix_operation_static.dot_product_2d(vect, v) / Matrix_operation_static.vector_squared_2d(v);
            to_return = Matrix_operation_static.subs_of_two_vectors(Matrix_operation_static.num_times_vector(v,num),vect);

            return to_return;

        }

        Vector mirror_vect_to_line(Vector vect, Vector tangent)
        {

            //reflected vector to be returned
            Vector to_return = new Vector();

            Vector v = tangent;
            

            double num = 2 * Matrix_operation_static.dot_product_2d(vect, v) / Matrix_operation_static.vector_squared_2d(v);
            to_return = Matrix_operation_static.subs_of_two_vectors(Matrix_operation_static.num_times_vector(v,num), vect);

            return to_return;

        }

        //get tangent vect
        Vector get_tangent_of_curvedM(Point a, Point b, double r,Point hit)
        {
            Vector result = new Vector();
            Point kozeppont = new Point();


            //kozeppont fele mutato vektor
            Vector x = new Vector();
            x['x'] =  (b.Y - a.Y);
            x['y'] = -1 *(b.X - a.X);

            //tavolsag
            double d = Math.Sqrt(Math.Pow(a.X-b.X,2) + Math.Pow(a.Y - b.Y, 2));
            
            //felezopont
            Point felezo = new Point();
            felezo.X = (a.X + b.X) / 2;
            felezo.Y = (a.Y + b.Y) / 2;

            double egyutthato = Math.Cos(Math.Asin(d / (2 * r))) * r / d;
            Console.WriteLine(d / (2 * r));
            Console.WriteLine(egyutthato);

            //kor kozeppontja 
           
            kozeppont.X = felezo.X + (egyutthato * x['x']);
            kozeppont.Y = felezo.Y + (egyutthato * x['y']);
            result['x'] = (kozeppont.Y - hit.Y);
            result['y'] = -1 * (kozeppont.X - hit.X);

            return result;
        }


        private void create_mirror()
        {
            //create the line
            Line new_line = new Line()
            {
                X1 = s_Point_1.X,
                Y1 = s_Point_1.Y,
                X2 = e_Point_1.X,
                Y2 = e_Point_1.Y,
                Stroke = Brushes.Plum,
                StrokeThickness = 4,
            };
            //display the line to the canvas
            cnv_display.Children.Add(new_line);
            //add the line to the lines List
            lines.Add(new_line);
            //add the line to the mirrors List
            mirrors.Add(new Mirror(s_Point_1, e_Point_1));

            mirror_create = false;
        }

        private void create_curved_mirror()
        {
            
            var g = new StreamGeometry();

            using (var gc = g.Open())
            {
                gc.BeginFigure(
                    startPoint: s_Point_1,
                    isFilled: false,
                    isClosed: false);

                gc.ArcTo(
                    point: e_Point_1,
                    size: new Size(radius_1, radius_1),
                    rotationAngle: 0d,
                    isLargeArc: false,


                    sweepDirection: SweepDirection.Clockwise,



                    isStroked: true,
                    isSmoothJoin: false);
            }

            var path = new Path
            {
                Stroke = Brushes.LightGreen,
                StrokeThickness = 4,
                Data = g
            };

            cnv_display.Children.Add(path);
            Curved_mirror new_mirr = new Curved_mirror(s_Point_1, e_Point_1, radius_1);
            curved_mirrors.Add(new_mirr);
            curv_path.Add(path);
            curved_mirror_create = false;
        }

        private void redraw_light()
        {
            double left = Canvas.GetLeft(light_ellipse);
            double top = Canvas.GetTop(light_ellipse);
            left += light.l_vect['x'];
            top += light.l_vect['y'];
            Canvas.SetLeft(light_ellipse, left);
            Canvas.SetTop(light_ellipse, top);
        }

        private void create_lense()
        {
           


            SweepDirection sweepDirection1 = SweepDirection.Clockwise;
            SweepDirection sweepDirection2 = SweepDirection.Counterclockwise;
            switch (cmbSelectLense.Text)
            {
                case "Homoru":
                    sweepDirection1 = SweepDirection.Clockwise;
                    sweepDirection2 = SweepDirection.Counterclockwise;
                    break;
                case "Domboru":
                    sweepDirection1 = SweepDirection.Counterclockwise;
                    sweepDirection2 = SweepDirection.Clockwise;
                    break;           
            }

            Path lense_line_1 = new Path();
            lense_line_1.Stroke = Brushes.LightCoral;
            lense_line_1.StrokeThickness = 4;

            Path lense_line_2 = new Path();
            lense_line_2.Stroke = Brushes.LightCoral;
            lense_line_2.StrokeThickness = 4;

            Path lense_curve_1 = new Path();
            lense_curve_1.Stroke = Brushes.LightCoral;
            lense_curve_1.StrokeThickness = 4;

            Path lense_curve_2 = new Path();
            lense_curve_2.Stroke = Brushes.LightCoral;
            lense_curve_2.StrokeThickness = 4;


            LineGeometry line_1 = new LineGeometry();
            line_1.StartPoint = s_Point_2;
            line_1.EndPoint = s_Point_1;

            LineGeometry line_2 = new LineGeometry();
            line_2.StartPoint = e_Point_2;
            line_2.EndPoint = e_Point_1;

            var curve_1 = new StreamGeometry();
            using (var gc = curve_1.Open())
            {
                gc.BeginFigure(
                    startPoint: s_Point_1,
                    isFilled: false,
                    isClosed: false);

                gc.ArcTo(
                    point: e_Point_1,
                    size: new Size(radius_1, radius_1),
                    rotationAngle: 0d,
                    isLargeArc: false,
                    sweepDirection: sweepDirection1,
                    isStroked: true,
                    isSmoothJoin: false);
            }

            var curve_2 = new StreamGeometry();
            using (var fc = curve_2.Open())
            {
                fc.BeginFigure(
                    startPoint: s_Point_2,
                    isFilled: false,
                    isClosed: false);

                fc.ArcTo(
                    point: e_Point_2,
                    size: new Size(radius_2, radius_2),
                    rotationAngle: 0d,
                    isLargeArc: false,
                    sweepDirection: sweepDirection2,
                    isStroked: true,
                    isSmoothJoin: false);
            }

            lense_line_1.Data = line_1;
            lense_line_2.Data = line_2;
            lense_curve_1.Data = curve_1;
            lense_curve_2.Data = curve_2;

            // Add path shape to the UI.
            cnv_display.Children.Add(lense_line_1);
            cnv_display.Children.Add(lense_line_2);
            cnv_display.Children.Add(lense_curve_1);
            cnv_display.Children.Add(lense_curve_2);

            Lense new_lense = new Lense(s_Point_1,s_Point_2,e_Point_1,e_Point_2,radius_1,radius_2);

            lenses.Add(new_lense);
            lense_path.Add(lense_line_1);
            lense_path.Add(lense_line_2);
            lense_path.Add(lense_curve_1);
            lense_path.Add(lense_curve_2);

            lense_create = false;
        }

        private Vector snell_law(Vector light, Vector surface, double n_from, double n_to)
        {
            Vector ret_vect = new Vector();
            Vector axis = new Vector();
            axis['x'] = surface['y'];
            axis['y'] = -1 * surface['x'];


            double angle = Matrix_operation_static.get_small_angle_of_vectors(light, axis);
            Console.WriteLine(angle);
            double interm = Math.Sin(angle);
            double interm2 = n_from / n_to;
            double angle_ = Math.Asin(Math.Sin(angle) * n_from / n_to);

            

            double angle_to_rotate = angle - angle_;
            Console.WriteLine(angle_to_rotate);

            //clockwise rotation
            Matrix clock_matrix = new Matrix();
            clock_matrix[1, 1] = Math.Cos(angle_to_rotate);
            clock_matrix[1, 2] = Math.Sin(angle_to_rotate);
            clock_matrix[2, 1] = -1 * Math.Sin(angle_to_rotate);
            clock_matrix[2, 2] = Math.Cos(angle_to_rotate);

            //counterclockwise rotation
            Matrix counterclkw = new Matrix();
            counterclkw[1, 1] = Math.Cos(angle_to_rotate);
            counterclkw[1, 2] = -1 * Math.Sin(angle_to_rotate);
            counterclkw[2,1] = Math.Sin(angle_to_rotate);
            counterclkw[2,2] = Math.Cos(angle_to_rotate);

            

            //check if it will refract
            if (angle_ >= Math.Asin(n_to / n_from) && n_from > n_to)
            {
                ret_vect = mirror_vect_to_line(light, surface);
                Console.WriteLine("hatarertek: " + Math.Asin(n_to / n_from));
            }
            else if(n_from > n_to)
            {
                if ((surface['x'] >= 0 && surface['y'] >= 0) || (surface['x'] <= 0 && surface['y'] >= 0))
                {

                    if (Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI / 2 || Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI)
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(counterclkw, light);
                    }
                    else
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(clock_matrix, light);
                    }


                }
                else
                {
                    if (Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI / 2 || Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI)
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(clock_matrix, light);
                        Console.WriteLine('a');

                    }
                    else
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(counterclkw, light);
                        Console.WriteLine('b');
                    }
                }
            }
            else
            {
                if((surface['x'] >= 0 && surface['y'] >= 0) || (surface['x'] <= 0 && surface['y'] >= 0))
                {
                    
                    if (Matrix_operation_static.get_angle_of_vectors(light,surface) > Math.PI / 2 || Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI)
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(clock_matrix, light);
                        

                    }
                    else
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(counterclkw, light);
                        
                    }
                

                }
                else
                {
                    if (Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI / 2 || Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI)
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(counterclkw, light);
                        Console.WriteLine('a');

                    }
                    else
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(clock_matrix, light);
                        Console.WriteLine('b');
                    }
                }
                
            }
 
            return ret_vect;
        }

        private Vector snell_law_for_curved(Vector light, Vector surface, double n_from, double n_to)
        {
            Vector ret_vect = new Vector();
            Vector axis = new Vector();
            axis['x'] = surface['y'];
            axis['y'] = -1 * surface['x'];


            double angle = Matrix_operation_static.get_small_angle_of_vectors(light, axis);
            Console.WriteLine(angle);
            double interm = Math.Sin(angle);
            double interm2 = n_from / n_to;
            double angle_ = Math.Asin(Math.Sin(angle) * n_from / n_to);



            double angle_to_rotate = angle - angle_;
            Console.WriteLine(angle_to_rotate);

            //clockwise rotation
            Matrix clock_matrix = new Matrix();
            clock_matrix[1, 1] = Math.Cos(angle_to_rotate);
            clock_matrix[1, 2] = Math.Sin(angle_to_rotate);
            clock_matrix[2, 1] = -1 * Math.Sin(angle_to_rotate);
            clock_matrix[2, 2] = Math.Cos(angle_to_rotate);

            //counterclockwise rotation
            Matrix counterclkw = new Matrix();
            counterclkw[1, 1] = Math.Cos(angle_to_rotate);
            counterclkw[1, 2] = -1 * Math.Sin(angle_to_rotate);
            counterclkw[2, 1] = Math.Sin(angle_to_rotate);
            counterclkw[2, 2] = Math.Cos(angle_to_rotate);



            //check if it will refract
            if (angle_ >= Math.Asin(n_to / n_from) && n_from > n_to)
            {
                ret_vect = mirror_vect_to_line(light, surface);
                Console.WriteLine("hatarertek: " + Math.Asin(n_to / n_from));
            }
            else if (n_from > n_to)
            {
                if ((surface['x'] >= 0 && surface['y'] >= 0) || (surface['x'] <= 0 && surface['y'] >= 0))
                {

                    if (Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI / 2 || Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI)
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(clock_matrix, light);
                    }
                    else
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(counterclkw, light);
                    }


                }
                else
                {
                    if (Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI / 2 || Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI)
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(counterclkw, light);
                        Console.WriteLine('a');

                    }
                    else
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(clock_matrix, light);
                        Console.WriteLine('b');
                    }
                }
            }
            else
            {
                if ((surface['x'] >= 0 && surface['y'] >= 0) || (surface['x'] <= 0 && surface['y'] >= 0))
                {

                    if (Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI / 2 || Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI)
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(counterclkw, light);


                    }
                    else
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(clock_matrix, light);

                    }


                }
                else
                {
                    if (Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI / 2 || Matrix_operation_static.get_angle_of_vectors(light, surface) > Math.PI)
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(clock_matrix, light);
                        Console.WriteLine('a');

                    }
                    else
                    {
                        ret_vect = Matrix_operation_static.matrix_times_vector(counterclkw, light);
                        Console.WriteLine('b');
                    }
                }

            }

            return ret_vect;
        }

        //set input data to a fields
        private void set_data(ref Point toSet, string input)
        {
            string[] arr = input.Split(',');
            toSet.X = Convert.ToDouble(arr[0]);
            toSet.Y = Convert.ToDouble(arr[1]);
        }
        private void set_data(ref double toSet, string input)
        {
            toSet = Convert.ToDouble(input);
        }
    }
}
