using System.CodeDom;
using System.Collections;

namespace System.CodeDom
{
    public class CodeSwitchStatement : CodeStatement
    {
        public CodeSwitchStatement()
        {
            CheckExpression = new CodeExpression();
            Sections = new CodeSwitchSectionStatementCollection();
        }

        public CodeExpression CheckExpression { get; set; }

        public CodeSwitchSectionStatementCollection Sections { get; }
    }
}
