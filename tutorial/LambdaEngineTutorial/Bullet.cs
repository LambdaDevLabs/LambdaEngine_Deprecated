using System.Numerics;
using LambdaEngine;
using LambdaEngine.RenderSystem;
using LambdaEngine.SceneModule;

namespace LambdaEngineTutorial;

public class Bullet : BehaviourComponent {
    private static Sprite sprite;

    private float speed = 25f;

    private float damage = 10;

    public Vector2 direction;

    [LifecycleAwake]
    protected virtual void Awake() {
        if (sprite == null) {
            sprite = Sprite.CreateWithTexture("Assets/bullet.bmp");
        }
    }
    
    [LifecycleStart]
    protected virtual void Start() {
        /*
         pixelsPerUnit = 100;
         spriteSize = 10x10 Pixels;
         defaultDiameter = 1 Unit = 100 Pixels;
         
         diameter = spriteSize / pixelsPerUnit = 10 / 100 = 0.1
         diameter = 0.1 Units
         radius = 0.05 Units
        */
        
        CircleCollider circleCollider = CreateComponent<CircleCollider>();
        circleCollider.Radius = 0.05f;
        
        SpriteRenderer spriteRenderer = CreateComponent<SpriteRenderer>();
        spriteRenderer.Sprite = sprite;
    }

    [LifecycleUpdate]
    protected virtual void Update() {
        transform.Translate(direction * (speed * Time.DeltaTime));

        // Destroy bullet if it leaves the visible area.
        if (transform.Position.Y >= Camera.Size) {
            Destroy(gameObject);
        }
    }

    [LifecycleCollisionEnter]
    protected virtual void CollisionEnter(Collision collision) {
        if (collision.gameObject?.TryGetComponent(out IDamageable damageable) == true) {
            damageable.Damage(damage);
            Destroy(gameObject);
        }
    }
}