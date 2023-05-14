using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookApp.Core.Entities.Abstract
{
    public interface ITableBase
    {
        DateTime CreatedAt { get; set; }
        DateTime? LastUpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
