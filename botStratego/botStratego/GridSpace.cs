using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratego
{
    [Serializable]
    public struct GridSpace
    {
        public SpaceType _type { get; set; }
        public bool _isPlayable { get; set; }
        public Piece _piece { get; set; }
    }
    public enum SpaceType { Player1=1,Player2=2,Empty=3,All=4}
}
