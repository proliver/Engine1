using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine1
{
    public class PhysicsHandler
    {
        GameObject parent;
        
        //I Don't like this public
        public Vector3 forceAcc = Vector3.Zero;

        protected List<IForceGenerator> forceGens;
        public GravityForceGenerator gravityForce { get; protected set; }

        private float objMass;
        public float mass
        {
            get { return objMass; }
            set
            {
                objMass = value;
                invMass = 1 / value;
                if(gravityForce != null)
                    gravityForce.updateMass(value);
            }
        }
        protected float invMass;

        public PhysicsHandler(GameObject Caller)
        {
            parent = Caller;
            forceGens = new List<IForceGenerator>();
        }

        protected void resetPhysics()
        {
            forceAcc = Vector3.Zero;
        }

        //TODO::perhaps make the gravityEnabled bool part of the object, and check here
        public void addGravitationalForce()
        {
            gravityForce = new GravityForceGenerator(mass);
            forceGens.Add( gravityForce);
        }

        public void removeGravity()
        {
            if (gravityForce == null)
                return;
            else
                forceGens.Remove(gravityForce);
        }

        public void addForce(IForceGenerator force)
        {
            forceGens.Add(force);
        }

        /// <summary>
        /// removes the force. Requrires pointer to force. This function is temporary.
        /// </summary>
        /// <param name="force"></param>
        public void deleteForce(IForceGenerator force)
        {
            forceGens.Remove(force);
        }

        //enable this to only be called once per frame?
        private void sumForces()
        {
            if (forceGens != null)
            {
                foreach (IForceGenerator force in forceGens)
                {
                    forceAcc += force.generateForce();
                }
            }
        }

        /// <summary>
        /// Applies forces to parent GameObject
        /// this will reset the force accumulator
        /// </summary>
        public void ApplyForces(GameTime gameTime)
        {
            resetPhysics();
            sumForces();

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 acceleration = (forceAcc * invMass) * elapsedTime;
            parent.Position += elapsedTime * (parent.Velocity + elapsedTime * acceleration * 0.5f);
            parent.Velocity += acceleration;
        }
    }
}
