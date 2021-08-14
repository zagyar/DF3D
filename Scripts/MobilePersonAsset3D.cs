using UnityEngine;
using System;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using static DaggerfallWorkshop.Game.MobilePersonMotor;

namespace Mod3D
{
    public sealed class MobilePersonAsset3D : MobilePersonAsset
    {
        public NonPlayerCharacterController character { get; private set; } // the character we are controlling
        private GameObject go;

        public MobilePersonMotor motor { get; private set; }

        private MobilePersonNPC npc { get; set; }
        public override bool IsIdle { get; set; } = true;
        public override void SetPerson(Races race, Genders gender, int personVariant, bool isGuard)
        {
            // Not sure if we are replacing the person and reusing the motor... so do the cleanup just in case
            if(go!=null)
            {
                character = null;
                Destroy(go);
                go = null;
            }
            switch (gender)
            {
                case Genders.Male:
                    go = ModLoader.GetAsset<GameObject>("DefaultMale.prefab");
                    break;
                case Genders.Female:
                    go = ModLoader.GetAsset<GameObject>("DefaultFemale.prefab");
                    break;
            }

            // for some reason the MobilePersonMotor is in position 0,0,0 while the floor is 0,-1,0
            // For now added 0,-1,0 to the Person model prefab...
            go.transform.SetParent(transform, false);

            if (npc == null)
                npc = GetComponentInParent<MobilePersonNPC>();
            
            if (motor == null)
                motor = GetComponentInParent<MobilePersonMotor>();

            //go.transform.SetParent(npc.transform, false);
        }

        public override Vector3 GetSize()
        {
            return go.GetComponentInChildren<SkinnedMeshRenderer>().bounds.size;
        }

        private void Update()
        {
            if (character == null)
                character = GetComponentInChildren<NonPlayerCharacterController>();

            if (character == null) return;

            if (IsIdle)
            {
                character.Stop();
            }
            else
            {
                character.Move();
            }
        }
    }
}