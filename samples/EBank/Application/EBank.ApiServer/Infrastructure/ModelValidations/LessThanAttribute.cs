using System;
using System.ComponentModel.DataAnnotations;

namespace EBank.ApiServer.Infrastructure.ModelValidations
{
    /// <summary>
    /// 为数据字段的值大于等于约束。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class LessThanAttribute : ValidationAttribute
    {
        /// <summary>
        /// 上限
        /// </summary>
        public object SupperLimit { get; }

        /// <summary>
        /// 初始化 <see cref="LessThanAttribute"/> 对象。
        /// </summary>
        /// <param name="supperLimit">上限</param>
        public LessThanAttribute(short supperLimit)
        {
            SupperLimit = supperLimit;
        }

        /// <summary>
        /// 初始化 <see cref="LessThanAttribute"/> 对象。
        /// </summary>
        /// <param name="supperLimit">上限</param>
        public LessThanAttribute(ushort supperLimit)
        {
            SupperLimit = supperLimit;
        }

        /// <summary>
        /// 初始化 <see cref="LessThanAttribute"/> 对象。
        /// </summary>
        /// <param name="supperLimit">上限</param>
        public LessThanAttribute(int supperLimit)
        {
            SupperLimit = supperLimit;
        }

        /// <summary>
        /// 初始化 <see cref="LessThanAttribute"/> 对象。
        /// </summary>
        /// <param name="supperLimit">上限</param>
        public LessThanAttribute(uint supperLimit)
        {
            SupperLimit = supperLimit;
        }

        /// <summary>
        /// 初始化 <see cref="LessThanAttribute"/> 对象。
        /// </summary>
        /// <param name="supperLimit">上限</param>
        public LessThanAttribute(long supperLimit)
        {
            SupperLimit = supperLimit;
        }

        /// <summary>
        /// 初始化 <see cref="LessThanAttribute"/> 对象。
        /// </summary>
        /// <param name="supperLimit">上限</param>
        public LessThanAttribute(ulong supperLimit)
        {
            SupperLimit = supperLimit;
        }

        /// <summary>
        /// 初始化 <see cref="LessThanAttribute"/> 对象。
        /// </summary>
        /// <param name="supperLimit">上限</param>
        public LessThanAttribute(float supperLimit)
        {
            SupperLimit = supperLimit;
        }

        /// <summary>
        /// 初始化 <see cref="LessThanAttribute"/> 对象。
        /// </summary>
        /// <param name="supperLimit">上限</param>
        public LessThanAttribute(double supperLimit)
        {
            SupperLimit = supperLimit;
        }

        /// <summary>
        /// 初始化 <see cref="LessThanAttribute"/> 对象。
        /// </summary>
        /// <param name="supperLimit">上限</param>
        public LessThanAttribute(decimal supperLimit)
        {
            SupperLimit = supperLimit;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var typeCode = Type.GetTypeCode(value.GetType());

            switch (typeCode)
            {
                case TypeCode.Int16:
                    if (short.TryParse(SupperLimit.ToString(), out var lowerLimitShortValue) &&
                        short.TryParse(value.ToString(), out var shortValue) &&
                        shortValue >= lowerLimitShortValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be less than {SupperLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.UInt16:
                    if (ushort.TryParse(SupperLimit.ToString(), out var lowerLimitUshortValue) &&
                        ushort.TryParse(value.ToString(), out var ushortValue) &&
                        ushortValue >= lowerLimitUshortValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be less than {SupperLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Int32:
                    if (int.TryParse(SupperLimit.ToString(), out var lowerLimitIntValue) &&
                        int.TryParse(value.ToString(), out var intValue) &&
                        intValue >= lowerLimitIntValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be less than {SupperLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.UInt32:
                    if (uint.TryParse(SupperLimit.ToString(), out var lowerLimitUintValue) &&
                        uint.TryParse(value.ToString(), out var uintValue) &&
                        uintValue >= lowerLimitUintValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be less than {SupperLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Int64:
                    if (long.TryParse(SupperLimit.ToString(), out var lowerLimitLongValue) &&
                        long.TryParse(value.ToString(), out var longValue) &&
                        longValue >= lowerLimitLongValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be less than {SupperLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.UInt64:
                    if (ulong.TryParse(SupperLimit.ToString(), out var lowerLimitUlongValue) &&
                        ulong.TryParse(value.ToString(), out var ulongValue) &&
                        ulongValue >= lowerLimitUlongValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be less than {SupperLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Single:
                    if (float.TryParse(SupperLimit.ToString(), out var lowerLimitFloatValue) &&
                        float.TryParse(value.ToString(), out var floatValue) &&
                        floatValue >= lowerLimitFloatValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be less than {SupperLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Double:
                    if (double.TryParse(SupperLimit.ToString(), out var lowerLimitDoubleValue) &&
                        double.TryParse(value.ToString(), out var doubleValue) &&
                        doubleValue >= lowerLimitDoubleValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be less than {SupperLimit}.", new[] { nameof(value) });
                    }
                    break;
                case TypeCode.Decimal:
                    if (decimal.TryParse(SupperLimit.ToString(), out var lowerLimitDecimalValue) &&
                        decimal.TryParse(value.ToString(), out var decimalValue) &&
                        decimalValue >= lowerLimitDecimalValue)
                    {
                        return new ValidationResult($"The {validationContext.DisplayName} must be less than {SupperLimit}.", new[] { nameof(value) });
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }

            return ValidationResult.Success;
        }
    }
}
