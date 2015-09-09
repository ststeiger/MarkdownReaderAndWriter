using System;
using System.Collections.Generic;
using System.Text;

namespace Arithmetica.Net2
{

    // http://stackoverflow.com/questions/4108828/generic-extension-method-to-see-if-an-enum-contains-a-flag
    public class MonoInternal
    {

        // Arithmetica.Net2.MonoInternal.EnumContains
        public static bool EnumContains(Enum keys, Enum flag)
        {
            ulong keysVal = Convert.ToUInt64(keys);
            ulong flagVal = Convert.ToUInt64(flag);

            return (keysVal & flagVal) == flagVal;
        }

    }
}
