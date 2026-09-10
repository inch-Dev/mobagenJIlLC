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
                canClick = false;
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
            switch (hit.collider.gameObject.GetComponent<Agent>().GetStateMachine().GetCurrentState())
            {
                case Alive:
					hit.collider.gameObject.GetComponent<Agent>().GetComponent<StateMachine>().SetCurrentState(hit.collider.gameObject.GetComponent<Dead>());
                    hit.collider.gameObject.GetComponent<Agent>().SetAliveRenderer(false);
                    hit.collider.gameObject.GetComponent<Agent>().SetDeadRenderer(true);
					break;
                case Dead:
					hit.collider.gameObject.GetComponent<Agent>().GetComponent<StateMachine>().SetCurrentState(hit.collider.gameObject.GetComponent<Alive>());
                    hit.collider.gameObject.GetComponent <Agent>().SetDeadRenderer(false);
                    hit.collider.gameObject.GetComponent<Agent>().SetAliveRenderer(true);
					break;
            }
		}
    }
}
