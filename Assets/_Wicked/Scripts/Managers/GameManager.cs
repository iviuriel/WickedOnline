using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked {
    public class GameManager : SingletonScene<GameManager>
    {
        public static GameManager Instance
        {
            get
            {
                return ((GameManager)mInstance);
            }
            set
            {
                mInstance = value;
            }
        }

        public PlayerManager playerManager;

        public List<PlayerManager> players = new List<PlayerManager>();

        private PlayerManager curPlayerTurn;

        /// DEV-----------------------------      
        public bool isDevelopmentMode = false;

        [ShowIfGroup("isDevelopmentMode")]
        [BoxGroup("isDevelopmentMode/Development")]
        public GameObject playerContainer;

        /// ----------------------------------


        void Start()
        {
            players.Clear();
            if (isDevelopmentMode)
            {
                PlayerManager player = GameObject.Instantiate(playerManager, playerContainer.transform);
                player.Init(1);
                players.Add(player);
                StartTurn(player.id);
            }
        }

        #region Turn Cycle

        public void StartTurn(int id)
        {
            PlayerManager player = players.Find(x => x.id == id);
            player.StartTurn();
        }

        public void EndTurn(int id)
        {
            return;
        }

        #endregion

        #region Location Functions

        public bool IsPlayerTurn(int id)
        {
            return curPlayerTurn.id == id;
        }
        #endregion

    }
}
