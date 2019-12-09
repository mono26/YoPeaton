using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crosswalk : MonoBehaviour
{
#region "Variables to set"
    [SerializeField]
    private CrossWalkTypes type = CrossWalkTypes.Bocacalle;
    [SerializeField]
    private Collider2D crossAreaBounds = null;
    [SerializeField]
    private Path[] connectedPaths;
#endregion

    private Dictionary<EntityController, WaitTicket> waitingPedestrians = new Dictionary<EntityController, WaitTicket>();
    private Dictionary<EntityController, WaitTicket> waitingCars = new Dictionary<EntityController, WaitTicket>();
    private Dictionary<EntityController, CrossingInfo> crossingPedestrians = new Dictionary<EntityController, CrossingInfo>();
    private Dictionary<EntityController, CrossingInfo> crossingCars = new Dictionary<EntityController, CrossingInfo>();

    private float crossWalkLenght = 0.0f;

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

    private void Start() 
    {
        if (crossAreaBounds)
        {
            crossWalkLenght = crossAreaBounds.bounds.size.x * transform.localScale.x;
        }
        else
        {
            DebugController.LogErrorMessage("Missing crooss are bouds reference.");
        }
        if (connectedPaths == null || connectedPaths.Length.Equals(0))
        {
            DebugController.LogErrorMessage($"{ gameObject.name } has no connectedPaths!");
        }
    }

    /// <summary>
    /// Called when a entity enter the crosswalk hotzone.
    /// </summary>
    /// <param name="_entity">Entity that entered.</param>
    public void OnEntering(EntityController _entity) {
        if (_entity.GetEntityType.Equals(EntityType.Pedestrian)) {
            if (!HasAlreadyATicket(_entity)) {
                WaitTicket newTicket = new WaitTicket();
                newTicket.waitStartTime = System.DateTime.UtcNow;
                waitingPedestrians.Add(_entity, newTicket);
                // serializableWaitPedestrians.AddData(_entity, newTicket);
            }
        }
        else if (_entity.GetEntityType.Equals(EntityType.Car)) {
            if (!HasAlreadyATicket(_entity)) {
                WaitTicket newTicket = new WaitTicket();
                newTicket.waitStartTime = System.DateTime.UtcNow;
                waitingCars.Add(_entity, newTicket);
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
        if (_entity.GetEntityType.Equals(EntityType.Car)) {
            if (waitingCars.ContainsKey(_entity)) {
                hasTicket = true;
            }
        }
        else {
            if (waitingPedestrians.ContainsKey(_entity)) {
                hasTicket = true;
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
        if (_entity.GetEntityType.Equals(EntityType.Car))
        {
            waitingCars.Remove(_entity);
        }
        else 
        {
            waitingPedestrians.Remove(_entity);
            // serializableWaitPedestrians.RemoveKey(_entity);
        }
    }

    /// <summary>
    /// Called when a entity started crossing the crosswalk.
    /// </summary>
    /// <param name="_entity">Entity that started crossing.</param>
    public void OnStartedCrossing(EntityController _entity) {
        // DebugController.LogMessage("Entity started crossing");
        if (_entity.GetEntityType.Equals(EntityType.Pedestrian)) {
            if (!crossingPedestrians.ContainsKey(_entity)) {
                _entity.GetMovableComponent.AddOnMovement(OnEntityMoved);
                CrossingInfo newInfo = new CrossingInfo();
                newInfo.lastPosition =  _entity.transform.position;
                crossingPedestrians.Add(_entity, newInfo);
            }
        }
        else if (_entity.GetEntityType.Equals(EntityType.Car))
        {
            if (!crossingCars.ContainsKey(_entity)) {
                if (crossingPedestrians.Count > 0) {
                    DebugController.LogMessage("This car is crossing with pedestrians doing it at the same time: " + _entity.gameObject.name);
                    if (_entity.gameObject.name == "PlayerCar_PFB Variant")
                    {
                        ScoreManager.instance.AddInfraction();
                        CanvasManager._instance.ActivateCheckOrCross(false);
                        CanvasManager._instance.GenerateFeedback("CrossWithPedestrian");
                    }
                    else if(_entity.gameObject.CompareTag("Car") && _entity.gameObject.name != "PlayerCar_PFB Variant")
                    {
                        _entity.SetCrossingWithPedestrianValue(true);
                    }
                }
                // DebugController.LogMessage("Adding car to crossing cars.");
                _entity.GetMovableComponent.AddOnMovement(OnEntityMoved);
                CrossingInfo newInfo = new CrossingInfo();
                newInfo.lastPosition = _entity.transform.position;
                crossingCars.Add(_entity, newInfo);
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
            if (crossingPedestrians.ContainsKey(_entity)) 
            {
                _entity.GetMovableComponent.RemoveOnMovement(OnEntityMoved);
                crossingPedestrians.Remove(_entity);
            }
        }
        else if (_entity.CompareTag("Car")) 
        {
            if (_entity.gameObject.name != "PlayerCar_PFB Variant")
            {
                _entity.SetCrossingWithPedestrianValue(false);
            }
            if (crossingCars.ContainsKey(_entity)) 
            {
                _entity.GetMovableComponent.RemoveOnMovement(OnEntityMoved);
                crossingCars.Remove(_entity);
            }
        }
    }

    /// <summary>
    /// Check if the entity can cross the crosswalk.
    /// </summary>
    /// <param name="_entity">Entity to check if it can cross.</param>
    /// <returns>Returns true for if the entity can cross, false if not.</returns>
    public bool HasTurn(EntityController _entity)
    {
        bool cross = true;
        WaitTicket entityTicket = GetWaitingTicket(_entity);
        Dictionary<EntityController, WaitTicket>.ValueCollection values = null;
        WaitTicket[] valuesArray = null;
        int waitTimeComparisson;
        int gaveCrossTimeComparison;
        if (_entity.GetEntityType.Equals(EntityType.Car) && !crossingCars.ContainsKey(_entity)) 
        {
            if (crossingPedestrians.Count > 0) 
            {
                cross = false;
            }
            else if (waitingPedestrians.Count > 0) {
                // TODO refactor into compare to pedestrians tickets.
                values = waitingPedestrians.Values;
            }
        }
        else if (_entity.GetEntityType.Equals(EntityType.Pedestrian) && !crossingPedestrians.ContainsKey(_entity)) 
        {
            if (crossingCars.Count > 0) 
            {
                cross = false;
            }
            else if (waitingCars.Count > 0) {
                if (!CanCrossWithCrossingPedestrians())
                {
                    values = waitingCars.Values;
                }
            }
        }
        if (values != null)
        {
            valuesArray = values.ToArray();
            values = null;
            for (int i = 0; i < valuesArray.Length; i++)
            {
                waitTimeComparisson = System.DateTime.Compare(entityTicket.waitStartTime, valuesArray[i].waitStartTime);
                gaveCrossTimeComparison = System.DateTime.Compare(entityTicket.gaveCrossTime, valuesArray[i].waitStartTime);
                if ((entityTicket.gaveCross && gaveCrossTimeComparison >= 0) || (!entityTicket.gaveCross && waitTimeComparisson >= 0))
                {
                    cross = false;
                    break;
                }
            }
        }
        return cross;
    }

    /// <summary>
    /// Checks if the cross is possible with the current crossing pedestrians.
    /// </summary>
    /// <returns>Return true if it can cross and false if not.</returns>
    private bool CanCrossWithCrossingPedestrians()
    {
        bool canCross = false;
        if (crossingPedestrians.Count > 0)
        {
            Dictionary<EntityController, CrossingInfo>.ValueCollection values = crossingPedestrians.Values;
            CrossingInfo[] valuesArray = values.ToArray();
            DebugController.LogMessage("Checking cross with other corssing pedestrians");
            for (int i = 0; i < valuesArray.Length; i++)
            {
                if (valuesArray[i].distanceTravelled < crossWalkLenght * 0.5f)
                {
                    DebugController.LogMessage("There is a pedestrian recently crossing");
                    canCross = true;
                    break;
                }
            }
            values = null;
            valuesArray = null;
        }
        return canCross;
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
            Dictionary<EntityController, WaitTicket>.KeyCollection keys = waitingPedestrians.Keys;
            EntityController[] keysArray = keys.ToArray();
            keys = null;
            for (int i = 0; i < keysArray.Length; i++) {
                if (keysArray[i] is AIController && ((AIController)keysArray[i]).GetCurrentState.Equals(AIState.WaitingAtCrossWalkAskingForCross)) {
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
        if (_entity.GetEntityType.Equals(EntityType.Car)) {
            if (waitingCars.ContainsKey(_entity))
            {
                tiquetToReturn = waitingCars[_entity];
            }
        }
        else {
            if (waitingPedestrians.ContainsKey(_entity))
            {
                tiquetToReturn = waitingPedestrians[_entity];
            }
        }
        return tiquetToReturn;
    }

    public void OnEntityGivingCross(EntityController _entityThatGaveCross) {
        WaitTicket ticket = GetWaitingTicket(_entityThatGaveCross);
        if (!ticket.Equals(WaitTicket.invalidTicket)) {
            ticket.gaveCrossTime = ticket.waitStartTime.AddSeconds(WaitTicket.maxWaitTimeInSeconds);
            ticket.gaveCross = true;
        }
        else {
            DebugController.LogErrorMessage(string.Format("{0} gave cross but there is no ticket under the entity" , _entityThatGaveCross.gameObject.name));
        }
    }

    private void OnTriggerEnter2D(Collider2D _other) 
    {
        if (_other.gameObject.CompareTag("Car") || _other.gameObject.CompareTag("Pedestrian")) 
        {
            EntityController entity = _other.transform.GetComponent<EntityController>();
            // DebugController.LogMessage(entity.ToString());
            if (IsAValidEntity(entity)) 
            {
                OnEntering(entity);
                entity.OnCrossWalkEntered(this);
            }
        }
    }

    private bool IsAValidEntity(EntityController _entity)
    {
        bool valid = false;
        if (_entity && !_entity.JustExitedCrossWalk(this))
        {
            foreach (Path path in connectedPaths)
            {
                if (path == _entity.GetCurrentPath)
                {
                    valid = true;
                    break;
                }
            }
        }
        return valid;
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

    private void OnEntityMoved(OnEntityMovementEventArgs _args)
    {
        if (_args.Entity.GetEntityType.Equals(EntityType.Pedestrian))
        {
            if (crossingPedestrians.ContainsKey(_args.Entity))
            {
                CrossingInfo info = crossingPedestrians[_args.Entity];
                // First calculate the distance travelled since last frame.
                float distance = (_args.Entity.transform.position - info.lastPosition).magnitude;
                // Add to the calculated distance the sotored distance to get the total.
                distance += info.distanceTravelled;
                info.distanceTravelled = distance;
                crossingPedestrians[_args.Entity] = info;
            }
            else
            {
                DebugController.LogErrorMessage(string.Format("{0} moved but it has no crossing info stored.", _args.Entity.gameObject.name));
            }
        }
        else if (_args.Entity.GetEntityType.Equals(EntityType.Car))
        {
            if (crossingCars.ContainsKey(_args.Entity))
            {
                CrossingInfo info = crossingCars[_args.Entity];
                // First calculate the distance travelled since last frame.
                float distance = (_args.Entity.transform.position - info.lastPosition).magnitude;
                // Add to the calculated distance the sotored distance to get the total.
                distance += info.distanceTravelled;
                info.distanceTravelled = distance;
                crossingCars[_args.Entity] = info;
            }
            else
            {
                DebugController.LogErrorMessage(string.Format("{0} moved but it has no crossing info stored.", _args.Entity.gameObject.name));
            }
        }
    }
}
