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
        public virtual void Update(string name,string userName,string email,string phoneNumber)
        {
            Name = name;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        public virtual string Name { get; set; }
        public virtual DateTime CreatedOn { get; protected set; }
        public virtual string Status { get; protected set; }
        public virtual string Type { get; set; }
        public virtual bool IsActive => Status == StatusActive;
        public virtual bool IsSuperAdmin => Type == TypeSuperAdmin;
        public virtual void Activate()
        {
            Status = StatusActive;
            LockoutEnd = DateTime.Now.AddDays(-1);
        }
        public virtual void Deactivate()
        {
            Status = StatusInactive;
            LockoutEnabled = true;
            LockoutEnd = DateTime.MaxValue;
        }

    }
}
