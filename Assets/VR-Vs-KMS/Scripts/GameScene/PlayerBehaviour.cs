using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviourPunCallbacks, IPunObservable
{
    // Start is called before the first frame update
    private int Life;
    public GameObject SpawnerContainer;
    private List<Transform> spawnPoints;
    void Start()
    {
        Life = GameConfig.GetInstance().LifeNumber;
        spawnPoints = new List<Transform>();
        SpawnerContainer = GameObject.Find("SpawnAreaContainer");
        GetSpawners();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if( (other.CompareTag("Viral") && gameObject.CompareTag("KMS")) || (other.CompareTag("Antiviral") && gameObject.CompareTag("VR")) )
        {
            HitByCharge();
            Destroy(other);
            if (Life == 0)
            {
                Respawn();
            }
        }
    }

    public void HitByCharge()
    {
        Life--;
    }

    public void Respawn()
    {
        gameObject.transform.position = spawnPoints[RandomSpawn()].position;
        ResetLifePoints();
    }

    public void GetSpawners()
    {
        foreach (Transform spawn in SpawnerContainer.GetComponentsInChildren<Transform>())
        {
            spawnPoints.Add(spawn);
        }
    }

    public int RandomSpawn()
    {
        return Random.Range(1, spawnPoints.Count-1);
    }

    public void ResetLifePoints()
    {
        Life = GameConfig.GetInstance().LifeNumber;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Life);
        }
        else
        {
            Life = (int)stream.ReceiveNext();
        }
    }
}
