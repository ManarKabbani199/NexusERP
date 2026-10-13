using System.Collections.Generic;
using NexusERP.Domain.Entities;

namespace NexusERP.Application.Interfaces
{
    public interface IStockMovementService
    {
        IEnumerable<StockMovement> GetAll();

        StockMovement GetById(int id);

        void CreateMovement(
            StockMovement movement,
            bool saveChanges = true);

        void CancelMovement(
            int movementId,
            string cancellationReason,
            string cancelledBy,
            bool saveChanges = true);
    }
}