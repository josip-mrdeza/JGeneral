using DiscordRPC;
using INFITF;

namespace JGeneral.IO.Interop.Discord
{
    public static class PresenceHelper
    {
        public static string AsId(this Presence presence)
        {
            return presence switch
            {
                Presence.JokiNC => "962723957650382898",
                Presence.Akali => "964600456921907291",
                _ =>  "964600456921907291"
            };
        }

        public static Assets LoadAssetsFromDisk()
        {
            return new Assets()
            {
                LargeImageKey = System.IO.File.ReadAllText("Discord/Config/largeKey.txt"),
                SmallImageKey = System.IO.File.ReadAllText("Discord/Config/smallKey.txt"),
                LargeImageText = System.IO.File.ReadAllText("Discord/Config/largeText.txt"),
                SmallImageText = System.IO.File.ReadAllText("Discord/Config/smallText.txt")
            };
        }
    }
}