using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosswalk : MonoBehaviour
{
    [SerializeField]
    private List<WaitTicket> waitingPedestrians = new List<WaitTicket>();
    [SerializeField]
    private List<WaitTicket> waitingCars = new List<WaitTicket>();
    [SerializeField]
    private List<EntityController> crossingPedestrians = new List<EntityController>();
    [SerializeField]
    private List<EntityController> crossingCars = new List<EntityController>();

    [SerializeField]
    private CrossWalkTypes type;

    public int GetNumberOfCrossingPedestrians {
        get {
            return crossingPedestrians.Count;
        }
    }

    public CrossWalkTypes GetCrossWalkType {
        get {
            return type;
        }
    }

    /// <summary>
    /// Called when a entity enter the crosswalk hotzone.
    /// </summary>
    /// <param name="_entity">Entity that entered.</param>
    public void OnEntering(EntityController _entity) {
        if (_entity.gameObject.CompareTag("Pedestrian")) {
            if (!HasAlreadyATicket(_entity)) {
                WaitTicket newTicket = new WaitTicket(_entity);
                waitingPedestrians.Add(newTicket);
            }
        }
        else if (_entity.gameObject.CompareTag("Car")) {
            if (!HasAlreadyATicket(_entity)) {
                WaitTicket newTicket = new WaitTicket(_entity);
                waitingCars.Add(newTicket);
            }
        }
    }

    /// <summary>
    /// Check if that entity already has a ticket.
    /// </summary>
    /// <param name="_entity">Entity to check in  existing tickets.</param>
    /// <returns></returns>
    private bool HasAlreadyATicket(EntityController _entity) {
        bool hasTicket = false;
        if (_entity.CompareTag("Car")) {
            for (int i = 0; i < waitingCars.Count; i++) {
                if (waitingCars[i].waitingEntity.Equals(_entity)) {
                    hasTicket = true;
                    break;
                }
            }
        }
        else {
            for (int i = 0; i < waitingPedestrians.Count; i++) {
                if (waitingPedestrians[i].waitingEntity.Equals(_entity)) {
                    hasTicket = true;
                    break;
                }
            }
        }
        return hasTicket;
    }

    /// <summary>
    /// Called when a entity exits the crosswalk hotzone.
    /// </summary>
    /// <param name="_entity">Entity that exited.</param>
    public void OnExited(EntityController _entity) {
        if (_entity.gameObject.CompareTag("Pedestrian") || _entity.gameObject.CompareTag("Car")) {
            if (HasAlreadyATicket(_entity)) {
                ClearTicket(_entity);
            }
        }
    }

    /// <summary>
    /// Clear the existing tickets that belong to the entity.
    /// </summary>
    /// <param name="_entity"> Entity to check for tickets.</param>
    private void ClearTicket(EntityController _entity) {
        if (_entity.CompareTag("Car")) {
            IEnumerable<WaitTicket> existingTickets = waitingCars.Where(ticket => ticket.waitingEntity.Equals(_entity));
            for (int i = 0; i < existingTickets.Count(); i++) {
                waitingCars.Remove(existingTickets.ElementAt(i));
            }
        }
        else {
            IEnumerable<WaitTicket> existingTickets = waitingPedestrians.Where(ticket => ticket.waitingEntity.Equals(_entity));
            for (int i = 0; i < existingTickets.Count(); i++) {
                waitingPedestrians.Remove(existingTickets.ElementAt(i));
            }
        }
    }

    /// <summary>
    /// Called when a entity started crossing the crosswalk.
    /// </summary>
    /// <param name="_entity">Entity that started crossing.</param>
    public void OnStartedCrossing(EntityController _entity) {
        // DebugController.LogMessage("Entity started crossing");
        if (_entity.gameObject.CompareTag("Pedestrian")) {
            if (!crossingPedestrians.Contains(_entity)) {
                crossingPedestrians.Add(_entity);
            }
        }
        else if (_entity.gameObject.CompareTag("Car")) {
            if (!crossingCars.Contains(_entity)) {
                if (crossingPedestrians.Count > 0) {
                    DebugController.LogMessage("This car is crossing with pedestrians doing it at the same time: " + _entity.gameObject.name);
                }
                // DebugController.LogMessage("Adding car to crossing cars.");
                crossingCars.Add(_entity);
            }
        }
    }

    /// <summary>
    /// Called when a entity finished crossing the crosswalk.
    /// </summary>
    /// <param name="_entity">Entity that finished crossing the crosswalk.</param>
    public void OnFinishedCrossing(EntityController _entity) {
        if (_entity.CompareTag("Pedestrian"))
        {
            if (crossingPedestrians.Contains(_entity)) 
            {
                crossingPedestrians.Remove(_entity);
            }
        }
        else if (_entity.CompareTag("Car")) 
        {
            if (crossingCars.Contains(_entity)) 
            {
                crossingCars.Remove(_entity);
            }
        }
    }

    /// <summary>
    /// Check if the entity can cross the crosswalk.
    /// </summary>
    /// <param name="_entity">Entity to check if it can cross.</param>
    /// <returns>Returns true for if the entity can cross, false if not.</returns>
    public bool CanCross(EntityController _entity)
    {
        bool cross = true;
        if (_entity.gameObject.CompareTag("Car") && !crossingCars.Contains(_entity)) {
            if (crossingPedestrians.Count > 0) {
                cross = false;
            }
            else if (waitingPedestrians.Count > 0) {
                WaitTicket entityTicket = GetWaitingTicket(_entity);
                int waitTimeComparisson;
                int gaveCrossTimeComparison;
                for (int i = 0; i < waitingPedestrians.Count; i++) {
                    waitTimeComparisson = System.DateTime.Compare(entityTicket.waitStartTime, waitingPedestrians[i].waitStartTime);
                    gaveCrossTimeComparison = System.DateTime.Compare(entityTicket.gaveCrossTime, waitingPedestrians[i].waitStartTime);
                    if (waitTimeComparisson >= 0 || (entityTicket.gaveCross && gaveCrossTimeComparison >= 0)) {
                        cross = false;
                        break;
                    }
                }
            }
        }
        else if (_entity.gameObject.CompareTag("Pedestrian") && !crossingPedestrians.Contains(_entity)) {
            if (crossingCars.Count > 0) {
                cross = false;
            }
            else if (waitingCars.Count > 0) {
                WaitTicket entityTicket = GetWaitingTicket(_entity);
                int waitTimeComparisson;
                int gaveCrossTimeComparison;
                for (int i = 0; i < waitingCars.Count; i++) {
                    waitTimeComparisson = System.DateTime.Compare(entityTicket.waitStartTime, waitingCars[i].waitStartTime);
                    gaveCrossTimeComparison = System.DateTime.Compare(entityTicket.gaveCrossTime, waitingCars[i].waitStartTime);
                    if (waitTimeComparisson >= 0 || (entityTicket.gaveCross && gaveCrossTimeComparison >= 0)) {
                        cross = false;
                        break;
                    }
                }
            }
        }
        return cross;
    }

    /// <summary>
    /// Use to see if the is any entity of the type waiting to cross.
    /// </summary>
    /// <param name="_entityType">Entity type to look if it's waiting for cross.</param>
    /// <returns></returns>
    public bool IsThereAEntityWaiting(EntityType _entityType) {
        bool isAEntityWaiting = false;
        if (_entityType.Equals(EntityType.Pedestrian)) {
            if (waitingPedestrians.Count > 0) {
                isAEntityWaiting = true;
            }
        }
        else if (_entityType.Equals(EntityType.Car)) {
            if (waitingCars.Count > 0) {
                isAEntityWaiting = true;
            }
        }
        return isAEntityWaiting;
    }

    /// <summary>
    /// Use to see if the is a entity of the type requesting for cross.
    /// </summary>
    /// <param name="_entityType">Entity type to look if it's asking for cross.</param>
    /// <returns></returns>
    public bool IsThereAEntityAskingForCross(EntityType _entityType) {
        bool isAnEntityAskingForPass = false;
        if (_entityType.Equals(EntityType.Pedestrian)) {
            for (int i = 0; i < waitingPedestrians.Count; i++) {
                if (((AIController)waitingPedestrians[i].waitingEntity).GetCurrentState.Equals(AIState.WaitingAtCrossWalkAndAskingForPass)) {
                    isAnEntityAskingForPass = true;
                    break;
                }
            }
        }
        // else if (_entityType.Equals(AIType.Car)) {
        //     if (waitingCars.Count > 0) {
        //         isAEntityWaiting = true;
        //     }
        // }
        return isAnEntityAskingForPass;
    }

    /// <summary>
    /// Gets the actual waiting ticket for the entity.
    /// </summary>
    /// <param name="_entity">Enity to look for ticket.</param>
    /// <returns></returns>
    public WaitTicket GetWaitingTicket(EntityController _entity) {
        WaitTicket tiquetToReturn = WaitTicket.invalidTicket;
        IEnumerable<WaitTicket> existingTickets;
        if (_entity.CompareTag("Car")) {
            existingTickets = waitingCars.Where(ticket => ticket.waitingEntity.Equals(_entity));
        }
        else {
            existingTickets = waitingPedestrians.Where(ticket => ticket.waitingEntity.Equals(_entity));
        }
        if (existingTickets.Count() > 0) {
            tiquetToReturn = existingTickets.ElementAt(0);
        }
        return tiquetToReturn;
    }

    public void OnEntityGivingCross(EntityController _entityThatGaveCross) {
        WaitTicket ticket = GetWaitingTicket(_entityThatGaveCross);
        if (!ticket.Equals(WaitTicket.invalidTicket)) {
            ticket.gaveCross = true;
        }
        else {
            DebugController.LogErrorMessage(string.Format("{0} gave cross but there is no ticket under the entity" , _entityThatGaveCross.gameObject.name));
        }
    }

    private void OnTriggerEnter2D(Collider2D _other) {
        if (_other.gameObject.CompareTag("Car") || _other.gameObject.CompareTag("Pedestrian")) {
            EntityController entity = _other.transform.GetComponent<EntityController>();
            if (entity  && !entity.JustExitedCrossWalk(this)) {
                OnEntering(entity);
                entity.OnCrossWalkEntered(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _other) {
        if (_other.CompareTag("Car") || _other.CompareTag("Pedestrian")) {
            EntityController entity = _other.transform.GetComponent<EntityController>();
            if (entity) {
                OnExited(entity);
                OnFinishedCrossing(entity);
                entity.OnCrossWalkExited(this);
            }
        }
    }
}
