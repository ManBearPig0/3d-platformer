using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private Animator Animator;
    private Rigidbody Rigidbody;
    private Text ScoreText;
    private float MoveSpeed;
    private float JumpStrength;
    private float TurnStrength;
    private float MoveDirection;
    private float TurnDirection;
    private bool IsGrounded;
    private bool IsJumping;
    private Vector3 SpawnPos;
    private Quaternion SpawnRot;
    private int Score;

    void Start()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        ScoreText = GameObject.Find("Score Text").GetComponentInChildren<Text>();
        SetScore();
        MoveSpeed = 5f;
        JumpStrength = 588.6f;
        TurnStrength = 200f;
        IsGrounded = true;
        IsJumping = false;
        SpawnPos = transform.position;
        SpawnRot = transform.rotation;
        Score = 0;
    }

    void Update() // Get inputs here mostly.
    {
        MoveDirection = Input.GetAxis("Vertical");
        TurnDirection = Input.GetAxis("Horizontal");

        IsJumping = Input.GetAxis("Jump") != 0f && Input.GetAxis("Jump") != 1f;

        if (IsGrounded && IsJumping)
        {
            Animator.SetBool("Jumping", true);
            Rigidbody.AddForce(Vector3.up * JumpStrength * .67f, ForceMode.Impulse);
            IsGrounded = false; // OnCollisionExit is not fast enough with setting onGround to false.
        }
        else
        {
            Animator.SetFloat("MoveDirection", MoveDirection);
            Animator.SetFloat("TurnDirection", TurnDirection);
        }

        if (Input.GetAxis("Cancel") == 1f)
        {
            SceneManager.LoadScene(0);
        }
    }

    void FixedUpdate() // AddForce/AddTorque reactions to inputs go here.
    {
        if (MoveDirection != 0)
        {
            Vector3 moveVelocity = transform.forward * MoveSpeed * MoveDirection;
            Rigidbody.velocity = new Vector3(moveVelocity.x, Rigidbody.velocity.y, moveVelocity.z);
        }

        Rigidbody.AddTorque(transform.up * TurnStrength * TurnDirection, ForceMode.Force);

        if (IsJumping)
        {          
            float jumpInverse = 1f - Input.GetAxis("Jump"); // The longer you hold jump, the less this becomes.
            Vector3 jumpVector = Vector3.up * JumpStrength * jumpInverse;
            Rigidbody.AddForce(jumpVector, ForceMode.Force);
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground" && col.GetContact(0).normal.y != 0) // If Unity-chan didn't hit the edge or top of a platform, she doesn't land.
        {
            IsGrounded = true;
            Animator.SetBool("Grounded", true);
            Animator.SetBool("Jumping", false);
        }

        if (col.gameObject.tag == "Bounds")
        {
            transform.position = SpawnPos;
            transform.rotation = SpawnRot;
            Rigidbody.velocity = Vector3.zero;
        }
        else if (col.gameObject.tag == "Enemy")
        {
            transform.position = SpawnPos;
            transform.rotation = SpawnRot;
            Rigidbody.velocity = Vector3.zero;
            Animator.Play("Lose", -1, 0f);
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Ground" && Rigidbody.velocity.y != 0) // UnityChan might have landed, so check if she's not falling.
        {
            IsGrounded = false;
            Animator.SetBool("Grounded", IsGrounded);
        }
    }

    void OnTriggerEnter(Collider trig)
    {
        if (trig.gameObject.tag == "Score")
        {
            Destroy(trig.gameObject);
            Score++;
            SetScore();
        }

        else if (trig.gameObject.tag == "Powerup")
        {
            Destroy(trig.gameObject);
            JumpStrength = 588.6f * 1.5f;
        }

        else if (trig.gameObject.tag == "Checkpoint")
        {
            SpawnPos = trig.transform.position;
            SpawnRot = trig.transform.rotation;
            Destroy(trig.gameObject);
        }

        else if (trig.gameObject.tag == "Goal" && Score == 5)
        {
            Destroy(trig.gameObject);
            Animator.Play("Win", -1, 0f);
            StartCoroutine(LoadNextLevel());
        }
    }

    void SetScore()
    {
        if (Score < 5)
        {
            ScoreText.text = "Score: " + Score.ToString() + "/5";
        }
        else
        {
            GameObject.Find("Goal").GetComponent<Renderer>().enabled = true; // Make the goal visible and interactable.
            ScoreText.text = "Get to the goal!";
        }
    }
}