using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core
{
    public class Category
    {
        protected int _Id;
        protected string _CategoryName;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  Id
        {
            get => _Id;
            set => _Id = value;
        }
        public string CategoryName
        {
            get => _CategoryName;
            set =>_CategoryName=value;
        }
    }
}
