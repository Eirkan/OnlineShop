using Customer.Core.Domain.Primitives;

namespace Customer.Core.Domain.Extensions
{
    public static class ValueObjectExtensions
    {
        /// <summary>
        /// Uses reflection to extract all public properties from the target value object
        /// </summary>
        public static IEnumerable<object> GetComponents<T>(this ValueObject target)
            where T : ValueObject
        {
            return target.GetType().GetProperties().Select(propInfo => propInfo.GetValue(target, null)!);
        }
    }
}
