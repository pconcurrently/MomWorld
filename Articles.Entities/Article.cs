using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace MomWorld.Entities
{
    public class Article
    {
        public Article()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime? PostedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? ViewNumber { get; set; }

        public string LastSeenUserId { get; set; }

        public string Description { get; set; }


        public int Status { get; set; }
    }

    public enum ArticleStatus
    {
        CreatedByAdmins = 0,
        Pending = 1,
        Approved = 2,
        Reported = 3,
        Bad = 4,
        Normal = 5
    }

    public class ArticleLike
    {
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string ArticleId { get; set; }

        public ArticleLike()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    //public class ApplicationUser : IdentityUser
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }

    //    public virtual ICollection<Article> Articles { get; set; }

    //}
}
