namespace InsurgencyWeapons.Helpers
{
    internal static class Sounds
    {
        public static SoundStyle GrenadeDetonation => new("InsurgencyWeapons/Sounds/Weapons/Grenades/det", 3);
        public static SoundStyle GrenadeTink => new("InsurgencyWeapons/Sounds/Weapons/Grenades/hit", 4);
        public static SoundStyle GrenadePin => new("InsurgencyWeapons/Sounds/Weapons/Grenades/pin");

        public static SoundStyle RocketDetonation => new("InsurgencyWeapons/Sounds/Weapons/RocketDetonations/det", 3);

        //Stielhandgranate M24
        #region
        public static SoundStyle M24StCap => new("InsurgencyWeapons/Sounds/Weapons/Grenades/cap");
        public static SoundStyle M24StRope => new("InsurgencyWeapons/Sounds/Weapons/Grenades/rope");
        public static SoundStyle M24StThrow => new("InsurgencyWeapons/Sounds/Weapons/Grenades/throw2");
        #endregion

        //Mark 2 Pineapple Grenade
        #region
        public static SoundStyle MK2Pin => new("InsurgencyWeapons/Sounds/Weapons/Grenades/pin");
        public static SoundStyle MK2Spoon => new("InsurgencyWeapons/Sounds/Weapons/Grenades/spoon");
        public static SoundStyle MK2Throw => new("InsurgencyWeapons/Sounds/Weapons/Grenades/throw");
        #endregion

        //RGO
        #region
        public static SoundStyle RGOSpoon => new("InsurgencyWeapons/Sounds/Weapons/Grenades/spoon2");
        #endregion
    }
}