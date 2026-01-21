using System.Numerics;
using LambdaEngine;
using LambdaEngine.InputSystem;
using LambdaEngine.SceneModule;

namespace LambdaEngineTutorial;

public class Player : BehaviourComponent {
    private SpriteRenderer spriteRenderer;

    public bool enable;
    
    private float speed = 7f;
    
    [LifecycleStart]
    protected virtual void Start() {
        Sprite sprite = Sprite.CreateWithTexture("Assets/player.bmp");
        
        spriteRenderer = CreateComponent<SpriteRenderer>();
        spriteRenderer.Sprite = sprite;
        spriteRenderer.Layer = 2;
    }

    [LifecycleUpdate]
    protected virtual void Update() {
        if (!enable) {
            return;
        }
        
        Move();
        Shoot();
    }

    private void Move() {
        Vector2 direction = Vector2.Zero;

        if (Input.GetKey(Keys.A)) {
            direction.X = -1;
        }
        else if (Input.GetKey(Keys.D)) {
            direction.X = 1;
        }
        
        transform.Translate(direction * (speed * Time.DeltaTime));
    }

    private void Shoot() {
        if (Input.GetKeyDown(Keys.SPACE)) {
            GameObject bulletObj = Instantiate(transform.Position);
            Bullet bullet = bulletObj.CreateComponent<Bullet>();
            bullet.direction = Vector2.UnitY;
        }
    }
}