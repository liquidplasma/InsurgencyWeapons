using Terraria.Audio;

namespace InsurgencyWeapons.Helpers
{
    internal static class Sounds
    {
        public static SoundStyle GrenadeDetonation => new("InsurgencyWeapons/Sounds/Weapons/GrenadeDetonations/det", 3);
        public static SoundStyle RocketDetonation => new("InsurgencyWeapons/Sounds/Weapons/RocketDetonations/det", 3);
    }
}