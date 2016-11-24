using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine1
{
    public abstract class GameObject
    {
        public bool physicsEnabled { get; set; } = false;
        public bool dynamicColisions { get; set; } = false;

        public Vector3 Velocity { get; set; } = Vector3.Zero;
        public virtual Vector3 Position { get; set; }

        protected Matrix Rotation = Matrix.Identity;
        protected Matrix Translation = Matrix.Identity;
        //merge anchorPosition and Position? No, we want anchor position to be arbitrary
        protected Vector3 anchorPosition = Vector3.Zero;

        public CollisionHandler collisionHandler;
        public PhysicsHandler physicsHandler;
        
        public float restitution = 1.0f;
        private bool gravBool = false;
        public bool gravityEnabled
        {
            get { return gravBool; }
            set
            {
                gravBool = value;
                if (value) physicsHandler.addGravitationalForce();
                else if (physicsHandler != null)
                    physicsHandler.removeGravity();
            }
        }

        private float massVal = 1.0f;
        public float mass
        {
            get { return massVal; }
            set
            {
                massVal = value;
                if (physicsHandler != null)
                {
                    physicsHandler.mass = value;
                }
            }
        }

        public GameObject()
        {
            physicsHandler = new PhysicsHandler(this);
        }
        public abstract void Draw(BasicEffect basicEffect);
        public abstract void Dispose();

        public virtual void UpdatePhysics(GameTime gameTime)
        {
            if (physicsHandler != null)
                physicsHandler.ApplyForces(gameTime);
        }
    }
}
