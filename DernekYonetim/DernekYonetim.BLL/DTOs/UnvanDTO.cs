﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DernekYonetim.BLL.DTOs
{
    public class UnvanDTO
    {
        public string Tanim { get; set; }

        public override string ToString()
        {
            return Tanim.ToString();
        }
    }
}
