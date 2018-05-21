using System;
using System.CodeDom;

namespace System.CodeDom
{
    public interface ICodeDomTextWriter
    {
        void WriteArgumentReferenceExpression(CodeArgumentReferenceExpression code);
        void WriteArrayCreateExpression(CodeArrayCreateExpression code);
        void WriteArrayIndexerExpression(CodeArrayIndexerExpression code);
        void WriteAssignStatement(CodeAssignStatement code);
        void WriteAttachEventStatement(CodeAttachEventStatement code);
        void WriteBaseReferenceExpression(CodeBaseReferenceExpression code);
        void WriteBinaryOperatorExpression(CodeBinaryOperatorExpression code);
        void WriteCastExpression(CodeCastExpression code);
        void WriteChecksumPragma(CodeChecksumPragma code);
        void WriteComment(CodeComment code);
        void WriteCommentStatement(CodeCommentStatement code);
        void WriteCompileUnit(CodeCompileUnit code);
        void WriteConditionStatement(CodeConditionStatement code);
        void WriteConstructor(CodeConstructor code);
        void WriteDefaultValueExpression(CodeDefaultValueExpression code);
        void WriteDelegateCreateExpression(CodeDelegateCreateExpression code);
        void WriteDelegateInvokeExpression(CodeDelegateInvokeExpression code);
        void WriteDirectionExpression(CodeDirectionExpression code);
        void WriteDirective(CodeDirective code);
        void WriteEntryPointMethod(CodeEntryPointMethod code);
        void WriteEventReferenceExpression(CodeEventReferenceExpression code);
        void WriteExpression(CodeExpression code);
        void WriteExpressionStatement(CodeExpressionStatement code);
        void WriteFieldReferenceExpression(CodeFieldReferenceExpression code);
        void WriteGotoStatement(CodeGotoStatement code);
        void WriteIndexerExpression(CodeIndexerExpression code);
        void WriteIterationStatement(CodeIterationStatement code);
        void WriteLabeledStatement(CodeLabeledStatement code);
        void WriteMemberEvent(CodeMemberEvent code);
        void WriteMemberField(CodeMemberField code);
        void WriteMemberMethod(CodeMemberMethod code);
        void WriteMemberProperty(CodeMemberProperty code);
        void WriteMethodInvokeExpression(CodeMethodInvokeExpression code);
        void WriteMethodReferenceExpression(CodeMethodReferenceExpression code);
        void WriteMethodReturnStatement(CodeMethodReturnStatement code);
        void WriteNamespace(CodeNamespace code);
        void WriteNamespaceImport(CodeNamespaceImport code);
        void WriteObject(CodeObject code);
        void WriteObjectCreateExpression(CodeObjectCreateExpression code);
        void WriteParameterDeclarationExpression(CodeParameterDeclarationExpression code);
        void WritePrimitiveExpression(CodePrimitiveExpression code);
        void WritePropertyReferenceExpression(CodePropertyReferenceExpression code);
        void WritePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression code);
        void WriteRegionDirective(CodeRegionDirective code);
        void WriteRemoveEventStatement(CodeRemoveEventStatement code);
        void WriteSnippetCompileUnit(CodeSnippetCompileUnit code);
        void WriteSnippetExpression(CodeSnippetExpression code);
        void WriteSnippetStatement(CodeSnippetStatement code);
        void WriteSnippetTypeMember(CodeSnippetTypeMember code);
        void WriteStatement(CodeStatement code);
        void WriteThisReferenceExpression(CodeThisReferenceExpression code);
        void WriteThrowExceptionStatement(CodeThrowExceptionStatement code);
        void WriteTryCatchFinallyStatement(CodeTryCatchFinallyStatement code);
        void WriteTypeConstructor(CodeTypeConstructor code);
        void WriteTypeDeclaration(CodeTypeDeclaration code);
        void WriteTypeDelegate(CodeTypeDelegate code);
        void WriteTypeMember(CodeTypeMember code);
        void WriteTypeOfExpression(CodeTypeOfExpression code);
        void WriteTypeParameter(CodeTypeParameter code);
        void WriteTypeReference(CodeTypeReference code);
        void WriteTypeReferenceExpression(CodeTypeReferenceExpression code);
        void WriteVariableDeclarationStatement(CodeVariableDeclarationStatement code);
        void WriteVariableReferenceExpression(CodeVariableReferenceExpression code);

    }


}
