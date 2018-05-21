using System;
using System.CodeDom;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace System.CodeDom
{
    public class CodeDomTextWriter : TextWriter, ICodeDomTextWriter
    {
        public CodeDomTextWriter()
        {
            Stream = new StringStream();
        }

        public CodeDomTextWriter(Stream stream)
        {
            Stream = stream;
        }

        public CodeDomTextWriter(TextWriter writer)
        {
            Writer = writer;
        }

        public TextWriter Writer { get; }

        public Stream Stream { get; }

        public override Encoding Encoding => Encoding.GetEncoding("utf-16");

        public override void Write(char value)
        {
            Write(new[] { value }, 0, 1);
        }

        public override void Write(char[] buffer)
        {
            if (buffer == null)
                return;

            Write(buffer, 0, buffer.Length);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            if (Stream != null)
            {
                for (int i = index; i < count; i++)
                {
                    var bytes = BitConverter.GetBytes(buffer[i]);
                    Stream.Write(bytes, 0, 1);
                }
            }

            if (Writer != null)
            {
                for (int i = index; i < count; i++)
                {
                    char ch = buffer[i];
                    Writer.Write(ch);
                }
            }
        }

        public override Task WriteAsync(char value)
        {
            Write(value);
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            if (Stream != null)
            {
                long position = Stream.Position;
                Stream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(Stream);
                string value = reader.ReadToEnd();
                Stream.Position = position;
                return value;
            }
            else
            {
                return Writer?.ToString();
            }
        }

        

        public virtual void WriteArgumentReferenceExpression(CodeArgumentReferenceExpression code)
        {
        }

        public virtual void WriteArrayCreateExpression(CodeArrayCreateExpression code)
        {
        }

        public virtual void WriteArrayIndexerExpression(CodeArrayIndexerExpression code)
        {
        }

        public virtual void WriteAssignStatement(CodeAssignStatement code)
        {
        }

        public virtual void WriteAttachEventStatement(CodeAttachEventStatement code)
        {
        }

        public virtual void WriteBaseReferenceExpression(CodeBaseReferenceExpression code)
        {
        }

        public virtual void WriteBinaryOperatorExpression(CodeBinaryOperatorExpression code)
        {
        }

        public virtual void WriteCastExpression(CodeCastExpression code)
        {
        }

        public virtual void WriteChecksumPragma(CodeChecksumPragma code)
        {
        }

        public virtual void WriteComment(CodeComment code)
        {
        }

        public virtual void WriteCommentStatement(CodeCommentStatement code)
        {
        }

        public virtual void WriteCompileUnit(CodeCompileUnit code)
        {
        }

        public virtual void WriteConditionStatement(CodeConditionStatement code)
        {
        }

        public virtual void WriteConstructor(CodeConstructor code)
        {
        }

        public virtual void WriteDefaultValueExpression(CodeDefaultValueExpression code)
        {
        }

        public virtual void WriteDelegateCreateExpression(CodeDelegateCreateExpression code)
        {
        }

        public virtual void WriteDelegateInvokeExpression(CodeDelegateInvokeExpression code)
        {
        }

        public virtual void WriteDirectionExpression(CodeDirectionExpression code)
        {
        }

        public virtual void WriteDirective(CodeDirective code)
        {
        }

        public virtual void WriteEntryPointMethod(CodeEntryPointMethod code)
        {
        }

        public virtual void WriteEventReferenceExpression(CodeEventReferenceExpression code)
        {
        }

        public virtual void WriteExpression(CodeExpression code)
        {
        }

        public virtual void WriteExpressionStatement(CodeExpressionStatement code)
        {
        }

        public virtual void WriteFieldReferenceExpression(CodeFieldReferenceExpression code)
        {
        }

        public virtual void WriteGotoStatement(CodeGotoStatement code)
        {
        }

        public virtual void WriteIndexerExpression(CodeIndexerExpression code)
        {
        }

        public virtual void WriteIterationStatement(CodeIterationStatement code)
        {
        }

        public virtual void WriteLabeledStatement(CodeLabeledStatement code)
        {
        }

        public virtual void WriteMemberEvent(CodeMemberEvent code)
        {
        }

        public virtual void WriteMemberField(CodeMemberField code)
        {
        }

        public virtual void WriteMemberMethod(CodeMemberMethod code)
        {
        }

        public virtual void WriteMemberProperty(CodeMemberProperty code)
        {
        }

        public virtual void WriteMethodInvokeExpression(CodeMethodInvokeExpression code)
        {
        }

        public virtual void WriteMethodReferenceExpression(CodeMethodReferenceExpression code)
        {
        }

        public virtual void WriteMethodReturnStatement(CodeMethodReturnStatement code)
        {
        }

        public virtual void WriteNamespace(CodeNamespace code)
        {
        }

        public virtual void WriteNamespaceImport(CodeNamespaceImport code)
        {
        }

        public virtual void WriteObject(CodeObject code)
        {
        }

        public virtual void WriteObjectCreateExpression(CodeObjectCreateExpression code)
        {
        }

        public virtual void WriteParameterDeclarationExpression(CodeParameterDeclarationExpression code)
        {
        }

        public virtual void WritePrimitiveExpression(CodePrimitiveExpression code)
        {
        }

        public virtual void WritePropertyReferenceExpression(CodePropertyReferenceExpression code)
        {
        }

        public virtual void WritePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression code)
        {
        }

        public virtual void WriteRegionDirective(CodeRegionDirective code)
        {
        }

        public virtual void WriteRemoveEventStatement(CodeRemoveEventStatement code)
        {
        }

        public virtual void WriteSnippetCompileUnit(CodeSnippetCompileUnit code)
        {
        }

        public virtual void WriteSnippetExpression(CodeSnippetExpression code)
        {
        }

        public virtual void WriteSnippetStatement(CodeSnippetStatement code)
        {
        }

        public virtual void WriteSnippetTypeMember(CodeSnippetTypeMember code)
        {
        }

        public virtual void WriteStatement(CodeStatement code)
        {
        }

        public virtual void WriteThisReferenceExpression(CodeThisReferenceExpression code)
        {
        }

        public virtual void WriteThrowExceptionStatement(CodeThrowExceptionStatement code)
        {
        }

        public virtual void WriteTryCatchFinallyStatement(CodeTryCatchFinallyStatement code)
        {
        }

        public virtual void WriteTypeConstructor(CodeTypeConstructor code)
        {
        }

        public virtual void WriteTypeDeclaration(CodeTypeDeclaration code)
        {
        }

        public virtual void WriteTypeDelegate(CodeTypeDelegate code)
        {
        }

        public virtual void WriteTypeMember(CodeTypeMember code)
        {
        }

        public virtual void WriteTypeOfExpression(CodeTypeOfExpression code)
        {
        }

        public virtual void WriteTypeParameter(CodeTypeParameter code)
        {
        }

        public virtual void WriteTypeReference(CodeTypeReference code)
        {
        }

        public virtual void WriteTypeReferenceExpression(CodeTypeReferenceExpression code)
        {
        }

        public virtual void WriteVariableDeclarationStatement(CodeVariableDeclarationStatement code)
        {
        }

        public virtual void WriteVariableReferenceExpression(CodeVariableReferenceExpression code)
        {
        }
    }
}
