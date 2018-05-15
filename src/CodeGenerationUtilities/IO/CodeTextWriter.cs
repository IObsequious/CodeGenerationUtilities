﻿using System.CodeDom.Compiler;

namespace System.IO
{
    public class CodeTextWriter : IndentedTextWriter
    {
        private const string FourSpaces = "    ";

        public CodeTextWriter() : base(new StringWriter(), FourSpaces)
        {

        }

        public CodeTextWriter(TextWriter writer) : base(writer)
        {
        }

        public CodeTextWriter(TextWriter writer, string tabString) : base(writer, tabString)
        {
        }

        #region Code Methods

        public virtual void WriteFileHeader()
        {
            WriteLine(@"//------------------------------------------------------------------------------");
            WriteLine(@"// <auto-generated>");
            WriteLine(@"//     This code was generated by a tool.");
            WriteLine(@"//     Runtime Version:4.0.30319.42000");
            WriteLine(@"//");
            WriteLine(@"//     Changes to this file may cause incorrect behavior and will be lost if");
            WriteLine(@"//     the code is regenerated.");
            WriteLine(@"// </auto-generated>");
            WriteLine(@"//------------------------------------------------------------------------------");

        }

        public virtual void WriteDocComment(string summary, bool singleLine = false)
        {
            if (singleLine)
            {
                WriteLine($"/// <summary>{summary}</summary>");
            }
            else
            {
                WriteLine($"/// <summary>");
                WriteLine($"/// {summary}");
                WriteLine($"/// </summary>");
            }
        }

        public virtual void WriteUsing(string @namespace)
        {
            WriteLine($"using {@namespace};");
        }

        public virtual void WriteRegion(string name)
        {
            WriteLine($"#region {name}");
            WriteLine();
        }

        public virtual void WriteEndRegion()
        {
            WriteLine();
            WriteLine("#endregion");
        }

        public virtual IDisposable CreateBlock()
        {
            return new Block(OpenBlock, CloseBlock);
        }

        public virtual void OpenBlock()
        {
            WriteLine("{");
            Indent++;
        }

        public virtual void CloseBlock()
        {
            Indent--;
            WriteLine("}");
        }


        private struct Block : IDisposable
        {

            private readonly Action _startAction;
            private readonly Action _endAction;

            public Block(Action startAction, Action endAction)
            {
                _startAction = startAction;
                _endAction = endAction;
                
                Start();
            }

            private void Start()
            {
                _startAction();
            }

            private void End()
            {
                _endAction();
            }

            public void Dispose()
            {
                End();
            }
        }

        #endregion

    }
}
