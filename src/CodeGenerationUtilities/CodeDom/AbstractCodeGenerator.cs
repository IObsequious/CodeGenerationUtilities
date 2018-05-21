using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace System.CodeDom
{
    public abstract class AbstractCodeGenerator : ICodeGenerator2
    {
        private const int ParameterMultilineThreshold = 15;
        private IndentedTextWriter _writer;
        protected bool inNestedBinary = false;
        protected bool usingsAlreadyWritten = false;

        protected AbstractCodeGenerator()
        {
            _writer = new IndentedTextWriter(new StringWriter());
        }

        protected AbstractCodeGenerator(TextWriter writer)
        {
            if (writer is IndentedTextWriter indentedTextWriter)
            {
                _writer = indentedTextWriter;
            }
            else
            {
                _writer = new IndentedTextWriter(writer);
            }
        }

        bool ICodeGenerator.Supports(GeneratorSupport supports)
        {
            return true;
        }

        void ICodeGenerator.GenerateCodeFromType(CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o)
        {
            bool setLocal = false;
            if (_writer != null && w != _writer.InnerWriter)
            {
                throw new InvalidOperationException();
            }
            if (_writer == null)
            {
                setLocal = true;
                Options = o ??
                          new CodeGeneratorOptions
                          {
                              BlankLinesBetweenMembers = false,
                              BracingStyle = "C",
                              ElseOnClosing = true,
                              IndentString = "    "
                          };
                _writer = new IndentedTextWriter(w, Options.IndentString);
            }

            try
            {
                GenerateType(e);
            }
            finally
            {
                if (setLocal)
                {
                    _writer = null;
                    Options = null;
                }
            }
        }

        void ICodeGenerator.GenerateCodeFromExpression(CodeExpression e, TextWriter w, CodeGeneratorOptions o)
        {
            bool setLocal = false;
            if (_writer != null && w != _writer.InnerWriter)
            {
                throw new InvalidOperationException();
            }
            if (_writer == null)
            {
                setLocal = true;
                Options = o ?? new CodeGeneratorOptions();
                _writer = new IndentedTextWriter(w, Options.IndentString);
            }

            try
            {
                GenerateExpression(e);
            }
            finally
            {
                if (setLocal)
                {
                    _writer = null;
                    Options = null;
                }
            }
        }

        void ICodeGenerator.GenerateCodeFromCompileUnit(CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o)
        {
            bool setLocal = false;
            if (_writer != null && w != _writer.InnerWriter)
            {
                throw new InvalidOperationException();
            }
            if (_writer == null)
            {
                setLocal = true;
                Options = o ?? new CodeGeneratorOptions();
                _writer = new IndentedTextWriter(w, Options.IndentString);
            }

            try
            {
                if (e is CodeSnippetCompileUnit)
                {
                    GenerateSnippetCompileUnit((CodeSnippetCompileUnit)e);
                }
                else
                {
                    GenerateCompileUnit(e);
                }
            }
            finally
            {
                if (setLocal)
                {
                    _writer = null;
                    Options = null;
                }
            }
        }

        void ICodeGenerator.GenerateCodeFromNamespace(CodeNamespace e, TextWriter w, CodeGeneratorOptions o)
        {
            bool setLocal = false;
            if (_writer != null && w != _writer.InnerWriter)
            {
                throw new InvalidOperationException();
            }
            if (_writer == null)
            {
                setLocal = true;
                Options = o ?? new CodeGeneratorOptions();
                _writer = new IndentedTextWriter(w, Options.IndentString);
            }

            try
            {
                GenerateNamespace(e);
            }
            finally
            {
                if (setLocal)
                {
                    _writer = null;
                    Options = null;
                }
            }
        }

        void ICodeGenerator.GenerateCodeFromStatement(CodeStatement e, TextWriter w, CodeGeneratorOptions o)
        {
            bool setLocal = false;
            if (_writer != null && w != _writer.InnerWriter)
            {
                throw new InvalidOperationException();
            }
            if (_writer == null)
            {
                setLocal = true;
                Options = o ?? new CodeGeneratorOptions();
                _writer = new IndentedTextWriter(w, Options.IndentString);
            }

            try
            {
                GenerateStatement(e);
            }
            finally
            {
                if (setLocal)
                {
                    _writer = null;
                    Options = null;
                }
            }
        }

        bool ICodeGenerator.IsValidIdentifier(string value)
        {
            return IsValidIdentifier(value);
        }

        void ICodeGenerator.ValidateIdentifier(string value)
        {
            ValidateIdentifier(value);
        }

        string ICodeGenerator.CreateEscapedIdentifier(string value)
        {
            return CreateEscapedIdentifier(value);
        }

        string ICodeGenerator.CreateValidIdentifier(string value)
        {
            return CreateValidIdentifier(value);
        }

        string ICodeGenerator.GetTypeOutput(CodeTypeReference type)
        {
            return GetTypeOutput(type);
        }

        public virtual void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions Options)
        {
            if (_writer != null)
            {
                throw new InvalidOperationException("");
            }
            this.Options = Options ?? new CodeGeneratorOptions();
            _writer = new IndentedTextWriter(writer, this.Options.IndentString);

            try
            {
                GenerateMember(member);
            }
            finally
            {
                CurrentClass = null;
                _writer = null;
                this.Options = null;
            }
        }

        public CodeTypeDeclaration CurrentClass { get; protected set; }

        public string CurrentTypeName
        {
            get
            {
                if (CurrentClass != null)
                {
                    return CurrentClass.Name;
                }
                return "<% unknown %>";
            }
        }

        public CodeTypeMember CurrentMember { get; protected set; }

        public string CurrentMemberName
        {
            get
            {
                if (CurrentMember != null)
                {
                    return CurrentMember.Name;
                }
                return "<% unknown %>";
            }
        }

        public bool IsCurrentInterface
        {
            get
            {
                if (CurrentClass != null && !(CurrentClass is CodeTypeDelegate))
                {
                    return CurrentClass.IsInterface;
                }
                return false;
            }
        }

        public bool IsCurrentClass
        {
            get
            {
                if (CurrentClass != null && !(CurrentClass is CodeTypeDelegate))
                {
                    return CurrentClass.IsClass;
                }
                return false;
            }
        }

        public bool IsCurrentStruct
        {
            get
            {
                if (CurrentClass != null && !(CurrentClass is CodeTypeDelegate))
                {
                    return CurrentClass.IsStruct;
                }
                return false;
            }
        }

        public bool IsCurrentEnum
        {
            get
            {
                if (CurrentClass != null && !(CurrentClass is CodeTypeDelegate))
                {
                    return CurrentClass.IsEnum;
                }
                return false;
            }
        }

        public bool IsCurrentDelegate
        {
            get
            {
                return CurrentClass is CodeTypeDelegate;
            }
        }

        public int Indent
        {
            get
            {
                return _writer.Indent;
            }
            set
            {
                _writer.Indent = value;
            }
        }

        protected abstract string NullToken { get; }

        public IndentedTextWriter Writer => _writer;

        public CodeGeneratorOptions Options { get; protected set; } = new CodeGeneratorOptions { BracingStyle = "C" };

        public abstract void GenerateDefaultReturnSwitchSectionStatement(CodeDefaultReturnSwitchSectionStatement e);

        public void GenerateType(CodeTypeDeclaration e)
        {
            CurrentClass = e;
            if (e.StartDirectives.Count > 0)
            {
                OutputDirectives(e.StartDirectives);
            }
            OutputCommentStatements(e.Comments);
            if (e.LinePragma != null) GenerateLinePragmaStart(e.LinePragma);
            GenerateTypeStart(e);
            if (Options.VerbatimOrder)
            {
                foreach (CodeTypeMember member in e.Members)
                {
                    GenerateMember(member, e);
                }
            }
            else
            {
                GenerateFields(e);
                GenerateSnippetMembers(e);
                GenerateTypeConstructors(e);
                GenerateConstructors(e);
                GenerateProperties(e);
                GenerateEvents(e);
                GenerateMethods(e);
                GenerateNestedTypes(e);
            }
            CurrentClass = e;
            GenerateTypeEnd(e);
            if (e.LinePragma != null) GenerateLinePragmaEnd(e.LinePragma);
            if (e.EndDirectives.Count > 0)
            {
                OutputDirectives(e.EndDirectives);
            }
        }

        public abstract void GenerateDirective(CodeDirective e);

        public abstract void GenerateChecksumPragma(CodeChecksumPragma e);

        public abstract void GenerateRegionDirective(CodeRegionDirective e);

        protected virtual void OutputDirectives(CodeDirectiveCollection directives)
        {
            foreach (CodeDirective directive in directives)
            {
                GenerateDirective(directive);
            }
        }

        public void GenerateMember(CodeTypeMember e)
        {
            GenerateMember(e, new CodeTypeDeclaration());
        }

        public void GenerateMember(CodeTypeMember member, CodeTypeDeclaration declaredType)
        {
            //if (Options.BlankLinesBetweenMembers)
            //{
            //    Writer.WriteLine();
            //}
            if (member is CodeTypeDeclaration)
            {
                ((ICodeGenerator)this).GenerateCodeFromType((CodeTypeDeclaration)member, _writer.InnerWriter, Options);
                CurrentClass = declaredType;
                return;
            }
            if (member.StartDirectives.Count > 0)
            {
                OutputDirectives(member.StartDirectives);
            }
            OutputCommentStatements(member.Comments);
            if (member.LinePragma != null)
            {
                GenerateLinePragmaStart(member.LinePragma);
            }
            switch (member)
            {
                case CodeMemberField _:
                    GenerateField((CodeMemberField)member);
                    break;
                case CodeMemberProperty _:
                    GenerateProperty((CodeMemberProperty)member);
                    break;
                case CodeMemberMethod _:
                    if (member is CodeConstructor)
                    {
                        GenerateConstructor((CodeConstructor)member);
                    }
                    else if (member is CodeTypeConstructor)
                    {
                        GenerateTypeConstructor((CodeTypeConstructor)member);
                    }
                    else if (member is CodeEntryPointMethod)
                    {
                        GenerateEntryPointMethod((CodeEntryPointMethod)member);
                    }
                    else
                    {
                        GenerateMethod((CodeMemberMethod)member);
                    }

                    break;
                case CodeMemberEvent _:
                    GenerateEvent((CodeMemberEvent)member);
                    break;
                case CodeSnippetTypeMember _:
                    int savedIndent = Indent;
                    Indent = 0;
                    GenerateSnippetMember((CodeSnippetTypeMember)member);
                    Indent = savedIndent;
                    Writer.WriteLine();
                    break;
            }
            if (member.LinePragma != null)
            {
                GenerateLinePragmaEnd(member.LinePragma);
            }
            if (member.EndDirectives.Count > 0)
            {
                OutputDirectives(member.EndDirectives);
            }
        }

        public void GenerateTypeConstructors(CodeTypeDeclaration e)
        {
            IEnumerator en = e.Members.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Current is CodeTypeConstructor)
                {
                    CurrentMember = (CodeTypeMember)en.Current;
                    if (Options.BlankLinesBetweenMembers)
                    {
                        Writer.WriteLine();
                    }
                    if (CurrentMember.StartDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.StartDirectives);
                    }
                    OutputCommentStatements(CurrentMember.Comments);
                    CodeTypeConstructor imp = (CodeTypeConstructor)en.Current;
                    if (imp.LinePragma != null) GenerateLinePragmaStart(imp.LinePragma);
                    GenerateTypeConstructor(imp);
                    if (imp.LinePragma != null) GenerateLinePragmaEnd(imp.LinePragma);
                    if (CurrentMember.EndDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.EndDirectives);
                    }
                }
            }
        }

        public void GenerateNamespaces(CodeCompileUnit e)
        {
            foreach (CodeNamespace n in e.Namespaces)
            {
                GenerateNamespace(n);
            }
        }

        public void GenerateTypes(CodeNamespace e)
        {
            foreach (CodeTypeDeclaration c in e.Types)
            {
                if (Options.BlankLinesBetweenMembers)
                {
                    Writer.WriteLine();
                }
                ((ICodeGenerator)this).GenerateCodeFromType(c, _writer.InnerWriter, Options);
            }
        }

        public void GenerateConstructors(CodeTypeDeclaration e)
        {
            IEnumerator en = e.Members.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Current is CodeConstructor)
                {
                    CurrentMember = (CodeTypeMember)en.Current;
                    if (Options.BlankLinesBetweenMembers)
                    {
                        Writer.WriteLine();
                    }
                    if (CurrentMember.StartDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.StartDirectives);
                    }
                    OutputCommentStatements(CurrentMember.Comments);
                    CodeConstructor imp = (CodeConstructor)en.Current;
                    if (imp.LinePragma != null) GenerateLinePragmaStart(imp.LinePragma);
                    GenerateConstructor(imp);
                    if (imp.LinePragma != null) GenerateLinePragmaEnd(imp.LinePragma);
                    if (CurrentMember.EndDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.EndDirectives);
                    }
                }
            }
        }

        public void GenerateEvents(CodeTypeDeclaration e)
        {
            IEnumerator en = e.Members.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Current is CodeMemberEvent)
                {
                    CurrentMember = (CodeTypeMember)en.Current;
                    if (Options.BlankLinesBetweenMembers)
                    {
                        Writer.WriteLine();
                    }
                    if (CurrentMember.StartDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.StartDirectives);
                    }
                    OutputCommentStatements(CurrentMember.Comments);
                    CodeMemberEvent imp = (CodeMemberEvent)en.Current;
                    if (imp.LinePragma != null) GenerateLinePragmaStart(imp.LinePragma);
                    GenerateEvent(imp);
                    if (imp.LinePragma != null) GenerateLinePragmaEnd(imp.LinePragma);
                    if (CurrentMember.EndDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.EndDirectives);
                    }
                }
            }
        }

        public void GenerateExpression(CodeExpression e)
        {
            switch (e)
            {
                case CodeArrayCreateExpression _:
                    GenerateArrayCreateExpression((CodeArrayCreateExpression)e);
                    break;
                case CodeBaseReferenceExpression _:
                    GenerateBaseReferenceExpression((CodeBaseReferenceExpression)e);
                    break;
                case CodeBinaryOperatorExpression _:
                    GenerateBinaryOperatorExpression((CodeBinaryOperatorExpression)e);
                    break;
                case CodeCastExpression _:
                    GenerateCastExpression((CodeCastExpression)e);
                    break;
                case CodeDelegateCreateExpression _:
                    GenerateDelegateCreateExpression((CodeDelegateCreateExpression)e);
                    break;
                case CodeFieldReferenceExpression _:
                    GenerateFieldReferenceExpression((CodeFieldReferenceExpression)e);
                    break;
                case CodeArgumentReferenceExpression _:
                    GenerateArgumentReferenceExpression((CodeArgumentReferenceExpression)e);
                    break;
                case CodeVariableReferenceExpression _:
                    GenerateVariableReferenceExpression((CodeVariableReferenceExpression)e);
                    break;
                case CodeIndexerExpression _:
                    GenerateIndexerExpression((CodeIndexerExpression)e);
                    break;
                case CodeArrayIndexerExpression _:
                    GenerateArrayIndexerExpression((CodeArrayIndexerExpression)e);
                    break;
                case CodeSnippetExpression _:
                    GenerateSnippetExpression((CodeSnippetExpression)e);
                    break;
                case CodeMethodInvokeExpression _:
                    GenerateMethodInvokeExpression((CodeMethodInvokeExpression)e);
                    break;
                case CodeMethodReferenceExpression _:
                    GenerateMethodReferenceExpression((CodeMethodReferenceExpression)e);
                    break;
                case CodeEventReferenceExpression _:
                    GenerateEventReferenceExpression((CodeEventReferenceExpression)e);
                    break;
                case CodeDelegateInvokeExpression _:
                    GenerateDelegateInvokeExpression((CodeDelegateInvokeExpression)e);
                    break;
                case CodeObjectCreateExpression _:
                    GenerateObjectCreateExpression((CodeObjectCreateExpression)e);
                    break;
                case CodeParameterDeclarationExpression _:
                    GenerateParameterDeclarationExpression((CodeParameterDeclarationExpression)e);
                    break;
                case CodeDirectionExpression _:
                    GenerateDirectionExpression((CodeDirectionExpression)e);
                    break;
                case CodePrimitiveExpression _:
                    GeneratePrimitiveExpression((CodePrimitiveExpression)e);
                    break;
                case CodePropertyReferenceExpression _:
                    GeneratePropertyReferenceExpression((CodePropertyReferenceExpression)e);
                    break;
                case CodePropertySetValueReferenceExpression _:
                    GeneratePropertySetValueReferenceExpression((CodePropertySetValueReferenceExpression)e);
                    break;
                case CodeThisReferenceExpression _:
                    GenerateThisReferenceExpression((CodeThisReferenceExpression)e);
                    break;
                case CodeTypeReferenceExpression _:
                    GenerateTypeReferenceExpression((CodeTypeReferenceExpression)e);
                    break;
                case CodeTypeOfExpression _:
                    GenerateTypeOfExpression((CodeTypeOfExpression)e);
                    break;
                case CodeDefaultValueExpression _:
                    GenerateDefaultValueExpression((CodeDefaultValueExpression)e);
                    break;
                default:
                    if (e == null)
                    {
                        throw new ArgumentNullException("e");
                    }
                    throw new ArgumentException();
            }
        }

        public void GenerateFields(CodeTypeDeclaration e)
        {
            IEnumerator en = e.Members.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Current is CodeMemberField)
                {
                    CurrentMember = (CodeTypeMember)en.Current;
                    if (Options.BlankLinesBetweenMembers)
                    {
                        Writer.WriteLine();
                    }
                    if (CurrentMember.StartDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.StartDirectives);
                    }
                    OutputCommentStatements(CurrentMember.Comments);
                    CodeMemberField imp = (CodeMemberField)en.Current;
                    if (imp.LinePragma != null) GenerateLinePragmaStart(imp.LinePragma);
                    GenerateField(imp);
                    if (imp.LinePragma != null) GenerateLinePragmaEnd(imp.LinePragma);
                    if (CurrentMember.EndDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.EndDirectives);
                    }
                }
            }
        }

        public void GenerateSnippetMembers(CodeTypeDeclaration e)
        {
            IEnumerator en = e.Members.GetEnumerator();
            bool hasSnippet = false;
            while (en.MoveNext())
            {
                if (en.Current is CodeSnippetTypeMember)
                {
                    hasSnippet = true;
                    CurrentMember = (CodeTypeMember)en.Current;
                    if (Options.BlankLinesBetweenMembers)
                    {
                        Writer.WriteLine();
                    }
                    if (CurrentMember.StartDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.StartDirectives);
                    }
                    OutputCommentStatements(CurrentMember.Comments);
                    CodeSnippetTypeMember imp = (CodeSnippetTypeMember)en.Current;
                    if (imp.LinePragma != null) GenerateLinePragmaStart(imp.LinePragma);
                    int savedIndent = Indent;
                    Indent = 0;
                    GenerateSnippetMember(imp);
                    Indent = savedIndent;
                    if (imp.LinePragma != null) GenerateLinePragmaEnd(imp.LinePragma);
                    if (CurrentMember.EndDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.EndDirectives);
                    }
                }
            }
            if (hasSnippet)
            {
                Writer.WriteLine();
            }
        }

        public virtual void GenerateSnippetCompileUnit(CodeSnippetCompileUnit e)
        {
            OutputDirectives(e.StartDirectives);
            if (e.LinePragma != null) GenerateLinePragmaStart(e.LinePragma);
            Writer.WriteLine(e.Value);
            if (e.LinePragma != null) GenerateLinePragmaEnd(e.LinePragma);
            if (e.EndDirectives.Count > 0)
            {
                OutputDirectives(e.EndDirectives);
            }
        }

        public void GenerateMethods(CodeTypeDeclaration e)
        {
            IEnumerator en = e.Members.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Current is CodeMemberMethod && !(en.Current is CodeTypeConstructor) && !(en.Current is CodeConstructor))
                {
                    CurrentMember = (CodeTypeMember)en.Current;
                    if (Options.BlankLinesBetweenMembers)
                    {
                        Writer.WriteLine();
                    }
                    if (CurrentMember.StartDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.StartDirectives);
                    }
                    OutputCommentStatements(CurrentMember.Comments);
                    CodeMemberMethod imp = (CodeMemberMethod)en.Current;
                    if (imp.LinePragma != null) GenerateLinePragmaStart(imp.LinePragma);
                    if (en.Current is CodeEntryPointMethod)
                    {
                        GenerateEntryPointMethod((CodeEntryPointMethod)en.Current);
                    }
                    else
                    {
                        GenerateMethod(imp);
                    }
                    if (imp.LinePragma != null) GenerateLinePragmaEnd(imp.LinePragma);
                    if (CurrentMember.EndDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.EndDirectives);
                    }
                }
            }
        }

        public void GenerateNestedTypes(CodeTypeDeclaration e)
        {
            IEnumerator en = e.Members.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Current is CodeTypeDeclaration)
                {
                    if (Options.BlankLinesBetweenMembers)
                    {
                        Writer.WriteLine();
                    }
                    CodeTypeDeclaration currentClass = (CodeTypeDeclaration)en.Current;
                    ((ICodeGenerator)this).GenerateCodeFromType(currentClass, _writer.InnerWriter, Options);
                }
            }
        }

        public abstract void GenerateCompileUnit(CodeCompileUnit e);

        public virtual void GenerateNamespace(CodeNamespace e)
        {
            OutputCommentStatements(e.Comments);
            GenerateNamespaceStart(e);
            GenerateNamespaceImports(e);
            Writer.WriteLine("");
            GenerateTypes(e);
            GenerateNamespaceEnd(e);
        }

        public void GenerateNamespaceImports(CodeNamespace e)
        {
            IEnumerator en = e.Imports.GetEnumerator();
            while (en.MoveNext())
            {
                CodeNamespaceImport imp = (CodeNamespaceImport)en.Current;
                if (imp.LinePragma != null) GenerateLinePragmaStart(imp.LinePragma);
                GenerateNamespaceImport(imp);
                if (imp.LinePragma != null) GenerateLinePragmaEnd(imp.LinePragma);
            }
        }

        public void GenerateProperties(CodeTypeDeclaration e)
        {
            IEnumerator en = e.Members.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Current is CodeMemberProperty)
                {
                    CurrentMember = (CodeTypeMember)en.Current;
                    if (Options.BlankLinesBetweenMembers)
                    {
                        Writer.WriteLine();
                    }
                    if (CurrentMember.StartDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.StartDirectives);
                    }
                    OutputCommentStatements(CurrentMember.Comments);
                    CodeMemberProperty imp = (CodeMemberProperty)en.Current;
                    if (imp.LinePragma != null) GenerateLinePragmaStart(imp.LinePragma);
                    GenerateProperty(imp);
                    if (imp.LinePragma != null) GenerateLinePragmaEnd(imp.LinePragma);
                    if (CurrentMember?.EndDirectives.Count > 0)
                    {
                        OutputDirectives(CurrentMember.EndDirectives);
                    }
                }
            }
        }

        public void GenerateStatement(CodeStatement e)
        {
            if (e.StartDirectives.Count > 0)
            {
                OutputDirectives(e.StartDirectives);
            }
            if (e.LinePragma != null)
            {
                GenerateLinePragmaStart(e.LinePragma);
            }
            if (e is CodeCommentStatement)
            {
                GenerateCommentStatement((CodeCommentStatement)e);
            }
            else if (e is CodeMethodReturnStatement)
            {
                GenerateMethodReturnStatement((CodeMethodReturnStatement)e);
            }
            else if (e is CodeConditionStatement)
            {
                GenerateConditionStatement((CodeConditionStatement)e);
            }
            else if (e is CodeTryCatchFinallyStatement)
            {
                GenerateTryCatchFinallyStatement((CodeTryCatchFinallyStatement)e);
            }
            else if (e is CodeAssignStatement)
            {
                GenerateAssignStatement((CodeAssignStatement)e);
            }
            else if (e is CodeExpressionStatement)
            {
                GenerateExpressionStatement((CodeExpressionStatement)e);
            }
            else if (e is CodeIterationStatement)
            {
                GenerateIterationStatement((CodeIterationStatement)e);
            }
            else if (e is CodeThrowExceptionStatement)
            {
                GenerateThrowExceptionStatement((CodeThrowExceptionStatement)e);
            }
            else if (e is CodeSnippetStatement)
            {
                int savedIndent = Indent;
                Indent = 0;
                GenerateSnippetStatement((CodeSnippetStatement)e);
                Indent = savedIndent;
            }
            else if (e is CodeVariableDeclarationStatement)
            {
                GenerateVariableDeclarationStatement((CodeVariableDeclarationStatement)e);
            }
            else if (e is CodeAttachEventStatement)
            {
                GenerateAttachEventStatement((CodeAttachEventStatement)e);
            }
            else if (e is CodeRemoveEventStatement)
            {
                GenerateRemoveEventStatement((CodeRemoveEventStatement)e);
            }
            else if (e is CodeGotoStatement)
            {
                GenerateGotoStatement((CodeGotoStatement)e);
            }
            else if (e is CodeLabeledStatement)
            {
                GenerateLabeledStatement((CodeLabeledStatement)e);
            }
            else
            {
                throw new ArgumentException();
            }
            if (e.LinePragma != null)
            {
                GenerateLinePragmaEnd(e.LinePragma);
            }
            if (e.EndDirectives.Count > 0)
            {
                OutputDirectives(e.EndDirectives);
            }
        }

        public void OutputStatements(CodeStatementCollection stms)
        {
            IEnumerator en = stms.GetEnumerator();
            while (en.MoveNext())
            {
                ((ICodeGenerator)this).GenerateCodeFromStatement((CodeStatement)en.Current, _writer.InnerWriter, Options);
            }
        }

        public virtual void OutputAttributeDeclarations(CodeAttributeDeclarationCollection attributes)
        {
            if (attributes.Count == 0) return;
            OutputAttributeDeclarationsStart(attributes);
            bool first = true;
            IEnumerator en = attributes.GetEnumerator();
            while (en.MoveNext())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    ContinueOnNewLine(", ");
                }
                CodeAttributeDeclaration current = (CodeAttributeDeclaration)en.Current;
                Writer.Write(current.Name);
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
            }
            OutputAttributeDeclarationsEnd(attributes);
        }

        public virtual void GenerateAttributeArgument(CodeAttributeArgument arg)
        {
            if (!string.IsNullOrEmpty(arg.Name))
            {
                OutputIdentifier(arg.Name);
                Writer.Write("=");
            }
            ((ICodeGenerator)this).GenerateCodeFromExpression(arg.Value, _writer.InnerWriter, Options);
        }

        public virtual void OutputDirection(FieldDirection dir)
        {
            switch (dir)
            {
                case FieldDirection.In:
                    break;
                case FieldDirection.Out:
                    Writer.Write("out ");
                    break;
                case FieldDirection.Ref:
                    Writer.Write("ref ");
                    break;
            }
        }

        public virtual void OutputFieldScopeModifier(MemberAttributes attributes)
        {
            switch (attributes & MemberAttributes.VTableMask)
            {
                case MemberAttributes.New:
                    Writer.Write("new ");
                    break;
            }
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
            }
        }

        public virtual void OutputMemberAccessModifier(MemberAttributes attributes)
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

        public virtual void OutputMemberScopeModifier(MemberAttributes attributes)
        {
            switch (attributes & MemberAttributes.VTableMask)
            {
                case MemberAttributes.New:
                    Writer.Write("new ");
                    break;
            }
            switch (attributes & MemberAttributes.ScopeMask)
            {
                case MemberAttributes.Abstract:
                    Writer.Write("abstract ");
                    break;
                case MemberAttributes.Final:
                    Writer.Write("");
                    break;
                case MemberAttributes.Static:
                    Writer.Write("static ");
                    break;
                case MemberAttributes.Override:
                    Writer.Write("override ");
                    break;
                default:
                    switch (attributes & MemberAttributes.AccessMask)
                    {
                        case MemberAttributes.Family:
                        case MemberAttributes.Public:
                            Writer.Write("virtual ");
                            break;
                    }
                    break;
            }
        }

        public abstract void GenerateTypeReference(CodeTypeReference typeRef);

        public virtual void OutputTypeAttributes(TypeAttributes attributes, bool isStruct, bool isEnum)
        {
            switch (attributes & TypeAttributes.VisibilityMask)
            {
                case TypeAttributes.Public:
                case TypeAttributes.NestedPublic:
                    Writer.Write("public ");
                    break;
                case TypeAttributes.NestedPrivate:
                    Writer.Write("private ");
                    break;
            }
            if (isStruct)
            {
                Writer.Write("struct ");
            }
            else if (isEnum)
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
                        Writer.Write("class ");
                        break;
                    case TypeAttributes.Interface:
                        Writer.Write("interface ");
                        break;
                }
            }
        }

        public virtual void OutputTypeNamePair(CodeTypeReference typeRef, string name)
        {
            GenerateTypeReference(typeRef);
            Writer.Write(" ");
            OutputIdentifier(name);
        }

        public virtual void OutputIdentifier(string ident)
        {
            Writer.Write(ident);
        }

        public virtual void OutputExpressionList(CodeExpressionCollection expressions)
        {
            OutputExpressionList(expressions, false);
        }

        public virtual void OutputExpressionList(CodeExpressionCollection expressions, bool newlineBetweenItems)
        {
            bool first = true;
            IEnumerator en = expressions.GetEnumerator();
            Indent++;
            while (en.MoveNext())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    if (newlineBetweenItems)
                    {
                        ContinueOnNewLine(",");
                    }
                    else
                    {
                        Writer.Write(", ");
                    }
                }
                ((ICodeGenerator)this).GenerateCodeFromExpression((CodeExpression)en.Current, _writer.InnerWriter, Options);
            }
            Indent--;
        }

        public virtual void OutputOperator(CodeBinaryOperatorType op)
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

        public virtual void OutputParameters(CodeParameterDeclarationExpressionCollection parameters)
        {
            bool first = true;
            bool multiline = parameters.Count > ParameterMultilineThreshold;
            if (multiline)
            {
                Indent += 3;
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
                    ContinueOnNewLine("");
                }
                GenerateExpression(current);
            }
            if (multiline)
            {
                Indent -= 3;
            }
        }

        public abstract void GenerateArrayCreateExpression(CodeArrayCreateExpression e);

        public abstract void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e);

        public virtual void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e)
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

        public virtual void ContinueOnNewLine(string st)
        {
            Writer.WriteLine(st);
        }

        public abstract void GenerateCastExpression(CodeCastExpression e);

        public abstract void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e);

        public abstract void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e);

        public abstract void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e);

        public abstract void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e);

        public abstract void GenerateIndexerExpression(CodeIndexerExpression e);

        public abstract void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e);

        public abstract void GenerateSnippetExpression(CodeSnippetExpression e);

        public abstract void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e);

        public abstract void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e);

        public abstract void GenerateEventReferenceExpression(CodeEventReferenceExpression e);

        public abstract void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e);

        public abstract void GenerateObjectCreateExpression(CodeObjectCreateExpression e);

        public virtual void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
        {
            if (e.CustomAttributes.Count > 0)
            {
                OutputAttributeDeclarations(e.CustomAttributes);
                Writer.Write(" ");
            }
            OutputDirection(e.Direction);
            OutputTypeNamePair(e.Type, e.Name);
        }

        public virtual void GenerateDirectionExpression(CodeDirectionExpression e)
        {
            OutputDirection(e.Direction);
            GenerateExpression(e.Expression);
        }

        public virtual void GeneratePrimitiveExpression(CodePrimitiveExpression e)
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
                Writer.Write("'" + e.Value + "'");
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

        public virtual void GenerateSingleFloatValue(float s)
        {
            Writer.Write(s.ToString("R", CultureInfo.InvariantCulture));
        }

        public virtual void GenerateDoubleValue(double d)
        {
            Writer.Write(d.ToString("R", CultureInfo.InvariantCulture));
        }

        public virtual void GenerateDecimalValue(decimal d)
        {
            Writer.Write(d.ToString(CultureInfo.InvariantCulture));
        }

        public virtual void GenerateDefaultValueExpression(CodeDefaultValueExpression e)
        {
            Writer.Write("default(");
            GenerateTypeReference(e.Type);
            Writer.Write(")");
        }

        public abstract void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e);

        public abstract void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e);

        public abstract void GenerateThisReferenceExpression(CodeThisReferenceExpression e);

        public virtual void GenerateTypeReferenceExpression(CodeTypeReferenceExpression e)
        {
            GenerateTypeReference(e.Type);
        }

        public virtual void GenerateTypeOfExpression(CodeTypeOfExpression e)
        {
            Writer.Write("typeof(");
            GenerateTypeReference(e.Type);
            Writer.Write(")");
        }

        public abstract void GenerateExpressionStatement(CodeExpressionStatement e);

        public abstract void GenerateIterationStatement(CodeIterationStatement e);

        public abstract void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e);

        public virtual void GenerateCommentStatement(CodeCommentStatement e)
        {
            if (e.Comment == null)
            {
                throw new ArgumentException();
            }
            GenerateComment(e.Comment);
        }

        public virtual void OutputCommentStatements(CodeCommentStatementCollection e)
        {
            foreach (CodeCommentStatement comment in e)
            {
                GenerateCommentStatement(comment);
            }
        }

        public abstract void GenerateComment(CodeComment e);

        public abstract void GenerateMethodReturnStatement(CodeMethodReturnStatement e);

        public abstract void GenerateConditionStatement(CodeConditionStatement e);

        public abstract void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e);

        public abstract void GenerateAssignStatement(CodeAssignStatement e);

        public abstract void GenerateAttachEventStatement(CodeAttachEventStatement e);

        public abstract void GenerateRemoveEventStatement(CodeRemoveEventStatement e);

        public abstract void GenerateGotoStatement(CodeGotoStatement e);

        public abstract void GenerateLabeledStatement(CodeLabeledStatement e);

        public virtual void GenerateSnippetStatement(CodeSnippetStatement e)
        {
            Writer.WriteLine(e.Value);
        }

        public abstract void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e);

        public abstract void GenerateLinePragmaStart(CodeLinePragma e);

        public abstract void GenerateLinePragmaEnd(CodeLinePragma e);

        public abstract void GenerateEvent(CodeMemberEvent e);

        public abstract void GenerateField(CodeMemberField e);

        public abstract void GenerateSnippetMember(CodeSnippetTypeMember e);

        public abstract void GenerateEntryPointMethod(CodeEntryPointMethod e);

        public abstract void GenerateMethod(CodeMemberMethod e);

        public abstract void GenerateProperty(CodeMemberProperty e);

        public abstract void GenerateConstructor(CodeConstructor e);

        public abstract void GenerateTypeConstructor(CodeTypeConstructor e);

        public abstract void GenerateTypeStart(CodeTypeDeclaration e);

        public abstract void GenerateTypeEnd(CodeTypeDeclaration e);

        public abstract void GenerateSwitchStatement(CodeSwitchStatement e);

        public abstract void GenerateReturnValueSwitchSectionStatement(CodeReturnValueSwitchSectionStatement e);

        public abstract void GenerateBreakSwitchSectionStatement(CodeBreakSwitchSectionStatement e);

        public abstract void GenerateFallThroughSwitchSectionStatement(CodeFallThroughSwitchSectionStatement e);

        public abstract void GenerateDefaultBreakSwitchSectionStatement(CodeDefaultBreakSwitchSectionStatement e);

        public abstract void GenerateSwitchSectionLabelExpression(CodeSwitchSectionLabelExpression e);

        public virtual void GenerateCompileUnitStart(CodeCompileUnit e)
        {
            if (e.StartDirectives.Count > 0)
            {
                OutputDirectives(e.StartDirectives);
            }
        }

        public virtual void GenerateCompileUnitEnd(CodeCompileUnit e)
        {
            if (e.EndDirectives.Count > 0)
            {
                OutputDirectives(e.EndDirectives);
            }
        }

        public abstract void GenerateNamespaceStart(CodeNamespace e);

        public abstract void GenerateNamespaceEnd(CodeNamespace e);

        public abstract void GenerateNamespaceImport(CodeNamespaceImport e);

        protected abstract void OutputAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes);

        protected abstract void OutputAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes);

        public abstract void GenerateTypeParameter(CodeTypeParameter e);

        public abstract void GenerateTypeDeclaration(CodeTypeDeclaration e);

        public abstract void GenerateDelegate(CodeTypeDelegate e);

        protected abstract bool Supports(GeneratorSupport support);

        public abstract bool IsValidIdentifier(string value);

        public virtual void ValidateIdentifier(string value)
        {
        }

        public abstract string CreateEscapedIdentifier(string value);

        public abstract string CreateValidIdentifier(string value);

        public abstract string GetTypeOutput(CodeTypeReference value);

        public abstract string QuoteSnippetString(string value);

        public virtual bool IsValidLanguageIndependentIdentifier(string value)
        {
            return IsValidTypeNameOrIdentifier(value, false);
        }

        public virtual bool IsValidLanguageIndependentTypeName(string value)
        {
            return IsValidTypeNameOrIdentifier(value, true);
        }

        public virtual bool IsValidTypeNameOrIdentifier(string value, bool isTypeName)
        {
            bool nextMustBeStartChar = true;
            if (value.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                UnicodeCategory uc = char.GetUnicodeCategory(ch);
                switch (uc)
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.ModifierLetter:
                    case UnicodeCategory.LetterNumber:
                    case UnicodeCategory.OtherLetter:
                        nextMustBeStartChar = false;
                        break;
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (nextMustBeStartChar && ch != '_')
                        {
                            return false;
                        }
                        nextMustBeStartChar = false;
                        break;
                    default:
                        if (isTypeName && IsSpecialTypeChar(ch, ref nextMustBeStartChar))
                        {
                            break;
                        }
                        return false;
                }
            }
            return true;
        }

        public virtual bool IsSpecialTypeChar(char ch, ref bool nextMustBeStartChar)
        {
            switch (ch)
            {
                case ':':
                case '.':
                case '$':
                case '+':
                case '<':
                case '>':
                case '-':
                case '[':
                case ']':
                case ',':
                case '&':
                case '*':
                    nextMustBeStartChar = true;
                    return true;
                case '`':
                    return true;
            }
            return false;
        }
    }
}
