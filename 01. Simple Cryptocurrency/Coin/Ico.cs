namespace Coin
{
    using System.Collections.Generic;

    public static class Ico
    {
        public static IEnumerable<IcoAccount> Accounts => new List<IcoAccount>()
        {
            new IcoAccount()
            {
                Address = "7v+K6cSv86mGLAhhLw3dtPWUp5rpDqtkKARG80OdElU=",
                Balance = 4_000_000,
            },

            new IcoAccount()
            {
                Address = "0WJ2IWr5Ucu/Cn5E/YMtmGcJI/vYiGS1+MgFnHOu8rA=",
                Balance = 2_000_000,
            },
        };

        public class IcoAccount
        {
            public string Address { get; set; }

            public double Balance { get; set; }
        }
    }
}
