using System;

namespace System.IO
{
    public class StringStream : MemoryStream
    {
        public StringStream()
        {
        }

        public StringStream(string text) : base()
        {
            Write(text);
        }

        public StringStream(int capacity) : base(capacity)
        {
        }

        public StringStream(byte[] buffer) : base(buffer)
        {
        }

        public StringStream(byte[] buffer, bool writable) : base(buffer, writable)
        {
        }

        public StringStream(byte[] buffer, int index, int count) : base(buffer, index, count)
        {
        }

        public StringStream(byte[] buffer, int index, int count, bool writable) : base(buffer, index, count, writable)
        {
        }

        public StringStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible) : base(buffer, index, count, writable, publiclyVisible)
        {
        }

        public void Write(char value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Write(bytes, 0, 1);
        }

        public void Write(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                byte[] bytes = BitConverter.GetBytes(ch);
                Write(bytes, 0, 1);
            }
        }

        public void WriteLine(char value)
        {
            Write(value);
            Write("\r\n");
        }
    
        public void WriteLine(string value)
        {
            Write(value);
            Write("\r\n");
        }

        public new int Length => ToString().Length;

        public char this[int index]
        {
            get
            {
                string text = ToString();
                return text[index];
            }
        }


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            long position = Position;
            Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(this);
            string value = reader.ReadToEnd();
            Position = position;
            return value;
        }
    }
}
