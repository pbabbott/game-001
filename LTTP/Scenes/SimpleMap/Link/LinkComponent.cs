using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace LTTP.Scenes.SimpleMap.Link
{
    public class LinkComponent : Component, ITriggerListener, IUpdatable
    {
        private SpriteAnimator _bodyAnimator;

        private Mover _mover;
        private float _moveSpeed = 100f;
        private SubpixelVector2 _subpixelV2 = new SubpixelVector2();

        private VirtualButton _primaryAttackInput;
        private VirtualIntegerAxis _xAxisInput;
        private VirtualIntegerAxis _yAxisInput;
        private LinkStateFactory _linkStateFactory;

        [Inspectable]
        private LinkAction Action = LinkAction.Standing;

        private SpriteAtlas _spriteAtlas;

        public override void OnRemovedFromEntity()
        {
            // deregister virtual input
            _primaryAttackInput.Deregister();
        }

        public override void OnAddedToEntity()
        {
            _bodyAnimator = Entity.AddComponent<SpriteAnimator>();
            //_bodyAnimator.Speed = 0.2f;
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

            _linkStateFactory = new LinkStateFactory(_primaryAttackInput, _bodyAnimator);
        }

        private void SetupAnimations()
        {
            // Method 1 for getting link sprites
            //var texture = Entity.Scene.Content.Load<Texture2D>(Content.Characters.Link);
            //ILinkSpriteAtlasFactory linkSprites = new LinkSprites(texture);
            //_spriteAtlas = linkSprites.GetSpriteAtlas();

            // New method for link sprites
            var texture = Entity.Scene.Content.Load<Texture2D>(Content.Characters.Link2);
            var linkSprites = new LinkSpriteAtlasFactory(texture);
            _spriteAtlas = linkSprites.GetSpriteAtlas();

            _bodyAnimator.AddAnimationsFromAtlas(_spriteAtlas);
        }

        private void SetupInput()
        {
            _primaryAttackInput = new VirtualButton();
            _primaryAttackInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.Space));

            // horizontal input from dpad, left stick or keyboard left/right
            _xAxisInput = new VirtualIntegerAxis();
            _xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.A, Keys.D));

            // vertical input from dpad, left stick or keyboard up/down
            _yAxisInput = new VirtualIntegerAxis();
            _yAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.W, Keys.S));
        }

        private string GetAtlasNameFromDirection(string prefix, Direction d)
        {
            if (d == Direction.Left)
                return prefix + Direction.Right.ToString();

            return prefix + d.ToString();
        }

        private void AnimateWalk(Direction direction)
        {
            var bodyAnimation = GetAtlasNameFromDirection("BodyWalk", direction);
            var spriteEffects = direction == Direction.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _bodyAnimator.SpriteEffects = spriteEffects;

            if (_bodyAnimator.IsAnimationActive(bodyAnimation))
                _bodyAnimator.UnPause();
            else
                _bodyAnimator.Play(bodyAnimation);
        }

        private void AnimateAttack(Direction direction)
        {
            var bodyAnimation = GetAtlasNameFromDirection("BodyAttack", direction);
            var spriteEffects = direction == Direction.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _bodyAnimator.SpriteEffects = spriteEffects;


            if (_bodyAnimator.IsAnimationActive(bodyAnimation))
                _bodyAnimator.UnPause();
            else
                _bodyAnimator.Play(bodyAnimation, SpriteAnimator.LoopMode.Once);

        }

        private void AnimateStand(Direction direction)
        {
            //_bodyAnimator.Stop();
            var standSpriteName = GetAtlasNameFromDirection("BodyStand", direction);
            var standSprite = _spriteAtlas.GetSprite(standSpriteName);
            _bodyAnimator.SetSprite(standSprite);
        }

        void IUpdatable.Update()
        {
            var movementVector = new Vector2(_xAxisInput.Value, _yAxisInput.Value);

            var previousState = _linkStateFactory.PreviousState;
            var previousAction = previousState.Action;
            var state = _linkStateFactory.GetNextState(movementVector);
            Action = state.Action;

            if (state.Action == LinkAction.Standing)
            {
                if (previousAction == LinkAction.Walking)
                    _bodyAnimator.Pause();

                if (previousAction == LinkAction.Attacking)
                    _bodyAnimator.Stop();

                AnimateStand(state.Direction);
            }
            if (state.Action == LinkAction.Walking)
            {
                AnimateWalk(state.Direction);
            }
            if (state.Action == LinkAction.Attacking)
            {
                AnimateAttack(state.Direction);
            }

                

            // Set "previous" state at the end so it will be available next time

            //if (_primaryAttackInput.IsPressed)
            //{
            //    _bodyAnimator.Play("BodyAttackRight", SpriteAnimator.LoopMode.ClampForever);
            //}

            //// Cut movement speed a little if moving in two directions at once.
            //if (moveDir.X != 0 && moveDir.Y != 0)
            //    moveDir *= 0.75f;

            //if (moveDir != Vector2.Zero)
            //{
            //    AnimateWalk(moveDir);

            //    var movement = moveDir * _moveSpeed * Time.DeltaTime;

            //    _mover.CalculateMovement(ref movement, out var res);
            //    _subpixelV2.Update(ref movement);
            //    _mover.ApplyMovement(movement);
            //}
            //else
            //{
            //    //_bodyAnimator.Pause();
            //    //AnimateStand();
            //}
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