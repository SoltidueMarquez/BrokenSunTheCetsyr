using UnityEngine;

namespace SpineCharacters
{
    public class Theresia : SpineCharacter
    {
        public override float PlayAnimationAppear()
        {
            character.AnimationState.ClearTracks();
            character.AnimationState.SetAnimation(0, "Skill", false);
            return character.skeleton.Data.FindAnimation("Skill").Duration;
        }

        public override float PlayAnimationIdle() { return 0; }
    
        public override float PlayAnimationClick() { return 0; }
    
        public override float PlayAnimationDrag() { return 0; }
    
        public override float PlayAnimationDisAppear() { return 0; }
    }
}
