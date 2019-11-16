using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnumCheck<T> {

    public static bool EnumArrayContains(T[] array, T value)
    {
        foreach (T e in array)
            if (value.Equals(e))
                return true;
        return false;
    }

    public static List<T> GetEnumValues()
    {
        List<T> ret = new List<T>();
        foreach(T e in Enum.GetValues(typeof(T)))
            ret.Add(e);
        return ret;
    }
}
