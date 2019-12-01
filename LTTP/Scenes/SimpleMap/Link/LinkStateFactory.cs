using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTTP.Scenes.SimpleMap.Link
{
    public enum LinkAction
    {
        Walking,
        Standing,
        Attacking
    }


    public class LinkState
    {
        public LinkState(LinkAction action, Direction direction)
        {
            Action = action;
            Direction = direction;
        }

        public LinkAction Action { get; set; }
        public Direction Direction { get; set; }
    }

    public class LinkStateFactory
    {
        private VirtualButton _primaryAttackInput;
        private SpriteAnimator _bodyAnimator;

        public LinkStateFactory(VirtualButton primaryAttackInput, SpriteAnimator bodyAnimator)
        {
            _primaryAttackInput = primaryAttackInput;
            _bodyAnimator = bodyAnimator;
        }

        public static LinkState DefaultState = new LinkState(LinkAction.Standing, Direction.Down);

        public LinkState PreviousState { get; set; } = DefaultState;

        private bool IsAttackAnimationPlaying =>
            !string.IsNullOrEmpty(_bodyAnimator.CurrentAnimationName)
            && _bodyAnimator.CurrentAnimationName.Contains("Attack")
            && _bodyAnimator.AnimationState == SpriteAnimator.State.Running;


        public LinkState GetNextState(Vector2 movementVector)
        {
            var action = DefaultState.Action;
            var direction = DefaultState.Direction;

            if (IsAttackAnimationPlaying || _primaryAttackInput.IsPressed)
            {
                // continue attacking all the way through the attack animation
                action = LinkAction.Attacking;
                direction = PreviousState.Direction;
            }
            else
            {
                if (movementVector == Vector2.Zero)
                {
                    action = LinkAction.Standing;
                    direction = PreviousState.Direction;
                }
                else
                {
                    action = LinkAction.Walking;
                    direction = GetWalkDirectionFromMovementVector(movementVector);
                }
            }

            // Before returning, set previous state at the end so it will be available next loop
            var result = new LinkState(action, direction);
            PreviousState = result;
            return result;
        }

        private Direction GetWalkDirectionFromMovementVector(Vector2 movementVector)
        {
            if (movementVector.Y < 0)
                return Direction.Up;

            if (movementVector.X < 0)
                return Direction.Left;

            if (movementVector.X > 0)
                return Direction.Right;

            if (movementVector.Y > 0)
                return Direction.Down;

            throw new ArgumentException("Cannot determine walk direction from zero movement vector");
        }
    }
}
