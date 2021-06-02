
using System;
using System.Globalization;

public  class TimeHelper
{
    /// <summary>
    /// 获取当前的时间戳
    /// </summary>
    /// <returns></returns>
    public static string Timestamp()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
    }

    
    /// <summary>
    /// 获取当前的时间,格式：yyyy-MM-dd HH:mm:ss
    /// </summary>
    /// <returns></returns>
    public static string CurrentTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
    }
}
