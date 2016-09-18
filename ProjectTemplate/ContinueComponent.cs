using System;
using Microsoft.Xna.Framework;
using Nez.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;
using Nez.AI.FSM;

namespace ProjectTemplate
{
    public class ContinueComponent : Component, ITriggerListener, IUpdatable
    {
        public bool ShouldContinue = false;
        private VirtualButton _continueInput;
        public override void onAddedToEntity()
        {
            setupInput();
        }
        

        void setupInput()
        {
            _continueInput = new VirtualButton();
            _continueInput.nodes.Add(new Nez.VirtualButton.KeyboardKey(Keys.Space));
        }

        void IUpdatable.update()
        {
            if (_continueInput.isPressed)
            {
                ShouldContinue = true;
            }
            var myScene = entity.scene as DeathScene;
            myScene.Update();
        }


        #region ITriggerListener implementation

        void ITriggerListener.onTriggerEnter(Collider other, Collider self)
        {
            Debug.log("triggerEnter: {0}", other.entity.name);
        }


        void ITriggerListener.onTriggerExit(Collider other, Collider self)
        {
            Debug.log("triggerExit: {0}", other.entity.name);
        }

        #endregion

    }
}

