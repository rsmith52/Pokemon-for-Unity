using System;
using System.Collections.Generic;

namespace Eventing
{
    [Serializable]
    public class Switch
    {
        #region Fields

        public static List<Switch> global_switches;

        public string name;
        public bool status;

        #endregion

    }
}