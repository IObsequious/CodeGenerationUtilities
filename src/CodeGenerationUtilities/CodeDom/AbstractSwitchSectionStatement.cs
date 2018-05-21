using System.CodeDom;

namespace System.CodeDom
{
    public abstract class AbstractSwitchSectionStatement : CodeStatement
    {
        public abstract CodeSwitchSectionLabelExpression Label { get; set; }
    }
}
