using HarmonyLib;

namespace Deez.TooMuchInfo.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("SerializeReadShared", MethodType.Normal)]
    internal class OnDataReceived
    {
        private static void Postfix(VRRig __instance)
        {
            __instance.UpdateName();
        }
    }
}