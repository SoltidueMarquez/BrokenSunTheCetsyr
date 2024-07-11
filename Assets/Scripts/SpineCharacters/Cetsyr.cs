using UnityEngine;

namespace SpineCharacters
{
    public class Cetsyr : SpineCharacter
    {
        public override float PlayAnimationAppear()
        {
            return PlayAnimation("Start", false);
        }
        
        public override float PlayAnimationDisAppear()
        {
            return PlayAnimation("Die", false);
        }
        
        public override float PlayAnimationIdle()
        {
            return PlayAnimation("Idle", false);
        }
        

        public override float PlayAnimationClick() { return 0; }
    
        public override float PlayAnimationDrag() { return 0; }
    
    }
}
