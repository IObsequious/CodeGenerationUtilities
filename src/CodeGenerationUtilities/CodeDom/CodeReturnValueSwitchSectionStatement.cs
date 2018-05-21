using System.CodeDom;

namespace System.CodeDom
{
    public class CodeReturnValueSwitchSectionStatement : AbstractSwitchSectionStatement
    {
        public CodeReturnValueSwitchSectionStatement()
        {
            Label = new CodeSwitchSectionLabelExpression();
            ReturnStatement = new CodeMethodReturnStatement();
            BodyStatements = new CodeStatementCollection();
        }

        public CodeReturnValueSwitchSectionStatement(CodeSwitchSectionLabelExpression label, CodeMethodReturnStatement returnStatement, params CodeStatement[] bodyStatements)
        {
            Label = label;
            BodyStatements = bodyStatements == null ? new CodeStatementCollection() : new CodeStatementCollection(bodyStatements);
            ReturnStatement = returnStatement;
        }

        public bool SingleLine { get; set; }

        public override CodeSwitchSectionLabelExpression Label { get; set; }

        public CodeStatementCollection BodyStatements { get; set; }

        public CodeMethodReturnStatement ReturnStatement { get; set; }

        public bool HasBody
        {
            get
            {
                return BodyStatements.Count > 0;
            }
        }
    }
}
