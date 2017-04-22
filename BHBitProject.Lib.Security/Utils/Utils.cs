using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHBitProject.Lib.Security
{
    public static class Utils
    {
        #region [GUID]

        public static class GUID
        {

            public static string CreateGUID()
            {
                return Guid.NewGuid().ToString();
            }

            public static string ParseGUID(string value)
            {
                return String.IsNullOrEmpty(value)
                    ? String.Empty
                    : Guid.Parse(value).ToString();
            }

            public static string ParseExactGUID(string input, string format)
            {
                return String.IsNullOrEmpty(input)
                    ? String.Empty
                    : Guid.ParseExact(input, format).ToString();
            }

        }
        #endregion
    }
}
