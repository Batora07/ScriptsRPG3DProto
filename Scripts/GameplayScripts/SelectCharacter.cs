using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
	public CircleNPC circleNPC;
	public Color playerCircleColor;
	public Color npcCircleColor;
	public Color lootCircleColor;
	public Color enemyCircleColor;
	[SerializeField]
	private int instanceID;

	public LivingObjectStatus status;

	public EntityStatus entityStatus;

	public void Awake()
	{
		circleNPC = GetComponent<CircleNPC>();
		instanceID = entityStatus.entityInfos.UID;
	}

	public void Start()
	{
		UnselectEntity();
	}

	private void Update()
	{
		CheckSelected();
	}

	private void CheckSelected()
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if(hit)
			{
				// select circle component and select component not null
				if(hitInfo.transform.GetComponent<CircleNPC>() != null && hitInfo.transform.GetComponent<SelectCharacter>() != null)
				{
					// the status of the selected entity is the same as this object
					if(status == hitInfo.transform.GetComponent<SelectCharacter>().status)
					{
						int instanceIDHit = hitInfo.transform.GetComponent<SelectCharacter>().entityStatus.entityInfos.UID;
						// same instance id as the current object
						if(instanceID == instanceIDHit)
						{
							
							hitInfo.transform.GetComponent<SelectCharacter>().circleNPC.projector.gameObject.SetActive(true);
							circleNPC.SetColorRadius(hitInfo.transform.GetComponent<SelectCharacter>().
							GetColorByStatus(hitInfo.transform.GetComponent<SelectCharacter>().status));
							DisplayEntityUnitFrame(hitInfo.transform.GetComponent<SelectCharacter>().entityStatus.entityInfos);
						}
						// same entity status but not the same object
						else
						{
							//Debug.Log("cas 2");
							UnselectEntity();
							SelectCharacter _selectChar = hitInfo.transform.GetComponent<SelectCharacter>();
							EntityStatus newChar = _selectChar.entityStatus;
						
							CircleNPC _circleNPC = hitInfo.transform.GetComponent<SelectCharacter>().circleNPC;
							
							_circleNPC.projector.gameObject.SetActive(true);
							_circleNPC.SetColorRadius(_selectChar.GetColorByStatus(_selectChar.status));
							DisplayEntityUnitFrame(_selectChar.entityStatus.entityInfos);
							DisplayPanel();
						}
					}
					else // not the same status for the selected object than this object
					{	
						EntityStatus newChar = hitInfo.transform.GetComponent<SelectCharacter>().entityStatus;
						if(entityStatus.entityInfos.UID != newChar.entityInfos.UID)
						{
							UnselectEntity();
						}
					}
					EnemyUnitFrameManager.instance.panelUI.gameObject.SetActive(false);
					EnemyUnitFrameManager.instance.SetupEntityInfos(EnemyUnitFrameManager.instance.entityInfos);
					DisplayPanel();
				}
				else // SELECT VOID
				{
					// not clicking on UI ? then unselect the entity
					if(!EventSystem.current.IsPointerOverGameObject())
						UnselectEntity();
				}
			}
			else
			{
				// not clicking on UI ? then unselect the entity
				if(!EventSystem.current.IsPointerOverGameObject())
					UnselectEntity();
			}
		}
	}

	private Color GetColorByStatus(LivingObjectStatus _status)
	{
		switch(_status)
		{
			case LivingObjectStatus.player:
				return playerCircleColor;

			case LivingObjectStatus.enemy:
				return enemyCircleColor;

			case LivingObjectStatus.npc:
				return npcCircleColor;

			case LivingObjectStatus.loot:
				return lootCircleColor;
		}

		return Color.black;
	}

	public void DisplayEntityUnitFrame(EntityInfos infos)
	{
		EnemyUnitFrameManager.instance.SetupEntityInfos(infos);
		EnemyUnitFrameManager.instance.SetHealthBarColor(status);
		EnemyUnitFrameManager.instance.EnablePanel();
	}

	public void DisplayPanel()
	{
		EnemyUnitFrameManager.instance.EnablePanel();
		EnemyUnitFrameManager.instance.SetCharacterTypePlayer();
	}

	public void UnselectEntity()
	{
		circleNPC.projector.gameObject.SetActive(false);
		EnemyUnitFrameManager.instance.panelUI.gameObject.SetActive(false);
	}
}
