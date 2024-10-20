using UnityEngine;
namespace BulldozeGhostLandfills.Utils
{
    public class Log
    {
        public const string TAG = "[BulldozeGhostLandfills] ";
        public static void Msg(object msg) => Debug.Log(TAG + msg.ToString());
        public static void Warn(object msg) => Debug.LogWarning(TAG + msg.ToString());
        public static void Err(object msg) => Debug.LogError(TAG + msg.ToString());
        public static void LogExpection(System.Exception e) => Debug.LogException(e);
    }

}
