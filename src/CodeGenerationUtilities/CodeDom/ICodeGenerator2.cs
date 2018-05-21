using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace System.CodeDom
{
    public interface ICodeGenerator2 : ICodeGenerator
    {
        void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions Options);

        void GenerateSwitchStatement(CodeSwitchStatement e);

        void GenerateReturnValueSwitchSectionStatement(CodeReturnValueSwitchSectionStatement e);

        void GenerateBreakSwitchSectionStatement(CodeBreakSwitchSectionStatement e);

        void GenerateFallThroughSwitchSectionStatement(CodeFallThroughSwitchSectionStatement e);

        void GenerateDefaultBreakSwitchSectionStatement(CodeDefaultBreakSwitchSectionStatement e);

        void GenerateDefaultReturnSwitchSectionStatement(CodeDefaultReturnSwitchSectionStatement e);

        void GenerateSwitchSectionLabelExpression(CodeSwitchSectionLabelExpression e);

        void GenerateType(CodeTypeDeclaration e);

        void GenerateDirective(CodeDirective e);

        void GenerateChecksumPragma(CodeChecksumPragma e);

        void GenerateRegionDirective(CodeRegionDirective e);

        void GenerateMember(CodeTypeMember member);

        void GenerateTypeConstructors(CodeTypeDeclaration e);

        void GenerateNamespaces(CodeCompileUnit e);

        void GenerateTypes(CodeNamespace e);

        void GenerateConstructors(CodeTypeDeclaration e);

        void GenerateEvents(CodeTypeDeclaration e);

        void GenerateExpression(CodeExpression e);

        void GenerateFields(CodeTypeDeclaration e);

        void GenerateSnippetMembers(CodeTypeDeclaration e);

        void GenerateSnippetCompileUnit(CodeSnippetCompileUnit e);

        void GenerateMethods(CodeTypeDeclaration e);

        void GenerateNestedTypes(CodeTypeDeclaration e);

        void GenerateCompileUnit(CodeCompileUnit e);

        void GenerateNamespace(CodeNamespace e);

        void GenerateNamespaceImports(CodeNamespace e);

        void GenerateProperties(CodeTypeDeclaration e);

        void GenerateStatement(CodeStatement e);

        void GenerateAttributeArgument(CodeAttributeArgument arg);

        void GenerateTypeReference(CodeTypeReference typeRef);

        void GenerateArrayCreateExpression(CodeArrayCreateExpression e);

        void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e);

        void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e);

        void GenerateCastExpression(CodeCastExpression e);

        void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e);

        void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e);

        void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e);

        void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e);

        void GenerateIndexerExpression(CodeIndexerExpression e);

        void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e);

        void GenerateSnippetExpression(CodeSnippetExpression e);

        void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e);

        void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e);

        void GenerateEventReferenceExpression(CodeEventReferenceExpression e);

        void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e);

        void GenerateObjectCreateExpression(CodeObjectCreateExpression e);

        void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e);

        void GenerateDirectionExpression(CodeDirectionExpression e);

        void GeneratePrimitiveExpression(CodePrimitiveExpression e);

        void GenerateDefaultValueExpression(CodeDefaultValueExpression e);

        void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e);

        void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e);

        void GenerateThisReferenceExpression(CodeThisReferenceExpression e);

        void GenerateTypeReferenceExpression(CodeTypeReferenceExpression e);

        void GenerateTypeOfExpression(CodeTypeOfExpression e);

        void GenerateExpressionStatement(CodeExpressionStatement e);

        void GenerateIterationStatement(CodeIterationStatement e);

        void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e);

        void GenerateCommentStatement(CodeCommentStatement e);

        void GenerateComment(CodeComment e);

        void GenerateMethodReturnStatement(CodeMethodReturnStatement e);

        void GenerateConditionStatement(CodeConditionStatement e);

        void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e);

        void GenerateAssignStatement(CodeAssignStatement e);

        void GenerateAttachEventStatement(CodeAttachEventStatement e);

        void GenerateRemoveEventStatement(CodeRemoveEventStatement e);

        void GenerateGotoStatement(CodeGotoStatement e);

        void GenerateLabeledStatement(CodeLabeledStatement e);

        void GenerateSnippetStatement(CodeSnippetStatement e);

        void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e);

        void GenerateLinePragmaStart(CodeLinePragma e);

        void GenerateLinePragmaEnd(CodeLinePragma e);

        void GenerateEvent(CodeMemberEvent e);

        void GenerateField(CodeMemberField e);

        void GenerateSnippetMember(CodeSnippetTypeMember e);

        void GenerateEntryPointMethod(CodeEntryPointMethod e);

        void GenerateMethod(CodeMemberMethod e);

        void GenerateProperty(CodeMemberProperty e);

        void GenerateConstructor(CodeConstructor e);

        void GenerateTypeConstructor(CodeTypeConstructor e);

        void GenerateTypeStart(CodeTypeDeclaration e);

        void GenerateTypeEnd(CodeTypeDeclaration e);

        void GenerateCompileUnitStart(CodeCompileUnit e);

        void GenerateCompileUnitEnd(CodeCompileUnit e);

        void GenerateNamespaceStart(CodeNamespace e);

        void GenerateNamespaceEnd(CodeNamespace e);

        void GenerateNamespaceImport(CodeNamespaceImport e);

        void GenerateTypeParameter(CodeTypeParameter e);

        void GenerateTypeDeclaration(CodeTypeDeclaration e);

        void GenerateDelegate(CodeTypeDelegate e);
    }
}
