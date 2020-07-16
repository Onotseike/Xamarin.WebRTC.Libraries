// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;

namespace WebRTC.Classes
{
    public class MediaConstraints
    {
        public MediaConstraints() : this(null, null)
        {
        }

        public MediaConstraints(IDictionary<string, string> mandatory, IDictionary<string, string> optional = null)
        {
            Mandatory = mandatory ?? new Dictionary<string, string>();
            Optional = optional ?? new Dictionary<string, string>();
        }

        public IDictionary<string, string> Mandatory { get; }
        public IDictionary<string, string> Optional { get; }
    }
}
