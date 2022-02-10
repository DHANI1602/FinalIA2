using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour, IReminder
{

    public PlayerModel _playerModel;

    private float timer;
    private float timer1;

    private float ultiTimer;
    private float currentultimer;

    private bool isFiring;
    private bool isCrashed;

    private Rigidbody2D _rb;
    public Vector2 dir;

    public ModifiedObjectPooler<Bullet> _bulletPool;
    public ModifiedObjectPooler<ExplosiveBullet> _explosiveBulletPool;

    private Memento<PlayerSnapshot> _memento = new Memento<PlayerSnapshot>();

    public void Save()
    {
        var data = new PlayerData();
        data.position = new SerializableVector3(transform.position);
        data.rotation = new SerializableVector4(transform.rotation);

        BinarySerializer.SaveBinary(data, $"{Application.dataPath}\\PlayerData.dat");
    }
    public void Load()
    {
        var data = BinarySerializer.LoadBinary<PlayerData>($"{Application.dataPath}\\PlayerData.dat");

        transform.position = new Vector3(data.position.x, data.position.y, data.position.z);
        transform.rotation = new Quaternion(data.rotation.x, data.rotation.y, data.rotation.z, data.rotation.w);
    }
    private void Awake()
    {
        var factory = new BulletFactory();
        _bulletPool = new ModifiedObjectPooler<Bullet>(factory.Create, Bullet.TurnOn, Bullet.TurnOff, 12);

        var explosiveBulletfactory = new ExplosiveFactory();
        _explosiveBulletPool = new ModifiedObjectPooler<ExplosiveBullet>(explosiveBulletfactory.Create, Bullet.TurnOn, Bullet.TurnOff, 2);


    }

    void Start()
    {
        StartToRecord();
        _playerModel.WH = GetComponent<WeaponHandler>();
        _rb = GetComponent<Rigidbody2D>();
        var memento = FindObjectOfType<MementoManager>();
        if (memento != null)
        {
            MementoManager.instance.Add(this);

        }
    }

    void Update()
    {
        dir = _playerModel.fireposition.position - transform.position;
        if (MainMenu.GameIsPaused == false)
        {

            if (Input.GetMouseButtonDown(0) & timer <= 0 && isFiring == false)
            {
                Fire();
                timer = _playerModel.timetoshoot;

            }
            else if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            if (Input.GetMouseButton(1) && timer1 <= 0)
            {

                _playerModel.WH._currentWeapon.Throw();
                timer1 = _playerModel.timetoshoot1;
                isFiring = true;

            }

            else if (Input.GetMouseButtonUp(1))
            {
                _playerModel.laser.SetActive(false);

                isFiring = false;


            }
            else if (timer1 > 0)
            {
                timer1 -= Time.deltaTime;
            }
            else if(Input.GetKeyDown(KeyCode.Space) && currentultimer <= 0)
            {
                Explote();
                currentultimer = ultiTimer;
            }
        }
        currentultimer -= Time.deltaTime;


    }

    private void FixedUpdate()
    {
        float inputVer = Input.GetAxis("Vertical");
        float inputHor = Input.GetAxis("Horizontal");
        _rb.AddRelativeForce(Vector2.up * inputVer);
        transform.Rotate(Vector3.forward * inputHor * Time.deltaTime * _playerModel.turnInput);


        if (Input.GetKey(KeyCode.W))
        {
            for (int i = 0; i < _playerModel.motorfire.Length; i++)
            {
                _playerModel.motorfire[i].SetActive(true);
            }
        }
        else
        {
            _playerModel.motorfire[0].SetActive(false);
            _playerModel.motorfire[1].SetActive(false);
        }

        StartToRecord();

    }

    //MEMENTO
    public void MakeSnapshot()
    {
        var snapshot = new PlayerSnapshot();
        snapshot.pos = transform.position;
        snapshot.rot = transform.rotation;

        _memento.Record(snapshot);
    }

    public void Rewind()
    {
        if (!_memento.CanRemember()) return;

        var snapshot = _memento.Remember();

        transform.position = snapshot.pos;
        transform.rotation = snapshot.rot;
    }

    public IEnumerator StartToRecord()
    {
        while (true)
        {
            MakeSnapshot();
            yield return new WaitForSeconds(1);
        }
    }

    private void Fire()
    {
        var bullet = _bulletPool.Get();
        bullet.pool = _bulletPool;
        bullet.transform.position = _playerModel.fireposition.transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.transform.up = transform.up;
    }
    private void Explote()
    {
        // List<Enemy> enemies = new List<Enemy>();
         List<Asteroid> asteroids = new List<Asteroid>();
        List<Asteroid> astToPush = new List<Asteroid>();
        var elements = _playerModel.query.Query()
                            .OfType<Asteroid>()
                            .Where(n => Vector3.Distance(transform.position, n.Position) < _playerModel.query.radius / 2);
        var pushed = _playerModel.query.Query()
                         .OfType<Asteroid>()
                         .Where(n => Vector3.Distance(transform.position, n.Position) > _playerModel.query.radius / 2);

        foreach (var item in elements)
        {
            asteroids.Add(item);
            item.Die();
        }
        foreach (var item in pushed)
        {
            astToPush.Add(item);
        }

        Debug.Log(asteroids.Count);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 && isCrashed == false || collision.gameObject.layer == 13)
        {
            EventManager.Trigger("LifeLoss");
            StartCoroutine(Crashing());
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 14)
        {
            EventManager.Trigger("LifeLoss");
            StartCoroutine(Crashing());
        }

        if (collision.gameObject.layer == 10)
        {
            collision.GetComponent<IDecorator>().Execute();
        }
    }

    IEnumerator Crashing()
    {
        isCrashed = true;

        yield return new WaitForSeconds(0.5f);

        isCrashed = false;
    }

    public bool GetisFiring()
    {
        return isFiring;
    }

    public float GetTimer()
    {
        return timer;
    }

    public float GetTimer1()
    {
        return timer1;
    }
}