namespace EBank.Domain.Models.Accounts
{
    public static class DomainRules
    {
        public static class PreConditions
        {
            public static class Account
            {
                public static class Name
                {
                    public static class Length
                    {
                        public const int Minimum = 6;

                        public const int Maximum = 32;
                    }
                }
            }
        }
    }
}
