namespace Game.Learnings.SL
{
    public enum GameResult { Win, Tie, Loss }
    public class BoardStateItem
    {
        public int[] BoardFields { get; set; }
        public GameResult GameResult { get; set; }
    }
}
