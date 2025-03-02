using BepInEx;
using HarmonyLib;
using UnityEngine;

[BepInPlugin("com.yourname.droneminer", "Scanner Drone Miner", "1.0.0")]
[BepInProcess("Subnautica.exe")]
public class DroneMinerPlugin : BaseUnityPlugin
{
    void Awake()
    {
        Harmony harmony = new Harmony("com.yourname.droneminer");
        harmony.PatchAll();
        Logger.LogInfo("Scanner Drone Miner Loaded!");
    }
    
    [HarmonyPatch(typeof(ScannerRoomCamera), "Update")]
    public class ScannerRoomCamera_Patch
    {
        static void Postfix(ScannerRoomCamera __instance)
        {
            if (Input.GetKeyDown(KeyCode.F)) 
            {
                TryMine(__instance);
            }
        }

        static void TryMine(ScannerRoomCamera drone)
        {
            Collider[] colliders = Physics.OverlapSphere(drone.transform.position, 2f);
            foreach (Collider col in colliders)
            {
                if (col.gameObject.GetComponent<BreakableResource>() != null)
                {
                    BreakableResource resource = col.gameObject.GetComponent<BreakableResource>();
                    resource.BreakIntoResources();
                    Logger.LogInfo("[Scanner Drone] Resource Mined!");
                }
            }
        }
    }
}
