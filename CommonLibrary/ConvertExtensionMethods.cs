using ClosedXML.Excel;
using CommonLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CommonLibrary
{
    public static class ConvertExtensionMethods
    {
        public static string ToDisplayString(this Enum en)
        {
            var fieldInfo = en.GetType().GetField(en.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null)
                en.ToString();
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : en.ToString();
        }

        public static DataTable ToDataTable(this Enum en)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("eValue", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("eText", Type.GetType("System.String")));

            foreach (int value in Enum.GetValues(en.GetType()))
            {
                DataRow dr = dt.NewRow();
                dr["eValue"] = value;
                dr["eText"] = Enum.Parse(en.GetType(), value.ToString());
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static string ToXML(this DataSet ds, bool IsReplaceSingleToDoubleQoute = true)
        {
            string s = ds.GetXmlSchema();
            s += ds.GetXml();

            if (IsReplaceSingleToDoubleQoute)
                s.Replace("'", "''");
            return s;
        }

        public static string ToXML(this DataTable dt, bool IsReplaceSingleToDoubleQoute = true)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            return ds.ToXML(IsReplaceSingleToDoubleQoute);
        }

        public static string ToXML<T>(this List<T> lst, bool omitXML = true)
        {
            //https://stackoverflow.com/questions/10218181/xmlserializer-serialize-stripping-the-xml-tag
            string s = string.Empty;
            XmlSerializer serialiser = new XmlSerializer(typeof(List<T>));
            using (var stream = new StringWriter())
            {
                serialiser.Serialize(stream, lst);
                s = stream.ToString();
            }
            if (omitXML)
                return s.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            else
                return s;
        }

        public static string ToXML(this object aObject, bool omitXML = true)
        {
            string s = string.Empty;
            using (System.IO.StringWriter stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(aObject.GetType());
                serializer.Serialize(stringwriter, aObject);
                s = stringwriter.ToString();
            }
            if (omitXML)
                return s.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            else
                return s;
        }

        public static string ToXML<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            XElement root = new XElement("root");

            foreach (var pair in dict)
            {
                XElement cElement = new XElement("dictionary");
                cElement.SetAttributeValue("key", pair.Key);
                cElement.SetAttributeValue("value", pair.Value);
                root.Add(cElement);
            }
            return root.ToString();
        }

        public static string ToHTML<T>(this List<T> aObject, string Columns = "")
        {
            object objEAL = Activator.CreateInstance(typeof(T));
            IList<T> iList = (IList<T>)aObject;
            if (iList == null || iList.Count == 0)
                return string.Empty;

            List<string> lColumns = objEAL.GetFields(Columns);

            StringBuilder sb = new StringBuilder("<table>");
            PropertyInfo prop;

            sb.Append("<tr>");
            for (int j = 0; j < lColumns.Count; j++)
            {
                prop = objEAL.GetType().GetProperty(lColumns[j]);
                sb.Append(String.Format("<th{1}>{0}<th>", prop.ToDisplayName(), prop.AddHeaderCssClass()));
            }
            sb.Append("</tr>");

            for (int i = 0; i < iList.Count; i++)
            {
                sb.Append("<tr>");
                for (int j = 0; j < lColumns.Count; j++)
                {
                    objEAL = iList[i];
                    prop = objEAL.GetType().GetProperty(lColumns[j]);
                    sb.Append(String.Format("<td{1}>{0}<td>", MyConvert.ToString(prop.GetValue(objEAL, null)), prop.AddDetailCssClass()));
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        public static string ToHTML(this object objEAL, string Columns = "")
        {
            if (objEAL == null)
                return string.Empty;

            List<string> lColumns = objEAL.GetFields(Columns);

            StringBuilder sb = new StringBuilder("<div>");
            PropertyInfo prop;

            for (int j = 0; j < lColumns.Count; j++)
            {
                sb.Append("<div>");
                prop = objEAL.GetType().GetProperty(lColumns[j]);
                sb.Append(String.Format("<div{1}>{0}</div><div{3}>{2}</div>", prop.ToDisplayName(), prop.AddParameterHeaderCssClass(), prop.GetValue(objEAL, null), prop.AddParameterDetailCssClass()));
                sb.Append("</div>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        public static List<string> GetFields(this object objEAL, string Columns = "")
        {
            List<string> lColumns = new List<string>();
            if (Columns == "")
            {
                foreach (var propertyInfo in objEAL.GetType().GetProperties())
                {
                    lColumns.Add(propertyInfo.Name);
                }
            }
            else
            {
                foreach (string str in Columns.Split(','))
                {
                    lColumns.Add(str);
                }
            }
            return lColumns;
        }

        public static string ToUserFriendlyName(this string str)
        {
            int position;
            string finalString;
            char prevChar;
            position = 0;
            str = str.Replace('_', ' ');
            prevChar = str.Substring(0, 1).ToUpper()[0];
            finalString = prevChar.ToString();
            position = position + 1;

            while (position < str.Length)
            {
                int strAscii = (int)str.Substring(position, 1)[0];
                int prevAscii = (int)prevChar;
                if (((strAscii >= 65 && strAscii <= 90) && (prevAscii < 65 || prevAscii > 90)) || ((strAscii >= 48 && strAscii <= 57) && (prevAscii < 48 || prevAscii > 57)))
                    finalString = finalString + ' ' + str.Substring(position, 1);
                else
                    finalString = finalString + str.Substring(position, 1);

                prevChar = str.Substring(position, 1)[0];
                position = position + 1;
            }
            return finalString;

        }

        public static string ToDisplayName(this PropertyInfo prop)
        {
            var descriptionAttributes = prop.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (descriptionAttributes != null && descriptionAttributes.Length > 0)
                return descriptionAttributes[0].Name;
            else
                return prop.Name.ToUserFriendlyName();
        }

        private static string AddHeaderCssClass(this PropertyInfo prop)
        {
            var descriptionAttributes = prop.GetCustomAttributes(typeof(CssClassAttribute), false) as CssClassAttribute[];

            if (descriptionAttributes != null && descriptionAttributes.Length > 0 && descriptionAttributes[0].HeaderName.Length > 0)
                return " class='" + descriptionAttributes[0].HeaderName + "'";
            else
                return string.Empty;
        }

        private static string AddDetailCssClass(this PropertyInfo prop)
        {
            var descriptionAttributes = prop.GetCustomAttributes(typeof(CssClassAttribute), false) as CssClassAttribute[];

            if (descriptionAttributes != null && descriptionAttributes.Length > 0 && descriptionAttributes[0].DetailName.Length > 0)
                return " class='" + descriptionAttributes[0].DetailName + "'";
            else
                return string.Empty;
        }

        private static string AddParameterHeaderCssClass(this PropertyInfo prop)
        {
            var descriptionAttributes = prop.GetCustomAttributes(typeof(CssClassAttribute), false) as CssClassAttribute[];

            if (descriptionAttributes != null && descriptionAttributes.Length > 0 && descriptionAttributes[0].ParameterHeaderName.Length > 0)
                return " class='" + descriptionAttributes[0].ParameterHeaderName + "'";
            else
                return string.Empty;
        }

        private static string AddParameterDetailCssClass(this PropertyInfo prop)
        {
            var descriptionAttributes = prop.GetCustomAttributes(typeof(CssClassAttribute), false) as CssClassAttribute[];

            if (descriptionAttributes != null && descriptionAttributes.Length > 0 && descriptionAttributes[0].ParameterDetailName.Length > 0)
                return " class='" + descriptionAttributes[0].ParameterDetailName + "'";
            else
                return string.Empty;
        }

        public static string ToCSV<T>(this List<T> aObject, string Seperator = ",", string Columns = "")
        {
            StringBuilder sb = new StringBuilder();
            string result = string.Empty;
            try
            {
                object objEAL = Activator.CreateInstance(typeof(T));
                IList<T> iList = (IList<T>)aObject;
                if (iList == null || iList.Count == 0)
                    return string.Empty;

                List<string> lColumns = objEAL.GetFields(Columns);

                PropertyInfo prop;
                List<string> header = new List<string>();
                for (int j = 0; j < lColumns.Count; j++)
                {
                    prop = objEAL.GetType().GetProperty(lColumns[j]);
                    header.Add(prop.ToDisplayName());
                }

                sb.Append(String.Join(Seperator, header.ToArray()));
                sb.Append(Environment.NewLine);

                for (int i = 0; i < iList.Count; i++)
                {
                    List<string> detail = new List<string>();
                    for (int j = 0; j < lColumns.Count; j++)
                    {
                        objEAL = iList[i];
                        prop = objEAL.GetType().GetProperty(lColumns[j]);
                        detail.Add(MyConvert.ToString(prop.GetValue(objEAL, null) == null ? "NULL" : prop.GetValue(objEAL, null)));
                    }
                    sb.Append(String.Join(Seperator, detail.ToArray()));
                    sb.Append(Environment.NewLine);
                }
                result = sb.ToString();
            }
            finally
            {
                sb.Clear();
                sb = null;
            }
            return result;
        }

        public static string ToCSV<T>(this List<T> aObject, string Columns = "")
        {
            StringBuilder sb = new StringBuilder();
            string result = string.Empty;
            try
            {
                object objEAL = Activator.CreateInstance(typeof(T));
                IList<T> iList = (IList<T>)aObject;
                if (iList == null || iList.Count == 0)
                    return string.Empty;

                List<string> lColumns = objEAL.GetFields(Columns);

                PropertyInfo prop;
                List<string> header = new List<string>();
                for (int j = 0; j < lColumns.Count; j++)
                {
                    prop = objEAL.GetType().GetProperty(lColumns[j]);
                    header.Add(prop.ToDisplayName());
                }

                sb.Append(String.Join(",", header.ToArray()));
                sb.Append(Environment.NewLine);

                for (int i = 0; i < iList.Count; i++)
                {
                    List<string> detail = new List<string>();
                    for (int j = 0; j < lColumns.Count; j++)
                    {
                        objEAL = iList[i];
                        prop = objEAL.GetType().GetProperty(lColumns[j]);
                        detail.Add(MyConvert.ToString(prop.GetValue(objEAL, null)));
                    }
                    sb.Append(String.Join(",", detail.ToArray()));
                    sb.Append(Environment.NewLine);
                }
                result = sb.ToString();
            }
            finally
            {
                sb.Clear();
                sb = null;
            }
            return result;
        }

        public static string ToCSVFile<T>(this List<T> aObject, string FilePath, string Columns = "")
        {
            StringBuilder sb = new StringBuilder();
            StreamWriter streamWriter = null;
            try
            {
                object objEAL = Activator.CreateInstance(typeof(T));
                IList<T> iList = (IList<T>)aObject;
                if (iList == null || iList.Count == 0)
                    return string.Empty;

                List<string> lColumns = objEAL.GetFields(Columns);
                PropertyInfo prop;

                List<string> header = new List<string>();
                for (int j = 0; j < lColumns.Count; j++)
                {
                    prop = objEAL.GetType().GetProperty(lColumns[j]);
                    header.Add(prop.ToDisplayName());
                }
                sb.Append(String.Join(",", header.ToArray()));
                streamWriter = new StreamWriter(FilePath);
                streamWriter.WriteLine(sb.ToString());

                for (int i = 0; i < iList.Count; i++)
                {
                    sb = new StringBuilder();
                    List<string> detail = new List<string>();
                    for (int j = 0; j < lColumns.Count; j++)
                    {
                        objEAL = iList[i];
                        prop = objEAL.GetType().GetProperty(lColumns[j]);
                        detail.Add(MyConvert.ToString(prop.GetValue(objEAL, null)));
                    }
                    sb.Append(String.Join(",", detail.ToArray()));
                    streamWriter.WriteLine(String.Join(Environment.NewLine, sb.ToString()));
                }
            }
            finally
            {
                sb.Clear();
                sb = null;
                streamWriter.Dispose();
                streamWriter = null;
            }
            return FilePath;
        }

        /// <summary>
        /// function to remove xml blank nodes
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns>XML string without blank node</returns>
        public static string RemoveXMLBlankNode(this string xmlString)
        {
            string result = xmlString;
            if (!String.IsNullOrEmpty(xmlString))
            {
                xmlString = xmlString.Replace("xsi:nil=\"true\"", "");
                var document = XDocument.Parse(xmlString);

                document.Descendants()
                           .Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value))
                        .Remove();
                result = document.ToString();
                result = HttpUtility.HtmlDecode(result);
            }
            return result;
        }

        /// <summary>
        /// function to remove xml blank nodes
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns>XML string without blank node</returns>
        public static string ReplaceAmpersandCharacter(this string xmlString)
        {
            string result = xmlString;
            if (!String.IsNullOrEmpty(xmlString))
            {
                result = xmlString.Replace("&", "&amp;");
            }
            return result;
        }
    }
}
