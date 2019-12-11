using LTTP.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace LTTP.Entities.Link
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
        public LinkAction Action { get; private set; } = LinkAction.Standing;

        public Direction Direction { get; private set; } = Direction.Down;

        private SpriteAtlas _spriteAtlas;

        public LinkComponent(SpriteAtlas spriteAtlas)
        {
            _spriteAtlas = spriteAtlas;
        }

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
            shadow.RenderLayer = -11; // ABOVE our tiled map layer so it is visible

            _bodyAnimator.AddAnimationsFromAtlas(_spriteAtlas);

            SetupInput();

            _linkStateFactory = new LinkStateFactory(_primaryAttackInput, _bodyAnimator);
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

       

        private void AnimateWalk()
        {
            var bodyAnimation = LinkSpriteAtlasFactory.GetAtlasNameFromDirection("BodyWalk", Direction);
            var spriteEffects = Direction == Direction.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _bodyAnimator.SpriteEffects = spriteEffects;

            if (_bodyAnimator.IsAnimationActive(bodyAnimation))
                _bodyAnimator.UnPause();
            else
                _bodyAnimator.Play(bodyAnimation);
        }

        private void AnimateAttack()
        {
            var bodyAnimation = LinkSpriteAtlasFactory.GetAtlasNameFromDirection("BodyAttack", Direction);
            var spriteEffects = Direction == Direction.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _bodyAnimator.SpriteEffects = spriteEffects;


            if (_bodyAnimator.IsAnimationActive(bodyAnimation))
                _bodyAnimator.UnPause();
            else
                _bodyAnimator.Play(bodyAnimation, SpriteAnimator.LoopMode.ClampForever);

        }

        private void AnimateStand()
        {
            //_bodyAnimator.Stop();
            var standSpriteName = LinkSpriteAtlasFactory.GetAtlasNameFromDirection("BodyStand", Direction);
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
            Direction = state.Direction;

            if (Action == LinkAction.Standing)
            {
                if (previousAction == LinkAction.Walking)
                    _bodyAnimator.Pause();

                if (previousAction == LinkAction.Attacking)
                    _bodyAnimator.Stop();

                AnimateStand();
            }
          
            if (Action == LinkAction.Attacking)
            {
                AnimateAttack();
            }

            if (Action == LinkAction.Walking)
            {
                AnimateWalk();

                // Cut movement speed a little if moving in two directions at once.
                if (movementVector.X != 0 && movementVector.Y != 0)
                    movementVector *= 0.75f;

                if (movementVector != Vector2.Zero)
                {
                    var movement = movementVector * _moveSpeed * Time.DeltaTime;

                    _mover.CalculateMovement(ref movement, out var res);
                    _subpixelV2.Update(ref movement);
                    _mover.ApplyMovement(movement);
                }
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