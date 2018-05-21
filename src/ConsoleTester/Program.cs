using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom;
using System.CodeDom.CSharp;
using System.Diagnostics;
using System.Threading;

namespace ConsoleTester
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            ModeConCols();

            string text = CodeGenerator.GenerateCompileUnit(new CodeCompileUnit
            {
                Namespaces =
                {
                    new CodeNamespace("ExampleNamespace")
                    {
                        
                        Imports =
                        {
                            new CodeNamespaceImport("System")
                        },
                        Types =
                        {
                            new CodeTypeDeclaration
                            {
                                Name = "ExampleClass",
                                IsClass = true,
                                IsPartial = true,
                                BaseTypes =
                                {   
                                    new CodeTypeReference("CollectionBase"),
                                    new CodeTypeReference(typeof(IDisposable))
                                },
                                TypeParameters =
                                {
                                    new CodeTypeParameter("TKey"),
                                    new CodeTypeParameter("TValue")
                                },
                                Members =
                                {
                                    new CodeMemberProperty
                                    {
                                        Attributes = MemberAttributes.Public,
                                        Type = new CodeTypeReference(typeof(string)),
                                        Name = "Name",
                                        HasGet = true,
                                        HasSet = true
                                    }
                                }
                            }
                        }
                    }
                },
                
            });

            string temp = Path.GetTempFileName();
            File.WriteAllText(temp, text);
            Process p = Process.Start("notepad.exe", temp);
            while (!p.HasExited)
            {
                Console.Write('.');
                Thread.Sleep(2000);
            }
            File.Delete(temp);

            // PressAnyKey();
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
