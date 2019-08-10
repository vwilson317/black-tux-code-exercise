namespace Tests
{
    public class EventRequest
    {
        //would put this in an enum but I don't want to deal with parsing at the moment
        public int Day { get; set; }
        public int DesiredSmall { get; set; }
        public int DesiredMedium { get; set; }
        public int DesiredLarge { get; set; }
    }
}