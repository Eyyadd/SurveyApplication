﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain.Entities
{
    public class BaseEntity : Audit
    {
        public int Id { get; set; }
    }
}
