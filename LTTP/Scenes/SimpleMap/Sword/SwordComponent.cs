using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

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

        private void SetRotationAndPosition(Direction direction, Vector2 linkPosition)
        {
            Vector2 offset = Vector2.Zero;
            var rotation = 0;
            CurrentFrame = _animator.CurrentFrame;

            if (direction == Direction.Down || direction == Direction.Up)
            {
                if (CurrentFrame == 0)
                {
                    rotation = 90;
                    offset = new Vector2(-10, 2);
                }
                if (CurrentFrame == 1)
                {
                    rotation = 45 + 23;
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
                    offset = new Vector2(5, 15);
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
                // Down is just the opposite of up!
                if (direction == Direction.Up)
                {
                    offset *= new Vector2(-1, -1);
                }
            }

            if (direction == Direction.Left || direction == Direction.Right)
            {
                if (CurrentFrame == 0)
                {
                    rotation = 0;
                    offset = new Vector2(4, -4);
                }
                else if (CurrentFrame == 1)
                {
                    rotation = 23;
                    offset = new Vector2(9, -4);
                }
                else if (CurrentFrame == 2)
                {
                    rotation = 23;
                    offset = new Vector2(13, 0);
                }
                else if (CurrentFrame == 3)
                {
                    rotation = 23;
                    offset = new Vector2(16, 2);
                }
                else if (CurrentFrame == 4)
                {
                    rotation = 45+23;
                    offset = new Vector2(10, 6);
                }
                else if (CurrentFrame == 5)
                {
                    rotation = 90;
                    offset = new Vector2(2, 8);
                    //offset = new Vector2(9, -4);
                }
             



                if (direction == Direction.Left)
                {
                    rotation *= -1;
                    offset *= new Vector2(-1, 1);
                }
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