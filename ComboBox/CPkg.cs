using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xumeijin
{
    public class CPkg
    {
        private Dictionary<string, object> _dict = new Dictionary<string, object>();

        public object this[string strKey, int iIndex]
        {
            get
            {
                if (this._dict.ContainsKey(strKey + "`" + iIndex.ToString()))
                {
                    return this._dict[strKey + "`" + iIndex.ToString()];
                }
                return null;
            }
            set
            {
                this._dict[strKey + "`" + iIndex.ToString()] = RuntimeHelpers.GetObjectValue(value);
            }
        }

        public object this[string strKey]
        {
            get
            {
                if (this._dict.ContainsKey(strKey + "`0"))
                {
                    return this._dict[strKey + "`0"];
                }
                return null;
            }
            set
            {
                this._dict[strKey + "`0"] = RuntimeHelpers.GetObjectValue(value);
            }
        }
    }
}
