using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Entity
{
    public class User : IdentityUser
    {
        public const string TypeSuperAdmin = "SUPERADMIN";
        public const string TypeAdmin = "ADMIN";
        public const string TypeGeneral = "GENERAL";
        public const string TypeExternal = "EXTERNAL";
        public const string TypeCustomer = "CUSTOMER";

        public const string StatusActive = "ACTIVE";
        public const string StatusInactive = "INACTIVE";


        protected User()
        {

        }
        public User(string name, string userName, string email, string type) : base(userName)
        {
            Name = name;
            UserName = userName;
            Email = email;
            CreatedOn = DateTime.Now;
            Type = type;
            Status = StatusActive;
        }
        public void Update(string name,string userName,string email,string phoneNumber)
        {
            Name = name;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        public string Name { get; set; }
        public DateTime CreatedOn { get; protected set; }
        public string Status { get; protected set; }
        public string Type { get; set; }
        public bool IsActive => Status == StatusActive;
        public bool IsSuperAdmin => Type == TypeSuperAdmin;
        public void Activate()
        {
            Status = StatusActive;
            LockoutEnd = DateTime.Now.AddDays(-1);
        }
        public void Deactivate()
        {
            Status = StatusInactive;
            LockoutEnabled = true;
            LockoutEnd = DateTime.MaxValue;
        }

    }
}
