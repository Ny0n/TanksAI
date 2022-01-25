using UnityEngine;

public class ControlPointManager : MonoBehaviour
{
    /*
     * desc du prof pour comment ça marche:
     *
     * - au début: quand une équipe arrive, il faut charger sa barre à 100% pour capturer -> capture = helipade est coloré, bar de chargement devient blanche (vide)
     * - **il faut toujours au moins un tank de l'equipe qui a capturé le point pour que la barre monte**
     * - si il y a un tank de chaque équipe, rien ne se passe
     * - si un tank de l'autre équipe vient pour capturer, ça commence direct sa capture à 0% (ça décharge pas l'autre, donc comme overwatch)
     * 
     * */
    
    [SerializeField] private FloatVariableSO _winPoints;
    [SerializeField] private TeamVariableSO _controllingTeam;
    [SerializeField] private BoolVariableSO _isSomeoneOnControlPoint;
    
    [SerializeField] private float _controlSpeed = 1;
    [SerializeField] private float _pointSpeed = 0.02f;
    
    // Start is called before the first frame update
    void Start()
    {
        _controllingTeam.Value = default;
        _isSomeoneOnControlPoint.Value = default;
    }

    // Update is called once per frame
    void Update()
    {
        if (_controllingTeam.Value != null)
        {
            // TODO UI
            _controllingTeam.Value.Points = Mathf.Min(_controllingTeam.Value.Points + _pointSpeed, _winPoints.Value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tank"))
            return;
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        _controllingTeam.Value = other.GetComponent<TankTeam>().Team;
        _isSomeoneOnControlPoint.Value = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Tank"))
            return;
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        
        _controllingTeam.Value = null;
        _isSomeoneOnControlPoint.Value = false;
    }
}
