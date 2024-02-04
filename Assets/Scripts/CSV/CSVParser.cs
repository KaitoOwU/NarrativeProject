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
        public static List<(string, string[])> UnparseDialogs(char spliter = ';')
        {
            List<(string ID, string[] Values)> data = new();
            try
            {
                TextAsset txt = (TextAsset)Resources.Load("Dialog", typeof(TextAsset));
                string fileContent = txt.text;
                string[] splitedContent = fileContent.Split("\n");

                data.AddRange(splitedContent.Select(str => str.Split(spliter)).Select(values => (values[0], values[1..])));
                return data;
            }

            catch (IOException)
            {
                Debug.LogWarning("File could not be read. Please check file isn't open");
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
