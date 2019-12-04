using Nez;

namespace LTTP.Scenes.SimpleMap.Sword
{
    public class SwordCollider : BoxCollider, ITriggerListener, IUpdatable
    {
        private SwordComponent _swordComponent;

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

        void IUpdatable.Update()
        {
            Entity.SetLocalPosition(_swordComponent.Entity.Position);

            if (_swordComponent.AnimationState == Nez.Sprites.SpriteAnimator.State.Running)
            {
                RotateCollider(_swordComponent.Direction);
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