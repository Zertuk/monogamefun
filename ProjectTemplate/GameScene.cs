using System;
using Nez.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Tiled;
using Nez;

namespace ProjectTemplate
{
    //[SampleScene( "Platformer", "Work in progress..." )]
    public class GameScene : DefaultScene
    {
        public GameScene()
        {
        }
        private World _world;
        public override void initialize()
        {
            // setup a pixel perfect screen that fits our map
            setDesignResolution(320, 240, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            Screen.setSize(320 * 3, 240 * 3);

            _world = new World();

            var tiledEntity = createEntity("tiled");
            var tiledmap = contentManager.Load<TiledMap>(_world.activeRoom.tilemap);
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledmap, "collision"));
            tiledMapComponent.renderLayer = 10;

            var playerEntity = createRigidEntity(new Vector2(50, 50), 10f, 0.3f, 0f, new Vector2(0, 0));

            // add a component to have the Camera follow the player
            //camera.entity.addComponent(new FollowCamera(playerEntity.entity));
        }

        ArcadeRigidbody createRigidEntity(Vector2 position, float mass, float friction, float elasticity, Vector2 velocity)
        {
            var rigidbody = new ArcadeRigidbody()
                .setMass(mass)
                .setFriction(friction)
                .setElasticity(elasticity)
                .setVelocity(velocity);

            var entity = createEntity(Utils.randomString(3));
            entity.transform.position = position;
            entity.addComponent(new Player());
            entity.addComponent(rigidbody);
            entity.addCollider(new CircleCollider());

            return rigidbody;
        }


    }
}

