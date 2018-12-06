using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aoc.Puzzles;

namespace Aoc
{
    /// <summary>
    ///               \ /
    ///             --;*;--
    ///               /o\
    ///              /_\_\
    ///             /_/_0_\
    ///            /_o_\_\_\
    ///           /_/_/_/_/o\
    ///          /@\_\_\@\_\_\
    ///         /_/_/O/_/_/_/_\
    ///        /_\_\_\_\_\o\_\_\
    ///       /_/0/_/_/_0_/_/@/_\
    ///      /_\_\_\_\_\_\_\_\_\_\
    ///     /_/o/_/_/@/_/_/o/_/0/_\
    ///           [___] 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var two = new _2();
            two.Run();

            var three = new _3();
            three.Run();

            var four = new _4();
            four.Run();

            var five = new _5();
            five.Run();


            Console.WriteLine(four.ToString());
            Console.ReadLine();
        }
    }
}