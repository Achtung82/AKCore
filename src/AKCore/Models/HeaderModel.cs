﻿using System.Collections.Generic;
using System.ComponentModel;

namespace AKCore.Models
{
    public class HeaderModel
    {
        public List<ModelMenu> Menus { get; set; }
        public bool LoggedIn { get; set; }
        public string UserName { get; set; }
    }
}