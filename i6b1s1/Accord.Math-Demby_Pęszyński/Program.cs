using Accord.Math;
using Accord.Math.Optimization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Accord.Math;
namespace TestBibliotekiOptymalizacyjnej
{
    class Program
    {
        static void Main(string[] args)
        {
            // Suppose we would like to fit a circle to the given points.
            double[][] inputs = new double[][]
            {
    new []{1.0, 7.0},
    new []{2.0, 6.0},
    new []{5.0, 8.0},
    new []{7.0, 7.0},
    new []{9.0, 5.0},
    new []{3.0, 7.0}
            };

            // In a least squares sense, we want the distance between a point and
            // the ideal circle center minus the radius to be zero.
            double[] outputs =  Vector.Zeros(inputs.GetColumn(0).Length);

            // Setup the solver with the Regression and Gradient functions and an
            // initial solution guess. We'll solve for 3 parameters: the circle
            // center and the radius.
            LevenbergMarquardt lm = new LevenbergMarquardt(3)
            {
                Function = (w, k) =>
                {
                    double dx = w[0] - k[0];
                    double dy = w[1] - k[1];
                    double dr = Math.Sqrt(dx * dx + dy * dy) - w[2];
                    return dr;
                },

                Gradient = (w, k, m) =>
                {
                    double dx = w[0] - k[0];
                    double dy = w[1] - k[1];
                    double di = Math.Sqrt(dx * dx + dy * dy);
                    m[0] = dx / di;
                    m[1] = dy / di;
                    m[2] = -1;
                },

                Solution = new double[] { 5.3794, 7.2532, 3.037 }
            };

            // Iteratively solve for the optimal solution according to some
            // convergence criteria
            double error = double.MaxValue;
            for (int i = 0; i < 50; i++)
            {
                lm.Minimize(inputs, outputs);
                if (lm.Value < error && error - lm.Value < 1.0e-12)
                {
                    break;
                }
                error = lm.Value;
            }
            double x = lm.Solution[0]; // 4.7398
            double y = lm.Solution[1]; // 2.9835
            double r = lm.Solution[2]; // 4.7142
            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(r);
            Console.ReadLine();
        }

    }
}
