﻿using System;
using System.CodeDom;

namespace System.CodeDom
{
    public class CodeDomFactory : ICodeDomFactory
    {
        public virtual CodeArgumentReferenceExpression CreateArgumentReferenceExpression(string parameterName) => new CodeArgumentReferenceExpression(parameterName);
        public virtual CodeArrayCreateExpression CreateArrayCreateExpression(Type createType, int size) => new CodeArrayCreateExpression(createType, size);
        public virtual CodeArrayCreateExpression CreateArrayCreateExpression(CodeTypeReference createType, CodeExpression[] initializers) => new CodeArrayCreateExpression(createType, initializers);
        public virtual CodeArrayCreateExpression CreateArrayCreateExpression(string createType, CodeExpression[] initializers) => new CodeArrayCreateExpression(createType, initializers);
        public virtual CodeArrayCreateExpression CreateArrayCreateExpression(Type createType, CodeExpression[] initializers) => new CodeArrayCreateExpression(createType, initializers);
        public virtual CodeArrayCreateExpression CreateArrayCreateExpression(CodeTypeReference createType, int size) => new CodeArrayCreateExpression(createType, size);
        public virtual CodeArrayCreateExpression CreateArrayCreateExpression(string createType, int size) => new CodeArrayCreateExpression(createType, size);
        public virtual CodeArrayCreateExpression CreateArrayCreateExpression(CodeTypeReference createType, CodeExpression size) => new CodeArrayCreateExpression(createType, size);
        public virtual CodeArrayCreateExpression CreateArrayCreateExpression(string createType, CodeExpression size) => new CodeArrayCreateExpression(createType, size);
        public virtual CodeArrayCreateExpression CreateArrayCreateExpression(Type createType, CodeExpression size) => new CodeArrayCreateExpression(createType, size);
        public virtual CodeArrayIndexerExpression CreateArrayIndexerExpression(CodeExpression targetObject, CodeExpression[] indices) => new CodeArrayIndexerExpression(targetObject, indices);
        public virtual CodeAssignStatement CreateAssignStatement(CodeExpression left, CodeExpression right) => new CodeAssignStatement(left, right);
        public virtual CodeAttachEventStatement CreateAttachEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener) => new CodeAttachEventStatement(eventRef, listener);
        public virtual CodeAttachEventStatement CreateAttachEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener) => new CodeAttachEventStatement(targetObject, eventName, listener);
        public virtual CodeBinaryOperatorExpression CreateBinaryOperatorExpression(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right) => new CodeBinaryOperatorExpression(left, op, right);
        public virtual CodeCastExpression CreateCastExpression(CodeTypeReference targetType, CodeExpression expression) => new CodeCastExpression(targetType, expression);
        public virtual CodeCastExpression CreateCastExpression(string targetType, CodeExpression expression) => new CodeCastExpression(targetType, expression);
        public virtual CodeCastExpression CreateCastExpression(Type targetType, CodeExpression expression) => new CodeCastExpression(targetType, expression);
        public virtual CodeChecksumPragma CreateChecksumPragma(string fileName, Guid checksumAlgorithmId, byte[] checksumData) => new CodeChecksumPragma(fileName, checksumAlgorithmId, checksumData);
        public virtual CodeComment CreateComment(string text) => new CodeComment(text);
        public virtual CodeComment CreateComment(string text, bool docComment) => new CodeComment(text, docComment);
        public virtual CodeCommentStatement CreateCommentStatement(CodeComment comment) => new CodeCommentStatement(comment);
        public virtual CodeCommentStatement CreateCommentStatement(string text) => new CodeCommentStatement(text);
        public virtual CodeCommentStatement CreateCommentStatement(string text, bool docComment) => new CodeCommentStatement(text, docComment);
        public virtual CodeConditionStatement CreateConditionStatement(CodeExpression condition, CodeStatement[] trueStatements) => new CodeConditionStatement(condition, trueStatements);
        public virtual CodeConditionStatement CreateConditionStatement(CodeExpression condition, CodeStatement[] trueStatements, CodeStatement[] falseStatements) => new CodeConditionStatement(condition, trueStatements, falseStatements);
        public virtual CodeDefaultValueExpression CreateDefaultValueExpression(CodeTypeReference type) => new CodeDefaultValueExpression(type);
        public virtual CodeDelegateCreateExpression CreateDelegateCreateExpression(CodeTypeReference delegateType, CodeExpression targetObject, string methodName) => new CodeDelegateCreateExpression(delegateType, targetObject, methodName);
        public virtual CodeDelegateInvokeExpression CreateDelegateInvokeExpression(CodeExpression targetObject) => new CodeDelegateInvokeExpression(targetObject);
        public virtual CodeDelegateInvokeExpression CreateDelegateInvokeExpression(CodeExpression targetObject, CodeExpression[] parameters) => new CodeDelegateInvokeExpression(targetObject, parameters);
        public virtual CodeDirectionExpression CreateDirectionExpression(FieldDirection direction, CodeExpression expression) => new CodeDirectionExpression(direction, expression);
        public virtual CodeEventReferenceExpression CreateEventReferenceExpression(CodeExpression targetObject, string eventName) => new CodeEventReferenceExpression(targetObject, eventName);
        public virtual CodeExpressionStatement CreateExpressionStatement(CodeExpression expression) => new CodeExpressionStatement(expression);
        public virtual CodeFieldReferenceExpression CreateFieldReferenceExpression(CodeExpression targetObject, string fieldName) => new CodeFieldReferenceExpression(targetObject, fieldName);
        public virtual CodeGotoStatement CreateGotoStatement(string label) => new CodeGotoStatement(label);
        public virtual CodeIndexerExpression CreateIndexerExpression(CodeExpression targetObject, CodeExpression[] indices) => new CodeIndexerExpression(targetObject, indices);
        public virtual CodeIterationStatement CreateIterationStatement(CodeStatement initStatement, CodeExpression testExpression, CodeStatement incrementStatement, CodeStatement[] statements) => new CodeIterationStatement(initStatement, testExpression, incrementStatement, statements);
        public virtual CodeLabeledStatement CreateLabeledStatement(string label) => new CodeLabeledStatement(label);
        public virtual CodeLabeledStatement CreateLabeledStatement(string label, CodeStatement statement) => new CodeLabeledStatement(label, statement);
        public virtual CodeMemberField CreateMemberField(CodeTypeReference type, string name) => new CodeMemberField(type, name);
        public virtual CodeMemberField CreateMemberField(string type, string name) => new CodeMemberField(type, name);
        public virtual CodeMemberField CreateMemberField(Type type, string name) => new CodeMemberField(type, name);
        public virtual CodeMethodInvokeExpression CreateMethodInvokeExpression(CodeExpression targetObject, string methodName, CodeExpression[] parameters) => new CodeMethodInvokeExpression(targetObject, methodName, parameters);
        public virtual CodeMethodInvokeExpression CreateMethodInvokeExpression(CodeMethodReferenceExpression method, CodeExpression[] parameters) => new CodeMethodInvokeExpression(method, parameters);
        public virtual CodeMethodReferenceExpression CreateMethodReferenceExpression(CodeExpression targetObject, string methodName) => new CodeMethodReferenceExpression(targetObject, methodName);
        public virtual CodeMethodReferenceExpression CreateMethodReferenceExpression(CodeExpression targetObject, string methodName, CodeTypeReference[] typeParameters) => new CodeMethodReferenceExpression(targetObject, methodName, typeParameters);
        public virtual CodeMethodReturnStatement CreateMethodReturnStatement(CodeExpression expression) => new CodeMethodReturnStatement(expression);
        public virtual CodeNamespace CreateNamespace(string name) => new CodeNamespace(name);
        public virtual CodeNamespaceImport CreateNamespaceImport(string nameSpace) => new CodeNamespaceImport(nameSpace);
        public virtual CodeObjectCreateExpression CreateObjectCreateExpression(string createType, CodeExpression[] parameters) => new CodeObjectCreateExpression(createType, parameters);
        public virtual CodeObjectCreateExpression CreateObjectCreateExpression(CodeTypeReference createType, CodeExpression[] parameters) => new CodeObjectCreateExpression(createType, parameters);
        public virtual CodeObjectCreateExpression CreateObjectCreateExpression(Type createType, CodeExpression[] parameters) => new CodeObjectCreateExpression(createType, parameters);
        public virtual CodeParameterDeclarationExpression CreateParameterDeclarationExpression(CodeTypeReference type, string name) => new CodeParameterDeclarationExpression(type, name);
        public virtual CodeParameterDeclarationExpression CreateParameterDeclarationExpression(string type, string name) => new CodeParameterDeclarationExpression(type, name);
        public virtual CodeParameterDeclarationExpression CreateParameterDeclarationExpression(Type type, string name) => new CodeParameterDeclarationExpression(type, name);
        public virtual CodePrimitiveExpression CreatePrimitiveExpression(object value) => new CodePrimitiveExpression(value);
        public virtual CodePropertyReferenceExpression CreatePropertyReferenceExpression(CodeExpression targetObject, string propertyName) => new CodePropertyReferenceExpression(targetObject, propertyName);
        public virtual CodeRegionDirective CreateRegionDirective(CodeRegionMode regionMode, string regionText) => new CodeRegionDirective(regionMode, regionText);
        public virtual CodeRemoveEventStatement CreateRemoveEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener) => new CodeRemoveEventStatement(eventRef, listener);
        public virtual CodeRemoveEventStatement CreateRemoveEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener) => new CodeRemoveEventStatement(targetObject, eventName, listener);
        public virtual CodeSnippetCompileUnit CreateSnippetCompileUnit(string value) => new CodeSnippetCompileUnit(value);
        public virtual CodeSnippetExpression CreateSnippetExpression(string value) => new CodeSnippetExpression(value);
        public virtual CodeSnippetStatement CreateSnippetStatement(string value) => new CodeSnippetStatement(value);
        public virtual CodeSnippetTypeMember CreateSnippetTypeMember(string text) => new CodeSnippetTypeMember(text);
        public virtual CodeThrowExceptionStatement CreateThrowExceptionStatement(CodeExpression toThrow) => new CodeThrowExceptionStatement(toThrow);
        public virtual CodeTryCatchFinallyStatement CreateTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses) => new CodeTryCatchFinallyStatement(tryStatements, catchClauses);
        public virtual CodeTryCatchFinallyStatement CreateTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses, CodeStatement[] finallyStatements) => new CodeTryCatchFinallyStatement(tryStatements, catchClauses, finallyStatements);
        public virtual CodeTypeDeclaration CreateTypeDeclaration(string name) => new CodeTypeDeclaration(name);
        public virtual CodeTypeDelegate CreateTypeDelegate(string name) => new CodeTypeDelegate(name);
        public virtual CodeTypeOfExpression CreateTypeOfExpression(CodeTypeReference type) => new CodeTypeOfExpression(type);
        public virtual CodeTypeOfExpression CreateTypeOfExpression(string type) => new CodeTypeOfExpression(type);
        public virtual CodeTypeOfExpression CreateTypeOfExpression(Type type) => new CodeTypeOfExpression(type);
        public virtual CodeTypeParameter CreateTypeParameter(string name) => new CodeTypeParameter(name);
        public virtual CodeTypeReference CreateTypeReference(Type type) => new CodeTypeReference(type);
        public virtual CodeTypeReference CreateTypeReference(Type type, CodeTypeReferenceOptions codeTypeReferenceOption) => new CodeTypeReference(type, codeTypeReferenceOption);
        public virtual CodeTypeReference CreateTypeReference(string typeName, CodeTypeReferenceOptions codeTypeReferenceOption) => new CodeTypeReference(typeName, codeTypeReferenceOption);
        public virtual CodeTypeReference CreateTypeReference(string typeName) => new CodeTypeReference(typeName);
        public virtual CodeTypeReference CreateTypeReference(string typeName, CodeTypeReference[] typeArguments) => new CodeTypeReference(typeName, typeArguments);
        public virtual CodeTypeReference CreateTypeReference(CodeTypeParameter typeParameter) => new CodeTypeReference(typeParameter);
        public virtual CodeTypeReference CreateTypeReference(string baseType, int rank) => new CodeTypeReference(baseType, rank);
        public virtual CodeTypeReference CreateTypeReference(CodeTypeReference arrayType, int rank) => new CodeTypeReference(arrayType, rank);
        public virtual CodeTypeReferenceExpression CreateTypeReferenceExpression(CodeTypeReference type) => new CodeTypeReferenceExpression(type);
        public virtual CodeTypeReferenceExpression CreateTypeReferenceExpression(string type) => new CodeTypeReferenceExpression(type);
        public virtual CodeTypeReferenceExpression CreateTypeReferenceExpression(Type type) => new CodeTypeReferenceExpression(type);
        public virtual CodeVariableDeclarationStatement CreateVariableDeclarationStatement(CodeTypeReference type, string name) => new CodeVariableDeclarationStatement(type, name);
        public virtual CodeVariableDeclarationStatement CreateVariableDeclarationStatement(string type, string name) => new CodeVariableDeclarationStatement(type, name);
        public virtual CodeVariableDeclarationStatement CreateVariableDeclarationStatement(Type type, string name) => new CodeVariableDeclarationStatement(type, name);
        public virtual CodeVariableDeclarationStatement CreateVariableDeclarationStatement(CodeTypeReference type, string name, CodeExpression initExpression) => new CodeVariableDeclarationStatement(type, name, initExpression);
        public virtual CodeVariableDeclarationStatement CreateVariableDeclarationStatement(string type, string name, CodeExpression initExpression) => new CodeVariableDeclarationStatement(type, name, initExpression);
        public virtual CodeVariableDeclarationStatement CreateVariableDeclarationStatement(Type type, string name, CodeExpression initExpression) => new CodeVariableDeclarationStatement(type, name, initExpression);
        public virtual CodeVariableReferenceExpression CreateVariableReferenceExpression(string variableName) => new CodeVariableReferenceExpression(variableName);

    }
}
