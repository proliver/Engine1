using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine1
{
    class Camera
    {

        private Vector3 cameraPosition = Vector3.Zero;
        //TODO:: this is for tests
        private Vector3 lookAtPosition;
        //TODO:: create rotation function/find function to bound [0,2pi]
        private float cameraRotationVertical = 0;
        private float cameraRotationHorizontal = 0;
        public Matrix world { private set; get; } = Matrix.Identity;
        private Matrix view;
        public Matrix Projection { private set; get; }

        //CamOrientation is a normalized vector representing the initial rotation of the camera
        public Camera( Vector3 cameraPos, Vector3 CamOrientation, float fov, float aspectRatio, float nearPlane, float farPlane)
        {
            //TODO:: check camOrientation is normalized
            if (CamOrientation == Vector3.Zero)
                throw new ArgumentException("Camera orientation should be a normalized vector");

            Projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farPlane);

            CamOrientation.Normalize();

            cameraRotationHorizontal = (float)(Math.Acos(CamOrientation.X) - (Math.PI/2));
            cameraRotationVertical = (float)Math.Acos(CamOrientation.Z);

            //lookatposition is for testing
            lookAtPosition = CamOrientation;
            cameraPosition = cameraPos;

        }

        public void RotateCameraHorizontal(float rotation)
        {
            cameraRotationHorizontal += rotation;
        }


        public void RotateCameraVertical(float rotation)
        {
            // make sure we cant look straight up
            float nudge = 0.000001f;
            cameraRotationVertical = MathHelper.Clamp(cameraRotationVertical + rotation,
                -MathHelper.PiOver2 + nudge, MathHelper.PiOver2 - nudge);
        }

        public void MovePlayer(Vector3 displacement)
        {
            Matrix movementMatrix = Matrix.CreateRotationY(cameraRotationHorizontal);
            Vector3 rotatedDisplacement = Vector3.Transform(displacement,movementMatrix);
            cameraPosition += rotatedDisplacement ;
        }

        public void SetPosition()
        {
            throw new NotImplementedException();
        }

        public void SetLookAt()
        {

        }

        public Matrix getView()
        {

            Vector3 LookVector = new Vector3(0f, 0f, 1f);
            Matrix rXMatrix = Matrix.CreateRotationX(cameraRotationVertical);
            Matrix rYMatrix = Matrix.CreateRotationY(cameraRotationHorizontal);
            Matrix transMatrix = rXMatrix * rYMatrix * Matrix.CreateTranslation(cameraPosition);

            LookVector = Vector3.Transform(LookVector, transMatrix);
            view = Matrix.CreateLookAt(cameraPosition, LookVector, Vector3.Up);

            return view;
        }
    }
}
