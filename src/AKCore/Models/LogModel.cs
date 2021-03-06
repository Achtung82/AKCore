﻿using System.Collections.Generic;
using AKCore.DataModel;
namespace AKCore.Models
{
    public class LogModel
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public IEnumerable<LogItem> Log { get; set; }
    }
}