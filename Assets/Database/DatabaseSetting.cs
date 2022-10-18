namespace Database
{
    public static class DatabaseSetting
    {
        public static string ConnectionUri => "mongodb+srv://freeaccount:19031981@generalcluster.aqkelbz.mongodb.net/?retryWrites=true&w=majority";
        public static string DatabaseName => "GENERAL";
        public static string PlayerCollection => "Testing";
        public static string AchievementCollection => "Achievements";
        public static string GamesCollection => "CoolGames";
    }
}