using UnityEngine;

public class UIRoot : MonoBehaviour
{
    [SerializeField]
    private MenuView menuView;
    public MenuView MenuView => menuView;


    [SerializeField]
    private GameView gameView;
    public GameView GameView => gameView;

    [SerializeField] 
    private PauseView pauseView;
    public PauseView PauseView => pauseView;

    [SerializeField] 
    private GameOverView gameOverView;
    public GameOverView GameOverView => gameOverView;

    [SerializeField]
    private LobbyView lobbyView;
    public LobbyView LobbyView => lobbyView;

    [SerializeField] 
    private ConfigView configView;
    public ConfigView ConfigView => configView;
    
    
    [SerializeField] 
    private StoreView storeView;
    public StoreView StoreView => storeView;
    [SerializeField] 
    private BossView bossView;
    public BossView BossView => bossView;
}
