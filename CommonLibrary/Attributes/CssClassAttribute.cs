using System;

namespace CommonLibrary.Attributes
{
    public class CssClassAttribute : Attribute
    {
        public string HeaderName = string.Empty;
        public string DetailName = string.Empty;
        public string ParameterHeaderName = string.Empty;
        public string ParameterDetailName = string.Empty;
    }
}
