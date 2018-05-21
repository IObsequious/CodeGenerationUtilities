using System.Collections.Generic;

namespace System.CodeDom
{
    public class CodeSwitchSectionStatementCollection : List<AbstractSwitchSectionStatement>
    {
        public CodeSwitchSectionStatementCollection()
        {
        }

        public CodeSwitchSectionStatementCollection(int capacity) : base(capacity)
        {
        }

        public CodeSwitchSectionStatementCollection(IEnumerable<AbstractSwitchSectionStatement> collection) : base(collection)
        {
        }
    }
}
