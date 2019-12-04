using System.Linq;
using Nez;
using Nez.Tiled;

namespace LTTP.Scenes.SimpleMap.Sword
{
    public class SwordCollider : BoxCollider, ITriggerListener, IUpdatable
    {
        private SwordComponent _swordComponent;
        private TiledMapRenderer _tiledMapRenderer;

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

        private void CheckCollisions()
        {
            var entityTile = _tiledMapRenderer.GetTileAtWorldPosition(Entity.Position);

            // fetch anything that we might overlap with at our position excluding ourself. We don't care about ourself here.
            var neighborColliders = Physics.BoxcastBroadphaseExcludingSelf(this);

            var overlappingColliders = neighborColliders
                .Where(x => x.Overlaps(this));

            if (overlappingColliders.Any())
            {
                var detailLayer = _tiledMapRenderer.TiledMap.Layers["details"] as TmxLayer;

                var overlappingDetailTiles = overlappingColliders
                    .SelectMany(x => detailLayer.GetTilesIntersectingBounds(this.Bounds));

                var removedCount = 0;

                foreach (var tile in overlappingDetailTiles)
                {
                    if (tile != null && (tile.Gid == 680 || tile.Gid == 1381))
                    {
                        removedCount++;
                        detailLayer.RemoveTile(tile.X, tile.Y);
                        _tiledMapRenderer.CollisionLayer.RemoveTile(tile.X, tile.Y);
                    }
                }

                if (removedCount > 0)
                {
                    _tiledMapRenderer.RemoveColliders();
                    _tiledMapRenderer.AddColliders();
                }

            }

         

        }

        void IUpdatable.Update()
        {
            Entity.SetLocalPosition(_swordComponent.Entity.Position);

            if (_swordComponent.AnimationState == Nez.Sprites.SpriteAnimator.State.Running)
            {
                RotateCollider(_swordComponent.Direction);
                CheckCollisions();
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