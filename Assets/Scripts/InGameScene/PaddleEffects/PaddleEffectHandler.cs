using UnityEngine;
using static UnityEditor.Progress;

public class PaddleEffectHandler : MonoBehaviour
{
    private PaddleMovement paddleMovement;
    private PaddleSizeHandler paddleSizeHandler;

    private void Start()
    {
        paddleMovement = GetComponent<PaddleMovement>();
        paddleSizeHandler = GetComponent<PaddleSizeHandler>();
    }

    // 이넘이 추가되면 복구 예정
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Item"))
    //    {
    //        Item item = other.GetComponent<Item>();

    //        if (item != null)
    //        {
    //            switch (item.itemType)
    //            {
    //                case ItemType.Speed:
    //                    // 속도 조정
    //                    paddleMovement.AdjustPaddleSpeed(2.0f);
    //                    break;

    //                case ItemType.Size:
    //                    // 크기 조정
    //                    paddleSizeHandler.AdjustPaddleSize(1.5f);
    //                    break;
    //            }
    //        }
    //    }
    //}
}