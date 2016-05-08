using System.Collections.Generic;
using FieldObject;

namespace Shooter
{
    interface IShooter
    {
        List<Bullet> Bullets { get; }
    }
}
