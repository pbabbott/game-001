using LTTP.Scenes.SimpleMap.Link.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using System;

namespace LTTP.Scenes.SimpleMap.Link
{
    public class LinkComponent : Component, ITriggerListener, IUpdatable
    {
        private SpriteAnimator _bodyAnimator;

        private Mover _mover;
        private float _moveSpeed = 100f;
        private SubpixelVector2 _subpixelV2 = new SubpixelVector2();

        private VirtualIntegerAxis _xAxisInput;
        private VirtualIntegerAxis _yAxisInput;

        private SpriteAtlas _spriteAtlas;

        private Direction _previousWalkDirection = Direction.Down;

        public override void OnAddedToEntity()
        {
            _bodyAnimator = Entity.AddComponent<SpriteAnimator>();
            _mover = Entity.AddComponent(new Mover());

            // add a shadow that will only be rendered when our player is behind the details layer of the tilemap (RenderLayer -1). The shadow
            // must be in a renderLayer ABOVE the details layer to be visible.
            var shadow = Entity.AddComponent(new SpriteMime(Entity.GetComponent<SpriteRenderer>()));
            shadow.Color = new Color(10, 10, 10, 80);
            shadow.Material = Material.StencilRead();

            // ABOVE our tiledmap layer so it is visible
            shadow.RenderLayer = -2;

            SetupAnimations();

            SetupInput();
        }

        private void SetupAnimations()
        {
            // Method 1 for getting link sprites
            //var texture = Entity.Scene.Content.Load<Texture2D>(Content.Characters.Link);
            //ILinkSpriteAtlasFactory linkSprites = new LinkSprites(texture);
            //_spriteAtlas = linkSprites.GetSpriteAtlas();

            // New method for link sprites
            var texture = Entity.Scene.Content.Load<Texture2D>(Content.Characters.Link2);
            ILinkSpriteAtlasFactory linkSprites = new LinkSpriteAtlasFactory(texture);
            _spriteAtlas = linkSprites.GetSpriteAtlas();


            _bodyAnimator.AddAnimationsFromAtlas(_spriteAtlas);
        }

        private void SetupInput()
        {
            // horizontal input from dpad, left stick or keyboard left/right
            _xAxisInput = new VirtualIntegerAxis();
            _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.A, Keys.D));

            // vertical input from dpad, left stick or keyboard up/down
            _yAxisInput = new VirtualIntegerAxis();
            _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.W, Keys.S));
        }

        private Direction GetWalkDirectionFromMovementVector(Vector2 moveDir)
        {
       

            if (moveDir.Y < 0)
            {
                return Direction.Up;
            }

            if (moveDir.X < 0)
            {
                return Direction.Left;
            }

            if (moveDir.X > 0)
            {
                return Direction.Right;
            }
            if (moveDir.Y > 0)
            {
                return Direction.Down;
            }

            throw new ArgumentException("Cannot determine walk direction from zero movement vector");
        }

        private string GetAtlasNameFromDirection(string prefix, Direction d)
        {
            if (d == Direction.Left)
                return prefix + Direction.Right.ToString();

            return prefix + d.ToString();
        }

        private void AnimateWalk(Vector2 moveDir)
        {
            var direction = GetWalkDirectionFromMovementVector(moveDir);
            var bodyAnimation = GetAtlasNameFromDirection("BodyWalk", direction);

            if (_previousWalkDirection != direction)
            {
                _previousWalkDirection = direction;
            }

            var spriteEffects = direction == Direction.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            var walkDirection = GetWalkDirectionFromMovementVector(moveDir);
            _bodyAnimator.SpriteEffects = spriteEffects;

            if (!_bodyAnimator.IsAnimationActive(bodyAnimation))
                _bodyAnimator.Play(bodyAnimation);
            else
                _bodyAnimator.UnPause();
        }

        private void AnimateStand()
        {
            _bodyAnimator.Pause();
            var standSpriteName = GetAtlasNameFromDirection("BodyStand", _previousWalkDirection);
            var standSprite = _spriteAtlas.GetSprite(standSpriteName);
            _bodyAnimator.SetSprite(standSprite);
        }

        void IUpdatable.Update()
        {
            
            var moveDir = new Vector2(_xAxisInput.Value, _yAxisInput.Value);

            // Cut movement speed a little if moving in two directions at once.
            if (moveDir.X != 0 && moveDir.Y != 0)
            {
                moveDir *= 0.75f;
            }

            if (moveDir != Vector2.Zero)
            {
                AnimateWalk(moveDir);

                var movement = moveDir * _moveSpeed * Time.DeltaTime;

                _mover.CalculateMovement(ref movement, out var res);
                _subpixelV2.Update(ref movement);
                _mover.ApplyMovement(movement);
            }
            else
            {
                //_bodyAnimator.Pause();
                AnimateStand();
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