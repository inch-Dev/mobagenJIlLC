using UnityEngine;

public class Mouse : MonoBehaviour,  IStateable
{
    public void HandleState(GameState state)
    {
        switch(state)
        {
            case GameState.CREATE:
                canClick = true;
                break;
            case GameState.PLAY:
                canClick = true;
                break;
        }
    }

    bool canClick = false;

    // Update is called once per frame
    void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
            Click();
    }

    void Click()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if(hit.collider != null && hit.collider.gameObject.GetComponent<Agent>())
        {
                if (hit.collider.gameObject.GetComponent<Agent>().GetStateMachine().GetSnapshotState() is Alive)
                {
                hit.collider.gameObject.GetComponent<Agent>().SetRenderer(false);
				    hit.collider.gameObject.GetComponent<Agent>().GetComponent<StateMachine>().SetCurrentState(hit.collider.gameObject.GetComponent<Dead>());
				hit.collider.gameObject.GetComponent<Agent>().GetComponent<StateMachine>().SetSnapshotState(hit.collider.gameObject.GetComponent<Dead>());
				Debug.Log("Setting state to dead");
                }
                else if (hit.collider.gameObject.GetComponent<Agent>().GetStateMachine().GetSnapshotState() is Dead)
                {
                hit.collider.gameObject.GetComponent<Agent>().SetRenderer(true);
				    hit.collider.gameObject.GetComponent<Agent>().GetComponent<StateMachine>().SetCurrentState(hit.collider.gameObject.GetComponent<Alive>());
                    hit.collider.gameObject.GetComponent<Agent>().GetComponent<StateMachine>().SetSnapshotState(hit.collider.gameObject.GetComponent<Alive>());
                Debug.Log("Setting state to alive");
                }
		}
    }
}
