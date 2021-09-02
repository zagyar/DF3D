using UnityEngine;
using System;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Entity;

namespace Mod3D
{
    public sealed class MobilePersonAsset3D : MobilePersonAsset
    {
        public NonPlayerCharacterController character { get; private set; } // the character we are controlling
        private GameObject go;

        public MobilePersonMotor motor { get; private set; }

        private MobilePersonNPC npc { get; set; }
        public override bool IsIdle { get; set; } = true;

        [SerializeField]
        private Races race;
        public Races Race { get => race; set { race = value; } }

        [SerializeField]
        private Genders gender;
        public Genders Gender { get => gender; set { gender = value; } }

        [SerializeField]
        private int personVariant;
        public int PersonVariant { get => personVariant; set { personVariant = value; } }

        [SerializeField]
        private bool isGuard;
        public bool IsGuard { get => isGuard; set { isGuard = value; } }

        [SerializeField]
        private string expectedAssetName;
        public string ExpectedAssetName { get => expectedAssetName; set { expectedAssetName = value; } }

        [SerializeField]
        private string assetName;
        public string AssetName { get => assetName; set { assetName = value; } }

        public override void SetPerson(Races race, Genders gender, int personVariant, bool isGuard)
        {
            Race = race;
            Gender = gender;
            PersonVariant = personVariant;
            IsGuard = isGuard;
            //breton_male_0_n = no guard
            //breton_male_0_g = guard
            var isGuardFormat = isGuard ? "g" : "n";
            ExpectedAssetName = $"{race.ToString().ToLowerInvariant()}_{gender.ToString().ToLowerInvariant()}_{personVariant}_{isGuardFormat}.prefab";
            //Debug.Log($"Race: {race} Gender:{gender} PersonVariant: {personVariant} Guard: {isGuard}");
            // Not sure if we are replacing the person and reusing the motor... so do the cleanup just in case
            if (go!=null)
            {
                character = null;
                Destroy(go);
                go = null;
            }

            go = ModLoader.GetAsset<GameObject>(AssetName) ?? ModLoader.GetAsset<GameObject>("DefaultMale.prefab");
            AssetName = go.name;

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