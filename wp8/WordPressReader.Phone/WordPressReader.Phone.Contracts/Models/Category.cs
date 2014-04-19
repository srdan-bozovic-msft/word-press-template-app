﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPressReader.Phone.Contracts.Models
{
    public class Category
    {
        public string Title { get; set; }
        public string ForeColor { get; set; }
        public string BackColor { get; set; }
        public string Tag { get; set; }
        public bool Wide { get; set; }
    }
}