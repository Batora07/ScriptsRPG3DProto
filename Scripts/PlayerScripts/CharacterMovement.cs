using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	private MovementMotor motor;

	public float move_Magnitude = 0.05f;
	public float speed = 0.7f;
	public float speed_Move_WhileAttack = 0.1f;
	public float speed_Attack = 1.5f;
	public float turnSpeed = 10f;
	public float speed_Jump = 20f;
	public float animJump = 4f;

	private float speed_Move_Multiplier = 1f;

	private Vector3 direction;

	private Animator anim;
	private Camera mainCamera;
	private PlayerHealth playerHealth;

	[SerializeField]
	private string attackIdentifier;
	[SerializeField]
	private string deathIdentifier;
	[SerializeField]
	private string hitIdentifier;
	[SerializeField]
	private string slashIdentifier;
	[SerializeField]
	private string bodyHitIdentifier;

	[SerializeField]
	private List<AudioClip> attackSounds = new List<AudioClip>();
	[SerializeField]
	private List<AudioClip> deathSounds = new List<AudioClip>();
	[SerializeField]
	private List<AudioClip> hitSounds = new List<AudioClip>();
	[SerializeField]
	private List<AudioClip> slashSounds = new List<AudioClip>();
	[SerializeField]
	private List<AudioClip> bodyHitSounds = new List<AudioClip>();

	private static string PARAMETER_STATE = "State";
	private static string PARAMETER_ATTACK_TYPE = "AttackType";
	private static string PARAMETER_ATTACK_INDEX = "AttackIndex";

	public AttackAnimation[] attack_Animations;
	public string[] combo_AttackList;
	public int combo_Type;

	private int attack_Index = 0;
	private string[] combo_List;
	private int attack_Stack;
	private float attack_Stack_TimeTemp;

	private bool isAttacking;
	private bool isJumping = false;
	private bool stopMotor = false;

	private GameObject atkPoint;
	[SerializeField]
	private AudioSource voiceAudio;
	[SerializeField]
	private AudioSource sfxAudio;

	public GameObject fireTornado;

	void Awake()
	{
		motor = GetComponent<MovementMotor>();
		anim = GetComponent<Animator>();
		playerHealth = GetComponent<PlayerHealth>();
	}

	void OnEnable()
	{
		PlayerAttack.damageDealtToEnemy += DamageDealtToEnemy;
		PlayerHealth.deathEvent += StopMotoring;
	}

	void OnDisable()
	{
		PlayerAttack.damageDealtToEnemy -= DamageDealtToEnemy;
		PlayerHealth.deathEvent -= StopMotoring;
	}

	void Start () {
		anim.applyRootMotion = false;

		mainCamera = Camera.main;

		atkPoint = GameObject.Find("Player Attack Point");
		atkPoint.SetActive(false);
		SetupAudioLists();
	}

	// Update is called once per frame
	void Update()
	{
		if(!stopMotor)
		{
			HandleAttackAnimations();

			if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				Skill_1();
			}

				/*if(Input.GetButtonDown("Fire2"))
				{
					Attack();
					StartCoroutine(SpecialAttack());
				}*/

			MovementAndJumping();
		}
	}

	private Vector3 MoveDirection
	{
		get { return direction; }

		set {
			direction = value * speed_Move_Multiplier;

			if (direction.magnitude > 0.1f)
			{
				var newRotation = Quaternion.LookRotation(direction);
				transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
			}

			direction *= speed * (Vector3.Dot(transform.forward, direction) + 1f) * 5f;
			motor.Move(direction);

			AnimationMove(motor.charController.velocity.magnitude * 0.1f);
		}
	}

	public Animator Anim
	{
		get
		{
			return anim;
		}

		set
		{
			anim = value;
		}
	}

	public MovementMotor Motor
	{
		get
		{
			return motor;
		}

		set
		{
			motor = value;
		}
	}

	public static string PARAMETER_STATE1
	{
		get
		{
			return PARAMETER_STATE;
		}

		set
		{
			PARAMETER_STATE = value;
		}
	}

	public static string PARAMETER_ATTACK_TYPE1
	{
		get
		{
			return PARAMETER_ATTACK_TYPE;
		}

		set
		{
			PARAMETER_ATTACK_TYPE = value;
		}
	}

	public static string PARAMETER_ATTACK_INDEX1
	{
		get
		{
			return PARAMETER_ATTACK_INDEX;
		}

		set
		{
			PARAMETER_ATTACK_INDEX = value;
		}
	}

	public List<AudioClip> AttackSounds
	{
		get
		{
			return attackSounds;
		}

		set
		{
			attackSounds = value;
		}
	}

	public List<AudioClip> DeathSounds
	{
		get
		{
			return deathSounds;
		}

		set
		{
			deathSounds = value;
		}
	}

	public List<AudioClip> HitSounds
	{
		get
		{
			return hitSounds;
		}

		set
		{
			hitSounds = value;
		}
	}

	public AudioSource VoiceAudio
	{
		get
		{
			return voiceAudio;
		}

		set
		{
			voiceAudio = value;
		}
	}

	public AudioSource SfxAudio
	{
		get
		{
			return sfxAudio;
		}

		set
		{
			sfxAudio = value;
		}
	}

	public List<AudioClip> SlashSounds
	{
		get
		{
			return slashSounds;
		}

		set
		{
			slashSounds = value;
		}
	}

	public List<AudioClip> BodyHitSounds
	{
		get
		{
			return bodyHitSounds;
		}

		set
		{
			bodyHitSounds = value;
		}
	}

	void Moving(Vector3 dir, float mult)
	{
		if(isAttacking)
		{
			speed_Move_Multiplier = speed_Move_WhileAttack * mult;
		}
		else if(isJumping)
		{
			speed_Move_Multiplier = speed_Move_WhileAttack * mult;
		}
		else
		{
			speed_Move_Multiplier = 1 * mult;
		}
		MoveDirection = dir;
	}

	void Jump()
	{
		StartCoroutine(Jumping());
	}

	void AnimationMove(float magnitude)
	{
		// Are we moving the character ?
		if (magnitude > move_Magnitude)
		{
			float speed_Animation = magnitude * 2f;

			if(speed_Animation < 1f)
			{
				speed_Animation = 1f;
			}
			// set the movement anim
			if(anim.GetInteger(PARAMETER_STATE) != 2)
			{
				anim.SetInteger(PARAMETER_STATE, 1);
				anim.speed = speed_Animation;
			}
		} else
		{
			// set the idle anim
			if(anim.GetInteger(PARAMETER_STATE) != 2 && anim.GetInteger(PARAMETER_STATE) != 4)
			{
				anim.SetInteger(PARAMETER_STATE, 0);
			}
		}
	}

	void MovementAndJumping()
	{
		Vector3 moveInput = Vector3.zero;
		// rotate the player before moving
		Vector3 forward = Quaternion.AngleAxis(-90, Vector3.up) * mainCamera.transform.right;

		// will return 0 / 1 if Right, or 0 / -1 if Left
		moveInput += forward * Input.GetAxis("Vertical");
		moveInput += mainCamera.transform.right * Input.GetAxis("Horizontal");

		moveInput.Normalize();
		if(!isJumping)
		{
			Moving(moveInput.normalized, 1f);
		}

		if(!isJumping)
		{
			if(Input.GetKey(KeyCode.Space))
			{
				Jump();
			}
		}
	}

	void StopMotoring()
	{
		stopMotor = true;
	}

	void FightAnimation()
	{
		if (combo_List!=null && attack_Index >= combo_List.Length)
		{
			ResetCombo();
		}

		if (combo_List != null && combo_List.Length > 0)
		{
			// convert the string type from the combo list as an int
			int motionIndex = int.Parse(combo_List[attack_Index]);

			// prevent an array out of bound
			if (motionIndex < attack_Animations.Length)
			{
				anim.SetInteger(PARAMETER_STATE, 2);
				anim.SetInteger(PARAMETER_ATTACK_TYPE, combo_Type);
				anim.SetInteger(PARAMETER_ATTACK_INDEX, attack_Index);
			}
		}
	}

	void ResetCombo()
	{
		attack_Index = 0;
		attack_Stack = 0;
		isAttacking = false;
	}

	void HandleAttackAnimations()
	{
		if (Time.time > attack_Stack_TimeTemp + 0.5f)
		{
			attack_Stack = 0;
		}

		// split the string array to get the "int" split by comma 
		// delimiter to store them in the comboList as an other array
		combo_List = combo_AttackList[combo_Type].Split("," [0]);

		if (anim.GetInteger(PARAMETER_STATE) == 2){
			anim.speed = speed_Attack;

			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

			if (stateInfo.IsTag("Attack"))
			{
				int motionIndex = int.Parse(combo_List[attack_Index]);

				// if the animation gets to its end
				if (stateInfo.normalizedTime > 0.9f)
				{
					anim.SetInteger(PARAMETER_STATE, 0);

					isAttacking = false;
					isJumping = false;
					attack_Index++;

					if (attack_Stack > 1)
					{
						FightAnimation();
					} else
					{
						if (attack_Index >= combo_List.Length)
						{
							ResetCombo();
						}
					}
				}
			}
		}
	}

	public void Skill_1()
	{
		// set the current skill as this one
		float cooldownSkill = GetComponent<PlayerStatus>().skills.skillsList[0].cooldown;
		SkillUIManager.instance.skillsUI[0].DisplayCooldownSkill(cooldownSkill);

		// semaphore to prevent attacking two times in a row
		if(isAttacking)
			return;

		StartCoroutine(WaitForEndAttack(cooldownSkill));

		SoundManager.instance.RandomizeSfx(VoiceAudio, attackSounds);
		if(attack_Stack < 1 ||
			(Time.time > attack_Stack_TimeTemp + 0.2f && Time.time < attack_Stack_TimeTemp + 1f))
		{
			attack_Stack++;
			attack_Stack_TimeTemp = Time.time;
		}
		FightAnimation();
	}

	void Attack_Began()
	{
		atkPoint.SetActive(true);
	}

	void Attack_End()
	{
		atkPoint.SetActive(false);
	}

	IEnumerator SpecialAttack()
	{
		yield return new WaitForSeconds(0.4f);

		Instantiate(fireTornado, transform.position + transform.forward * 2.5f, Quaternion.identity);
	}


	private IEnumerator Jumping()
	{
		motor.Stop();
		anim.SetInteger(PARAMETER_STATE, 4);
		isJumping = true;
		anim.SetBool("isJumping", isJumping);
		motor.Jump(speed_Jump);
		yield return new WaitForSeconds(animJump);
		isJumping = false;
		anim.SetBool("isJumping", isJumping);
		motor.Stop();
	}

	void SetupAudioLists()
	{
		List<Sounds> soundsList = new List<Sounds>();

		SceneName nameScene = SceneName.Common;
		int nbSoundsPlaylist = SoundManager.instance.soundObject.sounds.Length;

		for(int i = 0; i < nbSoundsPlaylist; ++i)
		{
			if(SoundManager.instance.soundObject.sounds[i].scene == nameScene)
			{
				int nbSoundsList = SoundManager.instance.soundObject.sounds[i].sounds.Length;
				for(int j = 0; j < nbSoundsList; ++j)
				{
					if(SoundManager.instance.soundObject.sounds[i].sounds[j].name.Contains(attackIdentifier))
					{
						AttackSounds.Add(SoundManager.instance.soundObject.sounds[i].sounds[j].trackFile);
					}
					else if(SoundManager.instance.soundObject.sounds[i].sounds[j].name.Contains(deathIdentifier))
					{
						DeathSounds.Add(SoundManager.instance.soundObject.sounds[i].sounds[j].trackFile);
					}
					else if(SoundManager.instance.soundObject.sounds[i].sounds[j].name.Contains(hitIdentifier))
					{
						HitSounds.Add(SoundManager.instance.soundObject.sounds[i].sounds[j].trackFile);
					}
				}
			}
			else if(SoundManager.instance.soundObject.sounds[i].scene == SceneName.SFX)
			{
				int nbSoundsList = SoundManager.instance.soundObject.sounds[i].sounds.Length;
				for(int j = 0; j < nbSoundsList; ++j)
				{
					if(SoundManager.instance.soundObject.sounds[i].sounds[j].name.Contains(slashIdentifier))
					{
						SlashSounds.Add(SoundManager.instance.soundObject.sounds[i].sounds[j].trackFile);
					}
					else if(SoundManager.instance.soundObject.sounds[i].sounds[j].name.Contains(bodyHitIdentifier))
					{
						BodyHitSounds.Add(SoundManager.instance.soundObject.sounds[i].sounds[j].trackFile);
					}
				}
			}
		}
	}

	public void DamageDealtToEnemy()
	{
		Debug.Log("damage dealth");
		SoundManager.instance.RandomizeSfx(sfxAudio, slashSounds);
	}

	public IEnumerator WaitForEndAttack(float cooldown)
	{
		isAttacking = true;
		yield return new WaitForSeconds(cooldown);
		isAttacking = false;
	}

} // class
