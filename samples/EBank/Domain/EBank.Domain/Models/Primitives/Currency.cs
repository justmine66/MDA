using MDA.Domain.Models;
using MDA.Infrastructure.Utils;
using System.Collections.Generic;

namespace EBank.Domain.Models.Primitives
{
    public class Currency : ValueObject
    {
        public static Currency Default = new Currency("CNY", "￥", "中国人民币");

        public Currency(string code, string symbol, string displayName)
        {
            PreConditions.NotNullOrEmpty(code, nameof(code));
            PreConditions.NotNullOrEmpty(symbol, nameof(symbol));
            PreConditions.NotNullOrEmpty(displayName, nameof(displayName));

            Code = code;
            Symbol = symbol;
            DisplayName = displayName;
        }

        /// <summary>
        /// 代码
        /// </summary>
        /// <example>CNY</example>
        public string Code { get; }

        /// <summary>
        /// 符号
        /// </summary>
        /// <example>￥</example>
        public string Symbol { get; }

        /// <summary>
        /// 显示名称
        /// </summary>
        /// <example>中国人民币</example>
        public string DisplayName { get; }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return Code;
            yield return Symbol;
            yield return DisplayName;
        }

        public override string ToString() => $"[Code: {Code}, Symbol: {Symbol}, Code: {DisplayName}]";
    }
}
