using System;

namespace GameMain
{
    public static class GUIDHelper
    {
        public static string Create()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}