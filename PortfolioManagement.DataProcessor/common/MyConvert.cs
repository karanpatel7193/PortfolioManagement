using System;

namespace PortfolioManagement.DataProcessor.Common
{
    public class MyConvert
    {
        /// <summary>
        /// Convert any object to string with handle null and DBNull, if any invalid value then return string.Empty value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted string value</returns>
        public static string ToString(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return string.Empty;
            else
            {
                try
                {
                    return Convert.ToString(Input);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Convert any object to int with handle null and DBNull, if any invalid value then return 0 value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted integer value</returns>
        public static int ToInt(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return 0;
            else
            {
                try
                {
                    return Convert.ToInt32(Input);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Convert any object to long with handle null and DBNull, if any invalid value then return 0 value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted long value</returns>
        public static long ToLong(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return 0;
            else
            {
                try
                {
                    return Convert.ToInt64(Input);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Convert any object to decimal with handle null and DBNull, if any invalid value then return 0 value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted decimal value</returns>
        public static decimal ToDecimal(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return 0;
            else
            {
                try
                {
                    return Convert.ToDecimal(Input);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Convert any object to bool with handle null and DBNull, if any invalid value then return false value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted bool value</returns>
        public static bool ToBoolean(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return false;
            else
            {
                try
                {
                    return Convert.ToBoolean(Input);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Convert any object to bool with handle null and DBNull, if any invalid value then return false value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted bool value</returns>
        public static DateTime ToDateTime(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return DateTime.MinValue;
            else
            {
                try
                {
                    return Convert.ToDateTime(Input);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// Convert any object to double with handle null and DBNull, if any invalid value then return 0 value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted double value</returns>
        public static double ToDouble(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return 0;
            else
            {
                try
                {
                    return Convert.ToDouble(Input);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Convert any object to string with handle null and DBNull, if any invalid value then return string.Empty value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted string value</returns>
        public static string ToNullableString(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return null;
            else
            {
                try
                {
                    return Convert.ToString(Input);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert any object to int with handle null and DBNull, if any invalid value then return 0 value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted integer value</returns>
        public static int? ToNullableInt(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return null;
            else
            {
                try
                {
                    return Convert.ToInt32(Input);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert any object to double with handle null and DBNull, if any invalid value then return 0 value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted double value</returns>
        public static double? ToNullableDouble(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return null;
            else
            {
                try
                {
                    return Convert.ToDouble(Input);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert any object to bool with handle null and DBNull, if any invalid value then return false value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted bool value</returns>
        public static DateTime? ToNullableDateTime(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return null;
            else
            {
                try
                {
                    return Convert.ToDateTime(Input);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert any object to long with handle null and DBNull, if any invalid value then return 0 value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted long value</returns>
        public static long? ToNullableLong(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return null;
            else
            {
                try
                {
                    return Convert.ToInt64(Input);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert any object to decimal with handle null and DBNull, if any invalid value then return 0 value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted decimal value</returns>
        public static decimal? ToNullableDecimal(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return null;
            else
            {
                try
                {
                    return Convert.ToDecimal(Input);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert any object to bool with handle null and DBNull, if any invalid value then return false value.
        /// </summary>
        /// <param name="Input">object input</param>
        /// <returns>converted bool value</returns>
        public static bool? ToNullableBoolean(object Input)
        {
            if (Input == null || Input == DBNull.Value)
                return null;
            else
            {
                try
                {
                    return Convert.ToBoolean(Input);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert Int to Int with handle null and 0, if any invalid value then return Null value.
        /// </summary>
        /// <param name="Input">Int? input</param>
        /// <returns>converted Int value</returns>
        public static int? ToNullableInt(int? Input)
        {
            if (Input == null || Input == 0)
                return null;
            else
            {
                try
                {
                    return Convert.ToInt32(Input);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static bool CheckBooleanValue(string val)
        {
            return (val == "0" || (String.Equals(val, Boolean.FalseString))) ? false : true;
        }

        public static DateTime? CheckDateTimeValue(string val)
        {
            return !String.IsNullOrEmpty(val) ? MyConvert.ToNullableDateTime(val) : null;
        }

        public static DateTime? ToIstDateTime(DateTime? dt)
        {
            if (dt == null)
                return null;

            DateTime dtReturn = MyConvert.ToDateTime(dt);
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById(AppSettings.IstTimeZoneName);
            if(dtReturn.Kind == DateTimeKind.Utc)
                dtReturn = TimeZoneInfo.ConvertTimeFromUtc(dtReturn, easternZone);
            else
            {
                var localZone = TimeZoneInfo.Local;
                dtReturn = TimeZoneInfo.ConvertTimeToUtc(dtReturn, localZone);
                dtReturn = TimeZoneInfo.ConvertTimeFromUtc(dtReturn, easternZone);
            }
            return dtReturn;
        }

        public static DateTime GetCurrentIstDateTime()
        {
            DateTime dt = DateTime.UtcNow;
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById(AppSettings.IstTimeZoneName);
            return TimeZoneInfo.ConvertTimeFromUtc(dt, easternZone);
        }
    }
}
