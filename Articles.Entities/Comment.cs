using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Comment
    {
        #region Properties
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        [Required]
        public string ArticleId { get; set; }

        public virtual Article Article { get; set; }

        [Required]
        public string UserName { get; set; }

        public DateTime? Date { get; set; }

        public string Content { get; set; }

        #endregion

        #region Constructor
        public Comment()
        {
            Id = Guid.NewGuid().ToString();
        }
        #endregion
    }
}
