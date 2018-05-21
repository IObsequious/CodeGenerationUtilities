using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.CodeAnalysis.MSBuild.Generators
{
    public class CodeGenerator
    {
        private readonly IndentedTextWriter Writer;

        private CodeTypeMember CurrentMember;

        private CodeTypeDeclaration CurrentType;

        private bool inNestedBinary;

        public CodeGenerator()
        {
            Writer = new IndentedTextWriter(new StringWriter());
        }

        public static string GenerateCodeCompileUnit(CodeCompileUnit unit)
        {
            CodeGenerator generator = new CodeGenerator();
            generator.GenerateCompileUnit(unit);
            return generator.Writer.ToString();
        }

        public void GenerateAccessibilityAndModifiers(MemberAttributes attributes)
        {
            GenerateMemberAccessModifier(attributes);
            GenerateVTableModifier(attributes);
            GenerateMemberScopeModifier(attributes);
        }

        public void GenerateAttributeArgument(CodeAttributeArgument arg)
        {
            if (!string.IsNullOrEmpty(arg.Name))
            {
                Writer.Write(arg.Name);
                Writer.Write("=");
            }
            GenerateExpression(arg.Value);
        }

        public void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
        {
            Writer.Write("]");
        }

        public void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
        {
            Writer.Write("[");
        }

        public void GenerateAttributes(CodeAttributeDeclarationCollection attributes)
        {
            GenerateAttributes(attributes, null, false);
        }

        public void GenerateAttributes(CodeAttributeDeclarationCollection attributes, string prefix)
        {
            GenerateAttributes(attributes, prefix, false);
        }

        public void GenerateAttributes(CodeAttributeDeclarationCollection attributes, string prefix, bool inLine)
        {
            if (attributes.Count == 0) return;
            IEnumerator en = attributes.GetEnumerator();
            bool paramArray = false;

            while (en.MoveNext())
            {
                CodeAttributeDeclaration current = (CodeAttributeDeclaration)en.Current;

                if (current.Name.Equals("system.paramarrayattribute", StringComparison.OrdinalIgnoreCase))
                {
                    paramArray = true;
                    continue;
                }

                GenerateAttributeDeclarationsStart(attributes);
                if (prefix != null)
                {
                    Writer.Write(prefix);
                }

                if (current.AttributeType != null)
                {
                    Writer.Write(GetTypeOutput(current.AttributeType));
                }
                Writer.Write("(");

                bool firstArg = true;
                foreach (CodeAttributeArgument arg in current.Arguments)
                {
                    if (firstArg)
                    {
                        firstArg = false;
                    }
                    else
                    {
                        Writer.Write(", ");
                    }

                    GenerateAttributeArgument(arg);
                }

                Writer.Write(")");
                GenerateAttributeDeclarationsEnd(attributes);
                if (inLine)
                {
                    Writer.Write(" ");
                }
                else
                {
                    Writer.WriteLine();
                }
            }

            if (paramArray)
            {
                if (prefix != null)
                {
                    Writer.Write(prefix);
                }
                Writer.Write("params");

                if (inLine)
                {
                    Writer.Write(" ");
                }
                else
                {
                    Writer.WriteLine();
                }
            }
        }

        public void GenerateCodeArgumentReferenceExpression(CodeArgumentReferenceExpression e)
        {
            Writer.Write(e.ParameterName);
        }

        public void GenerateCodeArrayCreateExpression(CodeArrayCreateExpression e)
        {
            Writer.Write("new ");

            CodeExpressionCollection init = e.Initializers;
            if (init.Count > 0)
            {
                GenerateType(e.CreateType);
                if (e.CreateType.ArrayRank == 0)
                {
                    Writer.Write("[]");
                }
                Writer.WriteLine();
                Writer.WriteLine("{");
                GenerateExpressionList(init, true);
                Writer.WriteLine();
                Writer.Write("}");
            }
            else
            {
                Writer.Write(GetBaseTypeOutput(e.CreateType));

                Writer.Write("[");
                if (e.SizeExpression != null)
                {
                    GenerateExpression(e.SizeExpression);
                }
                else
                {
                    Writer.Write(e.Size);
                }
                Writer.Write("]");
            }
        }

        public void GenerateCodeAssignStatement(CodeAssignStatement e)
        {
            GenerateExpression(e.Left);
            Writer.Write(" = ");
            GenerateExpression(e.Right);
            Writer.WriteLine(";");
        }

        public void GenerateCodeBinaryOperatorExpression(CodeBinaryOperatorExpression e)
        {
            bool indentedExpression = false;
            Writer.Write("(");

            GenerateExpression(e.Left);
            Writer.Write(" ");

            if (e.Left is CodeBinaryOperatorExpression || e.Right is CodeBinaryOperatorExpression)
            {
                if (!inNestedBinary)
                {
                    indentedExpression = true;
                    inNestedBinary = true;
                    Writer.Indent += 3;
                }
                Writer.WriteLine();
            }

            GenerateOperator(e.Operator);

            Writer.Write(" ");
            GenerateExpression(e.Right);

            Writer.Write(")");
            if (indentedExpression)
            {
                Writer.Indent -= 3;
                inNestedBinary = false;
            }
        }

        public void GenerateCodeBreakSwitchSectionStatement(CodeBreakSwitchSectionStatement e)
        {
            GenerateCodeSwitchSectionLabelExpression(e.Label);
            Writer.WriteLine();
            Writer.WriteLine("{");
            Writer.Indent++;
            if (e.BodyStatements.Count > 0)
            {

                GenerateStatements(e.BodyStatements);
            }
            Writer.WriteLine("break;");
            Writer.Indent--;
            Writer.WriteLine("}");
        }

        public void GenerateCodeCastExpression(CodeCastExpression e)
        {
            Writer.Write("((");
            GenerateType(e.TargetType);
            if (e.Expression is CodeVariableReferenceExpression)
            {
                Writer.Write(")");
                GenerateExpression(e.Expression);
                Writer.Write(")");
            }
            else
            {
                Writer.Write(")(");
                GenerateExpression(e.Expression);
                Writer.Write("))");
            }
        }

        public void GenerateCodeCommentStatement(CodeCommentStatement e)
        {
            GenerateComment(e.Comment);
        }

        public void GenerateCodeConditionStatement(CodeConditionStatement e)
        {
            Writer.Write("if (");
            GenerateExpression(e.Condition);
            Writer.WriteLine(")");
            Writer.WriteLine("{");
            Writer.Indent++;
            GenerateStatements(e.TrueStatements);
            Writer.Indent--;

            CodeStatementCollection falseStatemetns = e.FalseStatements;
            if (falseStatemetns.Count > 0)
            {
                Writer.Write("}");
                Writer.WriteLine("else");
                Writer.WriteLine("{");
                Writer.Indent++;
                GenerateStatements(e.FalseStatements);
                Writer.Indent--;
            }
            Writer.WriteLine("}");
        }

        public void GenerateCodeDefaultBreakSwitchSectionStatement(CodeDefaultBreakSwitchSectionStatement e)
        {
            Writer.WriteLine("default:");
            Writer.WriteLine("{");
            Writer.Indent++;
            if (e.BodyStatements.Count > 0)
            {

                GenerateStatements(e.BodyStatements);
            }
            Writer.WriteLine("break;");
            Writer.Indent--;
            Writer.WriteLine("}");
        }

        public void GenerateCodeDefaultReturnSwitchSectionStatement(CodeDefaultReturnSwitchSectionStatement e)
        {
            Writer.WriteLine("default:");
            Writer.WriteLine("{");
            Writer.Indent++;
            if (e.BodyStatements.Count > 0)
            {

                GenerateStatements(e.BodyStatements);
            }
            GenerateCodeMethodReturnStatement(e.ReturnStatement);
            Writer.Indent--;
            Writer.WriteLine("}");
        }

        public void GenerateCodeDefaultValueExpression(CodeDefaultValueExpression e)
        {
            Writer.Write("default(");
            GenerateType(e.Type);
            Writer.Write(")");
        }

        public void GenerateCodeExpressionStatement(CodeExpressionStatement e)
        {
            GenerateExpression(e.Expression);
            Writer.WriteLine(";");
        }

        public void GenerateCodeFallThroughSwitchSectionStatement(CodeFallThroughSwitchSectionStatement e)
        {
            GenerateCodeSwitchSectionLabelExpression(e.Label);
            Writer.WriteLine();
        }

        public void GenerateCodeMethodInvokeExpression(CodeMethodInvokeExpression e)
        {
            GenerateCodeMethodReferenceExpression(e.Method);
            Writer.Write("(");
            GenerateExpressionList(e.Parameters, false);
            Writer.Write(")");
        }

        public void GenerateCodeMethodReferenceExpression(CodeMethodReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                if (e.TargetObject is CodeVariableReferenceExpression variableReference)
                {
                    GenerateCodeVariableReferenceExpression(variableReference);
                }
                else if (e.TargetObject is CodeBinaryOperatorExpression binaryExpression)
                {
                    Writer.Write("(");
                    GenerateExpression(binaryExpression);
                    Writer.Write(")");
                }
                else
                {
                    GenerateExpression(e.TargetObject);
                    if (!(e.TargetObject is CodeThisReferenceExpression))
                    {
                        if (CurrentMember.Name == "Grammar")
                        {

                        }
                    }
                }
                Writer.Write(".");
            }
            Writer.Write(e.MethodName);

            if (e.TypeArguments.Count > 0)
            {
                Writer.Write(GetTypeArgumentsOutput(e.TypeArguments));
            }
        }

        public void GenerateCodeMethodReturnStatement(CodeMethodReturnStatement e)
        {
            Writer.Write("return ");
            GenerateExpression(e.Expression);
            Writer.WriteLine(';');
        }

        public void GenerateCodePropertyReferenceExpression(CodePropertyReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                GenerateExpression(e.TargetObject);
                Writer.Write(".");
            }
            Writer.Write(e.PropertyName);
        }

        public void GenerateCodeReturnValueSwitchSectionStatement(CodeReturnValueSwitchSectionStatement e)
        {
            GenerateCodeSwitchSectionLabelExpression(e.Label);
            if (e.SingleLine)
            {
                GenerateCodeMethodReturnStatement(e.ReturnStatement);
            }
            else
            {
                Writer.WriteLine();
                Writer.WriteLine("{");
                Writer.Indent++;
                if (e.BodyStatements.Count > 0)
                {

                    GenerateStatements(e.BodyStatements);
                }
                GenerateCodeMethodReturnStatement(e.ReturnStatement);
                Writer.Indent--;
                Writer.WriteLine("}");
            }
        }

        public void GenerateCodeSnippetStatement(CodeSnippetStatement e)
        {
            Writer.Write(e.Value);
        }

        public void GenerateCodeSnippetTypeMember(CodeSnippetTypeMember e)
        {
            Writer.Write(e.Text);
        }

        public void GenerateCodeSwitchSectionLabelExpression(CodeSwitchSectionLabelExpression e)
        {
            Writer.Write("case ");
            GenerateExpression(e.Expression);
            Writer.Write(": ");
        }

        public void GenerateCodeSwitchStatement(CodeSwitchStatement e)
        {
            Writer.Write("switch(");
            GenerateExpression(e.CheckExpression);
            Writer.WriteLine(")");
            Writer.WriteLine("{");
            Writer.Indent++;
            GenerateSwitchSections(e.Sections);
            Writer.Indent--;
            Writer.WriteLine("}");
        }

        public void GenerateCodeThisReferenceExpression(CodeThisReferenceExpression e)
        {
            Writer.Write("this");
        }

        public void GenerateCodeThrowExceptionStatement(CodeThrowExceptionStatement e)
        {
            Writer.Write("throw");
            if (e.ToThrow != null)
            {
                Writer.Write(" ");
                GenerateExpression(e.ToThrow);
            }
            Writer.WriteLine(";");
        }

        public void GenerateCodeTypeOfExpression(CodeTypeOfExpression e)
        {
            Writer.Write("typeof(");
            GenerateType(e.Type);
            Writer.Write(")");
        }

        public void GenerateCodeVariableDeclarationStatement(CodeVariableDeclarationStatement e)
        {
            GenerateTypeNamePair(e.Type, e.Name);
            if (e.InitExpression != null)
            {
                Writer.Write(" = ");
                GenerateExpression(e.InitExpression);
            }
            Writer.WriteLine(";");
        }

        public void GenerateCodeVariableReferenceExpression(CodeVariableReferenceExpression e)
        {
            Writer.Write(e.VariableName);
        }

        public void GenerateComment(CodeComment e)
        {
            Writer.Write(e.DocComment ? "/// <summary>" : "// ");
            Writer.Write(e.Text);
            Writer.WriteLine(e.DocComment ? "</summary>" : "");
        }

        public void GenerateComments(CodeCommentStatementCollection e)
        {
            foreach (CodeCommentStatement commentStatement in e)
            {
                GenerateCodeCommentStatement(commentStatement);
            }
        }

        public void GenerateCompileUnit(CodeCompileUnit unit)
        {
            Writer.WriteLine("//------------------------------------------------------------------------------");
            Writer.WriteLine("// <auto-generated>");
            Writer.WriteLine("// This code was generated by a tool.");
            Writer.WriteLine("// </auto-generated>");
            Writer.WriteLine("//------------------------------------------------------------------------------");
            Writer.WriteLine();

            foreach (CodeNamespaceImport import in ExtractImports(unit))
                Writer.WriteUsing(import.Namespace);

            foreach (CodeNamespace ns in unit.Namespaces)
            {
                GenerateNamespace(ns);
            }
        }

        public void GenerateConstructor(CodeConstructor e)
        {
            CurrentMember = e;

            Writer.WriteLine();
            GenerateAccessibilityAndModifiers(e.Attributes);
            Writer.Write(e.Name);
            Writer.OpenParen(false);
            GenerateParameterDeclarationExpressions(e.Parameters);
            Writer.WriteLine(')');
            Writer.PushIndent();

            CodeExpressionCollection baseArgs = e.BaseConstructorArgs;
            CodeExpressionCollection thisArgs = e.ChainedConstructorArgs;

            if (baseArgs.Count > 0)
            {
                Writer.Write(" : ");
                Writer.PushIndent();
                Writer.PushIndent();
                Writer.Write("base(");
                GenerateExpressions(baseArgs, false);
                Writer.Write(")");
                Writer.PopIndent();
                Writer.PopIndent();
            }

            if (thisArgs.Count > 0)
            {
                Writer.WriteLine(" : ");
                Writer.PushIndent();
                Writer.PushIndent();
                Writer.Write("this(");
                GenerateExpressions(thisArgs, false);
                Writer.Write(")");
                Writer.PopIndent();
                Writer.PopIndent();
            }
            Writer.PopIndent();
            Writer.WriteLine();
            Writer.OpenBrace();
            GenerateStatements(e.Statements);
            Writer.CloseBrace();

            CurrentMember = null;
        }

        public void GenerateExpression(CodeExpression e)
        {
            switch (e)
            {
                case CodeParameterDeclarationExpression parameterDeclarationExpression:
                    GenerateParameterDeclarationExpression(parameterDeclarationExpression);
                    break;
                case CodeMethodReferenceExpression methodReferenceExpression:
                    GenerateCodeMethodReferenceExpression(methodReferenceExpression);
                    break;
                case CodeThisReferenceExpression thisReferenceExpression:
                    GenerateCodeThisReferenceExpression(thisReferenceExpression);
                    break;
                case CodePropertyReferenceExpression propertyReference:
                    GenerateCodePropertyReferenceExpression(propertyReference);
                    break;
                case CodeCastExpression castExpression:
                    GenerateCodeCastExpression(castExpression);
                    break;
                case CodeObjectCreateExpression objectCreateExpression:
                    GenerateObjectCreateExpression(objectCreateExpression);
                    break;
                case CodePrimitiveExpression primativeExpression:
                    GeneratePrimitiveExpression(primativeExpression);
                    break;
                case CodeMethodInvokeExpression invokeExpression:
                    GenerateCodeMethodInvokeExpression(invokeExpression);
                    break;
                case CodeVariableReferenceExpression variableReference:
                    GenerateCodeVariableReferenceExpression(variableReference);
                    break;
                case CodeBinaryOperatorExpression binaryOperator:
                    GenerateCodeBinaryOperatorExpression(binaryOperator);
                    break;
                case CodeDefaultValueExpression defaultValue:
                    GenerateCodeDefaultValueExpression(defaultValue);
                    break;
                case CodeArrayCreateExpression arrayCreateExpression:
                    GenerateCodeArrayCreateExpression(arrayCreateExpression);
                    break;
                case CodeTypeOfExpression typeofExpression:
                    GenerateCodeTypeOfExpression(typeofExpression);
                    break;
                case CodeArgumentReferenceExpression argumentReference:
                    GenerateCodeArgumentReferenceExpression(argumentReference);
                    break;
                case CodeSwitchSectionLabelExpression switchSectionLabel:
                    GenerateCodeSwitchSectionLabelExpression(switchSectionLabel);
                    break;
            }
        }

        public void GenerateExpressionList(CodeExpressionCollection expressions, bool newlineBetweenItems)
        {
            bool first = true;
            IEnumerator en = expressions.GetEnumerator();
            Writer.Indent++;
            while (en.MoveNext())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    if (newlineBetweenItems)
                        Writer.WriteLine(",");
                    else
                        Writer.Write(", ");
                }
                GenerateExpression((CodeExpression)en.Current);
            }
            Writer.Indent--;
        }

        public void GenerateExpressions(CodeExpressionCollection expressions, bool newlineBetweenItems)
        {
            bool first = true;
            IEnumerator en = expressions.GetEnumerator();
            Writer.PushIndent();
            while (en.MoveNext())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    if (newlineBetweenItems)
                        Writer.WriteLine(',');
                    else
                        Writer.Write(", ");
                }
                GenerateExpression((CodeExpression)en.Current);
            }
            Writer.PopIndent();
        }

        public void GenerateFieldDeclaration(CodeMemberField e)
        {
            CurrentMember = e;
            if ((CurrentType is CodeTypeDelegate) || CurrentType.IsInterface) return;

            if (CurrentType.IsEnum)
            {
                if (e.CustomAttributes.Count > 0)
                {
                    GenerateAttributes(e.CustomAttributes);
                }
                Writer.Write(e.Name);
                if (e.InitExpression != null)
                {
                    Writer.Write(" = ");
                    GenerateExpression(e.InitExpression);
                }
                Writer.WriteLine(",");
            }
            else
            {
                if (e.CustomAttributes.Count > 0)
                {
                    GenerateAttributes(e.CustomAttributes);
                }
                GenerateAccessibilityAndModifiers(e.Attributes);

                if (GetUserDataValue<bool>(e.UserData, "ReadOnly"))
                {
                    Writer.Write("readonly ");
                }

                GenerateType(e.Type);
                Writer.Write(' ');
                Writer.Write(e.Name);
                Writer.WriteLine(';');
            }

            CurrentMember = null;
        }

        public virtual void GenerateIdentifier(string ident)
        {
            Writer.Write(ident);
        }

        public void GenerateMemberAccessModifier(MemberAttributes attributes)
        {
            switch (attributes & MemberAttributes.AccessMask)
            {
                case MemberAttributes.Assembly:
                    Writer.Write("internal ");
                    break;
                case MemberAttributes.FamilyAndAssembly:
                    Writer.Write("internal ");
                    break;
                case MemberAttributes.Family:
                    Writer.Write("protected ");
                    break;
                case MemberAttributes.FamilyOrAssembly:
                    Writer.Write("protected internal ");
                    break;
                case MemberAttributes.Private:
                    Writer.Write("private ");
                    break;
                case MemberAttributes.Public:
                    Writer.Write("public ");
                    break;
            }

            if (GetUserDataValue<bool>(CurrentMember.UserData, "IsVirtual"))
            {
                Writer.Write("virtual ");
            }
        }

        public void GenerateMemberScopeModifier(MemberAttributes attributes)
        {
            switch (attributes & MemberAttributes.ScopeMask)
            {
                case MemberAttributes.Abstract:
                    Writer.Write("abstract ");
                    break;
                case MemberAttributes.Final:
                    Writer.Write("sealed ");
                    break;
                case MemberAttributes.Static:
                    Writer.Write("static ");
                    break;
                case MemberAttributes.Override:
                    Writer.Write("override ");
                    break;
            }
        }

        public void GenerateMethod(CodeMemberMethod e)
        {
            CurrentMember = e;
            Writer.WriteLine();
            if (!(CurrentType.IsClass || CurrentType.IsStruct || CurrentType.IsEnum)) return;

            GenerateComments(e.Comments);

            if (!CurrentType.IsInterface)
            {
                if (e.PrivateImplementationType == null)
                {
                    GenerateMemberAccessModifier(e.Attributes);
                    GenerateVTableModifier(e.Attributes);
                    GenerateMemberScopeModifier(e.Attributes);
                }
            }
            else
            {
                GenerateVTableModifier(e.Attributes);
            }
            GenerateType(e.ReturnType);
            Writer.Write(" ");
            if (e.PrivateImplementationType != null)
            {
                Writer.Write(GetBaseTypeOutput(e.PrivateImplementationType));
                Writer.Write(".");
            }
            Writer.Write(e.Name);

            GenerateTypeParameters(e.TypeParameters);

            Writer.Write("(");
            GenerateParameterDeclarationExpressions(e.Parameters);
            Writer.Write(")");

            if (!CurrentType.IsInterface
                && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
            {
                Writer.WriteLine();

                Writer.WriteLine("{");

                Writer.Indent++;
                int indent = Writer.Indent;

                GenerateStatements(e.Statements);

                Writer.Indent--;
                Writer.WriteLine();
                Writer.WriteLine("}");
            }
            else
            {
                Writer.WriteLine(";");
            }

            CurrentMember = null;
        }

        public void GenerateNamespace(CodeNamespace ns)
        {
            if (!string.IsNullOrEmpty(ns.Name))
            {
                Writer.WriteLine();
                Writer.WriteLine($"namespace {ns.Name}");
                Writer.OpenBrace();

                foreach (CodeTypeDeclaration type in ns.Types)
                {
                    GenerateTypeDeclaration(type);
                }

                Writer.CloseBrace();
            }
            else
            {
                Writer.Write(string.Empty);
            }
        }

        public void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
        {
            Writer.Write("new ");
            GenerateType(e.CreateType);
            Writer.OpenParen(false);
            GenerateParameters(e.Parameters);
            Writer.CloseParen(false);
        }

        public void GenerateOperator(CodeBinaryOperatorType op)
        {
            switch (op)
            {
                case CodeBinaryOperatorType.Add:
                    Writer.Write("+");
                    break;
                case CodeBinaryOperatorType.Subtract:
                    Writer.Write("-");
                    break;
                case CodeBinaryOperatorType.Multiply:
                    Writer.Write("*");
                    break;
                case CodeBinaryOperatorType.Divide:
                    Writer.Write("/");
                    break;
                case CodeBinaryOperatorType.Modulus:
                    Writer.Write("%");
                    break;
                case CodeBinaryOperatorType.Assign:
                    Writer.Write("=");
                    break;
                case CodeBinaryOperatorType.IdentityInequality:
                    Writer.Write("!=");
                    break;
                case CodeBinaryOperatorType.IdentityEquality:
                    Writer.Write("==");
                    break;
                case CodeBinaryOperatorType.ValueEquality:
                    Writer.Write("==");
                    break;
                case CodeBinaryOperatorType.BitwiseOr:
                    Writer.Write("|");
                    break;
                case CodeBinaryOperatorType.BitwiseAnd:
                    Writer.Write("&");
                    break;
                case CodeBinaryOperatorType.BooleanOr:
                    Writer.Write("||");
                    break;
                case CodeBinaryOperatorType.BooleanAnd:
                    Writer.Write("&&");
                    break;
                case CodeBinaryOperatorType.LessThan:
                    Writer.Write("<");
                    break;
                case CodeBinaryOperatorType.LessThanOrEqual:
                    Writer.Write("<=");
                    break;
                case CodeBinaryOperatorType.GreaterThan:
                    Writer.Write(">");
                    break;
                case CodeBinaryOperatorType.GreaterThanOrEqual:
                    Writer.Write(">=");
                    break;
            }
        }

        public virtual void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
        {
            GenerateTypeNamePair(e.Type, e.Name);
        }

        public virtual void GenerateParameterDeclarationExpressions(CodeParameterDeclarationExpressionCollection parameters)
        {
            bool first = true;
            bool multiline = parameters.Count > 5;
            if (multiline)
            {
                Writer.Indent += 3;
            }
            IEnumerator en = parameters.GetEnumerator();
            while (en.MoveNext())
            {
                CodeParameterDeclarationExpression current = (CodeParameterDeclarationExpression)en.Current;
                if (first)
                {
                    first = false;
                }
                else
                {
                    Writer.Write(", ");
                }
                if (multiline)
                {
                    Writer.WriteLine();
                }
                GenerateParameterDeclarationExpression(current);
            }
            if (multiline)
            {
                Writer.Indent -= 3;
            }
        }

        public virtual void GenerateParameters(CodeExpressionCollection parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                CodeExpression expression = parameters[i];
                GenerateExpression(expression);
                if (i != parameters.Count - 1)
                {
                    Writer.Write(", ");
                }
            }
        }

        public void GenerateParameters(CodeParameterDeclarationExpressionCollection parameters)
        {

            GenerateParameterDeclarationExpressions(parameters);
        }

        public void GeneratePrimitiveExpression(CodePrimitiveExpression e)
        {
            if (e.Value is string str)
            {
                Writer.Write('"');
                Writer.Write(str);
                Writer.Write('"');
            }
            else if (e.Value is bool b)
            {
                if (b)
                {
                    Writer.Write("true");
                }
                else
                {
                    Writer.Write("false");
                }
            }
            else
            {
                Writer.Write(e.Value);
            }
        }

        public void GenerateProperty(CodeMemberProperty e)
        {
            CurrentMember = e;

            Writer.WriteLine();

            if (!CurrentType.IsInterface)
            {
                GenerateAccessibilityAndModifiers(e.Attributes);
            }
            GenerateType(e.Type);
            Writer.Write(" ");
            Writer.Write(e.Name);

            if (e.GetStatements.Count == 0)
            {
                if (e.HasGet && !e.HasSet)
                {
                    Writer.WriteLine(" { get; }");
                }
                else
                {
                    if (e.GetStatements.Count == 0 && e.SetStatements.Count == 0)
                        Writer.WriteLine(" { get; set; }");
                }
            }
            else
            {
                Writer.WriteLine();
                Writer.OpenBrace();
                Writer.WriteLine("get");
                Writer.OpenBrace();
                foreach (CodeStatement statement in e.GetStatements)
                {
                    GenerateStatement(statement);
                }
                Writer.CloseBrace();
                Writer.CloseBrace();
            }
            CurrentMember = null;
        }

        public void GenerateStatement(CodeStatement statement)
        {
            switch (statement)
            {
                case CodeSnippetStatement snippetStatement:
                    GenerateCodeSnippetStatement(snippetStatement);
                    break;
                case CodeMethodReturnStatement returnstatement:
                    GenerateCodeMethodReturnStatement(returnstatement);
                    break;
                case CodeExpressionStatement expressionStatement:
                    GenerateCodeExpressionStatement(expressionStatement);
                    break;
                case CodeConditionStatement conditionStatement:
                    GenerateCodeConditionStatement(conditionStatement);
                    break;
                case CodeVariableDeclarationStatement variableStatement:
                    GenerateCodeVariableDeclarationStatement(variableStatement);
                    break;
                case CodeAssignStatement assignStatement:
                    GenerateCodeAssignStatement(assignStatement);
                    break;
                case CodeThrowExceptionStatement exceptionStatement:
                    GenerateCodeThrowExceptionStatement(exceptionStatement);
                    break;
                case CodeSwitchStatement switchStatement:
                    GenerateCodeSwitchStatement(switchStatement);
                    break;
            }
        }

        public void GenerateStatements(CodeStatementCollection e)
        {
            foreach (CodeStatement statement in e)
            {
                GenerateStatement(statement);
            }
        }

        public void GenerateSwitchSections(CodeSwitchSectionStatementCollection e)
        {
            foreach (var section in e)
            {
                switch (section)
                {
                    case CodeReturnValueSwitchSectionStatement returnValueSection:
                        GenerateCodeReturnValueSwitchSectionStatement(returnValueSection);
                        break;
                    case CodeBreakSwitchSectionStatement breakSection:
                        GenerateCodeBreakSwitchSectionStatement(breakSection);
                        break;
                    case CodeFallThroughSwitchSectionStatement fallThroughSection:
                        GenerateCodeFallThroughSwitchSectionStatement(fallThroughSection);
                        break;
                    case CodeDefaultBreakSwitchSectionStatement defaultBreakSection:
                        GenerateCodeDefaultBreakSwitchSectionStatement(defaultBreakSection);
                        break;
                    case CodeDefaultReturnSwitchSectionStatement defaultReturnSection:
                        GenerateCodeDefaultReturnSwitchSectionStatement(defaultReturnSection);
                        break;
                }
            }
        }

        public void GenerateType(CodeTypeReference e)
        {
            Writer.Write(GetTypeOutput(e));
        }

        public void GenerateTypeDeclaration(CodeTypeDeclaration e)
        {
            CurrentType = e;
            Writer.WriteLine();
            GenerateComments(e.Comments);
            GenerateTypeSignature(e);
            Writer.WriteLine();
            Writer.OpenBrace();

            foreach (CodeTypeMember member in e.Members)
            {
                switch (member)
                {
                    case CodeSnippetTypeMember snippetMember: GenerateCodeSnippetTypeMember(snippetMember); break;
                    case CodeMemberField codeField: GenerateFieldDeclaration(codeField); break;
                    case CodeTypeDeclaration typeDeclaration: GenerateTypeDeclaration(typeDeclaration); break;
                    case CodeMemberProperty property: GenerateProperty(property); break;
                    case CodeConstructor constructor: GenerateConstructor(constructor); break;
                    case CodeMemberMethod method: GenerateMethod(method); break;
                }

                CurrentType = e;
            }

            Writer.CloseBrace();
            CurrentType = null;
        }

        public void GenerateTypeNamePair(CodeTypeReference typeRef, string name)
        {
            GenerateType(typeRef);
            Writer.Write(" ");
            Writer.Write(name);
        }

        public void GenerateTypeParameterConstraints(CodeTypeParameterCollection typeParameters)
        {
            if (typeParameters.Count == 0)
            {
                return;
            }

            for (int i = 0; i < typeParameters.Count; i++)
            {
                Writer.WriteLine();
                Writer.PushIndent();

                bool first = true;
                if (typeParameters[i].Constraints.Count > 0)
                {
                    foreach (CodeTypeReference typeRef in typeParameters[i].Constraints)
                    {
                        if (first)
                        {
                            Writer.Write("where ");
                            Writer.Write(typeParameters[i].Name);
                            Writer.Write(": ");
                            first = false;
                        }
                        else
                        {
                            Writer.Write(", ");
                        }
                        Writer.Write(GetTypeOutput(typeRef));
                    }
                }

                if (typeParameters[i].HasConstructorConstraint)
                {
                    if (first)
                    {
                        Writer.Write("where ");
                        Writer.Write(typeParameters[i].Name);
                        Writer.Write(" : new()");
                    }
                    else
                    {
                        Writer.Write(", new ()");
                    }
                }

                Writer.PopIndent();
            }
        }

        public void GenerateTypeParameters(CodeTypeParameterCollection tp)
        {
            if (tp.Count == 0)
            {
                return;
            }

            Writer.Write('<');
            bool first = true;
            for (int i = 0; i < tp.Count; i++)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Writer.Write(", ");
                }

                Writer.Write(tp[i].Name);
            }

            Writer.Write('>');
        }

        public void GenerateTypeSignature(CodeTypeDeclaration e)
        {
            TypeAttributes attributes = e.TypeAttributes;
            switch (attributes & TypeAttributes.VisibilityMask)
            {
                case TypeAttributes.Public:
                case TypeAttributes.NestedPublic:
                    Writer.Write("public ");
                    break;
                case TypeAttributes.NestedPrivate:
                    Writer.Write("private ");
                    break;
                case TypeAttributes.NestedFamily:
                    Writer.Write("protected ");
                    break;
                case TypeAttributes.NotPublic:
                case TypeAttributes.NestedAssembly:
                case TypeAttributes.NestedFamANDAssem:
                    Writer.Write("internal ");
                    break;
                case TypeAttributes.NestedFamORAssem:
                    Writer.Write("protected internal ");
                    break;
            }

            if (e.Attributes.HasFlag(MemberAttributes.Static))
            {
                Writer.Write("static ");
            }

            if (e.IsStruct)
            {
                if (e.IsPartial)
                {
                    Writer.Write("partial ");
                }
                Writer.Write("struct ");
            }
            else if (e.IsEnum)
            {
                Writer.Write("enum ");
            }
            else
            {
                switch (attributes & TypeAttributes.ClassSemanticsMask)
                {
                    case TypeAttributes.Class:
                        if ((attributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
                        {
                            Writer.Write("sealed ");
                        }
                        if ((attributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
                        {
                            Writer.Write("abstract ");
                        }
                        if (e.IsPartial)
                        {
                            Writer.Write("partial ");
                        }

                        Writer.Write("class ");

                        break;
                    case TypeAttributes.Interface:
                        if (e.IsPartial)
                        {
                            Writer.Write("partial ");
                        }
                        Writer.Write("interface ");
                        break;
                }
            }

            Writer.Write($"{e.Name}");

            if (e.TypeParameters.Count > 0)
                GenerateTypeParameters(e.TypeParameters);

            bool first = true;
            foreach (CodeTypeReference typeRef in e.BaseTypes)
            {
                if (first)
                {
                    Writer.Write(" : ");
                    first = false;
                }
                else
                {
                    Writer.Write(", ");
                }
                GenerateType(typeRef);
            }

            GenerateTypeParameterConstraints(e.TypeParameters);
        }

        public void GenerateVTableModifier(MemberAttributes attributes)
        {
            switch (attributes
                & MemberAttributes.VTableMask)
            {
                case MemberAttributes.New:
                    Writer.Write("new ");
                    break;
            }
        }

        public void GetTypeArgumentsOutput(CodeTypeReferenceCollection typeArguments, int start, int length, StringBuilder sb)
        {
            sb.Append('<');
            bool first = true;
            for (int i = start; i < start + length; i++)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }
                if (i < typeArguments.Count)
                    sb.Append(GetTypeOutput(typeArguments[i]));
            }
            sb.Append('>');
        }

        private List<CodeNamespaceImport> ExtractImports(CodeCompileUnit unit)
        {
            List<CodeNamespaceImport> list = new List<CodeNamespaceImport>();

            foreach (CodeNamespace ns in unit.Namespaces)
            {
                foreach (CodeNamespaceImport import in ns.Imports)
                {
                    list.Add(import);
                }
            }
            return list;
        }

        private string GetBaseTypeOutput(CodeTypeReference typeRef)
        {
            string s = typeRef.BaseType;
            if (s.Length == 0)
            {
                s = "void";
                return s;
            }

            string lowerCaseString = s.ToLower(CultureInfo.InvariantCulture).Trim();

            switch (lowerCaseString)
            {
                case "system.int16":
                    s = "short";
                    break;
                case "system.int32":
                    s = "int";
                    break;
                case "system.int64":
                    s = "long";
                    break;
                case "system.string":
                    s = "string";
                    break;
                case "system.object":
                    s = "object";
                    break;
                case "system.boolean":
                    s = "bool";
                    break;
                case "system.void":
                    s = "void";
                    break;
                case "system.char":
                    s = "char";
                    break;
                case "system.byte":
                    s = "byte";
                    break;
                case "system.uint16":
                    s = "ushort";
                    break;
                case "system.uint32":
                    s = "uint";
                    break;
                case "system.uint64":
                    s = "ulong";
                    break;
                case "system.sbyte":
                    s = "sbyte";
                    break;
                case "system.single":
                    s = "float";
                    break;
                case "system.double":
                    s = "double";
                    break;
                case "system.decimal":
                    s = "decimal";
                    break;
            }
            return s;
        }

        private String GetTypeArgumentsOutput(CodeTypeReferenceCollection typeArguments)
        {
            StringBuilder sb = new StringBuilder(128);
            GetTypeArgumentsOutput(typeArguments, 0, typeArguments.Count, sb);
            return sb.ToString();
        }

        private string GetTypeOutput(CodeTypeReference typeRef)
        {
            string s = String.Empty;

            CodeTypeReference baseTypeRef = typeRef;
            while (baseTypeRef.ArrayElementType != null)
            {
                baseTypeRef = baseTypeRef.ArrayElementType;
            }
            s += GetBaseTypeOutput(baseTypeRef);

            while (typeRef != null && typeRef.ArrayRank > 0)
            {
                char[] results = new char[typeRef.ArrayRank + 1];
                results[0] = '[';
                results[typeRef.ArrayRank] = ']';
                for (int i = 1; i < typeRef.ArrayRank; i++)
                {
                    results[i] = ',';
                }
                s += new string(results);
                typeRef = typeRef.ArrayElementType;
            }

            return s;
        }

        private T GetUserDataValue<T>(IDictionary dictionary, string key)
        {
            if (dictionary.Contains(key))
            {
                return (T)dictionary[key];
            }

            return default(T);
        }
    }
}
