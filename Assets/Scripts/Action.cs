using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction : ScriptableObject
{
    public bool enable = true;                      
    public bool destroy = false;                    
    public GameObject gameobject;                   
    public Transform transform;                     
    public ISSActionCallback callback;              

    protected GameAction() { }

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }
    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}


public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
	void SSActionEvent(GameAction source, SSActionEventType events = SSActionEventType.Competeted,
		int intParam = 0, string strParam = null, Object objectParam = null);
}

public class SSActionManager : MonoBehaviour, ISSActionCallback
{
	private Dictionary<int, GameAction> actions = new Dictionary<int, GameAction>();    
	private List<GameAction> waitingAdd = new List<GameAction>();                       
	private List<int> waitingDelete = new List<int>();                        

	protected void Update()
	{
		foreach (GameAction ac in waitingAdd)
		{
			actions[ac.GetInstanceID()] = ac;                                    
		}
		waitingAdd.Clear();

		foreach (KeyValuePair<int, GameAction> kv in actions)
		{
			GameAction ac = kv.Value;
			if (ac.destroy)         
			{
				waitingDelete.Add(ac.GetInstanceID());
			}
			else if (ac.enable)
			{
				ac.Update();
			}
		}

		foreach (int key in waitingDelete)
		{
			GameAction ac = actions[key];
			actions.Remove(key);
			Destroy(ac);
		}
		waitingDelete.Clear();
	}

	public void RunAction(GameObject gameobject, GameAction action, ISSActionCallback manager)
	{
		action.gameobject = gameobject;
		action.transform = gameobject.transform;
		action.callback = manager;
		waitingAdd.Add(action);
		action.Start();
	}

	public void SSActionEvent(GameAction source, SSActionEventType events = SSActionEventType.Competeted,
		int intParam = 0, string strParam = null, Object objectParam = null)
	{
	}
}
public class FlyActionManager : SSActionManager{
	public UFOFlyAction fly;
	public Controllor scene;
	protected void Start(){
		scene = (Controllor)GameDirector.GetInstance ().CurrentSceneControllor;
		scene.fam = this;
	}

	public void UFOfly(GameObject disk, float angle, float power){
		fly = UFOFlyAction.GetSSAction (disk.GetComponent<DiskData> ().direction, angle, power);
		this.RunAction (disk, fly, this);
	}
}
public class UFOFlyAction : GameAction
{
	public float gravity = -1.7f;                                
	private Vector3 start_vector;                              
	private Vector3 gravity_vector = Vector3.zero;             
	private float time;                                        
	private Vector3 current_angle = Vector3.zero;               

	private UFOFlyAction() { }
	public static UFOFlyAction GetSSAction(Vector3 direction, float angle, float power)
	{
		
		UFOFlyAction action = CreateInstance<UFOFlyAction>();
		if (direction.x == -1)
		{
			action.start_vector = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
		}
		else
		{
			action.start_vector = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
		}
		return action;
	}

	public override void Update()
	{
		
		time += Time.fixedDeltaTime;
		gravity_vector.y = gravity * time;
		transform.position += (start_vector + gravity_vector) * Time.fixedDeltaTime;
		current_angle.z = Mathf.Atan((start_vector.y + gravity_vector.y) / start_vector.x) * Mathf.Rad2Deg;
		transform.eulerAngles = current_angle;


		if (this.transform.position.y < -10)
		{
			this.destroy = true;
			this.callback.SSActionEvent(this);      
		}
	}

	public override void Start() { }
}