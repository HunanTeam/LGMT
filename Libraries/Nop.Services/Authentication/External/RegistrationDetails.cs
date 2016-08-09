//Contributor:  Nicholas Mayne


namespace Nop.Services.Authentication.External
{
    /// <summary>
    /// Registration details
    /// </summary>
    public struct RegistrationDetails
    {
        public RegistrationDetails(OpenAuthenticationParameters parameters)
            : this()
        {
            if (parameters.UserClaims != null)
                foreach (var claim in parameters.UserClaims)
                {
                    //email, username
                    if (string.IsNullOrEmpty(EmailAddress))
                    {
                        if (claim.Contact != null)
                        {
                            EmailAddress = claim.Contact.Email;
                            UserName = claim.Contact.Email;
                        }
                    }
                    //first name
                    if (string.IsNullOrEmpty(FirstName))
                        if (claim.Name != null)
                            FirstName = claim.Name.First;
                    //last name
                    if (string.IsNullOrEmpty(LastName))
                        if (claim.Name != null)
                            LastName = claim.Name.Last;

                    //nickname
                    if (string.IsNullOrEmpty(Nickname))
                        if (claim.Name.Nickname != null)
                            Nickname = claim.Name.Nickname;

                    //figureurl
                    if (string.IsNullOrEmpty(FigureUrl))
                        if (claim.Person.FigureUrl != null)
                            FigureUrl = claim.Person.FigureUrl;

                    //gender
                    if (string.IsNullOrEmpty(Gender))
                        if (claim.Person.Gender != null)
                            Gender = claim.Person.Gender;
                }
        }

        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Nickname { get; set; }
        public string Gender { get; set; }
        public string FigureUrl { get; private set; }
    }
}