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
using System.ComponentModel;

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

        public string LastModifiedUserId { get; set; }

        public string Description { get; set; }


        public int Status { get; set; }

        public string Tags { get; set; }

        public string[] Tags2 { get; set; }

        public string DescriptionImage { get; set; }
    }

    public enum ArticleStatus
    {
        [Description("Tạo bởi Admin")]
        CreatedByAdmins = 0,
        [Description("Chờ duyệt")]
        Pending = 1,
        [Description("Đã duyệt")]
        Approved = 2,
        [Description("Bị báo xấu")]
        Reported = 3,
        [Description("Bị khóa")]
        Bad = 4,
        [Description("Bình thường")]
        Normal = 5
    }

    public static class EnumHelper<T>
    {
        public static string GetEnumDescription(string value)
        {
            Type type = typeof(T);
            var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

            if (name == null)
            {
                return string.Empty;
            }
            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }
    }

    public class ArticleLike
    {
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string ArticleId { get; set; }

        public DateTime Date { get; set; }

        public ArticleLike()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
