using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTTP.Entities.Core
{
    public interface IEntityFactory
    {
        string EntityName { get; }
        Entity CreateEntity(Vector2 startingPosition);
    }
}
