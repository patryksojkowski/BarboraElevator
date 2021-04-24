using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarboraElevator.Services.Interfaces
{
    public interface IRouteValidationService
    {
        public bool IsRouteCorrect(int start, int end);
    }
}
