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
    
    [SerializeField] private TeamVariableSO _controllingTeam;
    [SerializeField] private BoolVariableSO _isSomeoneOnControlPoint;
    
    [SerializeField] private float _controlSpeed = 1;
    [SerializeField] private float _pointSpeed = 0.02f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tank"))
            return;

        _controllingTeam.Value = other.GetComponent<TankTeam>().Team;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_controllingTeam.Value != null)
        {
            // TODO
            _controllingTeam.Value.Points += _pointSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Tank"))
            return;

        _controllingTeam.Value = null;
    }
}
