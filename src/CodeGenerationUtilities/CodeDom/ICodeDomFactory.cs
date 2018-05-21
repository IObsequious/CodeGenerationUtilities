using System;
using System.CodeDom;

namespace System.CodeDom
{
    public interface ICodeDomFactory
    {
        CodeArgumentReferenceExpression CreateArgumentReferenceExpression(string parameterName);
        CodeArrayCreateExpression CreateArrayCreateExpression(Type createType, int size);
        CodeArrayCreateExpression CreateArrayCreateExpression(CodeTypeReference createType, CodeExpression[] initializers);
        CodeArrayCreateExpression CreateArrayCreateExpression(string createType, CodeExpression[] initializers);
        CodeArrayCreateExpression CreateArrayCreateExpression(Type createType, CodeExpression[] initializers);
        CodeArrayCreateExpression CreateArrayCreateExpression(CodeTypeReference createType, int size);
        CodeArrayCreateExpression CreateArrayCreateExpression(string createType, int size);
        CodeArrayCreateExpression CreateArrayCreateExpression(CodeTypeReference createType, CodeExpression size);
        CodeArrayCreateExpression CreateArrayCreateExpression(string createType, CodeExpression size);
        CodeArrayCreateExpression CreateArrayCreateExpression(Type createType, CodeExpression size);
        CodeArrayIndexerExpression CreateArrayIndexerExpression(CodeExpression targetObject, CodeExpression[] indices);
        CodeAssignStatement CreateAssignStatement(CodeExpression left, CodeExpression right);
        CodeAttachEventStatement CreateAttachEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener);
        CodeAttachEventStatement CreateAttachEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener);
        CodeBinaryOperatorExpression CreateBinaryOperatorExpression(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right);
        CodeCastExpression CreateCastExpression(CodeTypeReference targetType, CodeExpression expression);
        CodeCastExpression CreateCastExpression(string targetType, CodeExpression expression);
        CodeCastExpression CreateCastExpression(Type targetType, CodeExpression expression);
        CodeChecksumPragma CreateChecksumPragma(string fileName, Guid checksumAlgorithmId, byte[] checksumData);
        CodeComment CreateComment(string text);
        CodeComment CreateComment(string text, bool docComment);
        CodeCommentStatement CreateCommentStatement(CodeComment comment);
        CodeCommentStatement CreateCommentStatement(string text);
        CodeCommentStatement CreateCommentStatement(string text, bool docComment);
        CodeConditionStatement CreateConditionStatement(CodeExpression condition, CodeStatement[] trueStatements);
        CodeConditionStatement CreateConditionStatement(CodeExpression condition, CodeStatement[] trueStatements, CodeStatement[] falseStatements);
        CodeDefaultValueExpression CreateDefaultValueExpression(CodeTypeReference type);
        CodeDelegateCreateExpression CreateDelegateCreateExpression(CodeTypeReference delegateType, CodeExpression targetObject, string methodName);
        CodeDelegateInvokeExpression CreateDelegateInvokeExpression(CodeExpression targetObject);
        CodeDelegateInvokeExpression CreateDelegateInvokeExpression(CodeExpression targetObject, CodeExpression[] parameters);
        CodeDirectionExpression CreateDirectionExpression(FieldDirection direction, CodeExpression expression);
        CodeEventReferenceExpression CreateEventReferenceExpression(CodeExpression targetObject, string eventName);
        CodeExpressionStatement CreateExpressionStatement(CodeExpression expression);
        CodeFieldReferenceExpression CreateFieldReferenceExpression(CodeExpression targetObject, string fieldName);
        CodeGotoStatement CreateGotoStatement(string label);
        CodeIndexerExpression CreateIndexerExpression(CodeExpression targetObject, CodeExpression[] indices);
        CodeIterationStatement CreateIterationStatement(CodeStatement initStatement, CodeExpression testExpression, CodeStatement incrementStatement, CodeStatement[] statements);
        CodeLabeledStatement CreateLabeledStatement(string label);
        CodeLabeledStatement CreateLabeledStatement(string label, CodeStatement statement);
        CodeMemberField CreateMemberField(CodeTypeReference type, string name);
        CodeMemberField CreateMemberField(string type, string name);
        CodeMemberField CreateMemberField(Type type, string name);
        CodeMethodInvokeExpression CreateMethodInvokeExpression(CodeExpression targetObject, string methodName, CodeExpression[] parameters);
        CodeMethodInvokeExpression CreateMethodInvokeExpression(CodeMethodReferenceExpression method, CodeExpression[] parameters);
        CodeMethodReferenceExpression CreateMethodReferenceExpression(CodeExpression targetObject, string methodName);
        CodeMethodReferenceExpression CreateMethodReferenceExpression(CodeExpression targetObject, string methodName, CodeTypeReference[] typeParameters);
        CodeMethodReturnStatement CreateMethodReturnStatement(CodeExpression expression);
        CodeNamespace CreateNamespace(string name);
        CodeNamespaceImport CreateNamespaceImport(string nameSpace);
        CodeObjectCreateExpression CreateObjectCreateExpression(string createType, CodeExpression[] parameters);
        CodeObjectCreateExpression CreateObjectCreateExpression(CodeTypeReference createType, CodeExpression[] parameters);
        CodeObjectCreateExpression CreateObjectCreateExpression(Type createType, CodeExpression[] parameters);
        CodeParameterDeclarationExpression CreateParameterDeclarationExpression(CodeTypeReference type, string name);
        CodeParameterDeclarationExpression CreateParameterDeclarationExpression(string type, string name);
        CodeParameterDeclarationExpression CreateParameterDeclarationExpression(Type type, string name);
        CodePrimitiveExpression CreatePrimitiveExpression(object value);
        CodePropertyReferenceExpression CreatePropertyReferenceExpression(CodeExpression targetObject, string propertyName);
        CodeRegionDirective CreateRegionDirective(CodeRegionMode regionMode, string regionText);
        CodeRemoveEventStatement CreateRemoveEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener);
        CodeRemoveEventStatement CreateRemoveEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener);
        CodeSnippetCompileUnit CreateSnippetCompileUnit(string value);
        CodeSnippetExpression CreateSnippetExpression(string value);
        CodeSnippetStatement CreateSnippetStatement(string value);
        CodeSnippetTypeMember CreateSnippetTypeMember(string text);
        CodeThrowExceptionStatement CreateThrowExceptionStatement(CodeExpression toThrow);
        CodeTryCatchFinallyStatement CreateTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses);
        CodeTryCatchFinallyStatement CreateTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses, CodeStatement[] finallyStatements);
        CodeTypeDeclaration CreateTypeDeclaration(string name);
        CodeTypeDelegate CreateTypeDelegate(string name);
        CodeTypeOfExpression CreateTypeOfExpression(CodeTypeReference type);
        CodeTypeOfExpression CreateTypeOfExpression(string type);
        CodeTypeOfExpression CreateTypeOfExpression(Type type);
        CodeTypeParameter CreateTypeParameter(string name);
        CodeTypeReference CreateTypeReference(Type type);
        CodeTypeReference CreateTypeReference(Type type, CodeTypeReferenceOptions codeTypeReferenceOption);
        CodeTypeReference CreateTypeReference(string typeName, CodeTypeReferenceOptions codeTypeReferenceOption);
        CodeTypeReference CreateTypeReference(string typeName);
        CodeTypeReference CreateTypeReference(string typeName, CodeTypeReference[] typeArguments);
        CodeTypeReference CreateTypeReference(CodeTypeParameter typeParameter);
        CodeTypeReference CreateTypeReference(string baseType, int rank);
        CodeTypeReference CreateTypeReference(CodeTypeReference arrayType, int rank);
        CodeTypeReferenceExpression CreateTypeReferenceExpression(CodeTypeReference type);
        CodeTypeReferenceExpression CreateTypeReferenceExpression(string type);
        CodeTypeReferenceExpression CreateTypeReferenceExpression(Type type);
        CodeVariableDeclarationStatement CreateVariableDeclarationStatement(CodeTypeReference type, string name);
        CodeVariableDeclarationStatement CreateVariableDeclarationStatement(string type, string name);
        CodeVariableDeclarationStatement CreateVariableDeclarationStatement(Type type, string name);
        CodeVariableDeclarationStatement CreateVariableDeclarationStatement(CodeTypeReference type, string name, CodeExpression initExpression);
        CodeVariableDeclarationStatement CreateVariableDeclarationStatement(string type, string name, CodeExpression initExpression);
        CodeVariableDeclarationStatement CreateVariableDeclarationStatement(Type type, string name, CodeExpression initExpression);
        CodeVariableReferenceExpression CreateVariableReferenceExpression(string variableName);

    }


}
