using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel;

namespace MomWorld.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            //Note
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Description { get; set; }

        // TODO: Create Social Profile
        // public object Social { get; set; }

        public DateTime? DOB { get; set; }

        public string Address { get; set; }

        public string FullName { get; set; }

        public string ProfilePicture { get; set; }

        public DateTime? DatePregnancy { get; set; }

        public int Status { get; set; }
    }

    public enum IdentityStatus
    {
        [Description("Bị khóa")]
        Locked = 1,
        [Description("Bình thường")]
        Normal = 2
    }

    public class TopUsersModel
    {
        public string Id { get; set; }

        public int ArticlesNumber { get; set; }

        public int LikesNumber { get; set; }

        public TopUsersModel(string id, int arts, int likes )
        {
            Id = id;
            ArticlesNumber = arts;
            LikesNumber = likes;
        }
    }
}