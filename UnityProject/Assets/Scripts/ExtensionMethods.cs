using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    /// <summary>
    /// Returns "0" + value if value < 10, otherwise returns value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetAsDoubleDigit(this int value)
    {
        string s = value.ToString();
        if(value < 10)
        {
            s = "0" + s;
        }
        return s;
    }
}
