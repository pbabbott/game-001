using LTTP.Entities.Link;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace LTTP.Entities.Sword
{
    public class SwordComponent : Component, ITriggerListener, IUpdatable
    {
        private SpriteAnimator _animator;
        private LinkComponent _linkComponent;
        private Entity _player;
        private SpriteAtlas _spriteAtlas;

        public int CurrentFrame => _animator.CurrentFrame;
        public SpriteAnimator.State AnimationState => _animator.AnimationState;
        public Direction Direction => _linkComponent.Direction;

        public SwordComponent(SpriteAtlas spriteAtlas)
        {
            _spriteAtlas = spriteAtlas;
        }

        public override void OnAddedToEntity()
        {
            _animator = Entity.AddComponent<SpriteAnimator>();
            _animator.AddAnimationsFromAtlas(_spriteAtlas);


            _player = Entity.Scene.FindEntity("player");
            _linkComponent = _player.GetComponent<LinkComponent>();

        }

        private void AnimateAttack(Direction direction)
        {
            var animation = "SwordAttack";
            var layer = -1;

            if (direction == Direction.Down)
            {
                layer = -1;
                _animator.SpriteEffects = SpriteEffects.FlipVertically;
            }
            else if (direction == Direction.Up)
            {
                layer = 1;
                _animator.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (direction == Direction.Left)
            {
                layer = _animator.CurrentFrame < 3 ? 1 : -1;
                _animator.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (direction == Direction.Right)
            {
                layer = _animator.CurrentFrame < 3 ? 1 : -1;
                _animator.SpriteEffects = SpriteEffects.None;
            }

            //_animator.SetLayerDepth(layer);
            _animator.SetRenderLayer(layer);

            if (!_animator.IsRunning)
            {
                _animator.Play(animation, SpriteAnimator.LoopMode.ClampForever);
            }

        }

        private Vector2[] UpEntityOffset = new Vector2[]
        {
            new Vector2(-11, 3),
            new Vector2(-5, 9),
            new Vector2(3, 12),
            new Vector2(5, 15),
            new Vector2(8, 7),
            new Vector2(12, 2)
        };

        private Vector2[] RightEntityOffset = new Vector2[]
        {
            new Vector2(4, -4),
            new Vector2(9, -4),
            new Vector2(13, 0),
            new Vector2(16, 2),
            new Vector2(10, 6),
            new Vector2(2, 8)
        };

        private Vector2 GetEntityPositionOffset(Direction direction)
        {
            Vector2 offset = Vector2.Zero;

            if (direction == Direction.Down || direction == Direction.Up)
            {
                offset = UpEntityOffset[CurrentFrame];

                if (direction == Direction.Up)
                    offset *= new Vector2(-1, -1);
            }
            else if (direction == Direction.Left || direction == Direction.Right)
            {
                offset = RightEntityOffset[CurrentFrame];

                if (direction == Direction.Left)
                    offset *= new Vector2(-1, 1);
            }

            return offset;
        }

        private float[] UpDownAnimationRotation = new float[]
        {
            45 + 23 + 12,
            45 + 12,
            45 + 12,
            45,
            23,
            12,
        };

        private float[] LeftRightAnimationRotation = new float[]
        {
            12,
            23,
            23,
            23,
            45,
            45 + 23
        };

        private void RotateAnimation(Direction direction)
        {
            float rotation = 0;

            if (direction == Direction.Down || direction == Direction.Up)
            {
                rotation = UpDownAnimationRotation[CurrentFrame];
            }
            else if (direction == Direction.Left || direction == Direction.Right)
            {
                rotation = LeftRightAnimationRotation[CurrentFrame];

                if (direction == Direction.Left)
                    rotation *= -1;
            }

            this.Transform.SetLocalRotationDegrees(rotation);
        }

      
        void IUpdatable.Update()
        {
            
            var linkDirection = _linkComponent.Direction;
            var linkPosition = _player.Position;

            if (_animator.AnimationState == SpriteAnimator.State.Completed)
            {
                Entity.SetPosition(-100, -100);
            }

            if (_linkComponent.Action == LinkAction.Attacking)
            {
                AnimateAttack(linkDirection);

                var offset = GetEntityPositionOffset(linkDirection);
                Entity.SetPosition(linkPosition + offset);

                RotateAnimation(linkDirection);
            }
            else
            {
                Entity.SetPosition(-100, -100);

                var spriteName = "SwordHeld";
                var swordSprite = _spriteAtlas.GetSprite(spriteName);
                _animator.SetSprite(swordSprite);
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