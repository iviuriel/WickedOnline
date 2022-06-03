using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wicked
{
    public class BoardManager : SingletonScene<GameManager>
    {
        public static BoardManager Instance
        {
            get
            {
                return ((BoardManager)mInstance);
            }
            set
            {
                mInstance = value;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SelectLocationForPlayer(int playerId, Location location)
        {
            if (GameManager.Instance.IsPlayerTurn(playerId))
            {
                return;
            }
        }
    }
}
