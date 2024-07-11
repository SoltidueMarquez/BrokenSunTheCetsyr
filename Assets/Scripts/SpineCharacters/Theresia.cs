using UnityEngine;

namespace SpineCharacters
{
    public class Theresia : SpineCharacter
    {
        public override float PlayAnimationAppear()
        {
            return PlayAnimation("Skill", false);
        }
        
        public override float PlayAnimationDisAppear()
        {
            return PlayAnimation("Die", false);
        }

        public override float PlayAnimationIdle() { return 0; }
    
        public override float PlayAnimationClick() { return 0; }
    
        public override float PlayAnimationDrag() { return 0; }

    }
}
