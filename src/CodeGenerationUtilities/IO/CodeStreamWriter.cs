﻿using System;
using System;
using System.IO;
using System.Text;

namespace System.IO
{
    public class CodeStreamWriter : StreamWriter
    {
        private int _indentLevel = 0;

        private string _indentString = "    ";

        private bool _tabsPending = false;

        public CodeStreamWriter() : this(Path.GetTempFileName())
        {
        }

        public CodeStreamWriter(Stream stream) : base(stream)
        {
        }

        public CodeStreamWriter(Stream stream, Encoding encoding) : base(stream, encoding)
        {
        }

        public CodeStreamWriter(Stream stream, Encoding encoding, int bufferSize) : base(stream, encoding, bufferSize)
        {
        }

        public CodeStreamWriter(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen) : base(stream, encoding, bufferSize, leaveOpen)
        {
        }

        public CodeStreamWriter(string path) : base(path)
        {
            FilePath = path;
        }

        public CodeStreamWriter(string path, bool append) : base(path, append)
        {
            FilePath = path;
        }

        public CodeStreamWriter(string path, bool append, Encoding encoding) : base(path, append, encoding)
        {
            FilePath = path;
        }

        public CodeStreamWriter(string path, bool append, Encoding encoding, int bufferSize) : base(path, append, encoding, bufferSize)
        {
            FilePath = path;
        }

        public override bool AutoFlush
        {
            get
            {
                return true;
            }

            set
            {

            }
        }

        public override Stream BaseStream
        {
            get
            {
                return base.BaseStream;
            }
        }

        public override Encoding Encoding => Encoding.GetEncoding("utf-16");

        public string FilePath { get; }

        public int Indent
        {
            get
            {
                return _indentLevel;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _indentLevel = value;
            }
        }

        public string IndentString
        {
            get
            {
                return _indentString;
            }
            set
            {
                if (_indentString != null
                    && value != _indentString)
                {
                    _indentString = value;
                }

            }
        }

        public virtual void CloseBlock()
        {
            _indentLevel--;
            WriteLine("}");
        }

        public virtual void OpenBlock()
        {
            WriteLine("{");
            _indentLevel++;
        }

        public override void Write(bool value)
        {
            OutputTabs();
            base.Write(value);
        }

        public override void Write(char value)
        {
            OutputTabs();
            base.Write(value);
        }

        public override void Write(char[] buffer)
        {
            OutputTabs();
            base.Write(buffer);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            OutputTabs();
            base.Write(buffer, index, count);
        }

        public override void Write(double value)
        {
            OutputTabs();
            base.Write(value);
        }

        public override void Write(float value)
        {
            OutputTabs();
            base.Write(value);
        }

        public override void Write(int value)
        {
            OutputTabs();
            base.Write(value);
        }

        public override void Write(long value)
        {
            OutputTabs();
            base.Write(value);
        }

        public override void Write(object value)
        {
            OutputTabs();
            base.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            OutputTabs();
            base.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            OutputTabs();
            base.Write(format, arg0, arg1);
        }

        public override void Write(string format, params object[] arg)
        {
            OutputTabs();
            base.Write(format, arg);
        }

        public override void Write(string s)
        {
            OutputTabs();
            base.Write(s);
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

        public virtual void WriteEndRegion()
        {
            WriteLine();
            WriteLine("#endregion");
        }

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

        public override void WriteLine()
        {
            OutputTabs();
            base.WriteLine();
            _tabsPending = true;
        }

        public override void WriteLine(bool value)
        {
            OutputTabs();
            base.WriteLine(value);
            _tabsPending = true;
        }

        public override void WriteLine(char value)
        {
            OutputTabs();
            base.WriteLine(value);
            _tabsPending = true;
        }

        public override void WriteLine(char[] buffer)
        {
            OutputTabs();
            base.WriteLine(buffer);
            _tabsPending = true;
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            OutputTabs();
            base.WriteLine(buffer, index, count);
            _tabsPending = true;
        }

        public override void WriteLine(double value)
        {
            OutputTabs();
            base.WriteLine(value);
            _tabsPending = true;
        }

        public override void WriteLine(float value)
        {
            OutputTabs();
            base.WriteLine(value);
            _tabsPending = true;
        }

        public override void WriteLine(int value)
        {
            OutputTabs();
            base.WriteLine(value);
            _tabsPending = true;
        }

        public override void WriteLine(long value)
        {
            OutputTabs();
            base.WriteLine(value);
            _tabsPending = true;
        }

        public override void WriteLine(object value)
        {
            OutputTabs();
            base.WriteLine(value);
            _tabsPending = true;
        }

        public override void WriteLine(string format, object arg0)
        {
            OutputTabs();
            base.WriteLine(format, arg0);
            _tabsPending = true;
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            OutputTabs();
            base.WriteLine(format, arg0, arg1);
            _tabsPending = true;
        }

        public override void WriteLine(string format, params object[] arg)
        {
            OutputTabs();
            base.WriteLine(format, arg);
            _tabsPending = true;
        }

        public override void WriteLine(string s)
        {
            OutputTabs();
            base.WriteLine(s);
            _tabsPending = true;
        }

        public override void WriteLine(uint value)
        {
            OutputTabs();
            base.WriteLine(value);
            _tabsPending = true;
        }

        public virtual void WriteRegion(string name)
        {
            WriteLine($"#region {name}");
            WriteLine();
        }

        public virtual void WriteUsing(string @namespace)
        {
            WriteLine($"using {@namespace};");
        }

        protected virtual void OutputTabs()
        {
            if (_tabsPending)
            {
                for (int i = 0; i < _indentLevel; i++)
                {
                    base.Write(_indentString);
                }
                _tabsPending = false;
            }
        }
    }
}
