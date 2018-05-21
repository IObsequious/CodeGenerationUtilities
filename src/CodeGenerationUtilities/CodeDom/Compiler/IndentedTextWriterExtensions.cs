namespace System.CodeDom.Compiler
{
    public static class IndentedTextWriterExtensions
    {
        public static void CloseBrace(this IndentedTextWriter writer, bool newLine = true)
        {
            if (newLine)
            {
                writer.PopIndent();
                writer.WriteLine('}');
            }
            else
            {
                writer.Write('}');
            }
        }

        public static void CloseBracket(this IndentedTextWriter writer, bool newLine = true)
        {
            if (newLine)
            {
                writer.PopIndent();
                writer.WriteLine(']');
            }
            else
            {
                writer.Write(']');
            }
        }

        public static void CloseParen(this IndentedTextWriter writer, bool newLine = true)
        {
            if (newLine)
            {
                writer.PopIndent();
                writer.WriteLine(')');
            }
            else
            {
                writer.Write(')');
            }
        }

        public static void OpenBrace(this IndentedTextWriter writer, bool newLine = true)
        {
            if (newLine)
            {
                writer.WriteLine('{');
                writer.PushIndent();
            }
            else
            {
                writer.Write('}');
            }
        }

        public static void OpenBracket(this IndentedTextWriter writer, bool newLine = true)
        {
            if (newLine)
            {
                writer.WriteLine('[');
                writer.PushIndent();
            }
            else
            {
                writer.Write('[');
            }
        }

        public static void OpenParen(this IndentedTextWriter writer, bool newLine = true)
        {
            if (newLine)
            {
                writer.WriteLine('(');
                writer.PushIndent();
            }
            else
            {
                writer.Write('(');
            }
        }

        public static void PopIndent(this IndentedTextWriter writer)
        {
            writer.Indent--;
        }

        public static void PushIndent(this IndentedTextWriter writer)
        {
            writer.Indent++;
        }

        public static void WriteUsing(this IndentedTextWriter writer, string name)
        {
            writer.WriteLine($"using {name};");
        }
    }
}
