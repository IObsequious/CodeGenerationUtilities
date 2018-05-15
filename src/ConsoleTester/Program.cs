using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleTester
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            ModeConCols();
            
            CodeStreamWriter writer = new CodeStreamWriter();

            writer.WriteLine();
            writer.WriteLine("public class C");
            writer.OpenBlock();
            writer.WriteLine("public C()");
            writer.OpenBlock();
            writer.CloseBlock();
            writer.CloseBlock();

            writer.Close();

            string value = File.ReadAllText(writer.FilePath);

            File.Delete(writer.FilePath);

            PressAnyKey();
        }
        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private static void ModeConCols()
        {
            Console.SetWindowSize(160, 50);
        }
    }
}
