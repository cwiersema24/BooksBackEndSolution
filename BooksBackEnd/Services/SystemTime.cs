﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksBackEnd.Services
{
    public class SystemTime
    {
        public DateTime GetCurrent()
        {
            return DateTime.Now;
        }
    }
}
