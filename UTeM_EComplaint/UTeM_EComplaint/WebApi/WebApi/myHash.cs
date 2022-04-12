using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi
{
    public static class myHash
    {
        private static readonly UInt32 FNV_OFFSET_32 = 0x811c9dc5;   // 2166136261
        private static readonly UInt32 FNV_PRIME_32 = 0x1000193;     // 16777619

        // Unsigned 32bit integer FNV-1a
        public static UInt32 HashFnv32u(this string s)
        {
            // byte[] arr = Encoding.UTF8.GetBytes(s);      // 8 bit expanded unicode array
            char[] arr = s.ToCharArray();                   // 16 bit unicode is native .net 

            UInt32 hash = FNV_OFFSET_32;
            for (var i = 0; i < s.Length; i++)
            {
                // Strips unicode bits, only the lower 8 bits of the values are used
                hash = hash ^ unchecked((byte)(arr[i] & 0xFF));
                hash = hash * FNV_PRIME_32;
            }
            return hash;
        }

        // Signed hash for storing in SQL Server
        public static Int32 HashFnv32s(this string s)
        {
            return unchecked((int)s.HashFnv32u());
        }
    }
}
