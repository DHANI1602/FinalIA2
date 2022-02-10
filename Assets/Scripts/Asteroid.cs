using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour, IReminder, IGridEntity
{
    public float maxThrust;
    public float maxTorque;
    public Rigidbody2D rb;
    public float speed = 5;
    public int Size;
    public ParticleSystem PS;
    public ModifiedObjectPooler<Asteroid> pool;
    public AsteroidSpawner _asteroidSpawner;
    public int score;
    private LookUpTable<int, int> lookUpTablex4;
    private LookUpTable<int, int> lookUpTablex3;

    SpatialGrid grid;

    float lerpDuration = 1;

    AsteroidSnapshot snapshot;
    private Memento<AsteroidSnapshot> _memento = new Memento<AsteroidSnapshot>();

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public event Action<IGridEntity> OnMove;

    public Asteroid SetMaxThrust(float newThrust)
    {
        maxThrust = newThrust;
        return this;
    }

    public Asteroid SetMaxTorque(float newTorque)
    {
        maxTorque = newTorque;
        return this;
    }
    public void Save()
    {
        var data = new AsteroidData();
        data.position = new SerializableVector3(transform.position);
        data.maxThrust = maxThrust;
        data.maxTorque = maxTorque;
        data.Size = Size;

        BinarySerializer.SaveBinary(data, $"{Application.dataPath}\\Resources\\AsteroidData");
    }
    public void Load()
    {
        var data = BinarySerializer.LoadBinary<AsteroidData>($"{Application.dataPath}\\Resources\\AsteroidData");

        transform.position = new Vector3(data.position.x, data.position.y, data.position.z);
        maxThrust = data.maxThrust;
        maxTorque = data.maxTorque;
        Size = data.Size;
    }
    private void Awake()
    {
        MementoManager.instance.Add(this);

        lookUpTablex3 = new LookUpTable<int, int>(lUP => lUP * 3);
        lookUpTablex4 = new LookUpTable<int, int>(lUP => lUP * 4);

        score = AsteroidFlyweightPointer.asteroidflyweight.scoreflyweight;
    }

    void Start()
    {
        grid = FindObjectOfType<SpatialGrid>();
        rb = GetComponent<Rigidbody2D>();
        _asteroidSpawner = FindObjectOfType<AsteroidSpawner>();

    }
    private void OnEnable()
    {
        Vector2 thrust = new Vector2(Random.Range(-maxThrust, maxThrust), Random.Range(-maxThrust, maxThrust));
        float torque = Random.Range(-maxTorque, maxTorque);
        rb.AddForce(thrust);
            transform.Rotate(Vector3.right * torque * Time.deltaTime);
        MakeSnapshot();
        StartToRecord();
    }

    /*
    private int Multiplierx3(int a)
    {
        return a * 3;
    }
    private int Multiplierx4(int a)
    {
        return a * 4;
    }
    FUNCIONES REEMPLAZADAS POR LAMBDAS
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 8)
        {
            SetSize();
        }
    }
    public void SetSize()
    {
        var lookUpTablex3 = new LookUpTable<int, int>(lUP => lUP * 3);

            if (Size == 3)
            {
                EventManager.Trigger("SendScore", score);
                for (int i = 0; i < 2; i++)
                {
                    var asteroid = _asteroidSpawner._mediumAsteroidPool.Get();
                    asteroid.pool = _asteroidSpawner._mediumAsteroidPool;
                    asteroid.transform.position = transform.position;
                    asteroid.transform.SetParent(grid.transform);

                }

            }
            else if (Size == 2)
            {
                var newscore2 = lookUpTablex3.Get(score);
                EventManager.Trigger("SendScore", newscore2);

                var asteroid = _asteroidSpawner._smallAsteroidPool.Get();
                asteroid.pool = _asteroidSpawner._smallAsteroidPool;
                asteroid.transform.position = transform.position;
                asteroid.transform.SetParent(grid.transform);

        }
            else
            {
                var newscore3 = lookUpTablex4.Get(score);
                EventManager.Trigger("SendScore", newscore3);


            }

            ParticleSystem obj = Instantiate(PS, transform.position, transform.rotation);
            obj.Play();
            Destroy(obj.gameObject, 2f);
            Die();
        
    }
    public static void TurnOn(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
    }
    public static void TurnOff(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
    }
    public void Die()
    {
        pool.ReturnToPool(this);
    }

    public void MakeSnapshot()
    {
        snapshot = new AsteroidSnapshot();
        snapshot.pos = transform.position;

        _memento.Record(snapshot);
    }

    public void Rewind()
    {
        if (!_memento.CanRemember()) return;

         snapshot = _memento.Remember();

        if(this.isActiveAndEnabled)
        StartCoroutine(LerpPosition());
    }

    public IEnumerator StartToRecord()
    {
        while (true)
        {
            MakeSnapshot();
            yield return new WaitForSeconds(2f);
        }

    }

    IEnumerator LerpPosition()
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(transform.position, snapshot.pos, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = snapshot.pos;
    }


}
