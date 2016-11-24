using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine1
{
    static public class PhysicsGlobals
    {
        public const float GravityAcceleration = -9.81f;
        public const float MinimumBounceTollerance = -0.3f;

        public static List<CollisionHandler> collisionObjects = new List<CollisionHandler>();
    }
}
