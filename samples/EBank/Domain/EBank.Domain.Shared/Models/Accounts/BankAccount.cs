namespace EBank.Domain.Models.Accounts
{
    public partial class BankAccount
    {
        public static class Id
        {
            public static class Range
            {
                public const int Minimum = 0;
            }
        }

        public static class Name
        {
            public static class Length
            {
                public const int Minimum = 2;

                public const int Maximum = 32;
            }
        }

        public static class Bank
        {
            public static class Length
            {
                public const int Minimum = 2;

                public const int Maximum = 64;
            }
        }

        public static class InitialBalance
        {
            public static class Range
            {
                public const int Minimum = 0;

            }
        }
    }
}
