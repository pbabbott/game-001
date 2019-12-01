using Microsoft.Xna.Framework;
using Nez;
using Microsoft.Xna.Framework.Graphics;
using Nez.Sprites;
using LTTP.Sprites;

namespace LTTP.Scenes.SimpleMap.Link
{
    public class SwordComponent : Component, ITriggerListener, IUpdatable
    {
        private SpriteAnimator _animator;
        private Entity _player;
        private SpriteAtlas _spriteAtlas;
        [Inspectable]
        public int CurrentFrame { get; private set; }
        public SwordComponent(Entity player, SpriteAtlas spriteAtlas)
        {
            _player = player;
            _spriteAtlas = spriteAtlas;
        }

        public override void OnAddedToEntity()
        {
            _animator = Entity.AddComponent<SpriteAnimator>();
            _animator.AddAnimationsFromAtlas(_spriteAtlas);
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
                layer = -1;
                _animator.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (direction == Direction.Right)
            {
                layer = -1;
                _animator.SpriteEffects = SpriteEffects.None;
            }

            if (!_animator.IsRunning)  
            {
                _animator.SetLayerDepth(layer);
                _animator.Play(animation, SpriteAnimator.LoopMode.ClampForever);
            }
        }

        private void SetRotationAndPosition(Direction direction, Vector2 linkPosition)
        {
            Vector2 offset = Vector2.Zero;
            var rotation = 0;
            CurrentFrame = _animator.CurrentFrame;

            if (direction == Direction.Down)
            {
                if (CurrentFrame == 0)
                {
                    rotation = 90;
                    offset = new Vector2(-11, 2);
                }

                if (CurrentFrame == 1)
                {
                    rotation = 45+23;
                    offset = new Vector2(-5, 9);
                }

                if (CurrentFrame == 2)
                {
                    rotation = 45;
                    offset = new Vector2(3, 12);
                }

                if (CurrentFrame == 3)
                {
                    rotation = 45;
                    offset = new Vector2(5, 14);
                }

                if (CurrentFrame == 4)
                {
                    rotation = 11;
                    offset = new Vector2(8, 7);
                }

                if (CurrentFrame == 5)
                {
                    rotation = 0;
                    offset = new Vector2(12, 2);
                }
            }


            if (direction == Direction.Up)
            {
                offset = new Vector2(-4, -12);
            }


            if (direction == Direction.Left)
            {
                offset = new Vector2(-12, -2);
            }


            if (direction == Direction.Right)
            {
                offset = new Vector2(12, -2);
            }


            Entity.SetRotationDegrees(rotation);

            Entity.SetPosition(linkPosition + offset);

        }

        void IUpdatable.Update()
        {
            var linkComponent = _player.GetComponent<LinkComponent>();
            var linkDirection = linkComponent.Direction;

            if (_animator.AnimationState == SpriteAnimator.State.Completed)
            {
                Entity.SetPosition(-100, -100);
            }


            if (linkComponent.Action == LinkAction.Attacking)
            {
                AnimateAttack(linkDirection);
                SetRotationAndPosition(linkDirection, _player.Position);
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