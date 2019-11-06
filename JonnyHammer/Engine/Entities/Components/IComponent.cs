using JonnyHamer.Engine.Entities;
using JonnyHammer.Engine.Interfaces;

namespace JonnyHammer.Engine
{
    public interface IComponent : IDraw, IUpdate
    {
        Entity Entity { get; }
        void SetEntity(Entity entity);

        void Start();
    }
}
