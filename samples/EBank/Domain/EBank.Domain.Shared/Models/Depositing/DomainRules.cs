namespace EBank.Domain.Models.Depositing
{
    public static class DomainRules
    {
        public static class PreConditions
        {
            public static class DepositTransaction
            {
                public static class Amount
                {
                    public static class Range
                    {
                        public const int Minimum = 0;
                        public const int Maximum = 50000;
                    }
                }
            }
        }
    }
}
