using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine1
{
    public class GravityForceGenerator : IForceGenerator
    {
        protected Vector3 forceGravity = Vector3.Zero;
        protected float mass;

        public GravityForceGenerator(float gameObjectMass)
        {
            updateMass(gameObjectMass);
        }

        /// <summary>
        /// updates the force of gravity for a specified mass. If mass is infinite, force is zero.
        /// </summary>
        /// <param name="gameObjectMass"></param>
        public void updateMass(float gameObjectMass)
        {
            mass = gameObjectMass;
            if (float.IsInfinity(gameObjectMass))
                forceGravity = Vector3.Zero;
            else
                forceGravity = Vector3.Up * mass * PhysicsGlobals.GravityAcceleration;
        }

        public Vector3 generateForce()
        {
            return forceGravity;
        }
    }
}
