using System;

namespace Event_Management_System.Models.Base
{
    [Serializable]
    public class RegularUser : User
    {

        private string _address;

        public string Address
        {
            get => _address;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Address cannot be empty.");
                _address = value;
            }
        }
        
        protected RegularUser() { }
        
        public RegularUser(string username, string email, DateTime dateOfBirth, string password, string address)
            : base(username, email, dateOfBirth, password)
        {
            Address = address;
            UserTypes = UserType.Regular;
        }

    }
}