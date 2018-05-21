// -----------------------------------------------------------------------
// <copyright file="CodeDomCodeGenerator.ICodeParser.cs" company="Ollon, LLC">
//     Copyright (c) 2017 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace System.CodeDom
{
    public class CodeDomAssistantCodeDomProvider : CodeDomProvider
    {
        private readonly CodeDomAssistant generator;

        public CodeDomAssistantCodeDomProvider()
        {
            generator = new CodeDomAssistant();
        }

        public override string FileExtension
        {
            get
            {
                return "cs";
            }
        }

        public override ICodeParser CreateParser()
        {
            return generator;
        }

        [Obsolete("Callers should not use the ICodeGenerator interface and should instead use the methods directly on the CodeDomProvider class.")]
        public override ICodeGenerator CreateGenerator()
        {
            return generator;
        }

        [Obsolete("Callers should not use the ICodeCompiler interface and should instead use the methods directly on the CodeDomProvider class.")]
        public override ICodeCompiler CreateCompiler()
        {
            return null;
        }

        public override void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
        {
            generator.GenerateCodeFromMember(member, writer, options);
        }

        public override ICodeGenerator CreateGenerator(TextWriter output)
        {
            return new CodeDomAssistant(output);
        }

        public override ICodeGenerator CreateGenerator(string fileName)
        {
            return new CodeDomAssistant();
        }

        public override CodeCompileUnit Parse(TextReader codeStream)
        {
            return base.Parse(codeStream);
        }
    }
}
