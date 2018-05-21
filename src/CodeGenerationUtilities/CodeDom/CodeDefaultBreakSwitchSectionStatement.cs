using System.CodeDom;

namespace System.CodeDom
{
    public class CodeDefaultBreakSwitchSectionStatement : AbstractSwitchSectionStatement
    {
        public CodeDefaultBreakSwitchSectionStatement()
        {
            Label = new CodeSwitchSectionLabelExpression(new CodeVariableReferenceExpression("default"));
            BodyStatements = new CodeStatementCollection();
        }
        public override CodeSwitchSectionLabelExpression Label { get; set; }

        public CodeStatementCollection BodyStatements { get; set; }
    }
}
