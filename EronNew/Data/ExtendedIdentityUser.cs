namespace Microsoft.AspNetCore.Identity
{
    public class ExtendedIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool? Newsletter { get; set; }

        public string Country { get; set; }
    }
}
