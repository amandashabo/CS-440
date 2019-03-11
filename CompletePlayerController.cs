using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;

namespace CS_440
{
    public class CompletePlayerController : MonoBehaviour
    {

        public float speed;             //Floating point variable to store the player's movement speed.
        public Text PickupCountText;          //Store a reference to the UI Text component which will display the number of pickups collected.
        public Text StoneCountText;
        public Text TreeCountText;
        public Text winText;            //Store a reference to the UI Text component which will display the 'You win' message.

        private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
        private int PickupCount;              //Integer to store the number of pickups collected so far.
        private int StoneCount;
        private int TreeCount;
        private bool facingRight = true;
        private bool facingRightPrev = true;

        private Vector3 mousePosition;
        //  public float moveSpeed = 40f;

        // Use this for initialization
        void Start()
        {
            //Get and store a reference to the Rigidbody2D component so that we can access it.
            rb2d = GetComponent<Rigidbody2D>();

            //Initialize count to zero.
            PickupCount = 0;
            StoneCount = 0;
            TreeCount = 0;


            //Initialze winText to a blank string since we haven't won yet at beginning.
            winText.text = "";

            //Call our SetCountText function which will update the text with the current value for count.
            SetPickupText();
            SetStoneText();
            SetTreeText();
        }

        //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
        void FixedUpdate()
        {
            /*if (Input.GetMouseButton(1))
            {
                mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
            }*/

            if (Input.GetMouseButton(0))
            {
                //print("mouse");
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 tmpDir = mousePosition - transform.position;
                //print(tmpDir);
                //this line should disable moving near the end or the object will "jitter"
                //disable if-line to see what I mean
                if (tmpDir.sqrMagnitude > 0.1f)
                    rb2d.AddForce(tmpDir * speed);
                //transform.Translate(tmpDir.normalized * speed);
                //transform.Translate(tmpDir.normalized * moveSpeed * Time.deltaTime);

                //rb2d.AddForce(tmpDir * speed);
            }
            //print("move");
            //Store the current horizontal input in the float moveHorizontal.
            float moveHorizontal = Input.GetAxis("Horizontal");

            //Store the current vertical input in the float moveVertical.
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = Vector3.zero;

            //Move player in the direction of the input key
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a")) {
                transform.position += Vector3.left * speed * Time.deltaTime;
                facingRightPrev = false;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d")) {
                transform.position += Vector3.right * speed * Time.deltaTime;
                facingRightPrev = true;
            }
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w")) {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s")) {
                transform.position += Vector3.down * speed * Time.deltaTime;
            }

            if (facingRightPrev != facingRight)
            {
                Flip();
            }

            //Add movement to Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
            transform.Translate(moveDirection * speed * Time.deltaTime);
        }
           

        private void Flip()
        {

            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }



        //OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
        void OnTriggerEnter2D(Collider2D other)
        {
            //print("collision");
            //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
            if (other.gameObject.CompareTag("Pickup"))
            {
                //... then set the other object we just collided with to inactive.
                other.gameObject.SetActive(false);

                //Add one to the current value of our count variable.
                PickupCount += 1;

                //Update the currently displayed count by calling the SetCountText function.
                SetPickupText();
            }
            else if (other.gameObject.CompareTag("Stone"))
            {
                //... then set the other object we just collided with to inactive.
                other.gameObject.SetActive(false);

                //Add one to the current value of our count variable.
                StoneCount += 1;

                //Update the currently displayed count by calling the SetCountText function.
                SetStoneText();
            }
            else if (other.gameObject.CompareTag("Tree"))
            {
                //... then set the other object we just collided with to inactive.
                other.gameObject.SetActive(false);

                //Add one to the current value of our count variable.
                TreeCount += 1;

                //Update the currently displayed count by calling the SetCountText function.
                SetTreeText();
            }
            CheckWin();

        }

        //This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
        void SetPickupText()
        {
            //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
            PickupCountText.text = "Pickups: " + PickupCount.ToString();

            //Check if we've collected all 12 pickups. If we have...
            //if (count >= 12)
                //... then set the text property of our winText object to "You win!"
                //winText.text = "You win!";
        }
        void SetStoneText()
        {
            //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
            StoneCountText.text = "Stones: " + StoneCount.ToString();

            //Check if we've collected all 12 pickups. If we have...
            //if (count >= 12)
            //... then set the text property of our winText object to "You win!"
            //winText.text = "You win!";
        }
        void SetTreeText()
        {
            //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
            TreeCountText.text = "Trees: " + TreeCount.ToString();

            //Check if we've collected all 12 pickups. If we have...
            //if (count >= 12)
            //... then set the text property of our winText object to "You win!"
            //winText.text = "You win!";
        }
        void CheckWin()
        {

            //Check if we've collected all 12 pickups. If we have...
            if (PickupCount >= 6 && StoneCount >= 3 && TreeCount >= 5)
            //... then set the text property of our winText object to "You win!"
                winText.text = "You win!";
        }
    }
}