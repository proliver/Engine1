using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine1
{
    public abstract class CollisionHandler
    {
        /*
        //TODO::
        enable height, width, depth, radius to be defined here. 
        This will help stop forcing major design changes and let us learn.
        once the phyics engine works, we can refactor the physics code
        perhaps override and restrict the getters and setter in the child classes
        TODO:: modifyVariables(values[]);
        */
        /*
         * add dynamic/static switch
         * add colliderID
         * add collidertype
         * 
         * Can I cast to Child collider class?
         * move gravity here? probably not
         * */
        protected int ID { get; set; }
        public Vector3 Position { get; set; }
        public GameObject parent;
        //return vector3? or create event?
        static int NextID = 0;

        public CollisionHandler( GameObject Parent )
        {
            ID = GetNextID();
            PhysicsGlobals.collisionObjects.Add(this);
            this.parent = Parent;
        }

        //TODO:: implement this. remove from collision objects
        //public virtual void destroy();

        public abstract bool DetectCollsion(CollisionHandler other);
        public abstract void updatePhysics(GameTime gameTime,
                                           Vector3 contactNormal,
                                           float seperatingVelocity,
                                           float penDepth,
                                           float otherMass);
        public abstract Vector3 getContactNormal(CollisionHandler other);
        public abstract float getPenDepth(CollisionHandler other);
        public abstract float getSeperatingVelocity(CollisionHandler other);

        protected static int GetNextID()
        {
            NextID += 1;
            return NextID;
        }
    }
}
