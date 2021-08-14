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
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        private GameObject go;
        private MobileDirection LastDirection;

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
                character = GetComponentInChildren<ThirdPersonCharacter>();

            if (character == null) return;

            if (IsIdle)
                character.Move(Vector3.zero, 0f, 0f, false, false);
            else
            {
                var direction = Vector3.zero;
                var turn = 0f;
                switch (motor.CurrentDirection)
                {
                    //Vector3(float x, float y, float z)
                    case MobileDirection.North:
                        direction = Vector3.left;
                        break;
                    case MobileDirection.South:
                        direction = Vector3.right;
                        break;
                    case MobileDirection.East:
                        direction = Vector3.forward;
                        break;
                    case MobileDirection.West:
                        direction = Vector3.back;
                        break;
                }

                switch (LastDirection)
                {
                    //180 turns
                    case MobileDirection.South:
                        switch (motor.CurrentDirection)
                        {
                            case MobileDirection.North:
                                turn = 1f;
                                break;
                            case MobileDirection.West:
                                turn = 0.5f;
                                break;
                            case MobileDirection.East:
                                turn = -0.5f;
                                break;
                        }
                        break;
                    case MobileDirection.North:
                        switch (motor.CurrentDirection)
                        {
                            case MobileDirection.South:
                                turn = 1f;
                                break;
                            case MobileDirection.West:
                                turn = -0.5f;
                                break;
                            case MobileDirection.East:
                                turn = 0.5f;
                                break;
                        }
                        break;
                    case MobileDirection.East:
                        switch (motor.CurrentDirection)
                        {
                            case MobileDirection.West:
                                turn = 1f;
                                break;
                            case MobileDirection.North:
                                turn = -0.5f;
                                break;
                            case MobileDirection.South:
                                turn = 0.5f;
                                break;
                        }
                        break;
                    case MobileDirection.West:
                        switch (motor.CurrentDirection)
                        {
                            case MobileDirection.East:
                                turn = 1f;
                                break;
                            case MobileDirection.South:
                                turn = -0.5f;
                                break;
                            case MobileDirection.North:
                                turn = 0.5f;
                                break;
                        }
                        break;
                }
                
                var target = direction;// * motor.DistanceToTarget;
                //turn
                character.Move(target, 0f, 0.5f, false, false);
            }

            LastDirection = motor.CurrentDirection;
        }
    }
}