using UnityEngine.SceneManagement;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Items
{
    public class SingletonGeneratorHealth
    {
        private static float singletonGeneratorHealth;
        public static float SingletonGeneratorHealthValue
        {
            get { return singletonGeneratorHealth; }
            private set { singletonGeneratorHealth = value; }
        }

        private static SingletonGeneratorHealth instance;

        public static SingletonGeneratorHealth GetInstance()
        {
            if (instance == null)
            {
                instance = new SingletonGeneratorHealth();
            }
            return instance;
        }

        public float GetHealth()
        {
            return singletonGeneratorHealth;
        }

        public void AddHealth(float added)
        {
            singletonGeneratorHealth += added;
        }

        public void ResetHealth()
        {
            singletonGeneratorHealth = 0f;
        }
    }
}