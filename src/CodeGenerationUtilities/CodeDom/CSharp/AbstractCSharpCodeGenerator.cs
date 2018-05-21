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

namespace System.CodeDom.CSharp
{
    public abstract class AbstractCSharpCodeGenerator : CodeGeneratorBase
    {
        private bool generatingForLoop = false;

        protected AbstractCSharpCodeGenerator() : base()
        {
        }

        protected AbstractCSharpCodeGenerator(TextWriter writer) : base(writer)
        {
        }

        protected override string NullToken { get; } = "null";

        public override void GenerateFallThroughSwitchSectionStatement(CodeFallThroughSwitchSectionStatement e)
        {

        }

        public void AppendEscapedChar(StringBuilder b, char value)
        {
            if (b == null)
            {
                Writer.Write("\\u");
                Writer.Write(((int)value).ToString("X4", CultureInfo.InvariantCulture));
            }
            else
            {
                b.Append("\\u");
                b.Append(((int)value).ToString("X4", CultureInfo.InvariantCulture));
            }
        }

        public override string CreateEscapedIdentifier(string value)
        {
            if (IsKeyword(value) || IsPrefixTwoUnderscore(value))
            {
                return "@" + value;
            }
            return value;
        }

        public override string CreateValidIdentifier(string value)
        {
            if (IsPrefixTwoUnderscore(value))
            {
                value = "_" + value;
            }

            while (IsKeyword(value))
            {
                value = "_" + value;
            }

            return value;
        }

        public override void GenerateDefaultValueExpression(CodeDefaultValueExpression e)
        {
            Writer.Write("default(");
            GenerateTypeReference(e.Type);
            Writer.Write(")");
        }

        public void GenerateAccessibilityAndModifiers(MemberAttributes attributes)
        {
            GenerateMemberAccessModifier(attributes);
            GenerateVTableModifier(attributes);
            GenerateMemberScopeModifier(attributes);
        }

        public override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
        {
            OutputIdentifier(e.ParameterName);
        }

        public override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
        {
            Writer.Write("new ");

            CodeExpressionCollection init = e.Initializers;
            if (init.Count > 0)
            {
                GenerateTypeReference(e.CreateType);
                if (e.CreateType.ArrayRank == 0)
                {
                    Writer.Write("[]");
                }
                Writer.WriteLine(" {");
                Indent++;
                OutputExpressionList(init, true );
                Indent--;
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

                int nestedArrayDepth = e.CreateType.ArrayRank;
                for (int i = 0; i < nestedArrayDepth - 1; i++)
                {
                    Writer.Write("[]");
                }
            }
        }

        public override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
        {
            GenerateExpression(e.TargetObject);
            Writer.Write("[");
            bool first = true;
            foreach (CodeExpression exp in e.Indices)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Writer.Write(", ");
                }
                GenerateExpression(exp);
            }
            Writer.Write("]");
        }

        public override void GenerateAssignStatement(CodeAssignStatement e)
        {
            GenerateExpression(e.Left);
            Writer.Write(" = ");
            GenerateExpression(e.Right);
            if (!generatingForLoop)
            {
                Writer.WriteLine(";");
            }
        }

        public override void GenerateAttachEventStatement(CodeAttachEventStatement e)
        {
            GenerateEventReferenceExpression(e.Event);
            Writer.Write(" += ");
            GenerateExpression(e.Listener);
            Writer.WriteLine(";");
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

                OutputAttributeDeclarationsStart(attributes);
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
                OutputAttributeDeclarationsEnd(attributes);
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

        public override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
        {
            Writer.Write("base");
        }

        public override void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e)
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
                    Indent += 3;
                }
                ContinueOnNewLine("");
            }

            OutputOperator(e.Operator);

            Writer.Write(" ");
            GenerateExpression(e.Right);

            Writer.Write(")");
            if (indentedExpression)
            {
                Indent -= 3;
                inNestedBinary = false;
            }
        }

        public override void GenerateCastExpression(CodeCastExpression e)
        {
            Writer.Write("((");
            GenerateTypeReference(e.TargetType);
            Writer.Write(")(");
            GenerateExpression(e.Expression);
            Writer.Write("))");
        }

        public override void GenerateChecksumPragma(CodeChecksumPragma e)
        {
        }

        public override void GenerateComment(CodeComment e)
        {
            string commentLineStart = e.DocComment ? "///" : "//";
            Writer.Write(commentLineStart);
            Writer.Write(" ");

            string value = e.Text;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '\u0000')
                {
                    continue;
                }
                Writer.Write(value[i]);

                if (value[i] == '\r')
                {
                    if (i < value.Length - 1 && value[i + 1] == '\n')
                    {        
                        Writer.Write('\n');
                        i++;
                    }
                    for (int index = 0; index < Indent; ++index)
                    {
                        Writer.Write("    ");
                    }
                    Writer.Write(commentLineStart);
                }
                else if (value[i] == '\n')
                {
                    for (int index = 0; index < Indent; ++index)
                    {
                        Writer.Write("    ");
                    }
                    Writer.Write(commentLineStart);
                }
                else if (value[i] == '\u2028' || value[i] == '\u2029' || value[i] == '\u0085')
                {
                    Writer.Write(commentLineStart);
                }
            }
            Writer.WriteLine();
        }

        public override void GenerateCommentStatement(CodeCommentStatement e)
        {
            if (e.Comment == null)
                throw new ArgumentNullException();
            GenerateComment(e.Comment);
        }

        public override void GenerateCompileUnit(CodeCompileUnit e)
        {
            GenerateCompileUnitStart(e);
            foreach (CodeNamespace n in e.Namespaces)
            {
                GenerateNamespace(n);
            }
            GenerateCompileUnitEnd(e);
        }

        public override void GenerateCompileUnitEnd(CodeCompileUnit e)
        {
            if (e.EndDirectives.Count > 0)
            {
                OutputDirectives(e.EndDirectives);
            }
        }

        public override void GenerateCompileUnitStart(CodeCompileUnit e)
        {
            Writer.WriteLine("//------------------------------------------------------------------------------");
            Writer.WriteLine("// <auto-generated>");
            Writer.WriteLine("//     This code was generated by a tool.");
            Writer.Write("//     Runtime Version: ");
            Writer.WriteLine(Environment.Version.ToString());
            Writer.WriteLine("//");
            Writer.WriteLine("//     Changes to this file may cause incorrect behavior and will be lost if");
            Writer.WriteLine("//     the code is regenerated.");
            Writer.WriteLine("// </auto-generated>");
            Writer.WriteLine("//------------------------------------------------------------------------------");
            Writer.WriteLine("");

            SortedList importList = new SortedList();
            foreach (CodeNamespace nspace in e.Namespaces)
            {
                if (!string.IsNullOrEmpty(nspace.Name))
                {
                    usingsAlreadyWritten = true;

                    foreach (CodeNamespaceImport import in nspace.Imports)
                    {
                        if (!importList.ContainsKey(import.Namespace))
                            importList.Add(import.Namespace, import);
                    }
                }
            }

            foreach (DictionaryEntry entry in importList)
                GenerateNamespaceImport((CodeNamespaceImport)entry.Value);

            Writer.WriteLine("");

            if (e.AssemblyCustomAttributes.Count > 0)
            {
                GenerateAttributes(e.AssemblyCustomAttributes, "assembly: ");
                Writer.WriteLine("");
            }
        }

        public override void GenerateConditionStatement(CodeConditionStatement e)
        {
            Writer.Write("if (");
            GenerateExpression(e.Condition);
            Writer.Write(")");
            OutputStartingBrace();
            Indent++;
            OutputStatements(e.TrueStatements);
            Indent--;

            CodeStatementCollection falseStatemetns = e.FalseStatements;
            if (falseStatemetns.Count > 0)
            {
                Writer.Write("}");
                if (Options.ElseOnClosing)
                {
                    Writer.Write(" ");
                }
                else
                {
                    Writer.WriteLine("");
                }
                Writer.Write("else");
                OutputStartingBrace();
                Indent++;
                OutputStatements(e.FalseStatements);
                Indent--;
            }
            Writer.WriteLine("}");
        }

        public override void GenerateConstructor(CodeConstructor e)
        {
            if (!(IsCurrentClass || IsCurrentStruct)) return;

            if (e.CustomAttributes.Count > 0)
            {
                GenerateAttributes(e.CustomAttributes);
            }

            OutputMemberAccessModifier(e.Attributes);
            OutputIdentifier(CurrentTypeName);
            Writer.Write("(");
            OutputParameters(e.Parameters);
            Writer.Write(")");

            CodeExpressionCollection baseArgs = e.BaseConstructorArgs;
            CodeExpressionCollection thisArgs = e.ChainedConstructorArgs;

            if (baseArgs.Count > 0)
            {
                Writer.WriteLine(" : ");
                Indent++;
                Indent++;
                Writer.Write("base(");
                OutputExpressionList(baseArgs);
                Writer.Write(")");
                Indent--;
                Indent--;
            }

            if (thisArgs.Count > 0)
            {
                Writer.WriteLine(" : ");
                Indent++;
                Indent++;
                Writer.Write("this(");
                OutputExpressionList(thisArgs);
                Writer.Write(")");
                Indent--;
                Indent--;
            }

            OutputStartingBrace();
            Indent++;
            OutputStatements(e.Statements);
            Indent--;
            Writer.WriteLine("}");
        }

        public override void GenerateDecimalValue(decimal d)
        {
            Writer.Write(d.ToString(CultureInfo.InvariantCulture));
            Writer.Write('m');
        }

        public override void GenerateDelegate(CodeTypeDelegate e)
        {
            switch (e.TypeAttributes & TypeAttributes.VisibilityMask)
            {
                case TypeAttributes.Public:
                    Writer.Write("public ");
                    break;
                case TypeAttributes.NotPublic:
                    Writer.Write("internal ");
                    break;
            }

            Writer.Write("delegate ");
            GenerateTypeReference(e.ReturnType);
            Writer.Write(" ");
            OutputIdentifier(e.Name);
            Writer.Write("(");
            OutputParameters(e.Parameters);
            Writer.WriteLine(");");
        }

        public override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
        {
            Writer.Write("new ");
            GenerateTypeReference(e.DelegateType);
            Writer.Write("(");
            GenerateExpression(e.TargetObject);
            Writer.Write(".");
            OutputIdentifier(e.MethodName);
            Writer.Write(")");
        }

        public override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
        {
            if (e.TargetObject != null)
            {
                GenerateExpression(e.TargetObject);
            }
            Writer.Write("(");
            OutputExpressionList(e.Parameters);
            Writer.Write(")");
        }

        public override void GenerateDirective(CodeDirective e)
        {
            switch (e)
            {
                case CodeChecksumPragma codeChecksumPragma:
                    GenerateChecksumPragma(codeChecksumPragma);
                    break;
                case CodeRegionDirective codeRegionDirective:
                    GenerateRegionDirective(codeRegionDirective);
                    break;
            }
        }

        public override void GenerateDoubleValue(double d)
        {
            if (double.IsNaN(d))
            {
                Writer.Write("double.NaN");
            }
            else if (double.IsNegativeInfinity(d))
            {
                Writer.Write("double.NegativeInfinity");
            }
            else if (double.IsPositiveInfinity(d))
            {
                Writer.Write("double.PositiveInfinity");
            }
            else
            {
                Writer.Write(d.ToString("R", CultureInfo.InvariantCulture));
                Writer.Write("D");
            }
        }

        public override void GenerateEntryPointMethod(CodeEntryPointMethod e)
        {
            if (e.CustomAttributes.Count > 0)
            {
                GenerateAttributes(e.CustomAttributes);
            }
            Writer.Write("public static ");
            GenerateTypeReference(e.ReturnType);
            Writer.Write(" Main(params string[] args)");
            OutputStartingBrace();
            Indent++;

            OutputStatements(e.Statements);

            Indent--;
            Writer.WriteLine("}");
        }

        public override void GenerateEvent(CodeMemberEvent e)
        {
            if (IsCurrentDelegate || IsCurrentEnum) return;

            if (e.CustomAttributes.Count > 0)
            {
                GenerateAttributes(e.CustomAttributes);
            }

            if (e.PrivateImplementationType == null)
            {
                OutputMemberAccessModifier(e.Attributes);
            }
            Writer.Write("event ");
            string name = e.Name;
            if (e.PrivateImplementationType != null)
            {
                name = GetBaseTypeOutput(e.PrivateImplementationType) + "." + name;
            }
            OutputTypeNamePair(e.Type, name);
            Writer.WriteLine(";");
        }

        public override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                GenerateExpression(e.TargetObject);
                Writer.Write(".");
            }
            OutputIdentifier(e.EventName);
        }

        public override void GenerateExpressionStatement(CodeExpressionStatement e)
        {
            GenerateExpression(e.Expression);
            if (!generatingForLoop)
            {
                Writer.WriteLine(";");
            }
        }

        public override void GenerateField(CodeMemberField e)
        {
            if (IsCurrentDelegate || IsCurrentInterface) return;

            if (IsCurrentEnum)
            {
                if (e.CustomAttributes.Count > 0)
                {
                    GenerateAttributes(e.CustomAttributes);
                }
                OutputIdentifier(e.Name);
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

                OutputMemberAccessModifier(e.Attributes);

                if (e.UserData.Contains("IsReadOnly") && (bool)e.UserData["IsReadOnly"])
                {
                    Writer.Write("readonly ");
                }
                OutputVTableModifier(e.Attributes);
                OutputFieldScopeModifier(e.Attributes);
                OutputTypeNamePair(e.Type, e.Name);
                if (e.InitExpression != null)
                {
                    Writer.Write(" = ");
                    GenerateExpression(e.InitExpression);
                }
                Writer.WriteLine(";");
            }
        }

        public override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                GenerateExpression(e.TargetObject);
                Writer.Write(".");
            }
            OutputIdentifier(e.FieldName);
        }

        public override void GenerateGotoStatement(CodeGotoStatement e)
        {
            Writer.Write("goto ");
            Writer.Write(e.Label);
            Writer.WriteLine(";");
        }

        public override void GenerateIndexerExpression(CodeIndexerExpression e)
        {
            GenerateExpression(e.TargetObject);
            Writer.Write("[");
            bool first = true;
            foreach (CodeExpression exp in e.Indices)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Writer.Write(", ");
                }
                GenerateExpression(exp);
            }
            Writer.Write("]");
        }

        public override void GenerateIterationStatement(CodeIterationStatement e)
        {
            generatingForLoop = true;
            Writer.Write("for (");
            GenerateStatement(e.InitStatement);
            Writer.Write("; ");
            GenerateExpression(e.TestExpression);
            Writer.Write("; ");
            GenerateStatement(e.IncrementStatement);
            Writer.Write(")");
            OutputStartingBrace();
            generatingForLoop = false;
            Indent++;
            OutputStatements(e.Statements);
            Indent--;
            Writer.WriteLine("}");
        }

        public override void GenerateLabeledStatement(CodeLabeledStatement e)
        {
            Indent--;
            Writer.Write(e.Label);
            Writer.WriteLine(":");
            Indent++;
            if (e.Statement != null)
            {
                GenerateStatement(e.Statement);
            }
        }

        public override void GenerateLinePragmaEnd(CodeLinePragma e)
        {
            Writer.WriteLine();
            Writer.WriteLine("#line default");
            Writer.WriteLine("#line hidden");
        }

        public override void GenerateLinePragmaStart(CodeLinePragma e)
        {
            Writer.WriteLine("");
            Writer.Write("#line ");
            Writer.Write(e.LineNumber);
            Writer.Write(" \"");
            Writer.Write(e.FileName);
            Writer.Write("\"");
            Writer.WriteLine("");
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

            if (GetUserDataValue<bool>(CurrentMember?.UserData, "IsVirtual"))
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

        public override void GenerateMethod(CodeMemberMethod e)
        {
            if (!(IsCurrentClass || IsCurrentStruct || IsCurrentInterface)) return;

            if (e.CustomAttributes.Count > 0)
            {
                GenerateAttributes(e.CustomAttributes);
            }
            if (e.ReturnTypeCustomAttributes.Count > 0)
            {
                GenerateAttributes(e.ReturnTypeCustomAttributes, "return: ");
            }

            if (!IsCurrentInterface)
            {
                if (e.PrivateImplementationType == null)
                {
                    OutputMemberAccessModifier(e.Attributes);
                    OutputVTableModifier(e.Attributes);
                    OutputMemberScopeModifier(e.Attributes);
                }
            }
            else
            {
                OutputVTableModifier(e.Attributes);
            }
            GenerateTypeReference(e.ReturnType);
            Writer.Write(" ");
            if (e.PrivateImplementationType != null)
            {
                Writer.Write(GetBaseTypeOutput(e.PrivateImplementationType));
                Writer.Write(".");
            }
            OutputIdentifier(e.Name);

            GenerateTypeParameters(e.TypeParameters);

            Writer.Write("(");
            OutputParameters(e.Parameters);
            Writer.WriteLine(")");

            OutputTypeParameterConstraints(e.TypeParameters);

            if (!IsCurrentInterface
                && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
            {
                OutputStartingBrace();

                OutputStatements(e.Statements);

                Writer.CloseBrace();
            }
            else
            {
                Writer.WriteLine(";");
            }
        }

        public override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
        {
            GenerateMethodReferenceExpression(e.Method);
            Writer.Write("(");
            OutputExpressionList(e.Parameters);
            Writer.Write(")");
        }

        public override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                if (e.TargetObject is CodeBinaryOperatorExpression)
                {
                    Writer.Write("(");
                    GenerateExpression(e.TargetObject);
                    Writer.Write(")");
                }
                else
                {
                    GenerateExpression(e.TargetObject);
                }
                Writer.Write(".");
            }
            OutputIdentifier(e.MethodName);

            if (e.TypeArguments.Count > 0)
            {
                Writer.Write(GetTypeArgumentsOutput(e.TypeArguments));
            }
        }

        public override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
        {
            Writer.Write("return");
            if (e.Expression != null)
            {
                Writer.Write(" ");
                GenerateExpression(e.Expression);
            }
            Writer.WriteLine(";");
        }

        public override void GenerateNamespace(CodeNamespace e)
        {
            GenerateNamespaceStart(e);
            if (!usingsAlreadyWritten)
                GenerateNamespaceImports(e);
            Writer.WriteLine("");
            GenerateTypes(e);
            GenerateNamespaceEnd(e);
        }

        public override void GenerateNamespaceEnd(CodeNamespace e)
        {
            if (!string.IsNullOrEmpty(e.Name))
            {
                Indent = 1;
                Writer.CloseBrace();
            }
        }

        public override void GenerateNamespaceImport(CodeNamespaceImport e)
        {
            Writer.Write("using ");
            OutputIdentifier(e.Namespace);
            Writer.WriteLine(";");
        }

        public override void GenerateNamespaceStart(CodeNamespace e)
        {
            if (!string.IsNullOrEmpty(e.Name))
            {
                Writer.Write("namespace ");
                OutputIdentifier(e.Name);
                OutputStartingBrace();
            }
        }

        public override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
        {
            Writer.Write("new ");
            GenerateTypeReference(e.CreateType);
            Writer.Write("(");
            OutputExpressionList(e.Parameters);
            Writer.Write(")");
        }

        public void GeneratePrimitiveChar(char c)
        {
            Writer.Write('\'');
            switch (c)
            {
                case '\r':
                    Writer.Write("\\r");
                    break;
                case '\t':
                    Writer.Write("\\t");
                    break;
                case '\"':
                    Writer.Write("\\\"");
                    break;
                case '\'':
                    Writer.Write("\\\'");
                    break;
                case '\\':
                    Writer.Write("\\\\");
                    break;
                case '\0':
                    Writer.Write("\\0");
                    break;
                case '\n':
                    Writer.Write("\\n");
                    break;
                case '\u2028':
                case '\u2029':
                case '\u0084':
                case '\u0085':
                    AppendEscapedChar(null, c);
                    break;

                default:
                    if (char.IsSurrogate(c))
                    {
                        AppendEscapedChar(null, c);
                    }
                    else
                    {
                        Writer.Write(c);
                    }
                    break;
            }
            Writer.Write('\'');
        }

        public override void GeneratePrimitiveExpression(CodePrimitiveExpression e)
        {
            if (e.Value is char)
            {
                GeneratePrimitiveChar((char)e.Value);
            }
            else if (e.Value is sbyte)
            {
                Writer.Write(((sbyte)e.Value).ToString(CultureInfo.InvariantCulture));
            }
            else if (e.Value is ushort)
            {
                Writer.Write(((ushort)e.Value).ToString(CultureInfo.InvariantCulture));
            }
            else if (e.Value is uint)
            {
                Writer.Write(((uint)e.Value).ToString(CultureInfo.InvariantCulture));
                Writer.Write("u");
            }
            else if (e.Value is ulong)
            {
                Writer.Write(((ulong)e.Value).ToString(CultureInfo.InvariantCulture));
                Writer.Write("ul");
            }
            else
            {
                GeneratePrimitiveExpressionBase(e);
            }
        }

        public override void GenerateSwitchStatement(CodeSwitchStatement e)
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

        private void GenerateSwitchSections(CodeSwitchSectionStatementCollection e)
        {
            foreach (var section in e)
            {
                switch (section)
                {
                    case CodeReturnValueSwitchSectionStatement returnValueSection:
                        GenerateReturnValueSwitchSectionStatement(returnValueSection);
                        break;
                    case CodeBreakSwitchSectionStatement breakSection:
                        GenerateBreakSwitchSectionStatement(breakSection);
                        break;
                    case CodeFallThroughSwitchSectionStatement fallThroughSection:
                        GenerateFallThroughSwitchSectionStatement(fallThroughSection);
                        break;
                    case CodeDefaultBreakSwitchSectionStatement defaultBreakSection:
                        GenerateDefaultBreakSwitchSectionStatement(defaultBreakSection);
                        break;
                    case CodeDefaultReturnSwitchSectionStatement defaultReturnSection:
                        GenerateDefaultReturnSwitchSectionStatement(defaultReturnSection);
                        break;
                }
            }
        }

        public override void GenerateReturnValueSwitchSectionStatement(CodeReturnValueSwitchSectionStatement e)
        {
            GenerateSwitchSectionLabelExpression(e.Label);
            if (e.SingleLine)
            {
                GenerateMethodReturnStatement(e.ReturnStatement);
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
                GenerateMethodReturnStatement(e.ReturnStatement);
                Writer.Indent--;
                Writer.WriteLine("}");
            }
        }

        public override void GenerateBreakSwitchSectionStatement(CodeBreakSwitchSectionStatement e)
        {
            GenerateSwitchSectionLabelExpression(e.Label);
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

        public override void GenerateDefaultBreakSwitchSectionStatement(CodeDefaultBreakSwitchSectionStatement e)
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

        public override void GenerateDefaultReturnSwitchSectionStatement(CodeDefaultReturnSwitchSectionStatement e)
        {
            Writer.WriteLine("default:");
            Writer.WriteLine("{");
            Writer.Indent++;
            if (e.BodyStatements.Count > 0)
            {

                GenerateStatements(e.BodyStatements);
            }
            GenerateMethodReturnStatement(e.ReturnStatement);
            Writer.Indent--;
            Writer.WriteLine("}");
        }

        private void GenerateStatements(CodeStatementCollection e)
        {
            foreach (CodeStatement statement in e)
            {
                GenerateStatement(statement);
            }
        }


        public override void GenerateSwitchSectionLabelExpression(CodeSwitchSectionLabelExpression e)
        {
            Writer.Write("case ");
            GenerateExpression(e.Expression);
            Writer.Write(": ");
        }

        public void GeneratePrimitiveExpressionBase(CodePrimitiveExpression e)
        {
            if (e.Value == null)
            {
                Writer.Write(NullToken);
            }
            else if (e.Value is string)
            {
                Writer.Write(QuoteSnippetString((string)e.Value));
            }
            else if (e.Value is char)
            {
                Writer.Write("'" + e.Value.ToString() + "'");
            }
            else if (e.Value is byte)
            {
                Writer.Write(((byte)e.Value).ToString(CultureInfo.InvariantCulture));
            }
            else if (e.Value is short)
            {
                Writer.Write(((short)e.Value).ToString(CultureInfo.InvariantCulture));
            }
            else if (e.Value is int)
            {
                Writer.Write(((int)e.Value).ToString(CultureInfo.InvariantCulture));
            }
            else if (e.Value is long)
            {
                Writer.Write(((long)e.Value).ToString(CultureInfo.InvariantCulture));
            }
            else if (e.Value is float)
            {
                GenerateSingleFloatValue((float)e.Value);
            }
            else if (e.Value is double)
            {
                GenerateDoubleValue((double)e.Value);
            }
            else if (e.Value is decimal)
            {
                GenerateDecimalValue((decimal)e.Value);
            }
            else if (e.Value is bool)
            {
                if ((bool)e.Value)
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
                throw new ArgumentException();
            }
        }

        public override void GenerateProperty(CodeMemberProperty e)
        {

            CurrentMember = e;

            if (!CurrentClass.IsInterface)
            {
                GenerateAccessibilityAndModifiers(e.Attributes);
            }
            GenerateTypeReference(e.Type);
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

        public override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
        {
            if (e.TargetObject != null)
            {
                GenerateExpression(e.TargetObject);
                Writer.Write(".");
            }
            OutputIdentifier(e.PropertyName);
        }

        public override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
        {
            Writer.Write("value");
        }

        public override void GenerateRegionDirective(CodeRegionDirective e)
        {
            switch (e.RegionMode)
            {
                case CodeRegionMode.Start:
                    Writer.WriteLine("#region " + e.RegionText);
                    break;
                case CodeRegionMode.End:
                    Writer.WriteLine("#endregion");
                    break;
            }
        }

        public override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
        {
            GenerateEventReferenceExpression(e.Event);
            Writer.Write(" -= ");
            GenerateExpression(e.Listener);
            Writer.WriteLine(";");
        }

        public override void GenerateSingleFloatValue(float s)
        {
            if (float.IsNaN(s))
            {
                Writer.Write("float.NaN");
            }
            else if (float.IsNegativeInfinity(s))
            {
                Writer.Write("float.NegativeInfinity");
            }
            else if (float.IsPositiveInfinity(s))
            {
                Writer.Write("float.PositiveInfinity");
            }
            else
            {
                Writer.Write(s.ToString(CultureInfo.InvariantCulture));
                Writer.Write('F');
            }
        }

        public override void GenerateSnippetExpression(CodeSnippetExpression e)
        {
            Writer.Write(e.Value);
        }

        public override void GenerateSnippetMember(CodeSnippetTypeMember e)
        {
            Writer.Write(e.Text);
        }

        public override void GenerateSnippetStatement(CodeSnippetStatement e)
        {
            Writer.WriteLine(e.Value);
        }

        public override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
        {
            Writer.Write("this");
        }

        public override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
        {
            Writer.Write("throw");
            if (e.ToThrow != null)
            {
                Writer.Write(" ");
                GenerateExpression(e.ToThrow);
            }
            Writer.WriteLine(";");
        }

        public override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
        {
            Writer.Write("try");
            OutputStartingBrace();
            Indent++;
            OutputStatements(e.TryStatements);
            Indent--;
            CodeCatchClauseCollection catches = e.CatchClauses;
            if (catches.Count > 0)
            {
                IEnumerator en = catches.GetEnumerator();
                while (en.MoveNext())
                {
                    Writer.Write("}");
                    if (Options.ElseOnClosing)
                    {
                        Writer.Write(" ");
                    }
                    else
                    {
                        Writer.WriteLine("");
                    }
                    CodeCatchClause current = (CodeCatchClause)en.Current;
                    Writer.Write("catch (");
                    GenerateTypeReference(current.CatchExceptionType);
                    Writer.Write(" ");
                    OutputIdentifier(current.LocalName);
                    Writer.Write(")");
                    OutputStartingBrace();
                    Indent++;
                    OutputStatements(current.Statements);
                    Indent--;
                }
            }

            CodeStatementCollection finallyStatements = e.FinallyStatements;
            if (finallyStatements.Count > 0)
            {
                Writer.Write("}");
                if (Options.ElseOnClosing)
                {
                    Writer.Write(" ");
                }
                else
                {
                    Writer.WriteLine("");
                }
                Writer.Write("finally");
                OutputStartingBrace();
                Indent++;
                OutputStatements(finallyStatements);
                Indent--;
            }
            Writer.WriteLine("}");
        }

        public override void GenerateTypeConstructor(CodeTypeConstructor e)
        {
            if (!(IsCurrentClass || IsCurrentStruct)) return;

            if (e.CustomAttributes.Count > 0)
            {
                GenerateAttributes(e.CustomAttributes);
            }
            Writer.Write("static ");
            Writer.Write(CurrentTypeName);
            Writer.Write("()");
            OutputStartingBrace();
            Indent++;
            OutputStatements(e.Statements);
            Indent--;
            Writer.WriteLine("}");
        }

        public override void GenerateTypeDeclaration(CodeTypeDeclaration e)
        {
            ((ICodeGenerator)this).GenerateCodeFromType(e, Writer, Options);
        }

        public override void GenerateTypeEnd(CodeTypeDeclaration e)
        {
            if (!IsCurrentDelegate)
            {
                Writer.CloseBrace();
            }
        }

        public override void GenerateTypeOfExpression(CodeTypeOfExpression e)
        {
            Writer.Write("typeof(");
            GenerateTypeReference(e.Type);
            Writer.Write(")");
        }

        public override void GenerateTypeParameter(CodeTypeParameter e)
        {
            Writer.Write(e.Name);
        }

        public override void GenerateTypeReference(CodeTypeReference typeRef)
        {
            Writer.Write(GetTypeOutput(typeRef));
        }

        public override void GenerateTypeReferenceExpression(CodeTypeReferenceExpression e)
        {
            GenerateTypeReference(e.Type);
        }

        public override void GenerateTypeStart(CodeTypeDeclaration e)
        {
            if (e.CustomAttributes.Count > 0)
            {
                GenerateAttributes(e.CustomAttributes);
            }

            if (IsCurrentDelegate)
            {
                GenerateDelegate((CodeTypeDelegate)e);
            }
            else
            {
                OutputTypeAttributes(e);

                OutputIdentifier(e.Name);

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
                    GenerateTypeReference(typeRef);
                }

                OutputTypeParameterConstraints(e.TypeParameters);

                Writer.OpenBrace();
            }
        }

        public override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
        {
            OutputTypeNamePair(e.Type, e.Name);
            if (e.InitExpression != null)
            {
                Writer.Write(" = ");
                GenerateExpression(e.InitExpression);
            }
            if (!generatingForLoop)
            {
                Writer.WriteLine(";");
            }
        }

        public override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
        {
            OutputIdentifier(e.VariableName);
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

        public string GetBaseTypeOutput(CodeTypeReference typeRef)
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
                default:
                    StringBuilder sb = new StringBuilder(s.Length + 10);
                    if ((typeRef.Options & CodeTypeReferenceOptions.GlobalReference) != 0)
                    {
                        sb.Append("global::");
                    }

                    string baseType = typeRef.BaseType;

                    int lastIndex = 0;
                    int currentTypeArgStart = 0;
                    for (int i = 0; i < baseType.Length; i++)
                    {
                        switch (baseType[i])
                        {
                            case '+':
                            case '.':
                                sb.Append(CreateEscapedIdentifier(baseType.Substring(lastIndex, i - lastIndex)));
                                sb.Append('.');
                                i++;
                                lastIndex = i;
                                break;

                            case '`':
                                sb.Append(CreateEscapedIdentifier(baseType.Substring(lastIndex, i - lastIndex)));
                                i++;       
                                int numTypeArgs = 0;
                                while (i < baseType.Length && baseType[i] >= '0' && baseType[i] <= '9')
                                {
                                    numTypeArgs = numTypeArgs * 10 + (baseType[i] - '0');
                                    i++;
                                }

                                GetTypeArgumentsOutput(typeRef.TypeArguments, currentTypeArgStart, numTypeArgs, sb);
                                currentTypeArgStart += numTypeArgs;

                                if (i < baseType.Length && (baseType[i] == '+' || baseType[i] == '.'))
                                {
                                    sb.Append('.');
                                    i++;
                                }

                                lastIndex = i;
                                break;
                        }
                    }

                    if (lastIndex < baseType.Length)
                        sb.Append(CreateEscapedIdentifier(baseType.Substring(lastIndex)));

                    return sb.ToString();
            }
            return s;
        }

        public string GetTypeArgumentsOutput(CodeTypeReferenceCollection typeArguments)
        {
            StringBuilder sb = new StringBuilder(128);
            GetTypeArgumentsOutput(typeArguments, 0, typeArguments.Count, sb);
            return sb.ToString();
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

        public override string GetTypeOutput(CodeTypeReference value)
        {
            string s = string.Empty;

            CodeTypeReference baseTypeRef = value;
            while (baseTypeRef.ArrayElementType != null)
            {
                baseTypeRef = baseTypeRef.ArrayElementType;
            }
            s += GetBaseTypeOutput(baseTypeRef);

            while (value != null && value.ArrayRank > 0)
            {
                char[] results = new char[value.ArrayRank + 1];
                results[0] = '[';
                results[value.ArrayRank] = ']';
                for (int i = 1; i < value.ArrayRank; i++)
                {
                    results[i] = ',';
                }
                s += new string(results);
                value = value.ArrayElementType;
            }

            return s;
        }

        public override bool IsValidIdentifier(string value)
        {
            return true;
        }

        public override void OutputCommentStatements(CodeCommentStatementCollection e)
        {
            foreach (CodeCommentStatement comment in e)
            {
                GenerateCommentStatement(comment);
            }
        }

        public override void OutputFieldScopeModifier(MemberAttributes attributes)
        {
            switch (attributes & MemberAttributes.ScopeMask)
            {
                case MemberAttributes.Final:
                    break;
                case MemberAttributes.Static:
                    Writer.Write("static ");
                    break;
                case MemberAttributes.Const:
                    Writer.Write("const ");
                    break;
                default:
                    break;
            }
        }

        public override void OutputMemberAccessModifier(MemberAttributes attributes)
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
        }

        public override void OutputMemberScopeModifier(MemberAttributes attributes)
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

        public override void OutputOperator(CodeBinaryOperatorType op)
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

        public void OutputStartingBrace()
        {
            Writer.WriteLine();
            Writer.OpenBrace();
        }

        public void OutputTypeAttributes(CodeTypeDeclaration e)
        {
            if ((e.Attributes & MemberAttributes.New) != 0)
            {
                Writer.Write("new ");
            }

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
        }

        public void OutputTypeParameterConstraints(CodeTypeParameterCollection typeParameters)
        {
            if (typeParameters.Count == 0)
            {
                return;
            }

            for (int i = 0; i < typeParameters.Count; i++)
            {
                Writer.WriteLine();
                Indent++;

                bool first = true;
                if (typeParameters[i].Constraints.Count > 0)
                {
                    foreach (CodeTypeReference typeRef in typeParameters[i].Constraints)
                    {
                        if (first)
                        {
                            Writer.Write("where ");
                            Writer.Write(typeParameters[i].Name);
                            Writer.Write(" : ");
                            first = false;
                        }
                        else
                        {
                            Writer.Write(", ");
                        }
                        GenerateTypeReference(typeRef);
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

                Indent--;
            }
        }

        public void GenerateTypeParameters(CodeTypeParameterCollection typeParameters)
        {
            if (typeParameters.Count == 0)
            {
                return;
            }

            Writer.Write('<');
            bool first = true;
            for (int i = 0; i < typeParameters.Count; i++)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Writer.Write(", ");
                }

                CodeTypeParameter parameter = typeParameters[i];
                if (parameter.CustomAttributes.Count > 0)
                {
                    GenerateAttributes(parameter.CustomAttributes, null, true);
                    Writer.Write(' ');
                }

                GenerateTypeParameter(parameter);
            }

            Writer.Write('>');
        }

        public void OutputVTableModifier(MemberAttributes attributes)
        {
            switch (attributes & MemberAttributes.VTableMask)
            {
                case MemberAttributes.New:
                    Writer.Write("new ");
                    break;
            }
        }

        public override string QuoteSnippetString(string value)
        {
            return null;
        }

        protected override void OutputAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
        {
            Writer.CloseBracket();
            Writer.WriteLine();
        }

        protected override void OutputAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
        {
            Writer.OpenBracket();
        }

        protected override bool Supports(GeneratorSupport support)
        {
            return true;
        }

        private static bool IsKeyword(string name)
        {
            switch (name)
            {
                case "bool":
                case "byte":
                case "sbyte":
                case "short":
                case "ushort":
                case "int":
                case "uint":
                case "long":
                case "ulong":
                case "double":
                case "float":
                case "decimal":
                case "string":
                case "char":
                case "object":
                case "typeof":
                case "sizeof":
                case "null":
                case "true":
                case "false":
                case "if":
                case "else":
                case "while":
                case "for":
                case "foreach":
                case "do":
                case "switch":
                case "case":
                case "default":
                case "lock":
                case "try":
                case "throw":
                case "catch":
                case "finally":
                case "goto":
                case "break":
                case "continue":
                case "return":
                case "public":
                case "private":
                case "internal":
                case "protected":
                case "static":
                case "readonly":
                case "sealed":
                case "const":
                case "new":
                case "override":
                case "abstract":
                case "virtual":
                case "partial":
                case "ref":
                case "out":
                case "in":
                case "where":
                case "params":
                case "this":
                case "base":
                case "namespace":
                case "using":
                case "class":
                case "struct":
                case "interface":
                case "delegate":
                case "checked":
                case "get":
                case "set":
                case "add":
                case "remove":
                case "operator":
                case "implicit":
                case "explicit":
                case "fixed":
                case "extern":
                case "event":
                case "enum":
                case "unsafe":
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsPrefixTwoUnderscore(string value)
        {
            if (value.Length < 3)
            {
                return false;
            }
            else
            {
                return ((value[0] == '_') && (value[1] == '_') && (value[2] != '_'));
            }
        }

        private T GetUserDataValue<T>(IDictionary dictionary, string key)
        {
            if (dictionary?.Contains(key) ?? false)
            {
                return (T)dictionary[key];
            }

            return default(T);
        }
    }
}
