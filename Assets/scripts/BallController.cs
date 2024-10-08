using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Windows;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody Rb;
    public ParticleSystem explosionParticle;
    public float speed= 15.0f;
    private bool isTravelling;
    private UnityEngine.Vector3 travelDirection;
    private UnityEngine.Vector3 nextCollisionPosition;
    public int minSwipeRecognition=500;
    private UnityEngine.Vector2 swipePosLastFrame;
    private UnityEngine.Vector2 swipePosCurrentFrame;
    private UnityEngine.Vector2 currentSwipe;
    private Color solveColor;
    public AudioClip obstacleSound;
    public AudioSource audioSource;
    private void Start() {
        solveColor = Random.ColorHSV(0.5f,1);
        GetComponent<MeshRenderer>().material.color = solveColor;
        audioSource=GetComponent<AudioSource>();
        
    }
   
 
    
    private void FixedUpdate() {
        if (isTravelling) {
            
        Rb.velocity=speed*travelDirection;
        

        }
           Collider[] hitColliders = Physics.OverlapSphere(transform.position-(Vector3.up/2),0.05f);
           int i = 0;
           while (i < hitColliders.Length) {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if(ground&& !ground.isColor) {
                ground.ChangeColor(solveColor);
            }
            i++;
           }
        if (nextCollisionPosition != Vector3.zero) {
            if(Vector3.Distance(transform.position,nextCollisionPosition) <1) {
                isTravelling=false;
                travelDirection=Vector3.zero;
                nextCollisionPosition=Vector3.zero;
                audioSource.PlayOneShot(obstacleSound);


            }

        }
        if (isTravelling) 
        return;
        if (UnityEngine.Input.GetMouseButtonUp(0)) {
            swipePosLastFrame=Vector2.zero;
            currentSwipe = Vector2.zero;
        }
        if (UnityEngine.Input.GetMouseButton(0)) {
            swipePosCurrentFrame=new Vector2(UnityEngine.Input.mousePosition.x,UnityEngine.Input.mousePosition.y);
            if(swipePosLastFrame !=Vector2.zero) {
                currentSwipe=swipePosCurrentFrame-swipePosLastFrame;
                if(currentSwipe.sqrMagnitude<minSwipeRecognition) {
                    return;
                }
                currentSwipe.Normalize();
                //up/Down
                if(currentSwipe.x > -0.5f && currentSwipe.x <0.5) {
                    //go up/Down
                    setDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                    explosionParticle.Play();
                    Explode();

                }
                //go left/right
                if(currentSwipe.y > -0.5f && currentSwipe.y <0.5) {
                    setDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                     Explode();
                    
                    
                }
            }
            swipePosLastFrame=swipePosCurrentFrame;
        }
        if (UnityEngine.Input.GetMouseButtonUp(0)) {
            swipePosLastFrame = Vector2.zero;
            currentSwipe=Vector2.zero;
        }

    }
    
    private void setDestination(Vector3 direction)  {
        travelDirection =direction;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,direction,out hit,100f)) {
            nextCollisionPosition =  hit.point;
        }
        isTravelling = true;
    }
    void Explode() {
        // Instantiate explosionParticle at the ball's current position
        Instantiate(explosionParticle, transform.position, transform.rotation);
        explosionParticle.Play();
    }

}
