using Terraria.ModLoader;

namespace InsurgencyWeapons
{
    internal class InsurgencyModKeyBind : ModSystem
    {
        public static ModKeybind ReloadKey { get; private set; }
        public static ModKeybind QuickBuy { get; private set; }

        public override void Load()
        {
            ReloadKey = KeybindLoader.RegisterKeybind(Mod, "ReloadKey", "B");
            QuickBuy = KeybindLoader.RegisterKeybind(Mod, "QuickBuy", "N");
        }

        public override void Unload()
        {
            ReloadKey = QuickBuy = null;
        }
    }
}