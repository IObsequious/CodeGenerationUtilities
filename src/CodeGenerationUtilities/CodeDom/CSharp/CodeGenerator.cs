// -----------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="Ollon, LLC">
//     Copyright (c) 2018 Ollon, LLC. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.CodeDom.CSharp
{
    public static class CodeGenerator
    {
        private static ICodeGenerator2 GetCodeGenerator(TextWriter writer)
        {
            return new CSharpCodeGenerator(writer);
        }


        public static string GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateArgumentReferenceExpression(code);
            return builder.ToString();
        }

        public static string GenerateArrayCreateExpression(CodeArrayCreateExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateArrayCreateExpression(code);
            return builder.ToString();
        }

        public static string GenerateArrayIndexerExpression(CodeArrayIndexerExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateArrayIndexerExpression(code);
            return builder.ToString();
        }

        public static string GenerateAssignStatement(CodeAssignStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateAssignStatement(code);
            return builder.ToString();
        }

        public static string GenerateAttachEventStatement(CodeAttachEventStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateAttachEventStatement(code);
            return builder.ToString();
        }

        public static string GenerateBaseReferenceExpression(CodeBaseReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateBaseReferenceExpression(code);
            return builder.ToString();
        }

        public static string GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateBinaryOperatorExpression(code);
            return builder.ToString();
        }

        public static string GenerateCastExpression(CodeCastExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateCastExpression(code);
            return builder.ToString();
        }

        public static string GenerateChecksumPragma(CodeChecksumPragma code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateChecksumPragma(code);
            return builder.ToString();
        }

        public static string GenerateComment(CodeComment code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateComment(code);
            return builder.ToString();
        }

        public static string GenerateCommentStatement(CodeCommentStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateCommentStatement(code);
            return builder.ToString();
        }

        public static string GenerateCompileUnit(CodeCompileUnit code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateCompileUnit(code);
            return builder.ToString();
        }

        public static string GenerateConditionStatement(CodeConditionStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateConditionStatement(code);
            return builder.ToString();
        }

        public static string GenerateConstructor(CodeConstructor code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateConstructor(code);
            return builder.ToString();
        }

        public static string GenerateDefaultValueExpression(CodeDefaultValueExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateDefaultValueExpression(code);
            return builder.ToString();
        }

        public static string GenerateDelegateCreateExpression(CodeDelegateCreateExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateDelegateCreateExpression(code);
            return builder.ToString();
        }

        public static string GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateDelegateInvokeExpression(code);
            return builder.ToString();
        }

        public static string GenerateDirectionExpression(CodeDirectionExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateDirectionExpression(code);
            return builder.ToString();
        }

        public static string GenerateDirective(CodeDirective code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateDirective(code);
            return builder.ToString();
        }

        public static string GenerateEntryPointMethod(CodeEntryPointMethod code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateEntryPointMethod(code);
            return builder.ToString();
        }

        public static string GenerateEventReferenceExpression(CodeEventReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateEventReferenceExpression(code);
            return builder.ToString();
        }

        public static string GenerateExpression(CodeExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateExpression(code);
            return builder.ToString();
        }

        public static string GenerateExpressionStatement(CodeExpressionStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateExpressionStatement(code);
            return builder.ToString();
        }

        public static string GenerateFieldReferenceExpression(CodeFieldReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateFieldReferenceExpression(code);
            return builder.ToString();
        }

        public static string GenerateGotoStatement(CodeGotoStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateGotoStatement(code);
            return builder.ToString();
        }

        public static string GenerateIndexerExpression(CodeIndexerExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateIndexerExpression(code);
            return builder.ToString();
        }

        public static string GenerateIterationStatement(CodeIterationStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateIterationStatement(code);
            return builder.ToString();
        }

        public static string GenerateLabeledStatement(CodeLabeledStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateLabeledStatement(code);
            return builder.ToString();
        }

        public static string GenerateEvent(CodeMemberEvent code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateEvent(code);
            return builder.ToString();
        }

        public static string GenerateField(CodeMemberField code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateField(code);
            return builder.ToString();
        }

        public static string GenerateMethod(CodeMemberMethod code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateMethod(code);
            return builder.ToString();
        }

        public static string GenerateProperty(CodeMemberProperty code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateProperty(code);
            return builder.ToString();
        }

        public static string GenerateMethodInvokeExpression(CodeMethodInvokeExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateMethodInvokeExpression(code);
            return builder.ToString();
        }

        public static string GenerateMethodReferenceExpression(CodeMethodReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateMethodReferenceExpression(code);
            return builder.ToString();
        }

        public static string GenerateMethodReturnStatement(CodeMethodReturnStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateMethodReturnStatement(code);
            return builder.ToString();
        }

        public static string GenerateNamespace(CodeNamespace code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateNamespace(code);
            return builder.ToString();
        }

        public static string GenerateNamespaceImport(CodeNamespaceImport code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateNamespaceImport(code);
            return builder.ToString();
        }

        public static string GenerateObjectCreateExpression(CodeObjectCreateExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateObjectCreateExpression(code);
            return builder.ToString();
        }

        public static string GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateParameterDeclarationExpression(code);
            return builder.ToString();
        }

        public static string GeneratePrimitiveExpression(CodePrimitiveExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GeneratePrimitiveExpression(code);
            return builder.ToString();
        }

        public static string GeneratePropertyReferenceExpression(CodePropertyReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GeneratePropertyReferenceExpression(code);
            return builder.ToString();
        }

        public static string GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GeneratePropertySetValueReferenceExpression(code);
            return builder.ToString();
        }

        public static string GenerateRegionDirective(CodeRegionDirective code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateRegionDirective(code);
            return builder.ToString();
        }

        public static string GenerateRemoveEventStatement(CodeRemoveEventStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateRemoveEventStatement(code);
            return builder.ToString();
        }

        public static string GenerateSnippetCompileUnit(CodeSnippetCompileUnit code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateSnippetCompileUnit(code);
            return builder.ToString();
        }

        public static string GenerateSnippetExpression(CodeSnippetExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateSnippetExpression(code);
            return builder.ToString();
        }

        public static string GenerateSnippetStatement(CodeSnippetStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateSnippetStatement(code);
            return builder.ToString();
        }

        public static string GenerateSnippetTypeMember(CodeSnippetTypeMember code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateMember(code);
            return builder.ToString();
        }

        public static string GenerateStatement(CodeStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateStatement(code);
            return builder.ToString();
        }

        public static string GenerateThisReferenceExpression(CodeThisReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateThisReferenceExpression(code);
            return builder.ToString();
        }

        public static string GenerateThrowExceptionStatement(CodeThrowExceptionStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateThrowExceptionStatement(code);
            return builder.ToString();
        }

        public static string GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateTryCatchFinallyStatement(code);
            return builder.ToString();
        }

        public static string GenerateTypeConstructor(CodeTypeConstructor code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateTypeConstructor(code);
            return builder.ToString();
        }

        public static string GenerateTypeDeclaration(CodeTypeDeclaration code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateTypeDeclaration(code);
            return builder.ToString();
        }

        public static string GenerateTypeDelegate(CodeTypeDelegate code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateDelegate(code);
            return builder.ToString();
        }

        public static string GenerateTypeMember(CodeTypeMember code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateMember(code);
            return builder.ToString();
        }

        public static string GenerateTypeOfExpression(CodeTypeOfExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateTypeOfExpression(code);
            return builder.ToString();
        }

        public static string GenerateTypeParameter(CodeTypeParameter code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateTypeParameter(code);
            return builder.ToString();
        }

        public static string GenerateTypeReference(CodeTypeReference code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateTypeReference(code);
            return builder.ToString();
        }

        public static string GenerateTypeReferenceExpression(CodeTypeReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateTypeReferenceExpression(code);
            return builder.ToString();
        }

        public static string GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateVariableDeclarationStatement(code);
            return builder.ToString();
        }

        public static string GenerateVariableReferenceExpression(CodeVariableReferenceExpression code)
        {
            StringBuilder builder = new StringBuilder();
            GetCodeGenerator(new StringWriter(builder)).GenerateVariableReferenceExpression(code);
            return builder.ToString();
        }

    }
}
