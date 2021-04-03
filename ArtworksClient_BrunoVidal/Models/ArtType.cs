﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworksClient_BrunoVidal.Models
{
    public class ArtType
    {
        public ArtType()
        {
            this.Artworks = new HashSet<Artwork>();
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the art type name black.")]
        [StringLength(25, ErrorMessage = "Art type cannot be more than 25 characters long.")]
        public string Type { get; set; }

        [Timestamp]
        public Byte[] RowVersion { get; set; }

        public ICollection<Artwork> Artworks { get; set; }
    }
}
