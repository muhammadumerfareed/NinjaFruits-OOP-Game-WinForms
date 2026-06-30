
using System.Drawing;

namespace NingaFruits
{
    // Interface that defines collision behavior
    public interface ICollidable
    {
        void OnCollision(GameObject other);
    }
}
