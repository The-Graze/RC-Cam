using GorillaTag.Cosmetics;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RC_Cam.Patches
{
    [HarmonyPatch(typeof(RCVehicle))]
    [HarmonyPatch("OnEnable", MethodType.Normal)]
    internal class RCPatch
    {
        private static void Postfix(RCBlimp __instance)
        {
            if (__instance.connectedRemote != null)
            {
                Plugin.RCGo(__instance);
            }
        }
    }
    [HarmonyPatch(typeof(RCVehicle))]
    [HarmonyPatch("OnDisable", MethodType.Normal)]
    internal class RCPatch0
    {
        private static void Postfix(RCBlimp __instance)
        {
            if (__instance == Plugin.CurrentRC)
            {
                Plugin.RCStop();
            }
        }
    }
}
