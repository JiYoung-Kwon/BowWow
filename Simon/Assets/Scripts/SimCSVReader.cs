using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();

        FileInfo fi = new FileInfo(file);

        StreamReader sr = new StreamReader(fi.FullName);

        string strData = "";

        var strKey = sr.ReadLine().Split(',');

        while ((strData = sr.ReadLine()) != null)
        {
            var strValue = strData.Split(',');

            Dictionary<string, object> obj = new Dictionary<string, object>();
            for (int i = 0; i < strKey.Length; i++)
            {
                obj.Add(strKey[i], strValue[i]);
            }
            list.Add(obj);
        }
        sr.Close();
        return list;
    }
}
