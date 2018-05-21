using System.CodeDom;

namespace System.CodeDom
{
    public class CodeDefaultReturnSwitchSectionStatement : AbstractSwitchSectionStatement
    {
        public CodeDefaultReturnSwitchSectionStatement()
        {
            Label = new CodeSwitchSectionLabelExpression(new CodeVariableReferenceExpression("default"));
            BodyStatements = new CodeStatementCollection();
            ReturnStatement = new CodeMethodReturnStatement();
        }
        public override CodeSwitchSectionLabelExpression Label { get; set; }

        public CodeStatementCollection BodyStatements { get; set; }

        public CodeMethodReturnStatement ReturnStatement { get; set; }
    }
}
