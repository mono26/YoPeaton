public interface ICrossable
{
    /// <summary>
    /// Type of the crossable.
    /// </summary>
    CrossableType CrossableType { get; }
    /// <summary>
    /// Use to check if the entity can cross.
    /// </summary>
    /// <param name="_entity">Entity to check for cross availability.</param>
    /// <returns></returns>
    bool CanCross(EntityController _entity);
    /// <summary>
    /// Should be called when a entity starts crosssing.
    /// </summary>
    /// <param name="_entity">Entity that started crossing.</param>
    void OnStartedCrossing(EntityController _entity);
    /// <summary>
    /// Should be called when a entity finish crossing.
    /// </summary>
    /// <param name="_entity">Entity that finished crossing.</param>
    void OnFinishedCrossing(EntityController _entity);
    /// <summary>
    /// Should be called when a entity enters the crossable bounds. IE: Trigger Collider.
    /// </summary>
    /// <param name="_entity">Entity that entered bounds.</param>
    void OnEnter(EntityController _entity);
    /// <summary>
    /// Should be called when a entity exits the crossable bounds. IE: Trigger Collider.
    /// </summary>
    /// <param name="_entity">Entity that exited bounds.</param>
    void OnExited(EntityController _entity);
}
