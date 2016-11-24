using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Engine1
{
    struct VertexPositionNormalColor : IVertexType
    {
        Vector3 Position { get; set; }
        Vector3 Normal { get; set; }
        Color VectorColor { get; set; }
        private static int vectorSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3));

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexPositionNormalColor.VertexDeclaration;
            }
        }

        public VertexPositionNormalColor(Vector3 pos, Vector3 norm, Color col)
        {
            Position = pos;
            Normal = norm;
            VectorColor = col;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(vectorSize, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(2*vectorSize, VertexElementFormat.Color, VertexElementUsage.Color, 0)
        );
    }
}
