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

    /// <summary>
    /// Called when a entity enter the crosswalk hotzone.
    /// </summary>
    /// <param name="_entity">Entity that entered.</param>
    public void OnEntering(EntityController _entity) {
        if (_entity.CompareTag("Pedestrian")) {
            if (!HasAlreadyATicket(_entity)) {
                WaitTicket newTicket = new WaitTicket(_entity);
                waitingPedestrians.Add(newTicket);
            }
        }
        else if (_entity.CompareTag("Car")) {
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
        if (_entity.CompareTag("Pedestrian")) {
            if (HasAlreadyATicket(_entity)) {
                ClearTicket(_entity);
            }
        }
        else if (_entity.CompareTag("Car")) {
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
        if (_entity.CompareTag("Pedestrian")) {
            if (!crossingPedestrians.Contains(_entity)) {
                crossingPedestrians.Add(_entity);
            }
        }
        else if (_entity.CompareTag("Car")) {
            if (!crossingCars.Contains(_entity)) {
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
            if (crossingPedestrians.Contains(_entity)) {
                crossingPedestrians.Remove(_entity);
            }
        }
        else if (_entity.CompareTag("Car")) {
            if (crossingCars.Contains(_entity)) {
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
        if (_entity.CompareTag("Car") && !crossingCars.Contains(_entity)) {
            if (crossingPedestrians.Count > 0) {
                cross = false;
            }
            else if (waitingPedestrians.Count > 0) {
                WaitTicket entityTicket = GetWaitingTicket(_entity);
                for (int i = 0; i < waitingPedestrians.Count; i++) {
                    if (waitingPedestrians[i].waitStartTime <= entityTicket.waitStartTime) {
                        cross = false;
                    }
                }
            }
        }
        else if (_entity.CompareTag("Pedestrian") && !crossingPedestrians.Contains(_entity)) {
            if (crossingCars.Count > 0) {
                cross = false;
            }
            else if (waitingCars.Count > 0) {
                WaitTicket entityTicket = GetWaitingTicket(_entity);
                for (int i = 0; i < waitingCars.Count; i++) {
                    if (waitingCars[i].waitStartTime < entityTicket.waitStartTime) {
                        cross = false;
                    }
                }
            }
        }
        return cross;
    }

    /// <summary>
    /// Gets the actual waiting ticket for the entity.
    /// </summary>
    /// <param name="_entity">Enity to look for ticket.</param>
    /// <returns></returns>
    public WaitTicket GetWaitingTicket(EntityController _entity) {
        WaitTicket tiquetToReturn = new WaitTicket(_entity);
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

    private void OnTriggerEnter2D(Collider2D _other) {
        if (_other.gameObject.CompareTag("Car") || _other.gameObject.CompareTag("Pedestrian")) {
            AIController ai = _other.transform.GetComponent<AIController>();
            OnEntering(ai);
            ai.OnCrossWalkEntered(this);
        }
    }

    private void OnTriggerExit2D(Collider2D _other) {
        if (_other.CompareTag("Car") || _other.CompareTag("Pedestrian")) {
            AIController ai = _other.transform.GetComponent<AIController>();
            OnExited(ai);
            OnFinishedCrossing(ai);
            ai.OnCrossWalkExited(this);
        }
    }
}
