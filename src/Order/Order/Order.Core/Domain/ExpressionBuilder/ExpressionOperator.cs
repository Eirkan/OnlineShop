using System.Runtime.Serialization;

namespace Order.Core.Domain.ExpressionBuilder
{
    public enum ExpressionOperator
    {
        [EnumMember]
        Equals = 0,
        [EnumMember]
        NotEquals = 1,

        [EnumMember]
        GreaterThan = 2,
        [EnumMember]
        LessThan = 3,
        [EnumMember]
        GreaterThanOrEqual = 4,
        [EnumMember]
        LessThanOrEqual = 5,

        [EnumMember]
        Contains = 6,
        [EnumMember]
        StartsWith = 7,
        [EnumMember]
        EndsWith = 8,
    }
}