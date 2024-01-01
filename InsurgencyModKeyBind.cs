using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    internal class InsurgencyModKeyBind : ModSystem
    {
        public static ModKeybind ReloadKey { get; private set; }

        public override void Load()
        {
            ReloadKey = KeybindLoader.RegisterKeybind(Mod, "ReloadKey", "B");
        }

        public override void Unload()
        {
            ReloadKey = null;
        }
    }
}