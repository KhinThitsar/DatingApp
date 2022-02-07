
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class UserApp 
    {
         public int ID{get;set;}
         public string userName{get;set;}
         public byte[] PasswordHash{get;set;}
        public byte[] PasswordSalt{get;set;}
        public DateTime DateOfBirth{get;set;}
        public string KnownAs{get;set;}
        public DateTime Created{get;set;}
        public DateTime LastActive{get;set;}
        public string Gender{get;set;}
        public string Introduction{get;set;}

        public string LookingFor{get;set;}
        public string Interests{get;set;}
        public string City{get;set;}
        public string Country{get;set;}
        public ICollection<Photo> Photos{get;set;}

        public ICollection<UserLike> LikedByUsers{get;set;}
        public ICollection<UserLike> LikedUsers{get;set;}

        //public ICollection<AppUserRole> UserRoles{get;set;}
        // public int GetAge()
        // {
        //     return DateOfBirth.calculateAge();
        // }
    }
}