using System;
using System.Collections.Generic;

namespace Eventing
{
    [Serializable]
    public class Variable
    {
        #region Fields

        public static List<Variable> global_variables;

        public string name;
        public int value;

        #endregion

    }
}