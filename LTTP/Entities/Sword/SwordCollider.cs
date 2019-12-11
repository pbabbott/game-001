using Nez;
using Nez.Tiled;
using System.Linq;

namespace LTTP.Entities.Sword
{
    public class SwordCollider : BoxCollider, ITriggerListener, IUpdatable
    {
        private SwordComponent _swordComponent;
        private TiledMapRenderer _tiledMapRenderer;
        private SwordCollisionHandler _swordCollisionHandler;

        public SwordCollider(SwordComponent swordComponent)
        {
            _swordComponent = swordComponent;
        }

        private float[] UpDownColliderRotation = new float[]
        {
            90 - 10,
            23,
            12,
            -23-12,
            -45,
            -75,
        };

        private float[] LeftRightColliderRotation = new float[]
        {
            12,
            23 + 12 + 6,
            90,
            90 + 23,
            90 + 45 - 12,
            6,
        };

        public override void OnAddedToEntity()
        {
            _tiledMapRenderer = Entity.Scene.FindEntity("tiled-map").GetComponent<TiledMapRenderer>();
            _swordCollisionHandler = new SwordCollisionHandler(_tiledMapRenderer);
            this.SetHeight(16);
            this.SetWidth(2);
        }

        private void RotateCollider(Direction direction)
        {
            float rotation = 0;

            if (direction == Direction.Down || direction == Direction.Up)
            {
                rotation = UpDownColliderRotation[_swordComponent.CurrentFrame];
            }
            else if (direction == Direction.Left || direction == Direction.Right)
            {
                rotation = LeftRightColliderRotation[_swordComponent.CurrentFrame];

                if (direction == Direction.Left)
                    rotation *= -1;
            }

            this.Transform.SetLocalRotationDegrees(rotation);
        }

        private SwordCollisionResult CheckCollisions()
        {
            // fetch anything that we might overlap with at our position excluding ourself. We don't care about ourself here.
            //var neighborColliders = Physics.BoxcastBroadphaseExcludingSelf(this);
            //var overlappingColliders = neighborColliders.Where(x => x.Overlaps(this));

            var detailLayer = _tiledMapRenderer.TiledMap.Layers["details"] as TmxLayer;

            var overlappingDetailTiles = detailLayer
                .GetTilesIntersectingBounds(this.Bounds)
                .Where(x => x != null)
                .ToList();

            return new SwordCollisionResult()
            {
                DetailTiles = overlappingDetailTiles
            };

           
           
        }

        void IUpdatable.Update()
        {
            Entity.SetLocalPosition(_swordComponent.Entity.Position);

            if (_swordComponent.AnimationState == Nez.Sprites.SpriteAnimator.State.Running)
            {
                RotateCollider(_swordComponent.Direction);
                var collisionResult = CheckCollisions();
                _swordCollisionHandler.HandleCollisionResult(collisionResult);
            }
        }

        void ITriggerListener.OnTriggerEnter(Collider other, Collider self)
        {
            Debug.Log("triggerEnter: {0}", other.Entity.Name);
        }

        void ITriggerListener.OnTriggerExit(Collider other, Collider self)
        {
            Debug.Log("triggerExit: {0}", other.Entity.Name);
        }
    }
}