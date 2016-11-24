using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine1
{
    class PosNormal
    {
        public Vector3 Normal { get; set; } = Vector3.Zero;
        public Vector3 Position { get; set; } = Vector3.Zero;

        public PosNormal(Vector3 position, Vector3 normal)
        {
            Normal = normal;
            Position = position;
        }

    }
}
