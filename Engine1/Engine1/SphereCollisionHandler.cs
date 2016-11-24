using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine1
{
    class SphereCollisionHandler : CollisionHandler
    {
        public float Radius { get; set; }
        private float lastDistanceCheck = 0f;

        public SphereCollisionHandler(GameObject Parent) : base(Parent)
        {
        }

        public override bool DetectCollsion(CollisionHandler that)
        {
            if (that is SphereCollisionHandler)
            {
                if (checkSphereCollision((SphereCollisionHandler)that))
                    return true;
                else
                    return false;
            }
            if (that is HalfSpaceCollisionHandler)
            {
                return that.DetectCollsion(this);
            }
            throw new ArgumentException("Spheres are not able to detect collision with this handler");
        }


        /// <summary>
        /// moves object after collision detected.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="contactNormal"></param>
        /// <param name="seperatingVelocity"></param>
        /// <param name="penDepth"></param>
        /// <param name="otherMass"></param>
        public override void updatePhysics(GameTime gameTime,
                                   Vector3 contactNormal,
                                   float seperatingVelocity,
                                   float penDepth,
                                   float otherMass)
        {
            //TODO:: Impulse handler
            //TODO:: Fix this mess
            //TODO:: Refactor so this only gets called once
            //TODO:: pass in a restitution factor, don't grab from parents
            Vector3 deltaPosition;

            //the bounce increases due to pushing the object out and then applying velocity...
            //how to fix?

            float newSeperatingVelocity = -seperatingVelocity * parent.restitution;

            //Handle resting contact / added velocity on collision
            Vector3 accelationThisFrame = parent.physicsHandler.forceAcc;
            float frameSeperatingVelocity = Vector3.Dot(accelationThisFrame, contactNormal)
                                            * (float)gameTime.ElapsedGameTime.TotalSeconds;
            newSeperatingVelocity -= frameSeperatingVelocity * parent.restitution;

            if (newSeperatingVelocity > PhysicsGlobals.MinimumBounceTollerance)
            {
                newSeperatingVelocity = 0f;
            }

            float deltaV = newSeperatingVelocity - seperatingVelocity;
            float systemInvMass;
            
            //TODO:: pre compute the inverse masses
            if (float.IsInfinity(parent.mass))
            {
                return;
            }
            if (float.IsInfinity(otherMass))
            {
                //not sure what to do here... probably just invert this mass?
                //1/INF = 0 in float
                systemInvMass = (1.0f / parent.mass);
            }
            else
            {
                systemInvMass = (1.0f / otherMass) + (1.0f / parent.mass);
            }
            //TODO:: Probably just multiply by mass here, but check for INF mass.
            //REMEMBER:: sum of inverse != inverse of sum
            float impulse = deltaV / systemInvMass;

            Vector3 impulsePerInvMass = contactNormal * impulse;

            
            //update the velocity
            parent.Velocity -= impulsePerInvMass * (1.0f / parent.mass);


            if (float.IsPositiveInfinity(otherMass))
            {
                deltaPosition = penDepth * contactNormal;
            }
            else
            {
                deltaPosition = (penDepth * otherMass / (parent.mass + otherMass)) * contactNormal;
            }

            parent.Position += deltaPosition;
        }


        //TODO:: make this fire an event for sound ect.
        private bool checkSphereCollision(SphereCollisionHandler other)
        {
            bool retVal = false;
            float xDiff = Math.Abs(this.Position.X - other.Position.X);
            float yDiff = Math.Abs(this.Position.Y - other.Position.Y);
            float zDiff = Math.Abs(this.Position.Z - other.Position.Z);

            float sumRadius = this.Radius + other.Radius;

            float distance = (xDiff * xDiff) + (yDiff * yDiff) + (zDiff * zDiff);
            distance = (float)Math.Sqrt(distance);

            lastDistanceCheck = distance;

            if (distance < sumRadius) retVal = true;

            return retVal;
        }

        public override Vector3 getContactNormal(CollisionHandler other)
        {
            if (other is SphereCollisionHandler)
            {
                Vector3 cNorm;
                cNorm = parent.Position - other.parent.Position;
                cNorm.Normalize();
                return cNorm;
            }
            if (other is HalfSpaceCollisionHandler)
            {
                return other.getContactNormal(this);
            }
            else throw new Exception("No collision handler for this type exists");
        }

        public override float getPenDepth(CollisionHandler other)
        {
            if (other is SphereCollisionHandler)
            {
                return spherePenDepth((SphereCollisionHandler)other);
            }
            if (other is HalfSpaceCollisionHandler)
            {
                return - other.getPenDepth(this);
            }
            else throw new Exception("No collision pen depth calculation for this type exists");
        }

        private float spherePenDepth(SphereCollisionHandler other)
        {
            return Radius + other.Radius - lastDistanceCheck;
        }

        public override float getSeperatingVelocity(CollisionHandler other)
        {
            float retVal;
            //TODO:: don't recalculate
            Vector3 distanceNormal = getContactNormal(other);
            Vector3 velocity = parent.Velocity - other.parent.Velocity;
            retVal = Vector3.Dot(velocity, distanceNormal);
            return retVal;
        }
    }
}
