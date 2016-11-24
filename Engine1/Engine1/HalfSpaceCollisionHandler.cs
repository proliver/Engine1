using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine1
{
    class HalfSpaceCollisionHandler : CollisionHandler
    {
        public Vector3 normal = Vector3.Up;
        public HalfSpaceCollisionHandler(GameObject Parent) : base(Parent)
        {
        }

        public override bool DetectCollsion(CollisionHandler other)
        {
            if (other is SphereCollisionHandler)
            {
                return detectSphereCollision((SphereCollisionHandler) other);
            }
            else return false;
        }

        private bool detectSphereCollision(SphereCollisionHandler other)
        {
            float distance = Vector3.Dot( other.Position, normal) - Vector3.Dot( Position, normal);


            if (distance < other.Radius) 
                return true;

            else return false;

        }

        public override Vector3 getContactNormal(CollisionHandler other)
        {
            return normal;
        }

        public override float getPenDepth(CollisionHandler other)
        {
            if (other is SphereCollisionHandler)
            {
                float distance = Vector3.Dot(other.Position, normal) - Vector3.Dot(Position, normal);
                SphereCollisionHandler pointerObject = (SphereCollisionHandler) other;
                return distance - pointerObject.Radius;
            }
            if (other is HalfSpaceCollisionHandler)
            {
                throw new NotImplementedException();
            }
            else throw new Exception("no pen depth test of this type");
        }

        public override float getSeperatingVelocity(CollisionHandler other)
        {
            float retVal;
            Vector3 distanceNormal = normal;
            Vector3 velocity = parent.Velocity - other.parent.Velocity;
            retVal = Vector3.Dot(velocity, distanceNormal);
            return retVal;
        }

        //TODO:: Add this function, when we assume this object is not ground
        public override void updatePhysics(GameTime gameTime, Vector3 contactNormal, float seperatingVelocity, float penDepth, float otherMass)
        {
            return;
            //throw new NotImplementedException();
        }
    }
}
