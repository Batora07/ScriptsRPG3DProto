using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAISystem : MonoBehaviour {

	private float moveMagnitude = 0.05f;
	public float movement_Speed = 0.5f;
	private float speed_Move_Muliplier = 1f;

	public float distance_Attack = 4.5f;
	public float distance_MoveTo = 13f;
	public float distance_OffsetPlayer = 2f;
	public float turnSpeed = 10f;
	public float patrolRange = 10f;

	private int ai_Time = 0;
	private int ai_State = 0;

	private Transform player_Target;
	private Vector3 movement_Position;

	private MovementMotor motor;

	private Animator anim;
	private string PARAMETER_RUN = "Run";
	private string PARAMETER_ATTACK_ONE = "Attack1";
	private string PARAMETER_ATTACK_TWO = "Attack2";
	private EnemyHealth health;

	[SerializeField]
	private string attackIdentifier;
	[SerializeField]
	private string deathIdentifier;
	[SerializeField]
	private string hitIdentifier;
	[SerializeField]
	private string slashIdentifier;
	[SerializeField]
	private string bodyHitSoundIdentifier;

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

	[SerializeField]
	private GameObject rightAttackPoint, leftAttackPoint;

	[SerializeField]
	private AudioSource voiceAudio;

	[SerializeField]
	private AudioSource sfxAudio;

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

	void Awake () {
		Anim = GetComponent<Animator>();
		motor = GetComponent<MovementMotor>();
		health = GetComponent<EnemyHealth>();
	}
	
	void Start()
	{
		SetupAudioLists();
	}

	void Update () {
		EnemyAI();
	}

	void EnemyAI()
	{
		if(!health.isDead)
		{
			float distance = Vector3.Distance(movement_Position, transform.position);
			Quaternion target_Rotation = Quaternion.LookRotation(movement_Position - transform.position);
			target_Rotation.x = 0f;
			target_Rotation.z = 0f;

			transform.rotation = Quaternion.Lerp(transform.rotation, target_Rotation,
				turnSpeed * Time.deltaTime);

			if(player_Target != null)
			{
				movement_Position = player_Target.position;

				if(ai_Time <= 0)
				{
					ai_State = Random.Range(0, 4);
					ai_Time = Random.Range(10, 100);
				}
				else
				{
					ai_Time--;
				}

				// Are we close enough to attack ?
				if(distance <= distance_Attack)
				{
					if(ai_State == 0)
					{
						motor.Stop();

						// sound attack randomized
						SoundManager.instance.RandomizeSfx(VoiceAudio, attackSounds);
						SoundManager.instance.RandomizeSfx(SfxAudio, slashSounds);
						Attack();
					}
				}
				// Patrolling 
				else
				{
					// moves towards the player
					if(distance <= distance_MoveTo)
					{
						transform.rotation = Quaternion.Lerp(transform.rotation, target_Rotation, turnSpeed * Time.deltaTime);
					}
					// patrol at random points
					else
					{
						player_Target = null;
						if(ai_State == 0)
						{
							ai_State = 1;
							ai_Time = Random.Range(10, 500);

							movement_Position = transform.position + new Vector3(Random.Range(-patrolRange, patrolRange), 0f, Random.Range(-patrolRange, patrolRange));
						}
					}
				}
			}
			else
			{
				GameObject target = GameObject.FindGameObjectWithTag("Player");

				if(target != null)
				{
					float targetDistance = Vector3.Distance(target.transform.position, transform.position);

					if(targetDistance <= distance_MoveTo ||
						targetDistance <= distance_Attack)
					{
						player_Target = target.transform;
					}
				}
				
				if(ai_State == 0)
				{
					ai_State = 1;
					ai_Time = Random.Range(10, 200);

					movement_Position = transform.position + new Vector3(Random.Range(-patrolRange, patrolRange), 0f, Random.Range(-patrolRange, patrolRange));
				}
				if(ai_Time <= 0)
				{
					ai_State = Random.Range(0, 4);
					ai_Time = Random.Range(10, 200);
				}
				else
				{
					ai_Time--;
				}
			}

			MoveToPosition(movement_Position, 1f, motor.charController.velocity.magnitude);
		}
	}

	void MoveToPosition(Vector3 position, float speedMultiplier, float magnitude)
	{
		float speed = movement_Speed * speed_Move_Muliplier * 2 * 5 * speedMultiplier;

		Vector3 direction = position - transform.position;

		Quaternion newRotation = transform.rotation;

		direction.y = 0f;

		// moving
		if(direction.magnitude > distance_OffsetPlayer)
		{
			motor.Move(direction.normalized * speed);
			newRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, 
				newRotation, turnSpeed * Time.deltaTime);
		}
		// just stop
		else
		{
			motor.Stop();
		}

		AnimationMove(magnitude * 0.1f);

		CheckIfAttackEnded();
	}

	void AnimationMove(float magnitude)
	{
		// Are we moving ?
		if (magnitude > moveMagnitude)
		{
			float speedAnimation = magnitude * 2f;

			if (speedAnimation < 1)
			{
				speedAnimation = 1f;
			}
			// idle -> running 
			if (!Anim.GetBool(PARAMETER_RUN))
			{
				Anim.SetBool(PARAMETER_RUN, true);
				Anim.speed = speedAnimation;
			}
		} else
		{
			// Running -> stop
			if (Anim.GetBool(PARAMETER_RUN))
			{
				Anim.SetBool(PARAMETER_RUN, false);
			}
		}
	}

	void Attack()
	{
		if(Random.Range(0, 2) > 0)
		{
			Anim.SetBool(PARAMETER_ATTACK_ONE, true);
		} else
		{
			Anim.SetBool(PARAMETER_ATTACK_TWO, true);
		}
	}

	void CheckIfAttackEnded()
	{
		// Attack1 anim about to end on the base layer Anim
		if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
		{
			// are we at the end of the animation 
			if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
			{
				Anim.SetBool(PARAMETER_ATTACK_ONE, false);
				Anim.SetBool(PARAMETER_RUN, false);
			}
		}

		// Attack2 anim about to end on the base layer Anim 
		if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
		{
			// are we at the end of the animation 
			if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
			{
				Anim.SetBool(PARAMETER_ATTACK_TWO, false);
				Anim.SetBool(PARAMETER_RUN, false);
			}
		}
	}

	void RightAttack_Begin()
	{
		rightAttackPoint.SetActive(true);
	}

	void RightAttack_End()
	{
		rightAttackPoint.SetActive(false);
	}

	void LeftAttack_Begin()
	{
		leftAttackPoint.SetActive(true);
	}

	void LeftAttack_End()
	{
		leftAttackPoint.SetActive(false);
	}

	void SetupAudioLists()
	{
		List<Sounds> soundsList = new List<Sounds>();

		SceneName nameScene = GameManager.instance.nameScene;
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
					else if(SoundManager.instance.soundObject.sounds[i].sounds[j].name.Contains(bodyHitSoundIdentifier))
					{
						BodyHitSounds.Add(SoundManager.instance.soundObject.sounds[i].sounds[j].trackFile);
					}
				}
			}
		}
	}
} // class
