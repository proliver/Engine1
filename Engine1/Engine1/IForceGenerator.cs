using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine1
{
    public interface IForceGenerator
    {
        /// <summary>
        /// Interface for generating forces.
        /// </summary>
        /// <returns>Vector3 of the force.</returns>
        Vector3 generateForce();
    }
}
