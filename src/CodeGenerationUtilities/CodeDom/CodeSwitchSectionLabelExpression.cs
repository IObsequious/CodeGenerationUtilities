using System.CodeDom;

namespace System.CodeDom
{
    public class CodeSwitchSectionLabelExpression : CodeExpression
    {
        public CodeSwitchSectionLabelExpression()
        {
            Expression = new CodeExpression();
        }

        public CodeSwitchSectionLabelExpression(CodeExpression expression)
        {
            Expression = expression;
        }

        public CodeExpression Expression { get; set; }

    }
}
