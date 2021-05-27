namespace Coin
{
    using System.Collections.Generic;

    public static class Ico
    {
        public static IEnumerable<IcoAccount> Accounts => new List<IcoAccount>()
        {
            new IcoAccount()
            {
                Address = "Wmstgg5MNymzOTXRM2H8krJ05j9cj6ooioCuxvGal/0=",
                Balance = 4_000_000,
            },
            new IcoAccount()
            {
                Address = "Y8cL2lDxKQRcvjfyuho0my/rmLiQf2/1YUJq71a01SA=",
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
