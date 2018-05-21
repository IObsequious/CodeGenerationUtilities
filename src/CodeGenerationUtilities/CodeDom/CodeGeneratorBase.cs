using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.CodeDom
{
    /// <summary>
    /// Provides a default implementation of the <see cref="CodeGenerator" />.
    /// </summary>
    public abstract class CodeGeneratorBase : AbstractCodeGenerator
    {
        protected CodeGeneratorBase() : base()
        {
        }

        protected CodeGeneratorBase(TextWriter writer) : base(writer)
        {
        }

        protected override string NullToken
        {
            get
            {
                return "null";
            }
        }

        public override void GenerateSwitchSectionLabelExpression(CodeSwitchSectionLabelExpression e)
        {
        }

        public override string CreateEscapedIdentifier(string value)
        {
            return value;
        }

        public override string CreateValidIdentifier(string value)
        {
            return value;
        }

        public override void GenerateSwitchStatement(CodeSwitchStatement e)
        {
        }

        public override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
        {
        }

        public override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
        {
        }

        public override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
        {
        }

        public override void GenerateAssignStatement(CodeAssignStatement e)
        {
        }

        public override void GenerateAttachEventStatement(CodeAttachEventStatement e)
        {
        }

        protected override void OutputAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
        {
        }

        protected override void OutputAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
        {
        }

        public override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
        {
        }

        public override void GenerateCastExpression(CodeCastExpression e)
        {
        }

        public override void GenerateComment(CodeComment e)
        {
        }

        public override void GenerateConditionStatement(CodeConditionStatement e)
        {
        }

        public override void GenerateConstructor(CodeConstructor e)
        {
        }

        public override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
        {
        }

        public override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
        {
        }

        public override void GenerateEntryPointMethod(CodeEntryPointMethod e)
        {
        }

        public override void GenerateEvent(CodeMemberEvent e)
        {
        }

        public override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
        {
        }

        public override void GenerateExpressionStatement(CodeExpressionStatement e)
        {
        }

        public override void GenerateField(CodeMemberField e)
        {
        }

        public override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
        {
        }

        public override void GenerateGotoStatement(CodeGotoStatement e)
        {
        }

        public override void GenerateIndexerExpression(CodeIndexerExpression e)
        {
        }

        public override void GenerateIterationStatement(CodeIterationStatement e)
        {
        }

        public override void GenerateLabeledStatement(CodeLabeledStatement e)
        {
        }

        public override void GenerateLinePragmaEnd(CodeLinePragma e)
        {
        }

        public override void GenerateLinePragmaStart(CodeLinePragma e)
        {
        }

        public override void GenerateMethod(CodeMemberMethod e)
        {
        }

        public override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
        {
        }

        public override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
        {
        }

        public override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
        {
        }

        public override void GenerateNamespaceEnd(CodeNamespace e)
        {
        }

        public override void GenerateNamespaceImport(CodeNamespaceImport e)
        {
        }

        public override void GenerateNamespaceStart(CodeNamespace e)
        {
        }

        public override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
        {
        }

        public override void GenerateProperty(CodeMemberProperty e)
        {
        }

        public override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
        {
        }

        public override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
        {
        }

        public override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
        {
        }

        public override void GenerateSnippetExpression(CodeSnippetExpression e)
        {
        }

        public override void GenerateSnippetMember(CodeSnippetTypeMember e)
        {
        }

        public override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
        {
        }

        public override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
        {
        }

        public override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
        {
        }

        public override void GenerateTypeConstructor(CodeTypeConstructor e)
        {
        }

        public override void GenerateTypeEnd(CodeTypeDeclaration e)
        {
        }

        public override void GenerateTypeParameter(CodeTypeParameter e)
        {

        }

        public override void GenerateTypeDeclaration(CodeTypeDeclaration e)
        {
        }

        public override void GenerateDelegate(CodeTypeDelegate e)
        {
        }

        public override void GenerateTypeStart(CodeTypeDeclaration e)
        {
        }

        public override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
        {
        }

        public override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
        {
        }

        public override void GenerateDirective(CodeDirective e)
        {
        }

        public override void GenerateChecksumPragma(CodeChecksumPragma e)
        {
        }

        public override string GetTypeOutput(CodeTypeReference value)
        {
            return string.Empty;
        }

        public override bool IsValidIdentifier(string value)
        {
            return false;
        }

        public override string QuoteSnippetString(string value)
        {
            return value;
        }

        protected override bool Supports(GeneratorSupport support)
        {
            return true;
        }

        public override void GenerateDefaultValueExpression(CodeDefaultValueExpression e)
        {

        }
    }
}
