using BepInEx;
using BepInEx.Configuration;
using GorillaTag.Cosmetics;
using UnityEngine;
using YizziCamModV2;

namespace RC_Cam
{
    [BepInDependency("com.yizzi.gorillatag.yizzicammodv2")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static GameObject CamFollow ,ShoulderCam;
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
                CamFollow = CameraController.Instance.CameraFollower.gameObject;
                ShoulderCam = CameraController.Instance.CameraTablet.transform.FindChildRecursive("Shoulder Camera").gameObject;
                GameObject c = new GameObject("ExtraCam", typeof(Camera));
                CloneCam = c.GetComponent<Camera>();
                CloneCam.transform.SetParent(ShoulderCam.transform, false);
                CloneCam.transform.localRotation = Quaternion.Euler(Vector3.zero);
                CloneCam.cullingMask = CloneCam.transform.parent.GetComponent<Camera>().cullingMask;
                CloneCam.enabled = false;
                CameraController.Instance.CMVirtualCamera.enabled = true;
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
            CameraController.Instance.CMVirtualCamera.enabled = false;
            CamFollow.transform.SetParent(GorillaTagger.Instance.mainCamera.transform, false);
            CamFollow.transform.localRotation = Quaternion.Euler(Vector3.zero);
            CamFollow.transform.localPosition = Vector3.zero;
            if (FirstP.Value)
            {
                Camera.SetupCurrent(GorillaTagger.Instance.mainCamera.GetComponent<Camera>());
                CloneCam.enabled = false;
            }
            ShoulderCam.transform.localPosition = new Vector3(0.022f, 0.087f, 0.09f);
            ShoulderCam.transform.localRotation = Quaternion.Euler(0, YizziFLip(), 0);
        }

        static float YizziFLip()
        {
            if (CameraController.Instance.flipped)
            {
                return 180;
            }
            else
            {
                return 0;
            }
        }
    }
}
