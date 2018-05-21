using System.IO;

namespace System.CodeDom.CSharp
{
    /// <summary>
    /// Code Generator for C#
    /// </summary>
    public class CSharpCodeGenerator : AbstractCSharpCodeGenerator
    {
        /// <summary>
        /// Initializes a new Instance of the <see cref="T:System.CodeDom.CSharp.CSharpCodeGenerator"/> class.
        /// </summary>
        public CSharpCodeGenerator() : base()
        {

        }

        /// <summary>
        /// Initializes a new Instance of the <see cref="T:System.CodeDom.CSharp.CSharpCodeGenerator"/> class with
        /// the specified <see cref="T:System.IO.TextWriter"/>.
        /// </summary>
        /// <param name="output"></param>
        public CSharpCodeGenerator(TextWriter output) : base(output)
        {

        }
    }
}
