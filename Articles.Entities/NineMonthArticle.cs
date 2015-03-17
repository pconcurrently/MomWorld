using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class NineMonthArticle
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        [Required]
        public int Date { get; set; }

        [Required]
        public string Content { get; set; }

        #region Constructor
        public NineMonthArticle()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion
    }
}
