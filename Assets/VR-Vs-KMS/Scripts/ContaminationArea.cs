using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace vr_vs_kms
{
    public class ContaminationArea : MonoBehaviour
    {
        [System.Serializable]
        public struct BelongToProperties
        {
            public Color mainColor;
            public Color secondColor;
            
        }

        public BelongToProperties nobody;
        public BelongToProperties virus;
        public BelongToProperties scientist;

        private IEnumerator coroutine;

        private List<GameObject> playerList;

        private float faerieSpeed;
        public float cullRadius = 5f;

        private float radius = 1f;
        private ParticleSystem pSystem;
        private WindZone windZone;
        private int remainingGrenades;
        public float inTimer = 0f;
        private CullingGroup cullGroup;
        public int state;

        void Start()
        {
            populateParticleSystemCache();
            setupCullingGroup();

            BelongsToNobody();
            playerList = new List<GameObject>();
        }

        private void populateParticleSystemCache()
        {
            pSystem = this.GetComponentInChildren<ParticleSystem>();
        }


        /// <summary>
        /// This manage visibility of particle for the camera to optimize the rendering.
        /// </summary>
        private void setupCullingGroup()
        {
            Debug.Log($"setupCullingGroup {Camera.main}");
            cullGroup = new CullingGroup();
            cullGroup.targetCamera = Camera.main;
            cullGroup.SetBoundingSpheres(new BoundingSphere[] { new BoundingSphere(transform.position, cullRadius) });
            cullGroup.SetBoundingSphereCount(1);
            cullGroup.onStateChanged += OnStateChanged;
        }

        void OnStateChanged(CullingGroupEvent cullEvent)
        {
            Debug.Log($"cullEvent {cullEvent.isVisible}");
            if (cullEvent.isVisible)
            {
                pSystem.Play(true);
            }
            else
            {
                pSystem.Pause();
            }
        }

        void OnTriggerEnter(Collider coll)
        {
            if((coll.gameObject.tag=="KMS" && coll.gameObject.name == "Ch11") || (coll.gameObject.tag == "VR" && coll.gameObject.name == "VRCamera"))
            {
                if(playerList.Count == 0)
                {
                    coroutine = CapturingZone();
                    StartCoroutine(coroutine);
                }
                playerList.Add(coll.gameObject);
            }
        }

        private void OnTriggerStay(Collider coll)
        {
            Debug.Log("Luc");
            if ((coll.gameObject.tag == "KMS" && coll.gameObject.name == "Ch11") || (coll.gameObject.tag == "VR" && coll.gameObject.name == "VRCamera"))
            {
                Debug.Log("Dylan");
                if (coll.gameObject.GetComponent<PlayerBehaviour>().isDead)
                {
                    Debug.Log("Clem Exit Zone");
                    int id = playerList.IndexOf(coll.gameObject);
                    if (id == 0)
                    {
                        StopCoroutine(coroutine);
                    }
                    playerList.Remove(coll.gameObject);
                    if (playerList.Count > 0)
                    {
                        coroutine = CapturingZone();
                        StartCoroutine(coroutine);
                    }
                }
            }
        }

        void OnTriggerExit(Collider coll)
        {
            if ((coll.gameObject.tag == "KMS" && coll.gameObject.name == "Ch11") || (coll.gameObject.tag == "VR" && coll.gameObject.name == "VRCamera"))
            {
                Debug.Log("Exit Zone");
                int id = playerList.IndexOf(coll.gameObject);
                if (id == 0)
                {
                    StopCoroutine(coroutine);
                }
                playerList.Remove(coll.gameObject);
                if (playerList.Count > 0)
                {
                    coroutine = CapturingZone();
                    StartCoroutine(coroutine);
                }
            }
        }

        void Update()
        {
        }

        public IEnumerator CapturingZone()
        {
            yield return new WaitForSeconds(GameConfig.GetInstance().TimeToAreaContamination);
            if (playerList[0].tag == "KMS")
            {
                BelongsToScientists();
            }else if (playerList[0].tag == "VR")
            {
                BelongsToVirus();
            }
        }

        private void ColorParticle(ParticleSystem pSys, Color mainColor, Color accentColor)
        {
            // TODO: Solution to color particle 
            ParticleSystem.MainModule pMain = pSys.main;
            ParticleSystem.MinMaxGradient colors = new ParticleSystem.MinMaxGradient(mainColor, accentColor);
            colors.mode = ParticleSystemGradientMode.TwoColors;
            pMain.startColor = colors;
        }

        public void BelongsToNobody()
        {
            ColorParticle(pSystem, nobody.mainColor, nobody.secondColor);
            state = 0;
        }

        public void BelongsToVirus()
        {
            ColorParticle(pSystem, virus.mainColor, virus.secondColor);
            state = 1;
        }

        public void BelongsToScientists()
        {
            ColorParticle(pSystem, scientist.mainColor, scientist.secondColor);
            state = 2;
        }

        void OnDestroy()
        {
            if (cullGroup != null)
                cullGroup.Dispose();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, cullRadius);
        }
    }
}