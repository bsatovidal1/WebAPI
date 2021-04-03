using System;
using System.Collections.Generic;
using System.Text;

namespace Project1_ClientConsole_BrunoVidal
{
    class ArtType
    {
        public ArtType()
        {
            this.Artworks = new HashSet<Artwork>();
        }
        public int ID { get; set; }

        public string Type { get; set; }

        public ICollection<Artwork> Artworks { get; set; }
    }
}
