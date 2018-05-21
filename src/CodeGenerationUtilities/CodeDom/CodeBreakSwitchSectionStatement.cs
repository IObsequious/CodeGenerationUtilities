using System.CodeDom;

namespace System.CodeDom
{
    public class CodeBreakSwitchSectionStatement : AbstractSwitchSectionStatement
    {
        public CodeBreakSwitchSectionStatement()
        {
            Label = new CodeSwitchSectionLabelExpression();
            BodyStatements = new CodeStatementCollection();
        }
        public CodeBreakSwitchSectionStatement(CodeSwitchSectionLabelExpression label, params CodeStatement[] bodyStatements)
        {
            Label = label;
            BodyStatements = bodyStatements == null ? new CodeStatementCollection() : new CodeStatementCollection(bodyStatements);
        }

        public CodeBreakSwitchSectionStatement(CodeStatementCollection bodyStatements, CodeSwitchSectionLabelExpression label)
        {
            BodyStatements = bodyStatements;
            Label = label;
        }

        public CodeStatementCollection BodyStatements { get; set; }
        public override CodeSwitchSectionLabelExpression Label { get; set; }

    }
}
