
//プレイヤーの入力を取得するスクリプト
//
//責務
//・プレイヤーの入力を取得する
//・取得した入力を他のスクリプトに渡す

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] InputAction m_cutThread;               //糸を切るための入力
    [SerializeField] InputAction m_doorOperation;           //ドアの操作を行うための入力
    [SerializeField] InputAction m_signboardOperation;      //看板の操作を行うための入力
    [SerializeField] InputAction m_dungeonCoreOperation;    //ダンジョンコアの操作を行うための入力

    private Vector2 m_input;   //プレイヤーを動かすための入力

    public bool CutThreadDown => m_cutThread.WasPressedThisFrame();             //糸を切るための入力が押されたかどうか
    public bool DoorOperationDown => m_doorOperation.WasPressedThisFrame();     //インタラクションの入力が押されたかどうか
    public bool SignboardOperationDown => m_signboardOperation.WasPressedThisFrame(); //看板の操作の入力が押されたかどうか
    public bool DungeonCoreOperationDown => m_dungeonCoreOperation.WasPressedThisFrame(); //ダンジョンコアの操作の入力が押されたかどうか
    public Vector2 Input => m_input;

    //プレイヤーを動かすための入力を取得する
    public void OnMove(InputAction.CallbackContext context)
    {

        m_input = context.ReadValue<Vector2>();

    }

    private void OnEnable()
    {
        m_cutThread.Enable();
        m_doorOperation.Enable();
        m_signboardOperation.Enable();
        m_dungeonCoreOperation.Enable();
    }

    private void OnDisable()
    {
        m_cutThread.Disable();
        m_doorOperation.Disable();
        m_signboardOperation.Disable();
        m_dungeonCoreOperation.Disable();
    }

}
