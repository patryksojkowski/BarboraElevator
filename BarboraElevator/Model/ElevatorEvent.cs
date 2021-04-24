namespace BarboraElevator.Model
{
    public class ElevatorEvent
    {
        public long TimeStamp { get; set; }
        public string Subject { get; set; }

        public override string ToString()
        {
            return $"{TimeStamp}   {Subject}";
        }
    }
}
