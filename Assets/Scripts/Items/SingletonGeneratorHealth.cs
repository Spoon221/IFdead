namespace Items
{
    public class SingletonGeneratorHealth
    {
        public static float singletonGeneratorHealth;
        
        private static SingletonGeneratorHealth instance;

        public static SingletonGeneratorHealth GetInstance()
        {
            // return instance ??= new SingletonGeneratorHealth();
            if (instance != null) return instance;
            instance = new SingletonGeneratorHealth();
            return instance;

            // return new SingletonGeneratorHealth(); для локального тестирования
        }
        
        public float GetHealth()
        {
            return singletonGeneratorHealth;
        }

        public void AddHealth(float added)
        {
            singletonGeneratorHealth += added;
        }
    }
}