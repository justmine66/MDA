using System;
using System.ComponentModel.DataAnnotations;

namespace EBank.ApiServer.Infrastructure.ModelValidations
{
    /// <summary>
    /// 为数据字段的值大于等于约束。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class GreaterThanAttribute : ValidationAttribute
    {
        /// <summary>
        /// 下限
        /// </summary>
        public object LowerLimit { get; }

        /// <summary>
        /// 初始化 <see cref="GreaterThanAttribute"/> 对象。
        /// </summary>
        /// <param name="lowerLimit">下限</param>
        public GreaterThanAttribute(short lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        /// <summary>
        /// 初始化 <see cref="GreaterThanAttribute"/> 对象。
        /// </summary>
        /// <param name="lowerLimit">下限</param>
        public GreaterThanAttribute(ushort lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        /// <summary>
        /// 初始化 <see cref="GreaterThanAttribute"/> 对象。
        /// </summary>
        /// <param name="lowerLimit">下限</param>
        public GreaterThanAttribute(int lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        /// <summary>
        /// 初始化 <see cref="GreaterThanAttribute"/> 对象。
        /// </summary>
        /// <param name="lowerLimit">下限</param>
        public GreaterThanAttribute(uint lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        /// <summary>
        /// 初始化 <see cref="GreaterThanAttribute"/> 对象。
        /// </summary>
        /// <param name="lowerLimit">下限</param>
        public GreaterThanAttribute(long lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        /// <summary>
        /// 初始化 <see cref="GreaterThanAttribute"/> 对象。
        /// </summary>
        /// <param name="lowerLimit">下限</param>
        public GreaterThanAttribute(ulong lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        /// <summary>
        /// 初始化 <see cref="GreaterThanAttribute"/> 对象。
        /// </summary>
        /// <param name="lowerLimit">下限</param>
        public GreaterThanAttribute(float lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        /// <summary>
        /// 初始化 <see cref="GreaterThanAttribute"/> 对象。
        /// </summary>
        /// <param name="lowerLimit">下限</param>
        public GreaterThanAttribute(double lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        /// <summary>
        /// 初始化 <see cref="GreaterThanAttribute"/> 对象。
        /// </summary>
        /// <param name="lowerLimit">下限</param>
        public GreaterThanAttribute(decimal lowerLimit)
        {
            LowerLimit = lowerLimit;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var typeCode = Type.GetTypeCode(value.GetType());

            switch (typeCode)
            {
                case TypeCode.Int16:
                    if (short.TryParse(LowerLimit.ToString(), out var lowerLimitShortValue) &&
                        short.TryParse(value.ToString(), out var shortValue) &&
                        shortValue <= lowerLimitShortValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be greater than {LowerLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.UInt16:
                    if (ushort.TryParse(LowerLimit.ToString(), out var lowerLimitUshortValue) &&
                        ushort.TryParse(value.ToString(), out var ushortValue) &&
                        ushortValue <= lowerLimitUshortValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be greater than {LowerLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Int32:
                    if (int.TryParse(LowerLimit.ToString(), out var lowerLimitIntValue) &&
                        int.TryParse(value.ToString(), out var intValue) &&
                        intValue <= lowerLimitIntValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be greater than {LowerLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.UInt32:
                    if (uint.TryParse(LowerLimit.ToString(), out var lowerLimitUintValue) &&
                        uint.TryParse(value.ToString(), out var uintValue) &&
                        uintValue <= lowerLimitUintValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be greater than {LowerLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Int64:
                    if (long.TryParse(LowerLimit.ToString(), out var lowerLimitLongValue) &&
                        long.TryParse(value.ToString(), out var longValue) &&
                        longValue <= lowerLimitLongValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be greater than {LowerLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.UInt64:
                    if (ulong.TryParse(LowerLimit.ToString(), out var lowerLimitUlongValue) &&
                        ulong.TryParse(value.ToString(), out var ulongValue) &&
                        ulongValue <= lowerLimitUlongValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be greater than {LowerLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Single:
                    if (float.TryParse(LowerLimit.ToString(), out var lowerLimitFloatValue) &&
                        float.TryParse(value.ToString(), out var floatValue) &&
                        floatValue <= lowerLimitFloatValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be greater than {LowerLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Double:
                    if (double.TryParse(LowerLimit.ToString(), out var lowerLimitDoubleValue) &&
                        double.TryParse(value.ToString(), out var doubleValue) &&
                        doubleValue <= lowerLimitDoubleValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be greater than {LowerLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Decimal:
                    if (decimal.TryParse(LowerLimit.ToString(), out var lowerLimitDecimalValue) &&
                        decimal.TryParse(value.ToString(), out var decimalValue) &&
                        decimalValue <= lowerLimitDecimalValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be greater than {LowerLimit}.", new[] { nameof(value) });
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }

            return ValidationResult.Success;
        }
    }
}
