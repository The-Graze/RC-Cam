using BepInEx;
using BepInEx.Configuration;
using GorillaTag.Cosmetics;
using System;
using UnityEngine;
using Utilla;

namespace RC_Cam
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static GameObject CamFollow;
        public static RCVehicle CurrentRC;
        static Camera CloneCam;

        static ConfigEntry<bool> FirstP;
        Plugin()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            FirstP = Config.Bind("Settings", "FirstP", false, "Should The RC be in the first person view too");
        }
        public static void RCGo(RCVehicle veh)
        {
            CurrentRC = veh;
            if (CamFollow == null)
            {
                CamFollow = GorillaTagger.Instance.mainCamera.transform.FindChildRecursive("Camera Follower").gameObject;
                GameObject c = new GameObject("ExtraCam", typeof(Camera));
                CloneCam = c.GetComponent<Camera>();
                CloneCam.transform.SetParent(GorillaTagger.Instance.thirdPersonCamera.transform.GetChild(0), false);
                CloneCam.transform.localRotation = Quaternion.Euler(Vector3.zero);
                CloneCam.cullingMask = CloneCam.transform.parent.GetComponent<Camera>().cullingMask;
                CloneCam.enabled = false;
            }
            CamFollow.transform.SetParent(veh.transform, false);
            CamFollow.transform.localPosition = new Vector3(-0.5f, 0, 0);
            CamFollow.transform.localRotation = Quaternion.Euler(5,0,0);
            if (FirstP.Value)
            {
                Camera.SetupCurrent(CloneCam);
                CloneCam.enabled = true;
            }
        }

        public static void RCStop()
        {
            CamFollow.transform.SetParent(GorillaTagger.Instance.mainCamera.transform, false);
            CamFollow.transform.localRotation = Quaternion.Euler(Vector3.zero);
            CamFollow.transform.localPosition = Vector3.zero;
            if (FirstP.Value)
            {
                Camera.SetupCurrent(GorillaTagger.Instance.mainCamera.GetComponent<Camera>());
                CloneCam.enabled = false;
            }
        }
    }
}
