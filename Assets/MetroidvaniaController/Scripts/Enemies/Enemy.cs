using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float life = 10;
	public float damageByObs = 10;
	private bool isPlat;
	private bool isObstacle;
	private bool isLine;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;

	private bool facingRight = true;
	
	public float speed = 5f;

	public bool isInvincible = false;
	private bool isHitted = false;

	void Awake () {
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (life <= 0) {
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			StartCoroutine(DestroyEnemy());
		}

		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
		isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);
		isLine = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Line"));
		if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
		{
			if ((isPlat||isLine) && !isObstacle && !isHitted)
			{
				if (facingRight)
				{
					rb.velocity = new Vector2(-speed, rb.velocity.y);
				}
				else
				{
					rb.velocity = new Vector2(speed, rb.velocity.y);
				}
			}
			else
			{
				Flip();
			}
		}
	}

	void Flip (){
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(float damage) {
		if (!isInvincible) 
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime(0.1f));
		}
	}

	public void ApplyDamageByObs()
	{
		if (!isInvincible)
		{
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damageByObs;
			rb.velocity = Vector2.zero;
			rb.AddForce(new Vector2(500f, 100f));
			StartCoroutine(HitTime(5f));
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Obstacle")
		{
			ApplyDamageByObs();
		}
	}

	IEnumerator HitTime(float time)
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(time);
		isHitted = false;
		isInvincible = false;
	}

	IEnumerator DestroyEnemy()
	{
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		yield return new WaitForSeconds(0.25f);
		rb.velocity = new Vector2(0, rb.velocity.y);
		yield return new WaitForSeconds(3f);
		Destroy(gameObject);
	}
}
