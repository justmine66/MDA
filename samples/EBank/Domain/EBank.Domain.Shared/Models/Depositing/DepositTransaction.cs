namespace EBank.Domain.Models.Depositing
{
    public partial class DepositTransaction
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
