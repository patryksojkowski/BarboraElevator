namespace BarboraElevator.Services.Interfaces
{
    public interface IRouteValidationService
    {
        public bool IsRouteCorrect(int start, int end);
    }
}
