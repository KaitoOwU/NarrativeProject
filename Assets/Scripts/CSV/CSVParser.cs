using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kaito.CSVParser
{
    public static class CSV
    {

        /// <summary>
        ///   <para>Unparse a CSV file to read its content.</para>
        /// </summary>
        /// <param name="path">Path to the target resource to load.</param>
        /// <param name="spliter">Seperator for different values *; by default*</param>
        /// <returns>
        ///   <para>The CSV file unparsed.</para>
        /// </returns>
        public static Dictionary<string, string[]> Unparse(string path, char spliter = ';')
        {
            Dictionary<string, string[]> data = new();
            try
            {
                StreamReader reader = new(path);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(spliter);
                    data[values[0]] = values[1..];
                    Debug.Log(values[0] + " : " + data[values[0]].ToStringArray());
                }
                
                return data;
            }
            catch (IOException)
            {
                return null;
            }
        }

        private static string ToStringArray(this string[] array)
        {
            string value = string.Empty;
            array.ToList().ForEach(str => value += str + ", ");
            return value;
        }
    }

    public enum Language
    {
        ENGLISH = 0,
        FRANCAIS = 1
    }
}
