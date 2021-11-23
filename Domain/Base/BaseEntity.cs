using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public class BaseEntity
    {
        public int? Code { get; set; }

        public int? User { get; set; }

        public int? UsuInc { get; set; }

        public DateTime? DatInc { get; set; }

        public int? UsuAlt { get; set; }

        public DateTime? DatAlt { get; set; }

        public int? UsuDel { get; set; }

        public DateTime? DatDel { get; set; }
    }
}
