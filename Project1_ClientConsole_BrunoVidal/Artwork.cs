using System;
using System.Collections.Generic;
using System.Text;

namespace Project1_ClientConsole_BrunoVidal
{
    class Artwork
    {
        public int ID { get; set; }

        public string Summary
        {
            get
            {
                return Name + " - " + Completed.ToShortDateString();
            }
        }

        public string Name { get; set; }

        public DateTime Completed { get; set; }

        public string Description { get; set; }

        public double Value { get; set; }

        public int ArtTypeID { get; set; }
        public ArtType ArtType { get; set; }
    }
}
