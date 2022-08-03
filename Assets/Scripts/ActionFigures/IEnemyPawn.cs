namespace ActionFigures
{
    public interface IEnemyPawn
    {
        public EnemyController EnemyController { get; set; }
        public void Active();
        public bool Availabled();
    }
}