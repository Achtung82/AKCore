﻿using AKCore.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AKCore.Models
{
    public class PageEditModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(450)]
        public string Slug { get; set; }
        [DisplayName("Kräver inloggning")]
        public bool LoggedIn { get; set; }
        [DisplayName("Enbart utloggad")]
        public bool LoggedOut { get; set; }
        [DisplayName("Enbart balett")]
        public bool BalettOnly { get; set; }
        public string Template { get; set; }
        public int PageId { get; set; }
        public Revision SelectedRevision { get; set; }
        public DateTime LastModified { get; set; }
        public IList<Widget> Widgets { get; set; }
        public IEnumerable<Revision> Revisions { get; set; }
    }
}