using System;
using System.Collections.Generic;
using HarmonyLib;
using Photon.Pun;

namespace Deez.TooMuchInfo.Console;

[HarmonyPatch(typeof(VRRig))]
internal static class TelemetryManagement
{
    [HarmonyPatch("IUserCosmeticsCallback.OnGetUserCosmetics")]
    [HarmonyPostfix]
    private static void OnGetRigCosmetics(VRRig __instance)
    {
        NetPlayer player = __instance.creator;

        if (__instance == null || player.GetPlayerRef() == PhotonNetwork.LocalPlayer ||
            HamburburData.Admins.ContainsKey(player.UserId))
            return;

        Dictionary<string, Dictionary<string, string>> data = new()
        {
                [player.UserId] = new Dictionary<string, string>
                {
                        {
                                "nickname",
                                CleanString(player.NickName)
                        },
                        {
                                "cosmetics",
                                __instance._playerOwnedCosmetics.Concat()
                        },
                        {
                                "color",
                                $"{Math.Round(__instance.playerColor.r * 255)} {Math.Round(__instance.playerColor.g * 255)} {Math.Round(__instance.playerColor.b * 255)}"
                        },
                        {
                                "platform",
                                IsOnSteam(__instance) ? "STEAM" : "QUEST"
                        },
                },
        };
    }

    public static string CleanString(string input, int maxLength = 12)
    {
        input = new string(Array.FindAll(input.ToCharArray(), global::Utils.IsASCIILetterOrDigit));

        if (input.Length > maxLength)
            input = input[..(maxLength - 1)];

        input = input.ToUpper();

        return input;
    }

    public static bool IsOnSteam(VRRig Player)
    {
        string concat           = Player._playerOwnedCosmetics.Concat();
        int    customPropsCount = Player.Creator.GetPlayerRef().CustomProperties.Count;

        return concat.Contains("S. FIRST LOGIN") || concat.Contains("FIRST LOGIN") || customPropsCount >= 2;
    }
}