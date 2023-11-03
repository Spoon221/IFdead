using UnityEngine;

namespace Items
{
    public static class Utils
    {
        public static bool IsPlayer(Collider c)
        {
            return c.gameObject.transform.parent.gameObject.CompareTag("Player");
        }
    }
}