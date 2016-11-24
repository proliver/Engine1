using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Engine1
{
    struct VertexPositionNormal : IVertexType
    {
        Vector3 Position { get; set; }
        Vector3 Normal { get; set; }
        private static int vectorSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3));

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexPositionNormal.VertexDeclaration;
            }
        }

        public VertexPositionNormal(Vector3 pos, Vector3 norm)
        {
            Position = pos;
            Normal = norm;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(vectorSize, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );
    }
}
