namespace BarboraElevator.Services.Interfaces
{
    public interface IRouteValidationService
    {
        bool IsRouteCorrect(int start, int end);
        bool IsFloorNumberCorrect(int floorNumber);
    }
}
