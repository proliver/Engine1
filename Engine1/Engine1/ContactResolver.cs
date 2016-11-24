using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine1
{
    public static class ContactResolver
    {
        /// <summary>
        /// updates objA and objB post collision over some time
        /// </summary>
        /// <param name="collisionA"></param>
        /// <param name="collisionB"></param>
        /// <param name="timeStep"></param>
        public static void ResolveContact(CollisionHandler collisionA, CollisionHandler collisionB, GameTime gameTime)
        {
            bool collisionVector = collisionA.DetectCollsion(collisionB);

            if (collisionVector)
            {
                float seperatingVelocity = collisionA.getSeperatingVelocity(collisionB);
                Vector3 contactNormal = collisionA.getContactNormal(collisionB);
                float penetrationDepth = collisionA.getPenDepth(collisionB);

                collisionA.updatePhysics(gameTime, contactNormal, -seperatingVelocity,
                    penetrationDepth, collisionB.parent.mass);
                collisionB.updatePhysics(gameTime, -contactNormal, -seperatingVelocity,
                    penetrationDepth, collisionA.parent.mass);
            }
        }
        //put these in the collision handler? YES! Probably...
        //float penetrationDepth = getPenDepth();
        //protected abstract Vector3 getSeperatingVelocity();
        //protected abstract Vector3 getContactNormal();
    }
}
