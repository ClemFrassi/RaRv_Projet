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
    public GameObject Scientific;
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

    public void HitByCharge()
    {
        if(photonView.IsMine)
        {
            Debug.Log("clem hit by charge!");
            Life--;
            if (Life <= 0)
            {
                Respawn();
            }
        }
        
    }

    public void Respawn()
    {
        if(photonView.IsMine)
        {
            Scientific.SetActive(false);
            Scientific.transform.position = spawnPoints[RandomSpawn()].position;
            ResetLifePoints();
            Scientific.SetActive(true);
        }
        
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
