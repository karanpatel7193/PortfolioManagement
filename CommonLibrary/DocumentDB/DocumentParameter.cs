using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DocumentDB
{
    public class DocumentParameter
    {
        public GroupOperator Condition { get; set; } = GroupOperator.AND;

        public string Parameter { get; set; } = string.Empty;

        public FieldType Type { get; set; } = FieldType.String;

        public CompareOperator Compare { get; set; } = CompareOperator.Equal;

        public string Value { get; set; } = string.Empty;

        internal string DocumentValue
        {
            get
            {
                if (Type == FieldType.String)
                    return "\"" + Value + "\"";
                else if (Type == FieldType.Number)
                    return Value;
                else if (Type == FieldType.DateTime)
                    return Value;
                else
                    return string.Empty;
            }
        }

        public List<DocumentParameter> Childs = new List<DocumentParameter>();
    }

    public enum CompareOperator
    {
        [Display(Name = "==")]
        Equal = 1,

        [Display(Name = "!=")]
        NotEqual = 2,

        [Display(Name = ">")]
        GreaterThan = 3,

        [Display(Name = ">=")]
        GreaterThanEqual = 4,

        [Display(Name = "<")]
        LessThan = 5,

        [Display(Name = "<=")]
        LessThanEqual = 6,

        [Display(Name = "Contains")]
        Contains = 7,

        [Display(Name = "BeginWith")]
        BeginWith = 8,

        [Display(Name = "EndWith")]
        EndWith = 9,

        [Display(Name = "In")]
        In = 10,

        [Display(Name = "NotIn")]
        NotIn = 11

    }

    public enum GroupOperator
    {
        [Display(Name = "||")]
        OR = 1,

        [Display(Name = "&&")]
        AND = 2,

        [Display(Name = "!=")]
        NOT = 3,

        [Display(Name = "!||")]
        NOR = 4
    }

    public enum FieldType
    {
        String = 1,
        Number = 2,
        DateTime = 3
    }
}
