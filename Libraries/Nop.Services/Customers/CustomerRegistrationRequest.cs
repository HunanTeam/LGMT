using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers
{
    /// <summary>
    /// Customer registration request
    /// </summary>
    public class CustomerRegistrationRequest
    {
        /// <summary>
        /// Customer
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }
        public string CustomerPhone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Password format
        /// </summary>
        public PasswordFormat PasswordFormat { get; set; }

        public RegisterSource RegisterSource { get; set; }

        /// <summary>
        /// Is approved
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="email">Email</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="passwordFormat">Password fprmat</param>
        /// <param name="isApproved">Is approved</param>
        public CustomerRegistrationRequest(Customer customer, string customerPhone, string email, string username,
            string password,
            PasswordFormat passwordFormat,
            bool isApproved = true, RegisterSource source = RegisterSource.System)
        {
            this.Customer = customer;
            this.CustomerPhone = customerPhone;
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
            this.RegisterSource = source;
            this.IsApproved = isApproved;
        }
    }
}
