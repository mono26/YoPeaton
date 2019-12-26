using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crosswalk : MonoBehaviour, ICrossable, ITurnable
{
    #region "Variables to set"
    [SerializeField]
    private CrossWalkTypes crossWalkType = CrossWalkTypes.Bocacalle;
    [SerializeField]
    private Collider2D crossAreaBounds = null;
    [SerializeField]
    private Path[] connectedPaths;
    [SerializeField]
    private PathFreePassInfo[] freePassInfo;
#endregion

    private Dictionary<EntityController, WaitTicket> waitingPedestrians = new Dictionary<EntityController, WaitTicket>();
    private Dictionary<EntityController, WaitTicket> waitingCars = new Dictionary<EntityController, WaitTicket>();
    private Dictionary<EntityController, CrossingInfo> crossingPedestrians = new Dictionary<EntityController, CrossingInfo>();
    private Dictionary<EntityController, CrossingInfo> crossingCars = new Dictionary<EntityController, CrossingInfo>();

    private float crossWalkLenght = 0.0f;
    public CrossableType CrossableType { get; private set; }

    public int GetNumberOfCrossingPedestrians 
    {
        get 
        {
            return crossingPedestrians.Count;
        }
    }

    public CrossWalkTypes GetCrossWalkType 
    {
        get 
        {
            return crossWalkType;
        }
    }

    public TurnInfo CurrentTurn { get; private set; }

    public TurnCooldownInfo TurnInCooldown { get; private set; }

    private void Start() 
    {
        CrossableType = CrossableType.CrossWalk;
        //int randomType = UnityEngine.Random.Range(0, Enum.GetNames(typeof(EntityType)).Length);
        //ChangeTurn((EntityType)randomType);
        ChangeTurn(EntityType.Vehicle);
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

    //private void Update()
    //{
    //    UpdateTurn();
    //}

    /// <summary>
    /// Called when a entity enter the crosswalk hotzone.
    /// </summary>
    /// <param name="_entity">Entity that entered.</param>
    public void OnEnter(EntityController _entity) {
        if (_entity.GetEntityType.Equals(EntityType.Pedestrian)) {
            if (!HasAlreadyATicket(_entity)) {
                WaitTicket newTicket = new WaitTicket();
                newTicket.waitStartTime = System.DateTime.UtcNow;
                waitingPedestrians.Add(_entity, newTicket);
                // serializableWaitPedestrians.AddData(_entity, newTicket);
            }
        }
        else if (_entity.GetEntityType.Equals(EntityType.Vehicle)) {
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
        if (_entity.GetEntityType.Equals(EntityType.Vehicle)) {
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
        if (_entity.GetEntityType.Equals(EntityType.Vehicle))
        {
            waitingCars.Remove(_entity);
        }
        else 
        {
            waitingPedestrians.Remove(_entity);
        }
    }

    /// <summary>
    /// Called when a entity started crossing the crosswalk.
    /// </summary>
    /// <param name="_entity">Entity that started crossing.</param>
    public void OnStartedCrossing(EntityController _entity) {
        // DebugController.LogMessage("Entity started crossing");
        ClearTicket(_entity);
        //if (transform.parent.gameObject.name.Equals("CrossWalk_PFB"))
        //{
        //    ChangeTurn(_entity.GetEntityType);
        //}
        //else
        //{
        //    ChangeTurn(_entity.GetEntityType);
        //}
        ChangeTurn(_entity.GetEntityType);
        if (_entity.GetEntityType.Equals(EntityType.Pedestrian)) {
            if (!crossingPedestrians.ContainsKey(_entity)) {
                _entity.GetMovableComponent.AddOnMovement(OnEntityMoved);
                CrossingInfo newInfo = new CrossingInfo();
                newInfo.lastPosition =  _entity.transform.position;
                crossingPedestrians.Add(_entity, newInfo);
            }
        }
        else if (_entity.GetEntityType.Equals(EntityType.Vehicle))
        {
            if (!crossingCars.ContainsKey(_entity)) {
                if (crossingPedestrians.Count > 0) {
                    DebugController.LogMessage("This car is crossing with pedestrians doing it at the same time: " + _entity.gameObject.name);
                    // TODO remover eso de aca
                    if(_entity.gameObject.CompareTag("Car") && _entity.gameObject.name != "PlayerCar_PFB Variant")
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
    public bool CanCross(EntityController _entity)
    {
        bool cross = true;
        //cross = TicketHasTurn(_entity);
        cross = HasTurn(_entity);
        return cross;
    }

    private bool TicketHasTurn(EntityController _entity)
    {
        bool cross = true;
        WaitTicket entityTicket = GetWaitingTicket(_entity);
        Dictionary<EntityController, WaitTicket>.ValueCollection values = null;
        WaitTicket[] valuesArray = null;
        int waitTimeComparisson;
        int gaveCrossTimeComparison;
        if (_entity.GetEntityType.Equals(EntityType.Vehicle) && !crossingCars.ContainsKey(_entity))
        {
            if (crossingPedestrians.Count > 0)
            {
                cross = false;
            }
            else if (waitingPedestrians.Count > 0)
            {
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
            else if (waitingCars.Count > 0)
            {
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
        else if (_entityType.Equals(EntityType.Vehicle)) {
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
                if (keysArray[i] is AIController && ((AIController)keysArray[i]).GetCurrentState.Equals(AIState.WaitingAndAsking)) {
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
    private WaitTicket GetWaitingTicket(EntityController _entity) {
        WaitTicket tiquetToReturn = WaitTicket.invalidTicket;
        if (_entity.GetEntityType.Equals(EntityType.Vehicle)) {
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

    public void OnEntityGivingCross(EntityController _entity) {
        WaitTicket ticket = GetWaitingTicket(_entity);
        if (!ticket.Equals(WaitTicket.invalidTicket)) 
        {
            ticket.gaveCrossTime = ticket.waitStartTime.AddSeconds(WaitTicket.maxWaitTimeInSeconds);
            ticket.gaveCross = true;
            DebugController.LogErrorMessage($"crosswalk recibed gave cross from: { _entity.gameObject.name }");
        }
        else 
        {
            DebugController.LogErrorMessage(string.Format("{0} gave cross but there is no ticket under the entity" , _entity.gameObject.name));
        }
        EntityType other = (_entity.GetEntityType.Equals(EntityType.Vehicle)) ? EntityType.Pedestrian : EntityType.Vehicle;
        ChangeTurn(other);
        if (other.Equals(EntityType.Pedestrian) && TurnInCooldown.Type.Equals(EntityType.Pedestrian))
        {
            TurnInCooldown.ClearCooldown();
        }
        // float time = 3.0f;
    }

    /// <summary>
    /// Called when a entity entered the waiting zone.
    /// </summary>
    /// <param name="_entity"></param>
    public void OnEnteredWaitingZone(EntityController _entity)
    {
        if (IsAValidEntity(_entity))
        {
            OnEnter(_entity);
            _entity.OnCrossableEntered(this);
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

    public void OnExitedCrossingZone(EntityController _entity)
    {
        if (_entity)
        {
            OnExited(_entity);
            OnFinishedCrossing(_entity);
            _entity.OnCrossableExited(this);
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
        else if (_args.Entity.GetEntityType.Equals(EntityType.Vehicle))
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

    public void ChangeTurn(EntityType _type, float _duration)
    {
        TurnInfo nextTurn = new TurnInfo
        {
            Type = _type,
            EndTime = System.DateTime.UtcNow.AddSeconds(_duration)
        };
        TurnInfo pastTurn = CurrentTurn;
        //if (transform.parent.gameObject.name.Equals("CrossWalk_PFB"))
        //{
        //    CurrentTurn = nextTurn;
        //}
        //else
        //{
        //    CurrentTurn = nextTurn;
        //}
        CurrentTurn = nextTurn;
        if (_type.Equals(EntityType.Pedestrian))
        {
            TurnInCooldown = new TurnCooldownInfo()
            {
                Type = pastTurn.Type,
                CooldownFinish = System.DateTime.UtcNow.AddSeconds(TurnCooldownInfo.DEFAULT_COOLDOWN)
            };
            StartCoroutine(StartPedestrianTurn());
        }
    }

    public void ChangeTurn(EntityType _type)
    {
        ChangeTurn(_type, TurnInfo.DEFAULT_DURATION);
    }

    public bool HasTurn(EntityController _entity)
    {
        bool hasTurn = false;
        if (!IsTurnInCooldown(_entity.GetEntityType))
        {
            if (CurrentTurn.Type.Equals(_entity.GetEntityType))
            {
                hasTurn = true;
            }
            else if (CanCrossIfPathIsFree(_entity))
            {
                hasTurn = true;
            }
        }
        return hasTurn;
    }

    public bool CanCrossIfPathIsFree(EntityController _entity)
    {
        bool canCross = false;
        if (_entity.GetEntityType.Equals(EntityType.Pedestrian))
        {
            if (crossingCars.Count > 0)
            {
                Dictionary<EntityController, CrossingInfo>.KeyCollection cars = crossingCars.Keys;
                foreach (EntityController car in cars)
                {
                    if (car.GetMovableComponent.GetCurrentSpeed.Equals(0.0f))
                    {
                        canCross = true;
                    }
                }
            }
            else
            {
                if (waitingCars.Count > 0)
                {
                    Dictionary<EntityController, WaitTicket>.KeyCollection cars = waitingCars.Keys;
                    foreach (EntityController car in cars)
                    {
                        if (((AIController)car).GetCurrentState.Equals(AIState.Waiting) && car.GetMovableComponent.GetCurrentSpeed.Equals(0.0f))
                        {
                            canCross = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    canCross = true;
                }
            }
        }
        else if (_entity.GetEntityType.Equals(EntityType.Vehicle))
        {
            if (crossingPedestrians.Count == 0)
            {
                canCross = true;
            }
            else
            {
                foreach (KeyValuePair<EntityController, CrossingInfo> pedestrian in crossingPedestrians)
                {
                    if (pedestrian.Value.distanceTravelled > crossWalkLenght * 0.6f)
                    {
                        for (int i = 0; i < freePassInfo.Length; i++)
                        {
                            if (freePassInfo[i].path == _entity.GetCurrentPath)
                            {
                                for (int j = 0; j < freePassInfo[i].pathsToCheck.Length; j++)
                                {
                                    if (freePassInfo[i].pathsToCheck[j] == pedestrian.Key.GetCurrentPath)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        canCross = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return canCross;
    }

    public bool UpdateTurn()
    {
        bool updated = false;
        if (CurrentTurn.EndTime < DateTime.UtcNow)
        {
            if (CurrentTurn.Type.Equals(EntityType.Pedestrian))
            {
                if (crossingPedestrians.Count.Equals(0))
                {
                    ChangeTurn((CurrentTurn.Type.Equals(EntityType.Vehicle)) ? EntityType.Pedestrian : EntityType.Vehicle);
                    updated = true;
                }
                else
                {
                    TurnInCooldown = new TurnCooldownInfo()
                    {
                        Type = CurrentTurn.Type,
                        CooldownFinish = System.DateTime.UtcNow.AddSeconds(TurnCooldownInfo.DEFAULT_COOLDOWN)
                    };
                }
            }
            else
            {
                ChangeTurn((CurrentTurn.Type.Equals(EntityType.Vehicle)) ? EntityType.Pedestrian : EntityType.Vehicle);
                updated = true;
            }
        }
        return updated;
    }

    private IEnumerator StartPedestrianTurn()
    {
        while (!UpdateTurn())
        {
            yield return null;
        }
    }

    private bool IsTurnInCooldown(EntityType _type)
    {
        bool cooldown = false;
        if (TurnInCooldown.Type.Equals(_type))
        {
            cooldown = TurnInCooldown.IsInCooldown();
        }
        return cooldown;
    }
}
